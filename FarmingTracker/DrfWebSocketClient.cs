using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FarmingTracker
{
	public class DrfWebSocketClient : IDisposable
	{
		private readonly SemaphoreSlim _closeSemaphoreSlim = new SemaphoreSlim(1);

		private static readonly object _drfMessagesLock = new object();

		private static readonly object _letOnlyLatestWaitLock = new object();

		private static readonly object _disposeLock = new object();

		private List<DrfMessage> _drfMessages = new List<DrfMessage>();

		private ClientWebSocket? _clientWebSocket;

		private const char FIRST_LETTER_OF_KIND_SESSION_UPDATE = 's';

		private const string CLOSED_BY_CLIENT_DESCRIPTION = "closed by blish farming tracker module";

		private const string CLOSED_BY_SERVER_BECAUSE_AUTHENTICATION_FAILED_DESCRIPTION = "no valid session provided";

		private const int PARTIAL_RECEIVE_BUFFER_SIZE = 4000;

		private const int RECEIVE_BUFFER_SIZE = 40000;

		public const int MAX_CURRENCIES_IN_A_SINGLE_DROP = 10;

		private readonly byte[] _receiveBuffer = new byte[40000];

		private CancellationTokenSource _disposeCts = new CancellationTokenSource();

		private CancellationTokenSource _letOnlyLatestWaitCts = new CancellationTokenSource();

		private bool _disposed;

		public string WebSocketUrl { get; set; } = "wss://drf.rs/ws";


		public bool WindowsVersionIsTooLowToSupportWebSockets { get; set; }

		public event EventHandler? Connecting;

		public event EventHandler? ConnectedAndAuthenticationRequestSent;

		public event EventHandler<GenericEventArgs<Exception>>? ConnectFailed;

		public event EventHandler<GenericEventArgs<Exception>>? ConnectCrashed;

		public event EventHandler<GenericEventArgs<Exception>>? SendAuthenticationFailed;

		public event EventHandler<GenericEventArgs<string>>? AuthenticationFailed;

		public event EventHandler<GenericEventArgs<string>>? UnexpectedNotOpenWhileReceiving;

		public event EventHandler<GenericEventArgs<string>>? ReceivedMessage;

		public event EventHandler? ReceivedUnexpectedBinaryMessage;

		public event EventHandler<GenericEventArgs<Exception>>? ReceiveCrashed;

		public event EventHandler<GenericEventArgs<Exception>>? ReceiveFailed;

		public DrfWebSocketClient()
		{
			try
			{
				_clientWebSocket = new ClientWebSocket();
			}
			catch (PlatformNotSupportedException e)
			{
				WindowsVersionIsTooLowToSupportWebSockets = true;
				Module.Logger.Warn("Failed to initialize the DRF WebSocket client. This is typically caused by not using at least Windows 8. WebSockets are not supported in older Windows versions. The module will not work. PlatformNotSupportedException message: " + e.Message);
			}
		}

		public List<DrfMessage> GetDrfMessages()
		{
			List<DrfMessage> newEmptyList = new List<DrfMessage>();
			List<DrfMessage> receivedMessages;
			lock (_drfMessagesLock)
			{
				receivedMessages = _drfMessages;
				_drfMessages = newEmptyList;
			}
			return RemoveInvalidMessages(receivedMessages);
		}

		public void Dispose()
		{
			if (!WindowsVersionIsTooLowToSupportWebSockets)
			{
				lock (_disposeLock)
				{
					this.ConnectFailed = null;
					this.ConnectCrashed = null;
					this.SendAuthenticationFailed = null;
					this.AuthenticationFailed = null;
					this.UnexpectedNotOpenWhileReceiving = null;
					this.ReceivedMessage = null;
					this.ReceivedUnexpectedBinaryMessage = null;
					this.ReceiveCrashed = null;
					this.ReceiveFailed = null;
					_disposed = true;
					_disposeCts.Cancel();
				}
			}
		}

		public async Task Connect(string drfToken)
		{
			if (WindowsVersionIsTooLowToSupportWebSockets)
			{
				return;
			}
			CancellationTokenSource letOnlyLatestWaitCts = new CancellationTokenSource();
			lock (_letOnlyLatestWaitLock)
			{
				_letOnlyLatestWaitCts.Cancel();
				_letOnlyLatestWaitCts = letOnlyLatestWaitCts;
			}
			ClientWebSocket clientWebSocket = null;
			try
			{
				try
				{
					await _closeSemaphoreSlim.WaitAsync(letOnlyLatestWaitCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				CancellationTokenSource disposeCts;
				try
				{
					await Close().ConfigureAwait(continueOnCapturedContext: false);
					disposeCts = new CancellationTokenSource();
					lock (_disposeLock)
					{
						if (_disposed)
						{
							return;
						}
						_disposeCts.Cancel();
						_disposeCts = disposeCts;
					}
					clientWebSocket = new ClientWebSocket();
					_clientWebSocket = clientWebSocket;
				}
				finally
				{
					_closeSemaphoreSlim.Release();
				}
				this.Connecting?.Invoke(this, EventArgs.Empty);
				try
				{
					await clientWebSocket.ConnectAsync(new Uri(WebSocketUrl), disposeCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception e3)
				{
					clientWebSocket.Abort();
					if (!disposeCts.IsCancellationRequested)
					{
						this.ConnectFailed?.Invoke(this, new GenericEventArgs<Exception>(e3));
					}
					return;
				}
				ArraySegment<byte> authenticationSendBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Bearer " + drfToken));
				try
				{
					await clientWebSocket.SendAsync(authenticationSendBuffer, WebSocketMessageType.Text, endOfMessage: true, disposeCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception e2)
				{
					clientWebSocket.Abort();
					if (!disposeCts.IsCancellationRequested)
					{
						this.SendAuthenticationFailed?.Invoke(this, new GenericEventArgs<Exception>(e2));
					}
					return;
				}
				this.ConnectedAndAuthenticationRequestSent?.Invoke(this, EventArgs.Empty);
				Task.Run(delegate
				{
					ReceiveContinuously(clientWebSocket, disposeCts.Token);
				}, disposeCts.Token);
			}
			catch (OperationCanceledException)
			{
				clientWebSocket?.Abort();
			}
			catch (Exception e)
			{
				clientWebSocket?.Abort();
				this.ConnectCrashed?.Invoke(this, new GenericEventArgs<Exception>(e));
			}
		}

		private async void ReceiveContinuously(ClientWebSocket clientWebSocket, CancellationToken ctsToken)
		{
			_ = 1;
			try
			{
				while (!_disposed)
				{
					ctsToken.ThrowIfCancellationRequested();
					if (clientWebSocket.State != WebSocketState.Open)
					{
						if (!(clientWebSocket.CloseStatusDescription == "closed by blish farming tracker module"))
						{
							this.UnexpectedNotOpenWhileReceiving?.Invoke(this, new GenericEventArgs<string>("receive loop start " + CreateStatusMessage(clientWebSocket)));
						}
						break;
					}
					int offsetInReceiveBuffer = 0;
					WebSocketReceiveResult receiveResult;
					do
					{
						ArraySegment<byte> partialReceiveBuffer = new ArraySegment<byte>(_receiveBuffer, offsetInReceiveBuffer, 4000);
						receiveResult = await clientWebSocket.ReceiveAsync(partialReceiveBuffer, ctsToken).ConfigureAwait(continueOnCapturedContext: false);
						offsetInReceiveBuffer += receiveResult.Count;
					}
					while (!receiveResult.EndOfMessage);
					switch (receiveResult.MessageType)
					{
					case WebSocketMessageType.Binary:
						this.ReceivedUnexpectedBinaryMessage?.Invoke(this, null);
						break;
					case WebSocketMessageType.Text:
					{
						string receivedJson = Encoding.UTF8.GetString(_receiveBuffer, 0, offsetInReceiveBuffer);
						if (receivedJson[9] == 's')
						{
							break;
						}
						DrfMessage drfMessage = JsonConvert.DeserializeObject<DrfMessage>(receivedJson);
						if (drfMessage == null)
						{
							Module.Logger.Error("Failed to create drfMessage from json.");
							break;
						}
						lock (_drfMessagesLock)
						{
							_drfMessages.Add(drfMessage);
						}
						this.ReceivedMessage?.Invoke(this, new GenericEventArgs<string>($"({offsetInReceiveBuffer} bytes): {receivedJson}\n"));
						break;
					}
					case WebSocketMessageType.Close:
						if (!(clientWebSocket.CloseStatusDescription == "closed by blish farming tracker module"))
						{
							if (clientWebSocket.CloseStatusDescription == "no valid session provided")
							{
								this.AuthenticationFailed?.Invoke(this, new GenericEventArgs<string>(CreateStatusMessage(clientWebSocket)));
								return;
							}
							await clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, receiveResult.CloseStatusDescription, ctsToken).ConfigureAwait(continueOnCapturedContext: false);
							this.UnexpectedNotOpenWhileReceiving?.Invoke(this, new GenericEventArgs<string>("close message " + CreateStatusMessage(clientWebSocket)));
						}
						return;
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (WebSocketException e2)
			{
				if (!ctsToken.IsCancellationRequested)
				{
					this.ReceiveFailed?.Invoke(this, new GenericEventArgs<Exception>(e2));
				}
			}
			catch (Exception e)
			{
				if (!ctsToken.IsCancellationRequested)
				{
					this.ReceiveCrashed?.Invoke(this, new GenericEventArgs<Exception>(e));
				}
			}
			finally
			{
				clientWebSocket.Abort();
			}
		}

		private async Task Close()
		{
			try
			{
				if (_clientWebSocket == null)
				{
					Module.Logger.Error("Close() expects clientWebSocket to be set.");
				}
				else if (_clientWebSocket!.State == WebSocketState.Open || _clientWebSocket!.State == WebSocketState.CloseReceived || _clientWebSocket!.State == WebSocketState.Connecting)
				{
					await _clientWebSocket!.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "closed by blish farming tracker module", default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception)
			{
			}
		}

		private static string CreateStatusMessage(ClientWebSocket clientWebSocket)
		{
			return $"State.{clientWebSocket.State} CloseStatus.{clientWebSocket.CloseStatus} CloseStatusDescription: {clientWebSocket.CloseStatusDescription}";
		}

		private static List<DrfMessage> RemoveInvalidMessages(List<DrfMessage> drfMessages)
		{
			return drfMessages.Where((DrfMessage m) => m.Payload.Drop.Currencies.Count <= 10).ToList();
		}
	}
}
