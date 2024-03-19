using System;

namespace LiteDB
{
	internal class BsonReader
	{
		private readonly bool _utcDate;

		public BsonReader(bool utcDate)
		{
			_utcDate = utcDate;
		}

		public BsonDocument Deserialize(byte[] bson)
		{
			return ReadDocument(new ByteReader(bson));
		}

		public BsonDocument ReadDocument(ByteReader reader)
		{
			int length = reader.ReadInt32();
			int end = reader.Position + length - 5;
			BsonDocument obj = new BsonDocument();
			while (reader.Position < end)
			{
				string name;
				BsonValue value = ReadElement(reader, out name);
				obj.RawValue[name] = value;
			}
			reader.ReadByte();
			return obj;
		}

		public BsonArray ReadArray(ByteReader reader)
		{
			int length = reader.ReadInt32();
			int end = reader.Position + length - 5;
			BsonArray arr = new BsonArray();
			while (reader.Position < end)
			{
				string name;
				BsonValue value = ReadElement(reader, out name);
				arr.Add(value);
			}
			reader.ReadByte();
			return arr;
		}

		private BsonValue ReadElement(ByteReader reader, out string name)
		{
			byte type = reader.ReadByte();
			name = reader.ReadCString();
			switch (type)
			{
			case 1:
				return reader.ReadDouble();
			case 2:
				return reader.ReadBsonString();
			case 3:
				return ReadDocument(reader);
			case 4:
				return ReadArray(reader);
			case 5:
			{
				int length = reader.ReadInt32();
				byte subType = reader.ReadByte();
				byte[] bytes = reader.ReadBytes(length);
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
				return new ObjectId(reader.ReadBytes(12));
			case 8:
				return reader.ReadBoolean();
			case 9:
			{
				long ts = reader.ReadInt64();
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
				return reader.ReadInt32();
			case 18:
				return reader.ReadInt64();
			case 19:
				return reader.ReadDecimal();
			case byte.MaxValue:
				return BsonValue.MinValue;
			case 127:
				return BsonValue.MaxValue;
			}
			throw new NotSupportedException("BSON type not supported");
		}
	}
}
