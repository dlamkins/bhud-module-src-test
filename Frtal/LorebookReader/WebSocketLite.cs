using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frtal.LorebookReader
{
	public sealed class WebSocketLite : IDisposable
	{
		public enum FrameType
		{
			Text,
			Binary,
			Closed
		}

		private TcpClient _tcp;

		private SslStream _ssl;

		private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

		public async Task ConnectAsync(string host, string pathAndQuery, IDictionary<string, string> headers, CancellationToken ct)
		{
			_tcp = new TcpClient();
			using (ct.Register(delegate
			{
				try
				{
					_tcp.Close();
				}
				catch
				{
				}
			}))
			{
				await _tcp.ConnectAsync(host, 443).ConfigureAwait(continueOnCapturedContext: false);
			}
			ct.ThrowIfCancellationRequested();
			_ssl = new SslStream(_tcp.GetStream(), leaveInnerStreamOpen: false);
			await _ssl.AuthenticateAsClientAsync(host, null, SslProtocols.Tls12, checkCertificateRevocation: false).ConfigureAwait(continueOnCapturedContext: false);
			byte[] keyBytes = new byte[16];
			_rng.GetBytes(keyBytes);
			string wsKey = Convert.ToBase64String(keyBytes);
			StringBuilder sb = new StringBuilder();
			sb.Append("GET " + pathAndQuery + " HTTP/1.1\r\n");
			sb.Append("Host: " + host + "\r\n");
			sb.Append("Connection: Upgrade\r\n");
			sb.Append("Upgrade: websocket\r\n");
			sb.Append("Sec-WebSocket-Version: 13\r\n");
			sb.Append("Sec-WebSocket-Key: " + wsKey + "\r\n");
			foreach (KeyValuePair<string, string> kv in headers)
			{
				sb.Append(kv.Key + ": " + kv.Value + "\r\n");
			}
			sb.Append("\r\n");
			byte[] req = Encoding.ASCII.GetBytes(sb.ToString());
			await _ssl.WriteAsync(req, 0, req.Length, ct).ConfigureAwait(continueOnCapturedContext: false);
			string statusLine = (await ReadHandshakeResponseAsync(ct).ConfigureAwait(continueOnCapturedContext: false)).Split('\r')[0];
			if (!statusLine.Contains(" 101"))
			{
				throw new IOException("WebSocket handshake rejected: " + statusLine.Trim());
			}
		}

		private async Task<string> ReadHandshakeResponseAsync(CancellationToken ct)
		{
			MemoryStream buf = new MemoryStream();
			byte[] one = new byte[1];
			while (true)
			{
				if (await _ssl.ReadAsync(one, 0, 1, ct).ConfigureAwait(continueOnCapturedContext: false) == 0)
				{
					throw new IOException("Connection closed during handshake.");
				}
				buf.WriteByte(one[0]);
				if (buf.Length >= 4)
				{
					byte[] a = buf.GetBuffer();
					long L = buf.Length;
					if (a[L - 4] == 13 && a[L - 3] == 10 && a[L - 2] == 13 && a[L - 1] == 10)
					{
						break;
					}
				}
				if (buf.Length > 65536)
				{
					throw new IOException("Handshake response too large.");
				}
			}
			return Encoding.ASCII.GetString(buf.ToArray());
		}

		public Task SendTextAsync(string message, CancellationToken ct)
		{
			return SendFrameAsync(1, Encoding.UTF8.GetBytes(message), ct);
		}

		private async Task SendFrameAsync(byte opcode, byte[] payload, CancellationToken ct)
		{
			MemoryStream header = new MemoryStream();
			header.WriteByte((byte)(0x80u | opcode));
			if (payload.Length < 126)
			{
				header.WriteByte((byte)(0x80u | (uint)payload.Length));
			}
			else if (payload.Length <= 65535)
			{
				header.WriteByte(254);
				header.WriteByte((byte)(payload.Length >> 8));
				header.WriteByte((byte)((uint)payload.Length & 0xFFu));
			}
			else
			{
				header.WriteByte(byte.MaxValue);
				ulong len = (ulong)payload.Length;
				for (int j = 7; j >= 0; j--)
				{
					header.WriteByte((byte)(len >> 8 * j));
				}
			}
			byte[] mask = new byte[4];
			_rng.GetBytes(mask);
			header.Write(mask, 0, 4);
			byte[] masked = new byte[payload.Length];
			for (int i = 0; i < payload.Length; i++)
			{
				masked[i] = (byte)(payload[i] ^ mask[i % 4]);
			}
			byte[] head = header.ToArray();
			await _ssl.WriteAsync(head, 0, head.Length, ct).ConfigureAwait(continueOnCapturedContext: false);
			await _ssl.WriteAsync(masked, 0, masked.Length, ct).ConfigureAwait(continueOnCapturedContext: false);
			await _ssl.FlushAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<(FrameType Type, byte[] Data)> ReceiveAsync(CancellationToken ct)
		{
			MemoryStream message = new MemoryStream();
			int messageOpcode = -1;
			int item;
			while (true)
			{
				byte b0 = await ReadByteAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
				byte b1 = await ReadByteAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
				bool fin = (b0 & 0x80) != 0;
				int opcode = b0 & 0xF;
				bool maskedByServer = (b1 & 0x80) != 0;
				long len = b1 & 0x7F;
				switch (len)
				{
				case 126L:
				{
					int j = await ReadByteAsync(ct).ConfigureAwait(continueOnCapturedContext: false) << 8;
					len = j | await ReadByteAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
					break;
				}
				case 127L:
				{
					len = 0L;
					for (int j = 0; j < 8; j++)
					{
						long num = len << 8;
						len = num | await ReadByteAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
					}
					break;
				}
				}
				byte[] maskKey = null;
				if (maskedByServer)
				{
					maskKey = await ReadExactAsync(4, ct).ConfigureAwait(continueOnCapturedContext: false);
				}
				byte[] payload = await ReadExactAsync((int)len, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (maskKey != null)
				{
					for (int i = 0; i < payload.Length; i++)
					{
						payload[i] ^= maskKey[i % 4];
					}
				}
				switch (opcode)
				{
				case 10:
					continue;
				case 8:
					return (FrameType.Closed, payload);
				case 9:
					await SendFrameAsync(10, payload, ct).ConfigureAwait(continueOnCapturedContext: false);
					continue;
				case 1:
				case 2:
					messageOpcode = opcode;
					message.Write(payload, 0, payload.Length);
					break;
				case 0:
					message.Write(payload, 0, payload.Length);
					break;
				}
				if (fin)
				{
					switch (messageOpcode)
					{
					case -1:
						continue;
					default:
						item = 1;
						break;
					case 1:
						item = 0;
						break;
					}
					break;
				}
			}
			return ((FrameType)item, message.ToArray());
		}

		private async Task<byte> ReadByteAsync(CancellationToken ct)
		{
			return (await ReadExactAsync(1, ct).ConfigureAwait(continueOnCapturedContext: false))[0];
		}

		private async Task<byte[]> ReadExactAsync(int count, CancellationToken ct)
		{
			byte[] buf = new byte[count];
			int read = 0;
			using (ct.Register(delegate
			{
				try
				{
					_tcp.Close();
				}
				catch
				{
				}
			}))
			{
				int i;
				for (; read < count; read += i)
				{
					i = await _ssl.ReadAsync(buf, read, count - read, ct).ConfigureAwait(continueOnCapturedContext: false);
					if (i == 0)
					{
						throw new IOException("Connection closed by server.");
					}
				}
			}
			return buf;
		}

		public void Dispose()
		{
			try
			{
				_ssl?.Dispose();
			}
			catch
			{
			}
			try
			{
				_tcp?.Close();
			}
			catch
			{
			}
			_rng.Dispose();
		}
	}
}
