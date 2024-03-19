using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiteDB.Engine
{
	internal class FileReaderV7 : IFileReader
	{
		private const int V7_PAGE_SIZE = 4096;

		private readonly Stream _stream;

		private readonly AesEncryption _aes;

		private readonly BsonDocument _header;

		private byte[] _buffer = new byte[4096];

		public int UserVersion => _header["userVersion"];

		public FileReaderV7(Stream stream, string password)
		{
			_stream = stream;
			_header = ReadPage(0u);
			if (password == null && !_header["salt"].AsBinary.IsFullZero())
			{
				throw LiteException.InvalidPassword();
			}
			if (password != null)
			{
				if (_header["salt"].AsBinary.IsFullZero())
				{
					throw LiteException.FileNotEncrypted();
				}
				if (!AesEncryption.HashSHA1(password).SequenceEqual(_header["password"].AsBinary))
				{
					throw LiteException.InvalidPassword();
				}
			}
			_aes = ((password == null) ? null : new AesEncryption(password, _header["salt"].AsBinary));
		}

		public IEnumerable<string> GetCollections()
		{
			return _header["collections"].AsDocument.Keys;
		}

		public IEnumerable<IndexInfo> GetIndexes(string collection)
		{
			uint pageID = (uint)_header["collections"].AsDocument[collection].AsInt32;
			BsonDocument page = ReadPage(pageID);
			foreach (BsonValue index in page["indexes"].AsArray)
			{
				string name = Regex.Replace(index["name"].AsString, "[^a-z0-9]", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				if (name.Length > Constants.INDEX_NAME_MAX_LENGTH)
				{
					name = name.Substring(0, Constants.INDEX_NAME_MAX_LENGTH);
				}
				yield return new IndexInfo
				{
					Collection = collection,
					Name = name,
					Expression = index["expression"].AsString,
					Unique = index["unique"].AsBoolean
				};
			}
		}

		public IEnumerable<BsonDocument> GetDocuments(string collection)
		{
			uint colPageID = (uint)_header["collections"].AsDocument[collection].AsInt32;
			uint headPageID = (uint)ReadPage(colPageID)["indexes"][0]["headPageID"].AsInt32;
			HashSet<uint> indexPages = VisitIndexPages(headPageID);
			foreach (uint indexPageID in indexPages)
			{
				BsonDocument indexPage = ReadPage(indexPageID);
				foreach (BsonValue node in indexPage["nodes"].AsArray)
				{
					BsonValue dataBlock = node["dataBlock"];
					if (dataBlock["pageID"].AsInt32 == -1)
					{
						continue;
					}
					BsonDocument dataPage = ReadPage((uint)dataBlock["pageID"].AsInt32);
					if (dataPage["pageType"].AsInt32 != 4)
					{
						continue;
					}
					BsonDocument block = dataPage["blocks"].AsArray.FirstOrDefault((BsonValue x) => x["index"] == dataBlock["index"])?.AsDocument;
					if (block == null)
					{
						continue;
					}
					byte[] data = ((block["extendPageID"] == -1) ? block["data"].AsBinary : ReadExtendData((uint)block["extendPageID"].AsInt32));
					if (data.Length == 0)
					{
						continue;
					}
					BsonDocument doc = BsonSerializer.Deserialize(data);
					if (collection == "_chunks")
					{
						string[] parts = doc["_id"].AsString.Split('\\');
						if (!int.TryParse(parts[1], out var i))
						{
							throw LiteException.InvalidFormat("_id");
						}
						doc["_id"] = new BsonDocument
						{
							["f"] = parts[0],
							["n"] = i
						};
					}
					yield return doc;
				}
			}
		}

		private BsonDocument ReadPage(uint pageID)
		{
			if (pageID * 4096 > _stream.Length)
			{
				return null;
			}
			_stream.Position = pageID * 4096;
			_stream.Read(_buffer, 0, 4096);
			if (_aes != null && pageID != 0)
			{
				_buffer = _aes.Decrypt(_buffer);
			}
			ByteReader reader = new ByteReader(_buffer);
			BsonDocument page = new BsonDocument
			{
				["pageID"] = (int)reader.ReadUInt32(),
				["pageType"] = reader.ReadByte(),
				["prevPageID"] = (int)reader.ReadUInt32(),
				["nextPageID"] = (int)reader.ReadUInt32(),
				["itemCount"] = reader.ReadUInt16()
			};
			reader.ReadBytes(10);
			if (page["pageType"] == 1)
			{
				string strA = reader.ReadString(27);
				byte ver = reader.ReadByte();
				if (string.CompareOrdinal(strA, "** This is a LiteDB file **") != 0 || ver != 7)
				{
					throw LiteException.InvalidDatabase();
				}
				reader.ReadBytes(10);
				page["userVersion"] = reader.ReadUInt16();
				page["password"] = reader.ReadBytes(20);
				page["salt"] = reader.ReadBytes(16);
				page["collections"] = new BsonDocument();
				byte cols = reader.ReadByte();
				for (int l = 0; l < cols; l++)
				{
					string name = reader.ReadString();
					uint colPageID = reader.ReadUInt32();
					page["collections"][name] = (int)colPageID;
				}
			}
			else if (page["pageType"] == 2)
			{
				page["collectionName"] = reader.ReadString();
				page["indexes"] = new BsonArray();
				reader.ReadBytes(12);
				for (int k = 0; k < 16; k++)
				{
					BsonDocument index = new BsonDocument();
					string field = reader.ReadString();
					int eq = field.IndexOf('=');
					if (eq > 0)
					{
						index["name"] = field.Substring(0, eq);
						index["expression"] = field.Substring(eq + 1);
					}
					else
					{
						index["name"] = field;
						index["expression"] = "$." + field;
					}
					index["unique"] = reader.ReadBoolean();
					index["headPageID"] = (int)reader.ReadUInt32();
					reader.ReadBytes(12);
					if (field.Length > 0)
					{
						page["indexes"].AsArray.Add(index);
					}
				}
			}
			else if (page["pageType"] == 3)
			{
				page["nodes"] = new BsonArray();
				for (int j = 0; j < page["itemCount"].AsInt32; j++)
				{
					BsonDocument node = new BsonDocument { ["index"] = reader.ReadUInt16() };
					byte levels = reader.ReadByte();
					reader.ReadBytes(13);
					ushort length2 = reader.ReadUInt16();
					reader.ReadBytes(1 + length2);
					node["dataBlock"] = new BsonDocument
					{
						["pageID"] = (int)reader.ReadUInt32(),
						["index"] = reader.ReadUInt16()
					};
					node["prev"] = new BsonDocument
					{
						["pageID"] = (int)reader.ReadUInt32(),
						["index"] = reader.ReadUInt16()
					};
					node["next"] = new BsonDocument
					{
						["pageID"] = (int)reader.ReadUInt32(),
						["index"] = reader.ReadUInt16()
					};
					reader.ReadBytes((levels - 1) * 12);
					page["nodes"].AsArray.Add(node);
				}
			}
			else if (page["pageType"] == 4)
			{
				page["blocks"] = new BsonArray();
				for (int i = 0; i < page["itemCount"].AsInt32; i++)
				{
					BsonDocument block = new BsonDocument
					{
						["index"] = reader.ReadUInt16(),
						["extendPageID"] = (int)reader.ReadUInt32()
					};
					ushort length = reader.ReadUInt16();
					block["data"] = reader.ReadBytes(length);
					page["blocks"].AsArray.Add(block);
				}
			}
			else if (page["pageType"] == 5)
			{
				page["data"] = reader.ReadBytes(page["itemCount"].AsInt32);
			}
			return page;
		}

		private byte[] ReadExtendData(uint extendPageID)
		{
			using MemoryStream buffer = new MemoryStream();
			while (extendPageID != uint.MaxValue)
			{
				BsonDocument page = ReadPage(extendPageID);
				if (page["pageType"].AsInt32 != 5)
				{
					return new byte[0];
				}
				buffer.Write(page["data"].AsBinary, 0, page["itemCount"].AsInt32);
				extendPageID = (uint)page["nextPageID"].AsInt32;
			}
			return buffer.ToArray();
		}

		private HashSet<uint> VisitIndexPages(uint startPageID)
		{
			HashSet<uint> toVisit = new HashSet<uint>(new uint[1] { startPageID });
			HashSet<uint> visited = new HashSet<uint>();
			while (toVisit.Count > 0)
			{
				uint indexPageID = toVisit.First();
				toVisit.Remove(indexPageID);
				BsonDocument indexPage = ReadPage(indexPageID);
				if (indexPage == null || indexPage["pageType"] != 3)
				{
					continue;
				}
				visited.Add(indexPageID);
				foreach (BsonValue item in indexPage["nodes"].AsArray)
				{
					uint prev = (uint)item["prev"]["pageID"].AsInt32;
					uint next = (uint)item["next"]["pageID"].AsInt32;
					if (!visited.Contains(prev))
					{
						toVisit.Add(prev);
					}
					if (!visited.Contains(next))
					{
						toVisit.Add(next);
					}
				}
			}
			return visited;
		}
	}
}
