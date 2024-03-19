using System;

namespace LiteDB
{
	internal static class ExtendedLengthHelper
	{
		public static void ReadLength(byte typeByte, byte lengthByte, out BsonType type, out ushort length)
		{
			byte bsonType = (byte)(typeByte & 0x3Fu);
			byte lengthLSByte = lengthByte;
			byte lengthMSByte = (byte)(typeByte & 0xC0u);
			type = (BsonType)bsonType;
			length = (ushort)((lengthMSByte << 2) | lengthLSByte);
		}

		public static void WriteLength(BsonType type, ushort length, out byte typeByte, out byte lengthByte)
		{
			if (length > 1023)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			byte bsonType = (byte)type;
			byte lengthLSByte = (byte)length;
			byte lengthMSByte = (byte)((length & 0x300) >> 2);
			typeByte = (byte)(lengthMSByte | bsonType);
			lengthByte = lengthLSByte;
		}
	}
}
