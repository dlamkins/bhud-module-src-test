using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class IndexAll : Index
	{
		public IndexAll(string name, int order)
			: base(name, order)
		{
		}

		public override uint GetCost(CollectionIndex index)
		{
			return 100u;
		}

		public override IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index)
		{
			return indexer.FindAll(index, base.Order);
		}

		public override string ToString()
		{
			return $"FULL INDEX SCAN({base.Name})";
		}
	}
}
