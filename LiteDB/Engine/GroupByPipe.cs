using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class GroupByPipe : BasePipe
	{
		public GroupByPipe(TransactionService transaction, IDocumentLookup loader, SortDisk tempDisk, EnginePragmas pragmas)
			: base(transaction, loader, tempDisk, pragmas)
		{
		}

		public override IEnumerable<BsonDocument> Pipe(IEnumerable<IndexNode> nodes, QueryPlan query)
		{
			IEnumerable<BsonDocument> source = LoadDocument(nodes);
			foreach (BsonExpression expr in query.Filters)
			{
				source = Filter(source, expr);
			}
			if (query.OrderBy != null)
			{
				source = OrderBy(source, query.OrderBy.Expression, query.OrderBy.Order, 0, int.MaxValue);
			}
			IEnumerable<DocumentCacheEnumerable> groups = GroupBy(source, query.GroupBy);
			IEnumerable<BsonDocument> result = SelectGroupBy(groups, query.GroupBy);
			if (query.Offset > 0)
			{
				result = result.Skip(query.Offset);
			}
			if (query.Limit < int.MaxValue)
			{
				result = result.Take(query.Limit);
			}
			return result;
		}

		private IEnumerable<DocumentCacheEnumerable> GroupBy(IEnumerable<BsonDocument> source, GroupBy groupBy)
		{
			using IEnumerator<BsonDocument> enumerator = source.GetEnumerator();
			Done done = new Done
			{
				Running = enumerator.MoveNext()
			};
			while (done.Running)
			{
				BsonValue key = groupBy.Expression.ExecuteScalar(enumerator.Current, _pragmas.Collation);
				groupBy.Select.Parameters["key"] = key;
				IEnumerable<BsonDocument> group = YieldDocuments(key, enumerator, groupBy, done);
				yield return new DocumentCacheEnumerable(group, _lookup);
			}
		}

		private IEnumerable<BsonDocument> YieldDocuments(BsonValue key, IEnumerator<BsonDocument> enumerator, GroupBy groupBy, Done done)
		{
			yield return enumerator.Current;
			while (done.Running = enumerator.MoveNext())
			{
				BsonValue current = groupBy.Expression.ExecuteScalar(enumerator.Current, _pragmas.Collation);
				if (key == current)
				{
					yield return enumerator.Current;
					continue;
				}
				groupBy.Select.Parameters["key"] = current;
				break;
			}
		}

		private IEnumerable<BsonDocument> SelectGroupBy(IEnumerable<DocumentCacheEnumerable> groups, GroupBy groupBy)
		{
			string defaultName = groupBy.Select.DefaultFieldName();
			foreach (DocumentCacheEnumerable group in groups)
			{
				BsonValue value;
				try
				{
					if (groupBy.Having == null)
					{
						goto IL_00b2;
					}
					BsonValue filter = groupBy.Having.ExecuteScalar(group, null, null, _pragmas.Collation);
					if (filter.IsBoolean && filter.AsBoolean)
					{
						goto IL_00b2;
					}
					goto end_IL_006e;
					IL_00b2:
					value = groupBy.Select.ExecuteScalar(group, null, null, _pragmas.Collation);
					goto IL_00db;
					end_IL_006e:;
				}
				finally
				{
					group.Dispose();
				}
				continue;
				IL_00db:
				if (value.IsDocument)
				{
					yield return value.AsDocument;
					continue;
				}
				yield return new BsonDocument { [defaultName] = value };
			}
		}
	}
}
