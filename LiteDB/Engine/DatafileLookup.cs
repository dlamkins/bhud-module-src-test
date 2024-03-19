using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class DatafileLookup : IDocumentLookup
	{
		protected readonly DataService _data;

		protected readonly bool _utcDate;

		protected readonly HashSet<string> _fields;

		public DatafileLookup(DataService data, bool utcDate, HashSet<string> fields)
		{
			_data = data;
			_utcDate = utcDate;
			_fields = fields;
		}

		public virtual BsonDocument Load(IndexNode node)
		{
			Constants.ENSURE(node.DataBlock != PageAddress.Empty, "data block must be a valid block address");
			return Load(node.DataBlock);
		}

		public virtual BsonDocument Load(PageAddress rawId)
		{
			using BufferReader reader = new BufferReader(_data.Read(rawId), _utcDate);
			BsonDocument bsonDocument = reader.ReadDocument(_fields);
			bsonDocument.RawId = rawId;
			return bsonDocument;
		}
	}
}
