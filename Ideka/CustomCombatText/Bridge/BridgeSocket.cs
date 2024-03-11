using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;

namespace Ideka.CustomCombatText.Bridge
{
	public class BridgeSocket : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<BridgeSocket>();

		private const int BufferSize = 204800;

		private const int ConnectRetries = 10;

		private const int ReceiveRetries = 6;

		private static readonly TimeSpan ReceiveTimeout = TimeSpan.FromSeconds(10.0);

		private CancellationTokenSource? _cts;

		private readonly object _lock = new object();

		public bool Disposed { get; private set; }

		public event EventHandler<byte[]>? RawMessage;

		public async Task Start(int port)
		{
			CancellationTokenSource cts;
			CancellationToken ct;
			Socket socket;
			lock (_lock)
			{
				if (Disposed)
				{
					return;
				}
				_cts?.Cancel();
				cts = (_cts = new CancellationTokenSource());
				ct = cts.Token;
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
				{
					ReceiveBufferSize = 204800
				};
				ct.Register(delegate
				{
					try
					{
						if (socket.Connected)
						{
							Logger.Info("Socket disconnecting.");
							socket.Shutdown(SocketShutdown.Receive);
						}
					}
					catch (Exception ex3)
					{
						Logger.Warn(ex3, "Socket disconnect failed.");
					}
					finally
					{
						socket.Dispose();
					}
				});
			}
			for (int triesLeft = 10; triesLeft > 0; triesLeft--)
			{
				try
				{
					await socket.ConnectAsync(IPAddress.Loopback, port);
				}
				catch (Exception ex) when (triesLeft <= 1)
				{
					Logger.Info(ex, "Socket connection failed too many times.");
					Dispose();
					return;
				}
				catch
				{
					goto IL_01fe;
				}
				break;
				IL_01fe:
				await Task.Delay(TimeSpan.FromSeconds(5.0), ct);
			}
			ct.ThrowIfCancellationRequested();
			Logger.Info($"Socket connected to {socket.RemoteEndPoint}");
			int retries = 6;
			BridgeBuffer buffer = new BridgeBuffer();
			while (true)
			{
				int bytesRead;
				try
				{
					Task<int> receive = socket.ReceiveAsync(buffer.NextSegment, SocketFlags.None);
					if (await Task.WhenAny(receive, Task.Delay(ReceiveTimeout, ct)) != receive)
					{
						int num = retries - 1;
						retries = num;
						if (num > 0)
						{
							continue;
						}
						Logger.Info("Receive timed out too many times.");
						break;
					}
					bytesRead = receive.Result;
					goto IL_03d5;
				}
				catch (SocketException ex2) when (ex2.SocketErrorCode == SocketError.ConnectionReset)
				{
					int num = retries - 1;
					retries = num;
					if (num > 0)
					{
						Logger.Info((Exception)ex2, "Receive failed, retrying.");
						continue;
					}
					Logger.Warn((Exception)ex2, "Receive failed too many times.");
					break;
				}
				catch (Exception e2)
				{
					Logger.Error(e2, "Receive failed.");
					break;
				}
				IL_03d5:
				if (bytesRead <= 0)
				{
					Logger.Info("Connection closed.");
					break;
				}
				retries = 6;
				try
				{
					foreach (byte[] messageData in buffer.ProcessReceivedData(bytesRead))
					{
						this.RawMessage?.Invoke(this, messageData);
					}
				}
				catch (Exception e)
				{
					Logger.Error(e, "Failed processing data.");
					break;
				}
				ct.ThrowIfCancellationRequested();
			}
			cts.Cancel();
		}

		public void Dispose()
		{
			lock (_lock)
			{
				Disposed = true;
				_cts?.Cancel();
			}
		}
	}
}
