using System;
using System.Collections.Generic;
using System.Text;

namespace LiteDB.Engine
{
	internal class BufferWriter : IDisposable
	{
		private readonly IEnumerator<BufferSlice> _source;

		private BufferSlice _current;

		private int _currentPosition;

		private int _position;

		private bool _isEOF;

		public int Position => _position;

		public bool IsEOF => _isEOF;

		public BufferWriter(byte[] buffer)
			: this(new BufferSlice(buffer, 0, buffer.Length))
		{
		}

		public BufferWriter(BufferSlice buffer)
		{
			_source = null;
			_current = buffer;
		}

		public BufferWriter(IEnumerable<BufferSlice> source)
		{
			_source = source.GetEnumerator();
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

		public int Write(byte[] buffer, int offset, int count)
		{
			int bufferPosition = 0;
			while (bufferPosition < count)
			{
				int bytesLeft = _current.Count - _currentPosition;
				int bytesToCopy = Math.Min(count - bufferPosition, bytesLeft);
				if (buffer != null)
				{
					Buffer.BlockCopy(buffer, offset + bufferPosition, _current.Array, _current.Offset + _currentPosition, bytesToCopy);
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

		public int Write(byte[] buffer)
		{
			return Write(buffer, 0, buffer.Length);
		}

		public int Skip(int count)
		{
			return Write(null, 0, count);
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

		public void WriteCString(string value)
		{
			if (value.IndexOf('\0') > -1)
			{
				throw LiteException.InvalidNullCharInString();
			}
			int bytesCount = Encoding.UTF8.GetByteCount(value);
			int available = _current.Count - _currentPosition;
			if (bytesCount < available)
			{
				Encoding.UTF8.GetBytes(value, 0, value.Length, _current.Array, _current.Offset + _currentPosition);
				_current[_currentPosition + bytesCount] = 0;
				MoveForward(bytesCount + 1);
				return;
			}
			byte[] buffer = BufferPool.Rent(bytesCount);
			Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
			Write(buffer, 0, bytesCount);
			_current[_currentPosition] = 0;
			MoveForward(1);
			BufferPool.Return(buffer);
		}

		public void WriteString(string value, bool specs)
		{
			int count = Encoding.UTF8.GetByteCount(value);
			if (specs)
			{
				Write(count + 1);
			}
			if (count <= _current.Count - _currentPosition)
			{
				Encoding.UTF8.GetBytes(value, 0, value.Length, _current.Array, _current.Offset + _currentPosition);
				MoveForward(count);
			}
			else
			{
				byte[] buffer = BufferPool.Rent(count);
				Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
				Write(buffer, 0, count);
				BufferPool.Return(buffer);
			}
			if (specs)
			{
				Write((byte)0);
			}
		}

		private void WriteNumber<T>(T value, Action<T, byte[], int> toBytes, int size)
		{
			if (_currentPosition + size <= _current.Count)
			{
				toBytes(value, _current.Array, _current.Offset + _currentPosition);
				MoveForward(size);
				return;
			}
			byte[] buffer = BufferPool.Rent(size);
			toBytes(value, buffer, 0);
			Write(buffer, 0, size);
			BufferPool.Return(buffer);
		}

		public void Write(int value)
		{
			WriteNumber(value, BufferExtensions.ToBytes, 4);
		}

		public void Write(long value)
		{
			WriteNumber(value, BufferExtensions.ToBytes, 8);
		}

		public void Write(uint value)
		{
			WriteNumber(value, BufferExtensions.ToBytes, 4);
		}

		public void Write(double value)
		{
			WriteNumber(value, BufferExtensions.ToBytes, 8);
		}

		public void Write(decimal value)
		{
			int[] bits = decimal.GetBits(value);
			Write(bits[0]);
			Write(bits[1]);
			Write(bits[2]);
			Write(bits[3]);
		}

		public void Write(DateTime value)
		{
			Write(((value == DateTime.MinValue || value == DateTime.MaxValue) ? value : value.ToUniversalTime()).Ticks);
		}

		public void Write(Guid value)
		{
			byte[] bytes = value.ToByteArray();
			Write(bytes, 0, 16);
		}

		public void Write(ObjectId value)
		{
			if (_currentPosition + 12 <= _current.Count)
			{
				value.ToByteArray(_current.Array, _current.Offset + _currentPosition);
				MoveForward(12);
				return;
			}
			byte[] buffer = BufferPool.Rent(12);
			value.ToByteArray(buffer, 0);
			Write(buffer, 0, 12);
			BufferPool.Return(buffer);
		}

		public void Write(bool value)
		{
			_current[_currentPosition] = (value ? ((byte)1) : ((byte)0));
			MoveForward(1);
		}

		public void Write(byte value)
		{
			_current[_currentPosition] = value;
			MoveForward(1);
		}

		internal void Write(PageAddress address)
		{
			Write(address.PageID);
			Write(address.Index);
		}

		public int WriteArray(BsonArray value, bool recalc)
		{
			int bytesCount = value.GetBytesCount(recalc);
			Write(bytesCount);
			for (int i = 0; i < value.Count; i++)
			{
				WriteElement(i.ToString(), value[i]);
			}
			Write((byte)0);
			return bytesCount;
		}

		public int WriteDocument(BsonDocument value, bool recalc)
		{
			int bytesCount = value.GetBytesCount(recalc);
			Write(bytesCount);
			foreach (KeyValuePair<string, BsonValue> el in value.GetElements())
			{
				WriteElement(el.Key, el.Value);
			}
			Write((byte)0);
			return bytesCount;
		}

		private void WriteElement(string key, BsonValue value)
		{
			switch (value.Type)
			{
			case BsonType.Double:
				Write((byte)1);
				WriteCString(key);
				Write(value.AsDouble);
				break;
			case BsonType.String:
				Write((byte)2);
				WriteCString(key);
				WriteString(value.AsString, specs: true);
				break;
			case BsonType.Document:
				Write((byte)3);
				WriteCString(key);
				WriteDocument(value.AsDocument, recalc: false);
				break;
			case BsonType.Array:
				Write((byte)4);
				WriteCString(key);
				WriteArray(value.AsArray, recalc: false);
				break;
			case BsonType.Binary:
			{
				Write((byte)5);
				WriteCString(key);
				byte[] bytes = value.AsBinary;
				Write(bytes.Length);
				Write((byte)0);
				Write(bytes, 0, bytes.Length);
				break;
			}
			case BsonType.Guid:
			{
				Write((byte)5);
				WriteCString(key);
				Guid guid = value.AsGuid;
				Write(16);
				Write((byte)4);
				Write(guid);
				break;
			}
			case BsonType.ObjectId:
				Write((byte)7);
				WriteCString(key);
				Write(value.AsObjectId);
				break;
			case BsonType.Boolean:
				Write((byte)8);
				WriteCString(key);
				Write(value.AsBoolean ? ((byte)1) : ((byte)0));
				break;
			case BsonType.DateTime:
			{
				Write((byte)9);
				WriteCString(key);
				DateTime date = value.AsDateTime;
				Write(Convert.ToInt64((((date == DateTime.MinValue || date == DateTime.MaxValue) ? date : date.ToUniversalTime()) - BsonValue.UnixEpoch).TotalMilliseconds));
				break;
			}
			case BsonType.Null:
				Write((byte)10);
				WriteCString(key);
				break;
			case BsonType.Int32:
				Write((byte)16);
				WriteCString(key);
				Write(value.AsInt32);
				break;
			case BsonType.Int64:
				Write((byte)18);
				WriteCString(key);
				Write(value.AsInt64);
				break;
			case BsonType.Decimal:
				Write((byte)19);
				WriteCString(key);
				Write(value.AsDecimal);
				break;
			case BsonType.MinValue:
				Write(byte.MaxValue);
				WriteCString(key);
				break;
			case BsonType.MaxValue:
				Write((byte)127);
				WriteCString(key);
				break;
			}
		}

		public void Dispose()
		{
			_source?.Dispose();
		}
	}
}
