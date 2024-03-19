using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class IndexRange : Index
	{
		private readonly BsonValue _start;

		private readonly BsonValue _end;

		private readonly bool _startEquals;

		private readonly bool _endEquals;

		public IndexRange(string name, BsonValue start, BsonValue end, bool startEquals, bool endEquals, int order)
			: base(name, order)
		{
			_start = start;
			_end = end;
			_startEquals = startEquals;
			_endEquals = endEquals;
		}

		public override uint GetCost(CollectionIndex index)
		{
			return 20u;
		}

		public override IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index)
		{
			BsonValue start = ((base.Order == 1) ? _start : _end);
			BsonValue end = ((base.Order == 1) ? _end : _start);
			bool startEquals = ((base.Order == 1) ? _startEquals : _endEquals);
			bool endEquals = ((base.Order == 1) ? _endEquals : _startEquals);
			IndexNode first = ((start.Type == BsonType.MinValue) ? indexer.GetNode(index.Head) : ((start.Type == BsonType.MaxValue) ? indexer.GetNode(index.Tail) : indexer.Find(index, start, sibling: true, base.Order)));
			IndexNode node = first;
			if (startEquals && node != null)
			{
				while (!node.GetNextPrev(0, -base.Order).IsEmpty)
				{
					IndexNode node2;
					node = (node2 = indexer.GetNode(node.GetNextPrev(0, -base.Order)));
					if (node2.Key.CompareTo(start) != 0 || node.Key.IsMinValue || node.Key.IsMaxValue)
					{
						break;
					}
					yield return node;
				}
				node = first;
			}
			while (node != null && node.Key.CompareTo(start, indexer.Collation) == 0)
			{
				if (startEquals && !node.Key.IsMinValue && !node.Key.IsMaxValue)
				{
					yield return node;
				}
				node = indexer.GetNode(node.GetNextPrev(0, base.Order));
			}
			while (node != null)
			{
				int diff = node.Key.CompareTo(end, indexer.Collation);
				if (endEquals && diff == 0 && !node.Key.IsMinValue && !node.Key.IsMaxValue)
				{
					yield return node;
				}
				else
				{
					if (diff != -base.Order || node.Key.IsMinValue || node.Key.IsMaxValue)
					{
						break;
					}
					yield return node;
				}
				node = indexer.GetNode(node.GetNextPrev(0, base.Order));
			}
		}

		public override string ToString()
		{
			if (_start.IsMinValue && !_endEquals)
			{
				return $"INDEX SCAN({base.Name} < {_end})";
			}
			if (_start.IsMinValue && _endEquals)
			{
				return $"INDEX SCAN({base.Name} <= {_end})";
			}
			if (_end.IsMaxValue && !_startEquals)
			{
				return $"INDEX SCAN({base.Name} > {_start})";
			}
			if (_end.IsMaxValue && _startEquals)
			{
				return $"INDEX SCAN({base.Name} >= {_start})";
			}
			return $"INDEX RANGE SCAN({base.Name} BETWEEN {_start} AND {_end})";
		}
	}
}
