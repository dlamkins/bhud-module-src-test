using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace LiteDB.Engine
{
	internal class SysFileCsv : SystemCollection
	{
		private static readonly IFormatProvider _numberFormat = CultureInfo.InvariantCulture.NumberFormat;

		public SysFileCsv()
			: base("$file_csv")
		{
		}

		public override IEnumerable<BsonDocument> Input(BsonValue options)
		{
			string filename = SystemCollection.GetOption(options, "filename")?.AsString ?? throw new LiteException(0, "Collection $" + base.Name + " requires string as 'filename' or a document field 'filename'");
			string encoding = SystemCollection.GetOption(options, "encoding", "utf-8").AsString;
			char delimiter = SystemCollection.GetOption(options, "delimiter", ",").AsString[0];
			List<string> header = new List<string>();
			if (options.IsDocument && options["header"].IsArray)
			{
				header.AddRange(options["header"].AsArray.Select((BsonValue x) => x.AsString));
			}
			using FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using StreamReader reader = new StreamReader(fs, Encoding.GetEncoding(encoding));
			if (header.Count == 0)
			{
				bool newLine2;
				do
				{
					string key2 = ReadString(reader, delimiter, out newLine2);
					if (key2 == null)
					{
						break;
					}
					header.Add(key2);
				}
				while (!newLine2);
			}
			int index = 0;
			BsonDocument doc = new BsonDocument();
			while (true)
			{
				bool newLine;
				string value = ReadString(reader, delimiter, out newLine);
				if (value != null)
				{
					if (index < header.Count)
					{
						string key = header[index++];
						doc[key] = value;
					}
					if (newLine)
					{
						yield return doc;
						doc = new BsonDocument();
						index = 0;
					}
					continue;
				}
				break;
			}
		}

		public override int Output(IEnumerable<BsonDocument> source, BsonValue options)
		{
			string filename = SystemCollection.GetOption(options, "filename")?.AsString ?? throw new LiteException(0, "Collection $file_json requires string as 'filename' or a document field 'filename'");
			bool overwritten = SystemCollection.GetOption(options, "overwritten", false).AsBoolean;
			string encoding = SystemCollection.GetOption(options, "encoding", "utf-8").AsString;
			char delimiter = SystemCollection.GetOption(options, "delimiter", ",").AsString[0];
			bool header = SystemCollection.GetOption(options, "header", true).AsBoolean;
			int index = 0;
			IList<string> headerFields = null;
			FileStream fs = null;
			StreamWriter writer = null;
			try
			{
				foreach (BsonDocument doc in source)
				{
					if (index++ == 0)
					{
						fs = new FileStream(filename, (!overwritten) ? FileMode.CreateNew : FileMode.OpenOrCreate);
						writer = new StreamWriter(fs, Encoding.GetEncoding(encoding));
						headerFields = doc.Keys.ToList();
						if (header)
						{
							int idxHeader = 0;
							foreach (KeyValuePair<string, BsonValue> elem in doc)
							{
								if (idxHeader++ > 0)
								{
									writer.Write(delimiter);
								}
								writer.Write(elem.Key);
							}
							writer.WriteLine();
						}
					}
					else
					{
						writer.WriteLine();
					}
					int idxValue = 0;
					foreach (string field in headerFields)
					{
						BsonValue value = doc[field];
						if (idxValue++ > 0)
						{
							writer.Write(delimiter);
						}
						WriteValue(value, writer);
					}
				}
				if (index > 0)
				{
					writer.Flush();
					return index;
				}
				return index;
			}
			finally
			{
				writer?.Dispose();
				fs?.Dispose();
			}
		}

		private void WriteValue(BsonValue value, StreamWriter writer)
		{
			switch (value.Type)
			{
			case BsonType.MinValue:
			case BsonType.Null:
			case BsonType.Document:
			case BsonType.Array:
			case BsonType.MaxValue:
				writer.Write("null");
				break;
			case BsonType.Int64:
				writer.Write(value.AsInt64.ToString(CultureInfo.InvariantCulture.NumberFormat));
				break;
			case BsonType.Decimal:
				writer.Write(value.AsDecimal.ToString(CultureInfo.InvariantCulture.NumberFormat));
				break;
			case BsonType.Binary:
				writer.Write("\"" + Convert.ToBase64String(value.AsBinary) + "\"");
				break;
			case BsonType.ObjectId:
				writer.Write("\"" + value.AsObjectId.ToString() + "\"");
				break;
			case BsonType.Guid:
				writer.Write("\"" + value.AsGuid.ToString() + "\"");
				break;
			case BsonType.DateTime:
				writer.Write("\"" + value.AsDateTime.ToUniversalTime().ToString("o") + "\"");
				break;
			default:
				writer.Write(value.ToString());
				break;
			}
		}

		private string ReadString(TextReader reader, char delimiter, out bool newLine)
		{
			StringBuilder sb = new StringBuilder();
			int c = reader.Read();
			while (c == 10 || c == 13)
			{
				c = reader.Read();
			}
			if (c == -1)
			{
				newLine = true;
				return null;
			}
			if (c == 34)
			{
				while (c != -1)
				{
					c = reader.Read();
					if (c == 34)
					{
						int next = reader.Read();
						if (next != 34)
						{
							c = next;
							break;
						}
						sb.Append('"');
					}
					else
					{
						sb.Append((char)c);
					}
				}
			}
			else
			{
				while (c != 10 && c != 13 && c != delimiter && c != -1)
				{
					sb.Append((char)c);
					c = reader.Read();
				}
			}
			newLine = c == 10 || c == 13;
			return sb.ToString();
		}
	}
}
