using System;
using System.Text;
using LiteDB.Engine;

namespace LiteDB
{
	internal static class BufferSliceExtensions
	{
		public static bool ReadBool(this BufferSlice buffer, int offset)
		{
			return buffer.Array[buffer.Offset + offset] != 0;
		}

		public static byte ReadByte(this BufferSlice buffer, int offset)
		{
			return buffer.Array[buffer.Offset + offset];
		}

		public static short ReadInt16(this BufferSlice buffer, int offset)
		{
			return BitConverter.ToInt16(buffer.Array, buffer.Offset + offset);
		}

		public static ushort ReadUInt16(this BufferSlice buffer, int offset)
		{
			return BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
		}

		public static int ReadInt32(this BufferSlice buffer, int offset)
		{
			return BitConverter.ToInt32(buffer.Array, buffer.Offset + offset);
		}

		public static uint ReadUInt32(this BufferSlice buffer, int offset)
		{
			return BitConverter.ToUInt32(buffer.Array, buffer.Offset + offset);
		}

		public static long ReadInt64(this BufferSlice buffer, int offset)
		{
			return BitConverter.ToInt64(buffer.Array, buffer.Offset + offset);
		}

		public static double ReadDouble(this BufferSlice buffer, int offset)
		{
			return BitConverter.ToDouble(buffer.Array, buffer.Offset + offset);
		}

		public static decimal ReadDecimal(this BufferSlice buffer, int offset)
		{
			int a = buffer.ReadInt32(offset);
			int b = buffer.ReadInt32(offset + 4);
			int c = buffer.ReadInt32(offset + 8);
			int d = buffer.ReadInt32(offset + 12);
			return new decimal(new int[4] { a, b, c, d });
		}

		public static ObjectId ReadObjectId(this BufferSlice buffer, int offset)
		{
			return new ObjectId(buffer.Array, buffer.Offset + offset);
		}

		public static Guid ReadGuid(this BufferSlice buffer, int offset)
		{
			return new Guid(buffer.ReadBytes(offset, 16));
		}

		public static byte[] ReadBytes(this BufferSlice buffer, int offset, int count)
		{
			byte[] bytes = new byte[count];
			Buffer.BlockCopy(buffer.Array, buffer.Offset + offset, bytes, 0, count);
			return bytes;
		}

		public static DateTime ReadDateTime(this BufferSlice buffer, int offset)
		{
			long ticks = buffer.ReadInt64(offset);
			return ticks switch
			{
				0L => DateTime.MinValue, 
				3155378975999999999L => DateTime.MaxValue, 
				_ => new DateTime(ticks, DateTimeKind.Utc), 
			};
		}

		public static PageAddress ReadPageAddress(this BufferSlice buffer, int offset)
		{
			return new PageAddress(buffer.ReadUInt32(offset), buffer[offset + 4]);
		}

		public static string ReadString(this BufferSlice buffer, int offset, int count)
		{
			return Encoding.UTF8.GetString(buffer.Array, buffer.Offset + offset, count);
		}

		public static BsonValue ReadIndexKey(this BufferSlice buffer, int offset)
		{
			ExtendedLengthHelper.ReadLength(buffer[offset++], buffer[offset], out var type, out var len);
			switch (type)
			{
			case BsonType.Null:
				return BsonValue.Null;
			case BsonType.Int32:
				return buffer.ReadInt32(offset);
			case BsonType.Int64:
				return buffer.ReadInt64(offset);
			case BsonType.Double:
				return buffer.ReadDouble(offset);
			case BsonType.Decimal:
				return buffer.ReadDecimal(offset);
			case BsonType.String:
				offset++;
				return buffer.ReadString(offset, len);
			case BsonType.Document:
			{
				using BufferReader r = new BufferReader(buffer);
				r.Skip(offset);
				return r.ReadDocument();
			}
			case BsonType.Array:
			{
				using BufferReader bufferReader = new BufferReader(buffer);
				bufferReader.Skip(offset);
				return bufferReader.ReadArray();
			}
			case BsonType.Binary:
				offset++;
				return buffer.ReadBytes(offset, len);
			case BsonType.ObjectId:
				return buffer.ReadObjectId(offset);
			case BsonType.Guid:
				return buffer.ReadGuid(offset);
			case BsonType.Boolean:
				return buffer[offset] != 0;
			case BsonType.DateTime:
				return buffer.ReadDateTime(offset);
			case BsonType.MinValue:
				return BsonValue.MinValue;
			case BsonType.MaxValue:
				return BsonValue.MaxValue;
			default:
				throw new NotImplementedException();
			}
		}

		public static void Write(this BufferSlice buffer, bool value, int offset)
		{
			buffer.Array[buffer.Offset + offset] = (value ? ((byte)1) : ((byte)0));
		}

		public static void Write(this BufferSlice buffer, byte value, int offset)
		{
			buffer.Array[buffer.Offset + offset] = value;
		}

		public static void Write(this BufferSlice buffer, short value, int offset)
		{
			value.ToBytes(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, ushort value, int offset)
		{
			value.ToBytes(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, int value, int offset)
		{
			value.ToBytes(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, uint value, int offset)
		{
			value.ToBytes(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, long value, int offset)
		{
			value.ToBytes(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, double value, int offset)
		{
			value.ToBytes(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, decimal value, int offset)
		{
			int[] bits = decimal.GetBits(value);
			buffer.Write(bits[0], offset);
			buffer.Write(bits[1], offset + 4);
			buffer.Write(bits[2], offset + 8);
			buffer.Write(bits[3], offset + 12);
		}

		public static void Write(this BufferSlice buffer, DateTime value, int offset)
		{
			value.ToUniversalTime().Ticks.ToBytes(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, PageAddress value, int offset)
		{
			value.PageID.ToBytes(buffer.Array, buffer.Offset + offset);
			buffer[offset + 4] = value.Index;
		}

		public static void Write(this BufferSlice buffer, Guid value, int offset)
		{
			buffer.Write(value.ToByteArray(), offset);
		}

		public static void Write(this BufferSlice buffer, ObjectId value, int offset)
		{
			value.ToByteArray(buffer.Array, buffer.Offset + offset);
		}

		public static void Write(this BufferSlice buffer, byte[] value, int offset)
		{
			Buffer.BlockCopy(value, 0, buffer.Array, buffer.Offset + offset, value.Length);
		}

		public static void Write(this BufferSlice buffer, string value, int offset)
		{
			Encoding.UTF8.GetBytes(value, 0, value.Length, buffer.Array, buffer.Offset + offset);
		}

		public static void WriteIndexKey(this BufferSlice buffer, BsonValue value, int offset)
		{
			if (value.IsString)
			{
				string str = value.AsString;
				ushort strLength = (ushort)Encoding.UTF8.GetByteCount(str);
				ExtendedLengthHelper.WriteLength(BsonType.String, strLength, out var typeByte2, out var lengthByte2);
				buffer[offset++] = typeByte2;
				buffer[offset++] = lengthByte2;
				buffer.Write(str, offset);
				return;
			}
			if (value.IsBinary)
			{
				byte[] arr = value.AsBinary;
				ExtendedLengthHelper.WriteLength(BsonType.Binary, (ushort)arr.Length, out var typeByte, out var lengthByte);
				buffer[offset++] = typeByte;
				buffer[offset++] = lengthByte;
				buffer.Write(arr, offset);
				return;
			}
			buffer[offset++] = (byte)value.Type;
			switch (value.Type)
			{
			case BsonType.Int32:
				buffer.Write(value.AsInt32, offset);
				break;
			case BsonType.Int64:
				buffer.Write(value.AsInt64, offset);
				break;
			case BsonType.Double:
				buffer.Write(value.AsDouble, offset);
				break;
			case BsonType.Decimal:
				buffer.Write(value.AsDecimal, offset);
				break;
			case BsonType.Document:
			{
				using (BufferWriter w = new BufferWriter(buffer))
				{
					w.Skip(offset);
					w.WriteDocument(value.AsDocument, recalc: true);
				}
				break;
			}
			case BsonType.Array:
			{
				using (BufferWriter bufferWriter = new BufferWriter(buffer))
				{
					bufferWriter.Skip(offset);
					bufferWriter.WriteArray(value.AsArray, recalc: true);
				}
				break;
			}
			case BsonType.ObjectId:
				buffer.Write(value.AsObjectId, offset);
				break;
			case BsonType.Guid:
				buffer.Write(value.AsGuid, offset);
				break;
			case BsonType.Boolean:
				buffer[offset] = (value.AsBoolean ? ((byte)1) : ((byte)0));
				break;
			case BsonType.DateTime:
				buffer.Write(value.AsDateTime, offset);
				break;
			default:
				throw new NotImplementedException();
			case BsonType.MinValue:
			case BsonType.Null:
			case BsonType.MaxValue:
				break;
			}
		}
	}
}
