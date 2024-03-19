using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LiteDB
{
	public class JsonSerializer
	{
		public static string Serialize(BsonValue value)
		{
			StringBuilder sb = new StringBuilder();
			Serialize(value, sb);
			return sb.ToString();
		}

		public static void Serialize(BsonValue value, TextWriter writer)
		{
			new JsonWriter(writer).Serialize(value ?? BsonValue.Null);
		}

		public static void Serialize(BsonValue value, StringBuilder sb)
		{
			using StringWriter writer = new StringWriter(sb);
			new JsonWriter(writer).Serialize(value ?? BsonValue.Null);
		}

		public static BsonValue Deserialize(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			using StringReader sr = new StringReader(json);
			return new JsonReader(sr).Deserialize();
		}

		public static BsonValue Deserialize(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return new JsonReader(reader).Deserialize();
		}

		public static IEnumerable<BsonValue> DeserializeArray(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			return new JsonReader(new StringReader(json)).DeserializeArray();
		}

		public static IEnumerable<BsonValue> DeserializeArray(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return new JsonReader(reader).DeserializeArray();
		}
	}
}
