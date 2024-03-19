using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LiteDB.Engine
{
	internal class SysFileJson : SystemCollection
	{
		public SysFileJson()
			: base("$file_json")
		{
		}

		public override IEnumerable<BsonDocument> Input(BsonValue options)
		{
			string filename = SystemCollection.GetOption(options, "filename")?.AsString ?? throw new LiteException(0, "Collection $" + base.Name + " requires string as 'filename' or a document field 'filename'");
			string encoding = SystemCollection.GetOption(options, "encoding", "utf-8").AsString;
			using FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using StreamReader reader = new StreamReader(fs, Encoding.GetEncoding(encoding));
			IEnumerable<BsonDocument> source = from x in new JsonReader(reader).DeserializeArray()
				select x.AsDocument;
			foreach (BsonDocument item in source)
			{
				yield return item;
			}
		}

		public override int Output(IEnumerable<BsonDocument> source, BsonValue options)
		{
			string filename = SystemCollection.GetOption(options, "filename")?.AsString ?? throw new LiteException(0, "Collection $file_json requires string as filename or a document field 'filename'");
			bool pretty = SystemCollection.GetOption(options, "pretty", false).AsBoolean;
			int indent = SystemCollection.GetOption(options, "indent", 4).AsInt32;
			string encoding = SystemCollection.GetOption(options, "encoding", "utf-8").AsString;
			bool overwritten = SystemCollection.GetOption(options, "overwritten", false).AsBoolean;
			int index = 0;
			FileStream fs = null;
			StreamWriter writer = null;
			JsonWriter json = null;
			try
			{
				foreach (BsonDocument doc in source)
				{
					if (index++ == 0)
					{
						fs = new FileStream(filename, (!overwritten) ? FileMode.CreateNew : FileMode.OpenOrCreate);
						writer = new StreamWriter(fs, Encoding.GetEncoding(encoding));
						json = new JsonWriter(writer)
						{
							Pretty = pretty,
							Indent = indent
						};
						writer.WriteLine("[");
					}
					else
					{
						writer.WriteLine(",");
					}
					json.Serialize(doc);
				}
				if (index > 0)
				{
					writer.WriteLine();
					writer.Write("]");
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
	}
}
