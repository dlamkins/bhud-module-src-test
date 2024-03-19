using System;
using System.Collections.Generic;
using LiteDB.Engine;

namespace LiteDB
{
	public class BsonSerializer
	{
		public static byte[] Serialize(BsonDocument doc)
		{
			if (doc == null)
			{
				throw new ArgumentNullException("doc");
			}
			byte[] buffer = new byte[doc.GetBytesCount(recalc: true)];
			using BufferWriter writer = new BufferWriter(buffer);
			writer.WriteDocument(doc, recalc: false);
			return buffer;
		}

		public static BsonDocument Deserialize(byte[] buffer, bool utcDate = false, HashSet<string> fields = null)
		{
			if (buffer == null || buffer.Length == 0)
			{
				throw new ArgumentNullException("buffer");
			}
			using BufferReader reader = new BufferReader(buffer, utcDate);
			return reader.ReadDocument(fields);
		}
	}
}
