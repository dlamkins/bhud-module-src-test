using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class QueryPlan
	{
		public string Collection { get; set; }

		public Index Index { get; set; }

		public string IndexExpression { get; set; }

		public uint IndexCost { get; internal set; }

		public bool IsIndexKeyOnly { get; set; }

		public List<BsonExpression> Filters { get; set; } = new List<BsonExpression>();


		public List<BsonExpression> IncludeBefore { get; set; } = new List<BsonExpression>();


		public List<BsonExpression> IncludeAfter { get; set; } = new List<BsonExpression>();


		public OrderBy OrderBy { get; set; }

		public GroupBy GroupBy { get; set; }

		public Select Select { get; set; }

		public HashSet<string> Fields { get; set; }

		public int Limit { get; set; } = int.MaxValue;


		public int Offset { get; set; }

		public bool ForUpdate { get; set; }

		public QueryPlan(string collection)
		{
			Collection = collection;
		}

		public BasePipe GetPipe(TransactionService transaction, Snapshot snapshot, SortDisk tempDisk, EnginePragmas pragmas)
		{
			if (GroupBy == null)
			{
				return new QueryPipe(transaction, GetLookup(snapshot, pragmas), tempDisk, pragmas);
			}
			return new GroupByPipe(transaction, GetLookup(snapshot, pragmas), tempDisk, pragmas);
		}

		public IDocumentLookup GetLookup(Snapshot snapshot, EnginePragmas pragmas)
		{
			DataService data = new DataService(snapshot);
			IndexService indexer = new IndexService(snapshot, pragmas.Collation);
			IDocumentLookup lookup = Index as IDocumentLookup;
			if (lookup == null)
			{
				lookup = ((!IsIndexKeyOnly) ? ((IDocumentLookup)new DatafileLookup(data, pragmas.UtcDate, Fields)) : ((IDocumentLookup)new IndexLookup(indexer, Fields.Single())));
			}
			return lookup;
		}

		public BsonDocument GetExecutionPlan()
		{
			BsonDocument doc = new BsonDocument
			{
				["collection"] = Collection,
				["snaphost"] = (ForUpdate ? "write" : "read"),
				["pipe"] = ((GroupBy == null) ? "queryPipe" : "groupByPipe")
			};
			doc["index"] = new BsonDocument
			{
				["name"] = Index.Name,
				["expr"] = IndexExpression,
				["order"] = Index.Order,
				["mode"] = Index.ToString(),
				["cost"] = (int)IndexCost
			};
			doc["lookup"] = new BsonDocument
			{
				["loader"] = ((Index is IndexVirtual) ? "virtual" : (IsIndexKeyOnly ? "index" : "document")),
				["fields"] = ((Fields.Count == 0) ? new BsonValue("$") : new BsonArray(Fields.Select((string x) => new BsonValue(x))))
			};
			if (IncludeBefore.Count > 0)
			{
				doc["includeBefore"] = new BsonArray(IncludeBefore.Select((BsonExpression x) => new BsonValue(x.Source)));
			}
			if (Filters.Count > 0)
			{
				doc["filters"] = new BsonArray(Filters.Select((BsonExpression x) => new BsonValue(x.Source)));
			}
			if (OrderBy != null)
			{
				doc["orderBy"] = new BsonDocument
				{
					["expr"] = OrderBy.Expression.Source,
					["order"] = OrderBy.Order
				};
			}
			if (Limit != int.MaxValue)
			{
				doc["limit"] = Limit;
			}
			if (Offset != 0)
			{
				doc["offset"] = Offset;
			}
			if (IncludeAfter.Count > 0)
			{
				doc["includeAfter"] = new BsonArray(IncludeAfter.Select((BsonExpression x) => new BsonValue(x.Source)));
			}
			if (GroupBy != null)
			{
				doc["groupBy"] = new BsonDocument
				{
					["expr"] = GroupBy.Expression.Source,
					["having"] = GroupBy.Having?.Source,
					["select"] = GroupBy.Select?.Source
				};
			}
			else
			{
				doc["select"] = new BsonDocument
				{
					["expr"] = Select.Expression.Source,
					["all"] = Select.All
				};
			}
			return doc;
		}
	}
}
