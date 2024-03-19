using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class IndexIn : Index
	{
		private readonly BsonArray _values;

		public IndexIn(string name, BsonArray values, int order)
			: base(name, order)
		{
			_values = values;
		}

		public override uint GetCost(CollectionIndex index)
		{
			if (!index.Unique)
			{
				return (uint)(_values.Count * 10);
			}
			return (uint)_values.Count;
		}

		public override IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index)
		{
			foreach (BsonValue value in _values.Distinct())
			{
				IndexEquals idx = new IndexEquals(base.Name, value);
				foreach (IndexNode item in idx.Execute(indexer, index))
				{
					yield return item;
				}
			}
		}

		public override string ToString()
		{
			return $"INDEX SEEK({base.Name} IN {JsonSerializer.Serialize(_values)})";
		}
	}
}
