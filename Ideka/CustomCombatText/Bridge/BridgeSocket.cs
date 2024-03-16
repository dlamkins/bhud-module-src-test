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

		public event EventHandler<ArraySegment<byte>>? RawMessage;

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
					catch (Exception ex2)
					{
						Logger.Warn(ex2, "Socket disconnect failed.");
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
			try
			{
				_ = 2;
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
							return;
						}
						bytesRead = receive.Result;
						goto IL_0375;
					}
					catch (Exception e2)
					{
						Logger.Error(e2, "Receive failed.");
						return;
					}
					IL_0375:
					if (bytesRead <= 0)
					{
						break;
					}
					retries = 6;
					try
					{
						foreach (ArraySegment<byte> segment in buffer.ProcessReceivedData(bytesRead))
						{
							this.RawMessage?.Invoke(this, segment);
						}
					}
					catch (Exception e)
					{
						Logger.Error(e, "Failed processing data.");
						return;
					}
					ct.ThrowIfCancellationRequested();
				}
				Logger.Info("Connection closed.");
			}
			finally
			{
				cts.Cancel();
			}
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
