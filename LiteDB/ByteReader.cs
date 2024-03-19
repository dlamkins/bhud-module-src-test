using System;
using System.Text;

namespace LiteDB
{
	internal class ByteReader
	{
		private readonly byte[] _buffer;

		private readonly int _length;

		private int _pos;

		public int Position
		{
			get
			{
				return _pos;
			}
			set
			{
				_pos = value;
			}
		}

		public ByteReader(byte[] buffer)
		{
			_buffer = buffer;
			_length = buffer.Length;
			_pos = 0;
		}

		public void Skip(int length)
		{
			_pos += length;
		}

		public byte ReadByte()
		{
			byte result = _buffer[_pos];
			_pos++;
			return result;
		}

		public bool ReadBoolean()
		{
			byte num = _buffer[_pos];
			_pos++;
			return num != 0;
		}

		public ushort ReadUInt16()
		{
			_pos += 2;
			return BitConverter.ToUInt16(_buffer, _pos - 2);
		}

		public uint ReadUInt32()
		{
			_pos += 4;
			return BitConverter.ToUInt32(_buffer, _pos - 4);
		}

		public ulong ReadUInt64()
		{
			_pos += 8;
			return BitConverter.ToUInt64(_buffer, _pos - 8);
		}

		public short ReadInt16()
		{
			_pos += 2;
			return BitConverter.ToInt16(_buffer, _pos - 2);
		}

		public int ReadInt32()
		{
			_pos += 4;
			return BitConverter.ToInt32(_buffer, _pos - 4);
		}

		public long ReadInt64()
		{
			_pos += 8;
			return BitConverter.ToInt64(_buffer, _pos - 8);
		}

		public float ReadSingle()
		{
			_pos += 4;
			return BitConverter.ToSingle(_buffer, _pos - 4);
		}

		public double ReadDouble()
		{
			_pos += 8;
			return BitConverter.ToDouble(_buffer, _pos - 8);
		}

		public decimal ReadDecimal()
		{
			_pos += 16;
			int a = BitConverter.ToInt32(_buffer, _pos - 16);
			int b = BitConverter.ToInt32(_buffer, _pos - 12);
			int c = BitConverter.ToInt32(_buffer, _pos - 8);
			int d = BitConverter.ToInt32(_buffer, _pos - 4);
			return new decimal(new int[4] { a, b, c, d });
		}

		public byte[] ReadBytes(int count)
		{
			byte[] buffer = new byte[count];
			Buffer.BlockCopy(_buffer, _pos, buffer, 0, count);
			_pos += count;
			return buffer;
		}

		public string ReadString()
		{
			int length = ReadInt32();
			string @string = Encoding.UTF8.GetString(_buffer, _pos, length);
			_pos += length;
			return @string;
		}

		public string ReadString(int length)
		{
			string @string = Encoding.UTF8.GetString(_buffer, _pos, length);
			_pos += length;
			return @string;
		}

		public string ReadBsonString()
		{
			int length = ReadInt32();
			string @string = Encoding.UTF8.GetString(_buffer, _pos, length - 1);
			_pos += length;
			return @string;
		}

		public string ReadCString()
		{
			int pos = _pos;
			int length = 0;
			while (true)
			{
				if (_buffer[pos] == 0)
				{
					string @string = Encoding.UTF8.GetString(_buffer, _pos, length);
					_pos += length + 1;
					return @string;
				}
				if (pos > _length)
				{
					break;
				}
				pos++;
				length++;
			}
			return "_";
		}

		public DateTime ReadDateTime()
		{
			return new DateTime(ReadInt64(), DateTimeKind.Utc).ToLocalTime();
		}

		public Guid ReadGuid()
		{
			return new Guid(ReadBytes(16));
		}

		public ObjectId ReadObjectId()
		{
			return new ObjectId(ReadBytes(12));
		}

		public BsonValue ReadBsonValue(ushort length)
		{
			return ReadByte() switch
			{
				1 => BsonValue.Null, 
				2 => ReadInt32(), 
				3 => ReadInt64(), 
				4 => ReadDouble(), 
				5 => ReadDecimal(), 
				6 => ReadString(length), 
				7 => new BsonReader(utcDate: false).ReadDocument(this), 
				8 => new BsonReader(utcDate: false).ReadArray(this), 
				9 => ReadBytes(length), 
				10 => ReadObjectId(), 
				11 => ReadGuid(), 
				12 => ReadBoolean(), 
				13 => ReadDateTime(), 
				0 => BsonValue.MinValue, 
				14 => BsonValue.MaxValue, 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}
