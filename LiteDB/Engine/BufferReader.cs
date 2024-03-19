using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LiteDB.Engine
{
	internal class BufferReader : IDisposable
	{
		private readonly IEnumerator<BufferSlice> _source;

		private readonly bool _utcDate;

		private BufferSlice _current;

		private int _currentPosition;

		private int _position;

		private bool _isEOF;

		public int Position => _position;

		public bool IsEOF => _isEOF;

		public BufferReader(byte[] buffer, bool utcDate = false)
			: this(new BufferSlice(buffer, 0, buffer.Length), utcDate)
		{
		}

		public BufferReader(BufferSlice buffer, bool utcDate = false)
		{
			_source = null;
			_utcDate = utcDate;
			_current = buffer;
		}

		public BufferReader(IEnumerable<BufferSlice> source, bool utcDate = false)
		{
			_source = source.GetEnumerator();
			_utcDate = utcDate;
			_source.MoveNext();
			_current = _source.Current;
		}

		private bool MoveForward(int count)
		{
			if (_isEOF)
			{
				return false;
			}
			Constants.ENSURE(_currentPosition + count <= _current.Count, "forward is only for current segment");
			_currentPosition += count;
			_position += count;
			if (_currentPosition == _current.Count)
			{
				if (_source == null || !_source.MoveNext())
				{
					_isEOF = true;
				}
				else
				{
					_current = _source.Current;
					_currentPosition = 0;
				}
				return true;
			}
			return false;
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			int bufferPosition = 0;
			while (bufferPosition < count)
			{
				int bytesLeft = _current.Count - _currentPosition;
				int bytesToCopy = Math.Min(count - bufferPosition, bytesLeft);
				if (buffer != null)
				{
					Buffer.BlockCopy(_current.Array, _current.Offset + _currentPosition, buffer, offset + bufferPosition, bytesToCopy);
				}
				bufferPosition += bytesToCopy;
				MoveForward(bytesToCopy);
				if (_isEOF)
				{
					break;
				}
			}
			Constants.ENSURE(count == bufferPosition, "current value must fit inside defined buffer");
			return bufferPosition;
		}

		public int Skip(int count)
		{
			return Read(null, 0, count);
		}

		public void Consume()
		{
			if (_source != null)
			{
				while (_source.MoveNext())
				{
				}
			}
		}

		public string ReadString(int count)
		{
			string value;
			if (_currentPosition + count <= _current.Count)
			{
				value = Encoding.UTF8.GetString(_current.Array, _current.Offset + _currentPosition, count);
				MoveForward(count);
			}
			else
			{
				byte[] buffer = BufferPool.Rent(count);
				Read(buffer, 0, count);
				value = Encoding.UTF8.GetString(buffer, 0, count);
				BufferPool.Return(buffer);
			}
			return value;
		}

		public string ReadCString()
		{
			if (TryReadCStringCurrentSegment(out var value))
			{
				return value;
			}
			using MemoryStream mem = new MemoryStream();
			int initialCount = _current.Count - _currentPosition;
			mem.Write(_current.Array, _current.Offset + _currentPosition, initialCount);
			MoveForward(initialCount);
			while (_current[_currentPosition] != 0 && !_isEOF)
			{
				mem.WriteByte(_current[_currentPosition]);
				MoveForward(1);
			}
			MoveForward(1);
			return Encoding.UTF8.GetString(mem.ToArray());
		}

		private bool TryReadCStringCurrentSegment(out string value)
		{
			int pos = _currentPosition;
			int count = 0;
			for (; pos < _current.Count; pos++)
			{
				if (_current[pos] == 0)
				{
					value = Encoding.UTF8.GetString(_current.Array, _current.Offset + _currentPosition, count);
					MoveForward(count + 1);
					return true;
				}
				count++;
			}
			value = null;
			return false;
		}

		private T ReadNumber<T>(Func<byte[], int, T> convert, int size)
		{
			T value;
			if (_currentPosition + size <= _current.Count)
			{
				value = convert(_current.Array, _current.Offset + _currentPosition);
				MoveForward(size);
			}
			else
			{
				byte[] buffer = BufferPool.Rent(size);
				Read(buffer, 0, size);
				value = convert(buffer, 0);
				BufferPool.Return(buffer);
			}
			return value;
		}

		public int ReadInt32()
		{
			return ReadNumber(BitConverter.ToInt32, 4);
		}

		public long ReadInt64()
		{
			return ReadNumber(BitConverter.ToInt64, 8);
		}

		public uint ReadUInt32()
		{
			return ReadNumber(BitConverter.ToUInt32, 4);
		}

		public double ReadDouble()
		{
			return ReadNumber(BitConverter.ToDouble, 8);
		}

		public decimal ReadDecimal()
		{
			int a = ReadInt32();
			int b = ReadInt32();
			int c = ReadInt32();
			int d = ReadInt32();
			return new decimal(new int[4] { a, b, c, d });
		}

		public DateTime ReadDateTime()
		{
			DateTime date = new DateTime(ReadInt64(), DateTimeKind.Utc);
			if (!_utcDate)
			{
				return date;
			}
			return date.ToLocalTime();
		}

		public Guid ReadGuid()
		{
			Guid value;
			if (_currentPosition + 16 <= _current.Count)
			{
				value = _current.ReadGuid(_currentPosition);
				MoveForward(16);
			}
			else
			{
				value = new Guid(ReadBytes(16));
			}
			return value;
		}

		public ObjectId ReadObjectId()
		{
			ObjectId value;
			if (_currentPosition + 12 <= _current.Count)
			{
				value = new ObjectId(_current.Array, _current.Offset + _currentPosition);
				MoveForward(12);
			}
			else
			{
				byte[] buffer = BufferPool.Rent(12);
				Read(buffer, 0, 12);
				value = new ObjectId(buffer);
				BufferPool.Return(buffer);
			}
			return value;
		}

		public bool ReadBoolean()
		{
			bool result = _current[_currentPosition] != 0;
			MoveForward(1);
			return result;
		}

		public byte ReadByte()
		{
			byte result = _current[_currentPosition];
			MoveForward(1);
			return result;
		}

		internal PageAddress ReadPageAddress()
		{
			return new PageAddress(ReadUInt32(), ReadByte());
		}

		public byte[] ReadBytes(int count)
		{
			byte[] buffer = new byte[count];
			Read(buffer, 0, count);
			return buffer;
		}

		public BsonValue ReadIndexKey()
		{
			return ReadByte() switch
			{
				1 => BsonValue.Null, 
				2 => ReadInt32(), 
				3 => ReadInt64(), 
				4 => ReadDouble(), 
				5 => ReadDecimal(), 
				6 => ReadString(ReadByte()), 
				7 => ReadDocument(), 
				8 => ReadArray(), 
				9 => ReadBytes(ReadByte()), 
				10 => ReadObjectId(), 
				11 => ReadGuid(), 
				12 => ReadBoolean(), 
				13 => ReadDateTime(), 
				0 => BsonValue.MinValue, 
				14 => BsonValue.MaxValue, 
				_ => throw new NotImplementedException(), 
			};
		}

		public BsonDocument ReadDocument(HashSet<string> fields = null)
		{
			int length = ReadInt32();
			int end = _position + length - 5;
			HashSet<string> remaining = ((fields == null || fields.Count == 0) ? null : new HashSet<string>(fields, StringComparer.OrdinalIgnoreCase));
			BsonDocument doc = new BsonDocument();
			while (_position < end && (remaining == null || (remaining != null && remaining.Count > 0)))
			{
				string name;
				BsonValue value = ReadElement(remaining, out name);
				if (value != null)
				{
					doc[name] = value;
					remaining?.Remove(name);
				}
			}
			MoveForward(1);
			return doc;
		}

		public BsonArray ReadArray()
		{
			int length = ReadInt32();
			int end = _position + length - 5;
			BsonArray arr = new BsonArray();
			while (_position < end)
			{
				string name;
				BsonValue value = ReadElement(null, out name);
				arr.Add(value);
			}
			MoveForward(1);
			return arr;
		}

		private BsonValue ReadElement(HashSet<string> remaining, out string name)
		{
			byte type = ReadByte();
			name = ReadCString();
			if (remaining != null && !remaining.Contains(name))
			{
				int num;
				switch (type)
				{
				default:
					num = 0;
					break;
				case 3:
				case 4:
					num = ReadInt32() - 4;
					break;
				case 5:
					num = ReadInt32() + 1;
					break;
				case 2:
					num = ReadInt32();
					break;
				case 19:
					num = 16;
					break;
				case 7:
					num = 12;
					break;
				case 1:
				case 9:
				case 18:
					num = 8;
					break;
				case 16:
					num = 4;
					break;
				case 8:
					num = 1;
					break;
				case 10:
				case 127:
				case byte.MaxValue:
					num = 0;
					break;
				}
				int length3 = num;
				if (length3 > 0)
				{
					Skip(length3);
				}
				return null;
			}
			switch (type)
			{
			case 1:
				return ReadDouble();
			case 2:
			{
				int length2 = ReadInt32();
				string text = ReadString(length2 - 1);
				MoveForward(1);
				return text;
			}
			case 3:
				return ReadDocument();
			case 4:
				return ReadArray();
			case 5:
			{
				int length = ReadInt32();
				byte subType = ReadByte();
				byte[] bytes = ReadBytes(length);
				switch (subType)
				{
				case 0:
					return bytes;
				case 4:
					return new Guid(bytes);
				}
				break;
			}
			case 7:
				return ReadObjectId();
			case 8:
				return ReadBoolean();
			case 9:
			{
				long ts = ReadInt64();
				switch (ts)
				{
				case 253402300800000L:
					return DateTime.MaxValue;
				case -62135596800000L:
					return DateTime.MinValue;
				default:
				{
					DateTime date = BsonValue.UnixEpoch.AddMilliseconds(ts);
					return _utcDate ? date : date.ToLocalTime();
				}
				}
			}
			case 10:
				return BsonValue.Null;
			case 16:
				return ReadInt32();
			case 18:
				return ReadInt64();
			case 19:
				return ReadDecimal();
			case byte.MaxValue:
				return BsonValue.MinValue;
			case 127:
				return BsonValue.MaxValue;
			}
			throw new NotSupportedException("BSON type not supported");
		}

		public void Dispose()
		{
			_source?.Dispose();
		}
	}
}
