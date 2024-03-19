using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal interface IFileReader
	{
		IEnumerable<string> GetCollections();

		IEnumerable<IndexInfo> GetIndexes(string name);

		IEnumerable<BsonDocument> GetDocuments(string collection);
	}
}
