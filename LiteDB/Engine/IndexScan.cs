using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class IndexScan : Index
	{
		private readonly Func<BsonValue, bool> _func;

		public IndexScan(string name, Func<BsonValue, bool> func, int order)
			: base(name, order)
		{
			_func = func;
		}

		public override uint GetCost(CollectionIndex index)
		{
			return 80u;
		}

		public override IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index)
		{
			return from i in indexer.FindAll(index, base.Order)
				where _func(i.Key)
				select i;
		}

		public override string ToString()
		{
			return $"FULL INDEX SCAN({base.Name})";
		}
	}
}
