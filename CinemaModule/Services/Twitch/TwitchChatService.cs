using System;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Models.Twitch;
using Microsoft.Xna.Framework;

namespace CinemaModule.Services.Twitch
{
	public class TwitchChatService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<TwitchChatService>();

		private const string TwitchIrcServer = "irc.chat.twitch.tv";

		private const int TwitchIrcPort = 6667;

		private static readonly Random ChatColorRandom = new Random();

		private static readonly Color TwitchPurpleColor = new Color(169, 112, 255);

		private TcpClient _tcpClient;

		private StreamReader _reader;

		private StreamWriter _writer;

		private CancellationTokenSource _cts;

		private Task _readTask;

		private string _currentChannel;

		private string _authToken;

		private string _username;

		private bool _isConnected;

		private bool _isDisposed;

		public bool IsConnected => _isConnected;

		public string Username => _username;

		public bool IsAuthenticated
		{
			get
			{
				if (!string.IsNullOrEmpty(_username))
				{
					return !string.IsNullOrEmpty(_authToken);
				}
				return false;
			}
		}

		public event EventHandler<TwitchChatMessageEventArgs> MessageReceived;

		public event EventHandler<TwitchChatConnectionEventArgs> ConnectionStateChanged;

		public void SetCredentials(string username, string authToken)
		{
			_username = username;
			_authToken = authToken;
		}

		public async Task ConnectAsync(string channel)
		{
			if (string.IsNullOrWhiteSpace(channel))
			{
				Logger.Warn("Cannot connect to chat - channel name is empty");
				return;
			}
			channel = channel.ToLowerInvariant().TrimStart('#');
			if (!_isConnected || !(_currentChannel == channel))
			{
				await DisconnectAsync();
				_currentChannel = channel;
				_cts = new CancellationTokenSource();
				try
				{
					await EstablishConnectionAsync();
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to connect to chat for #" + channel);
					RaiseConnectionStateChanged(isConnected: false, "Connection failed: " + ex.Message);
				}
			}
		}

		public async Task DisconnectAsync()
		{
			if (!_isConnected && _tcpClient == null)
			{
				return;
			}
			_cts?.Cancel();
			try
			{
				if (_readTask != null)
				{
					await Task.WhenAny(_readTask, Task.Delay(1000));
				}
			}
			catch
			{
			}
			CleanupConnection();
			_isConnected = false;
			_currentChannel = null;
			RaiseConnectionStateChanged(isConnected: false, "Disconnected");
		}

		public async Task SendMessageAsync(string message)
		{
			if (!_isConnected || string.IsNullOrWhiteSpace(message))
			{
				return;
			}
			if (string.IsNullOrEmpty(_authToken))
			{
				Logger.Warn("Cannot send message - not authenticated");
				return;
			}
			try
			{
				await _writer.WriteLineAsync("PRIVMSG #" + _currentChannel + " :" + message);
				await _writer.FlushAsync();
				TwitchChatMessage ownMessage = new TwitchChatMessage(_username, _username, message, TwitchPurpleColor);
				RaiseMessageReceived(ownMessage);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to send chat message");
			}
		}

		private async Task EstablishConnectionAsync()
		{
			if (_isDisposed || (_cts?.IsCancellationRequested ?? false))
			{
				return;
			}
			RaiseConnectionStateChanged(isConnected: false, "Connecting...");
			TcpClient tcpClient = new TcpClient();
			try
			{
				await tcpClient.ConnectAsync("irc.chat.twitch.tv", 6667);
			}
			catch (ObjectDisposedException)
			{
				return;
			}
			catch (NullReferenceException)
			{
				return;
			}
			catch (SocketException ex)
			{
				Logger.Warn("Socket error connecting to Twitch IRC: " + ex.Message);
				tcpClient.Close();
				return;
			}
			if (_isDisposed || (_cts?.IsCancellationRequested ?? false))
			{
				tcpClient.Close();
				return;
			}
			_tcpClient = tcpClient;
			NetworkStream stream = tcpClient.GetStream();
			StreamWriter writer = new StreamWriter(stream)
			{
				AutoFlush = true
			};
			StreamReader reader = new StreamReader(stream);
			_writer = writer;
			_reader = reader;
			if (_isDisposed || (_cts?.IsCancellationRequested ?? false))
			{
				return;
			}
			await SendAuthenticationAsync(writer);
			if (_isDisposed || (_cts?.IsCancellationRequested ?? false))
			{
				return;
			}
			await JoinChannelAsync(writer);
			if (!_isDisposed && !(_cts?.IsCancellationRequested ?? false))
			{
				_isConnected = true;
				RaiseConnectionStateChanged(isConnected: true, "Connected to #" + _currentChannel);
				_readTask = Task.Run(() => ReadMessagesAsync(_cts.Token));
			}
		}

		private async Task SendAuthenticationAsync(StreamWriter writer)
		{
			await writer.WriteLineAsync("CAP REQ :twitch.tv/tags twitch.tv/commands");
			if (!string.IsNullOrEmpty(_authToken))
			{
				string token = (_authToken.StartsWith("oauth:", StringComparison.OrdinalIgnoreCase) ? _authToken : ("oauth:" + _authToken));
				await writer.WriteLineAsync("PASS " + token);
				await writer.WriteLineAsync("NICK " + _username);
			}
			else
			{
				string anonNick = $"justinfan{ChatColorRandom.Next(10000, 99999)}";
				await writer.WriteLineAsync("NICK " + anonNick);
			}
		}

		private async Task ReconnectAnonymouslyAsync()
		{
			_ = _currentChannel;
			CleanupConnection();
			_isConnected = false;
			string savedToken = _authToken;
			string savedUsername = _username;
			_authToken = null;
			_username = null;
			_cts = new CancellationTokenSource();
			try
			{
				await EstablishConnectionAsync();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to reconnect anonymously");
				_authToken = savedToken;
				_username = savedUsername;
			}
		}

		private async Task JoinChannelAsync(StreamWriter writer)
		{
			await writer.WriteLineAsync("JOIN #" + _currentChannel);
		}

		private async Task ReadMessagesAsync(CancellationToken token)
		{
			try
			{
				while (!token.IsCancellationRequested && !_isDisposed && (_tcpClient?.Connected ?? false))
				{
					StreamReader reader = _reader;
					if (reader != null)
					{
						string line = await reader.ReadLineAsync();
						if (line != null)
						{
							ProcessIrcMessage(line);
							continue;
						}
						break;
					}
					break;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Exception ex3) when (!token.IsCancellationRequested && !_isDisposed)
			{
				Logger.Error(ex3, "Error reading from IRC");
				HandleDisconnection();
			}
		}

		private void ProcessIrcMessage(string rawMessage)
		{
			if (string.IsNullOrEmpty(rawMessage))
			{
				return;
			}
			if (rawMessage.StartsWith("PING"))
			{
				HandlePing(rawMessage);
			}
			else if (rawMessage.Contains("NOTICE") && rawMessage.Contains("Login unsuccessful"))
			{
				Logger.Warn("Authentication failed - reconnecting anonymously");
				ReconnectAnonymouslyAsync();
			}
			else if (rawMessage.Contains("PRIVMSG"))
			{
				TwitchChatMessage chatMessage = ParsePrivMsg(rawMessage);
				if (chatMessage != null)
				{
					RaiseMessageReceived(chatMessage);
				}
			}
		}

		private void HandlePing(string pingMessage)
		{
			try
			{
				string pongResponse = pingMessage.Replace("PING", "PONG");
				_writer.WriteLine(pongResponse);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to send PONG");
			}
		}

		private TwitchChatMessage ParsePrivMsg(string rawMessage)
		{
			try
			{
				TwitchChatMessage message = new TwitchChatMessage();
				if (rawMessage.StartsWith("@"))
				{
					int tagsEnd = rawMessage.IndexOf(' ');
					if (tagsEnd > 0)
					{
						string tagsSection = rawMessage.Substring(1, tagsEnd - 1);
						ParseTags(tagsSection, message);
						rawMessage = rawMessage.Substring(tagsEnd + 1);
					}
				}
				int privmsgIndex = rawMessage.IndexOf("PRIVMSG");
				if (privmsgIndex < 0)
				{
					return null;
				}
				Match usernameMatch = Regex.Match(rawMessage, ":(\\w+)!");
				if (usernameMatch.Success)
				{
					message.Username = usernameMatch.Groups[1].Value;
				}
				if (string.IsNullOrEmpty(message.DisplayName))
				{
					message.DisplayName = message.Username;
				}
				int messageStart = rawMessage.IndexOf(':', privmsgIndex);
				if (messageStart >= 0)
				{
					string text = rawMessage.Substring(messageStart + 1);
					if (text.StartsWith("\u0001ACTION ") && text.EndsWith("\u0001"))
					{
						message.IsAction = true;
						text = text.Substring(8, text.Length - 9);
					}
					message.Message = text;
				}
				return message;
			}
			catch (Exception ex)
			{
				Logger.Debug("Failed to parse PRIVMSG: " + ex.Message);
				return null;
			}
		}

		private void ParseTags(string tagsSection, TwitchChatMessage message)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			string[] array = tagsSection.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				string[] parts = array[i].Split(new char[1] { '=' }, 2);
				if (parts.Length != 2)
				{
					continue;
				}
				string key = parts[0];
				string value = parts[1];
				if (!(key == "display-name"))
				{
					if (key == "color")
					{
						message.UserColor = ParseColor(value);
					}
				}
				else
				{
					message.DisplayName = value;
				}
			}
		}

		private Color ParseColor(string colorHex)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(colorHex) || !colorHex.StartsWith("#"))
			{
				return GetRandomChatColor();
			}
			try
			{
				string text = colorHex.TrimStart('#');
				int r = Convert.ToInt32(text.Substring(0, 2), 16);
				int g = Convert.ToInt32(text.Substring(2, 2), 16);
				int b = Convert.ToInt32(text.Substring(4, 2), 16);
				return new Color(r, g, b);
			}
			catch
			{
				return GetRandomChatColor();
			}
		}

		private Color GetRandomChatColor()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			Color[] colors = (Color[])(object)new Color[15]
			{
				new Color(255, 0, 0),
				new Color(0, 0, 255),
				new Color(0, 128, 0),
				new Color(178, 34, 34),
				new Color(255, 127, 80),
				new Color(154, 205, 50),
				new Color(255, 69, 0),
				new Color(46, 139, 87),
				new Color(218, 165, 32),
				new Color(210, 105, 30),
				new Color(95, 158, 160),
				new Color(30, 144, 255),
				new Color(255, 105, 180),
				new Color(138, 43, 226),
				new Color(0, 255, 127)
			};
			return colors[ChatColorRandom.Next(colors.Length)];
		}

		private void RaiseMessageReceived(TwitchChatMessage message)
		{
			this.MessageReceived?.Invoke(this, new TwitchChatMessageEventArgs(message));
		}

		private void HandleDisconnection()
		{
			_isConnected = false;
			RaiseConnectionStateChanged(isConnected: false, "Connection lost");
			CleanupConnection();
		}

		private void CleanupConnection()
		{
			try
			{
				_reader?.Dispose();
				_writer?.Dispose();
				_tcpClient?.Close();
				_tcpClient?.Dispose();
			}
			catch
			{
			}
			finally
			{
				_reader = null;
				_writer = null;
				_tcpClient = null;
			}
			_cts?.Dispose();
			_cts = null;
		}

		private void RaiseConnectionStateChanged(bool isConnected, string status)
		{
			this.ConnectionStateChanged?.Invoke(this, new TwitchChatConnectionEventArgs(isConnected, status));
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_cts?.Cancel();
				CleanupConnection();
			}
		}
	}
}
