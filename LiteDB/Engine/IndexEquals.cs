using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class IndexEquals : Index
	{
		private readonly BsonValue _value;

		public IndexEquals(string name, BsonValue value)
			: base(name, 1)
		{
			_value = value;
		}

		public override uint GetCost(CollectionIndex index)
		{
			if (index.Unique)
			{
				return 1u;
			}
			return 10u;
		}

		public override IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index)
		{
			IndexNode node2 = indexer.Find(index, _value, sibling: false, 1);
			if (node2 == null)
			{
				yield break;
			}
			yield return node2;
			if (index.Unique)
			{
				yield break;
			}
			IndexNode first = node2;
			while (!node2.Next[0].IsEmpty)
			{
				IndexNode node3;
				node2 = (node3 = indexer.GetNode(node2.Next[0]));
				if (node3.Key.CompareTo(_value, indexer.Collation) != 0 || node2.Key.IsMinValue || node2.Key.IsMaxValue)
				{
					break;
				}
				yield return node2;
			}
			node2 = first;
			while (!node2.Prev[0].IsEmpty)
			{
				IndexNode node3;
				node2 = (node3 = indexer.GetNode(node2.Prev[0]));
				if (node3.Key.CompareTo(_value, indexer.Collation) == 0 && !node2.Key.IsMinValue && !node2.Key.IsMaxValue)
				{
					yield return node2;
					continue;
				}
				break;
			}
		}

		public override string ToString()
		{
			return $"INDEX SEEK({base.Name} = {_value})";
		}
	}
}
