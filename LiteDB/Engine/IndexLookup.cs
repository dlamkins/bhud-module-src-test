namespace LiteDB.Engine
{
	internal class IndexLookup : IDocumentLookup
	{
		private readonly IndexService _indexer;

		private readonly string _name;

		public IndexLookup(IndexService indexer, string name)
		{
			_indexer = indexer;
			_name = name;
		}

		public BsonDocument Load(IndexNode node)
		{
			Constants.ENSURE(!node.DataBlock.IsEmpty, "Never should be empty rawid");
			return new BsonDocument
			{
				[_name] = node.Key,
				RawId = node.DataBlock
			};
		}

		public BsonDocument Load(PageAddress rawId)
		{
			IndexNode node = _indexer.GetNode(rawId);
			return Load(node);
		}
	}
}
