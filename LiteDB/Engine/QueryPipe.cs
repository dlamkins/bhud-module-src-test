using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class QueryPipe : BasePipe
	{
		public QueryPipe(TransactionService transaction, IDocumentLookup loader, SortDisk tempDisk, EnginePragmas pragmas)
			: base(transaction, loader, tempDisk, pragmas)
		{
		}

		public override IEnumerable<BsonDocument> Pipe(IEnumerable<IndexNode> nodes, QueryPlan query)
		{
			IEnumerable<BsonDocument> source = LoadDocument(nodes);
			foreach (BsonExpression path2 in query.IncludeBefore)
			{
				source = Include(source, path2);
			}
			foreach (BsonExpression expr in query.Filters)
			{
				source = Filter(source, expr);
			}
			if (query.OrderBy != null)
			{
				source = OrderBy(source, query.OrderBy.Expression, query.OrderBy.Order, query.Offset, query.Limit);
			}
			else
			{
				if (query.Offset > 0)
				{
					source = source.Skip(query.Offset);
				}
				if (query.Limit < int.MaxValue)
				{
					source = source.Take(query.Limit);
				}
			}
			foreach (BsonExpression path in query.IncludeAfter)
			{
				source = Include(source, path);
			}
			if (query.Select.All)
			{
				return SelectAll(source, query.Select.Expression);
			}
			return Select(source, query.Select.Expression);
		}

		private IEnumerable<BsonDocument> Select(IEnumerable<BsonDocument> source, BsonExpression select)
		{
			string defaultName = select.DefaultFieldName();
			foreach (BsonDocument doc in source)
			{
				BsonValue value = select.ExecuteScalar(doc, _pragmas.Collation);
				if (value.IsDocument)
				{
					yield return value.AsDocument;
					continue;
				}
				yield return new BsonDocument { [defaultName] = value };
			}
		}

		private IEnumerable<BsonDocument> SelectAll(IEnumerable<BsonDocument> source, BsonExpression select)
		{
			DocumentCacheEnumerable cached = new DocumentCacheEnumerable(source, _lookup);
			string defaultName = select.DefaultFieldName();
			IEnumerable<BsonValue> result = select.Execute(cached, _pragmas.Collation);
			foreach (BsonValue value in result)
			{
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
