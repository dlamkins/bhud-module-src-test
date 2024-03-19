using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class IndexVirtual : Index, IDocumentLookup
	{
		private readonly IEnumerable<BsonDocument> _source;

		private Dictionary<uint, BsonDocument> _cache = new Dictionary<uint, BsonDocument>();

		public IndexVirtual(IEnumerable<BsonDocument> source)
			: base(null, 0)
		{
			_source = source;
		}

		public override uint GetCost(CollectionIndex index)
		{
			return 100u;
		}

		public override IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<IndexNode> Run(CollectionPage col, IndexService indexer)
		{
			uint rawId = 0u;
			foreach (BsonDocument doc in _source)
			{
				rawId++;
				doc.RawId = new PageAddress(rawId, 0);
				if (_cache != null)
				{
					_cache[rawId] = doc;
					if (_cache.Count > 2000)
					{
						_cache = null;
					}
				}
				yield return new IndexNode(doc);
			}
		}

		public BsonDocument Load(IndexNode node)
		{
			return node.Key as BsonDocument;
		}

		public BsonDocument Load(PageAddress rawId)
		{
			if (_cache == null)
			{
				throw new LiteException(0, $"OrderBy/GroupBy operation are supported only in virtual collection with less than {2000} documents");
			}
			return _cache[rawId.PageID];
		}

		public override string ToString()
		{
			return $"FULL COLLECTION SCAN";
		}
	}
}
