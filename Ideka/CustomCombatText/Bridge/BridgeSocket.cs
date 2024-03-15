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

		private const int MessageHeaderSize = 8;

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
					goto IL_0202;
				}
				break;
				IL_0202:
				await Task.Delay(TimeSpan.FromSeconds(5.0), ct);
			}
			ct.ThrowIfCancellationRequested();
			Logger.Info($"Socket connected to {socket.RemoteEndPoint}");
			while (true)
			{
				int messageSize;
				using (RentedArray<byte> rentedArray = new RentedArray<byte>(8))
				{
					if (!(await Receive(socket, rentedArray.Array, rentedArray.Length, ct)))
					{
						break;
					}
					messageSize = BitConverter.ToInt32(rentedArray.Array, 0);
					goto IL_031f;
				}
				IL_031f:
				if (messageSize <= 0)
				{
					Logger.Warn("Received message with negative or zero size.");
					break;
				}
				using (RentedArray<byte> rentedArray = new RentedArray<byte>(messageSize))
				{
					if (!(await Receive(socket, rentedArray.Array, rentedArray.Length, ct)))
					{
						break;
					}
					this.RawMessage?.Invoke(this, rentedArray.Array);
					goto IL_0410;
				}
				IL_0410:
				ct.ThrowIfCancellationRequested();
			}
			cts.Cancel();
		}

		private static async Task<bool> Receive(Socket socket, byte[] buffer, int length, CancellationToken ct)
		{
			int retries = 6;
			int bytesRead = 0;
			while (bytesRead < length)
			{
				ct.ThrowIfCancellationRequested();
				try
				{
					Task<int> receive = socket.ReceiveAsync(new ArraySegment<byte>(buffer, bytesRead, length - bytesRead), SocketFlags.None);
					if (await Task.WhenAny(receive, Task.Delay(ReceiveTimeout, ct)) != receive)
					{
						int num = retries - 1;
						retries = num;
						if (num <= 0)
						{
							Logger.Info("Receive timed out too many times.");
							return false;
						}
					}
					else
					{
						if (receive.Result <= 0)
						{
							Logger.Info("Connection closed.");
							return false;
						}
						bytesRead += receive.Result;
					}
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Receive failed.");
					return false;
				}
			}
			return true;
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
