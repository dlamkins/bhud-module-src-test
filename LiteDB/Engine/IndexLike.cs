using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class IndexLike : Index
	{
		private readonly string _startsWith;

		private readonly bool _equals;

		private readonly bool _testSqlLike;

		private readonly string _pattern;

		public IndexLike(string name, BsonValue value, int order)
			: base(name, order)
		{
			_pattern = value.AsString;
			_startsWith = _pattern.SqlLikeStartsWith(out _testSqlLike);
			_equals = _pattern == _startsWith;
		}

		public override uint GetCost(CollectionIndex index)
		{
			if (_startsWith.Length > 0)
			{
				return 10u;
			}
			return 100u;
		}

		public override IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index)
		{
			if (_startsWith.Length <= 0)
			{
				return ExecuteLike(indexer, index);
			}
			return ExecuteStartsWith(indexer, index);
		}

		private IEnumerable<IndexNode> ExecuteStartsWith(IndexService indexer, CollectionIndex index)
		{
			IndexNode first = indexer.Find(index, _startsWith, sibling: true, base.Order);
			IndexNode node2 = first;
			if (first == null)
			{
				yield break;
			}
			while (node2 != null && !node2.Key.IsMinValue && !node2.Key.IsMaxValue)
			{
				string valueString2 = (node2.Key.IsString ? node2.Key.AsString : (node2.Key.IsNull ? "" : node2.Key.ToString()));
				if (!(_equals ? valueString2.Equals(_startsWith, StringComparison.OrdinalIgnoreCase) : valueString2.StartsWith(_startsWith, StringComparison.OrdinalIgnoreCase)))
				{
					break;
				}
				if (!_testSqlLike || (_testSqlLike && valueString2.SqlLike(_pattern, indexer.Collation)))
				{
					yield return node2;
				}
				node2 = indexer.GetNode(node2.GetNextPrev(0, -base.Order));
			}
			node2 = indexer.GetNode(first.GetNextPrev(0, base.Order));
			while (node2 != null && !node2.Key.IsMinValue && !node2.Key.IsMaxValue)
			{
				string valueString = (node2.Key.IsString ? node2.Key.AsString : (node2.Key.IsNull ? "" : node2.Key.ToString()));
				if (_equals ? valueString.Equals(_pattern, StringComparison.OrdinalIgnoreCase) : valueString.StartsWith(_startsWith, StringComparison.OrdinalIgnoreCase))
				{
					if (!node2.DataBlock.IsEmpty && (!_testSqlLike || (_testSqlLike && valueString.SqlLike(_pattern, indexer.Collation))))
					{
						yield return node2;
					}
					node2 = indexer.GetNode(node2.GetNextPrev(0, base.Order));
					continue;
				}
				break;
			}
		}

		private IEnumerable<IndexNode> ExecuteLike(IndexService indexer, CollectionIndex index)
		{
			return from x in indexer.FindAll(index, base.Order)
				where x.Key.IsString && x.Key.AsString.SqlLike(_pattern, indexer.Collation)
				select x;
		}

		public override string ToString()
		{
			return string.Format("{0}({1} LIKE \"{2}\")", (_startsWith.Length > 0) ? "INDEX SEEK (+RANGE SCAN)" : "FULL INDEX SCAN", base.Name, _pattern);
		}
	}
}
