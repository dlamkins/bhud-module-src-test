using System;
using System.Collections.Generic;

namespace Ideka.CustomCombatText.Bridge
{
	public class BridgeBuffer
	{
		private const int MessageHeaderSize = 8;

		private const int BufferSize = 8192;

		private int _totalBuffered;

		private int? _messageSize;

		private readonly byte[] _data = new byte[8192];

		public ArraySegment<byte> NextSegment
		{
			get
			{
				int freeSpace = _data.Length - _totalBuffered;
				if (freeSpace <= 0)
				{
					throw new Exception("Bridge buffer filled up.");
				}
				return new ArraySegment<byte>(_data, _totalBuffered, freeSpace);
			}
		}

		public IEnumerable<byte[]> ProcessReceivedData(int bytesRead)
		{
			_totalBuffered += bytesRead;
			if (_totalBuffered > _data.Length)
			{
				throw new ArgumentOutOfRangeException("bytesRead", "Apparent bridge buffer overflow.");
			}
			while (_totalBuffered > 0)
			{
				if (!_messageSize.HasValue && tryRead(8, out var headerData))
				{
					int header = BitConverter.ToInt32(headerData, 0);
					if (header <= 0)
					{
						throw new Exception("Received message with negative or zero size.");
					}
					_messageSize = header;
					continue;
				}
				int? messageSize2 = _messageSize;
				if (messageSize2.HasValue)
				{
					int messageSize = messageSize2.GetValueOrDefault();
					if (tryRead(messageSize, out var messageData))
					{
						_messageSize = null;
						yield return messageData;
						continue;
					}
					break;
				}
				break;
			}
			bool tryRead(int size, out byte[] output)
			{
				output = new byte[size];
				if (_totalBuffered < size)
				{
					return false;
				}
				Buffer.BlockCopy(_data, 0, output, 0, size);
				Buffer.BlockCopy(_data, size, _data, 0, _totalBuffered - size);
				_totalBuffered -= size;
				return true;
			}
		}
	}
}
