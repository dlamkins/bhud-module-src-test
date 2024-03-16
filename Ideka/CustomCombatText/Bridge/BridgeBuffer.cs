using System;
using System.Collections.Generic;

namespace Ideka.CustomCombatText.Bridge
{
	public class BridgeBuffer
	{
		private const int BufferSize = 8192;

		private const int HeaderBufferSize = 8;

		private int _bufferedStart;

		private int _bufferedTotal;

		private int? _messageSize;

		private readonly byte[] _data = new byte[8192];

		private int FreeStart => _bufferedStart + _bufferedTotal;

		public ArraySegment<byte> NextSegment
		{
			get
			{
				int freeSpace = _data.Length - FreeStart;
				if (freeSpace <= 0)
				{
					throw new Exception("Bridge buffer filled up.");
				}
				return new ArraySegment<byte>(_data, FreeStart, freeSpace);
			}
		}

		public IEnumerable<ArraySegment<byte>> ProcessReceivedData(int bytesRead)
		{
			_bufferedTotal += bytesRead;
			if (FreeStart > _data.Length)
			{
				throw new ArgumentOutOfRangeException("bytesRead", "Apparent overflow.");
			}
			while (_bufferedTotal > 0)
			{
				int? num;
				if (!_messageSize.HasValue)
				{
					num = tryRead(8);
					if (num.HasValue)
					{
						int headerOffset = num.GetValueOrDefault();
						int header = BitConverter.ToInt32(_data, headerOffset);
						if (header <= 0)
						{
							throw new Exception("Received message with negative or zero size.");
						}
						_messageSize = header;
						continue;
					}
				}
				num = _messageSize;
				if (!num.HasValue)
				{
					break;
				}
				int messageSize = num.GetValueOrDefault();
				num = tryRead(messageSize);
				if (!num.HasValue)
				{
					break;
				}
				int messageOffset = num.GetValueOrDefault();
				_messageSize = null;
				yield return new ArraySegment<byte>(_data, messageOffset, messageSize);
			}
			Buffer.BlockCopy(_data, _bufferedStart, _data, 0, _bufferedTotal);
			_bufferedStart = 0;
			int? tryRead(int size)
			{
				if (_bufferedTotal < size)
				{
					return null;
				}
				int bufferedStart = _bufferedStart;
				_bufferedStart += size;
				_bufferedTotal -= size;
				return bufferedStart;
			}
		}
	}
}
