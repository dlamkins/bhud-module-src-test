using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal abstract class BasePipe
	{
		protected readonly TransactionService _transaction;

		protected readonly IDocumentLookup _lookup;

		protected readonly SortDisk _tempDisk;

		protected readonly EnginePragmas _pragmas;

		public BasePipe(TransactionService transaction, IDocumentLookup lookup, SortDisk tempDisk, EnginePragmas pragmas)
		{
			_transaction = transaction;
			_lookup = lookup;
			_tempDisk = tempDisk;
			_pragmas = pragmas;
		}

		public abstract IEnumerable<BsonDocument> Pipe(IEnumerable<IndexNode> nodes, QueryPlan query);

		protected IEnumerable<BsonDocument> LoadDocument(IEnumerable<IndexNode> nodes)
		{
			foreach (IndexNode node in nodes)
			{
				yield return _lookup.Load(node);
				_transaction.Safepoint();
			}
		}

		protected IEnumerable<BsonDocument> Include(IEnumerable<BsonDocument> source, BsonExpression path)
		{
			string last = null;
			Snapshot snapshot = null;
			IndexService indexer = null;
			DataService data = null;
			CollectionIndex index = null;
			IDocumentLookup lookup = null;
			foreach (BsonDocument doc in source)
			{
				foreach (BsonValue value2 in (from x in path.Execute(doc, _pragmas.Collation)
					where x.IsDocument || x.IsArray
					select x).ToList())
				{
					if (value2.IsDocument)
					{
						DoInclude(value2.AsDocument);
						continue;
					}
					foreach (BsonDocument item in from x in value2.AsArray
						where x.IsDocument
						select x.AsDocument)
					{
						DoInclude(item);
					}
				}
				yield return doc;
			}
			void DoInclude(BsonDocument value)
			{
				BsonValue refId = value["$id"];
				BsonValue refCol = value["$ref"];
				if (!refId.IsNull && refCol.IsString)
				{
					if (last != refCol.AsString)
					{
						last = refCol.AsString;
						snapshot = _transaction.CreateSnapshot(LockMode.Read, last, addIfNotExists: false);
						indexer = new IndexService(snapshot, _pragmas.Collation);
						data = new DataService(snapshot);
						lookup = new DatafileLookup(data, _pragmas.UtcDate, null);
						index = snapshot.CollectionPage?.PK;
					}
					if (index != null)
					{
						IndexNode node = indexer.Find(index, refId, sibling: false, 1);
						if (node != null)
						{
							BsonDocument source2 = lookup.Load(node);
							value.Remove("$ref");
							foreach (KeyValuePair<string, BsonValue> element in source2.Where((KeyValuePair<string, BsonValue> x) => x.Key != "_id"))
							{
								value[element.Key] = element.Value;
							}
						}
						else
						{
							value["$missing"] = true;
						}
					}
				}
			}
		}

		protected IEnumerable<BsonDocument> Filter(IEnumerable<BsonDocument> source, BsonExpression expr)
		{
			foreach (BsonDocument doc in source)
			{
				BsonValue result = expr.ExecuteScalar(doc, _pragmas.Collation);
				if (result.IsBoolean && result.AsBoolean)
				{
					yield return doc;
				}
			}
		}

		protected IEnumerable<BsonDocument> OrderBy(IEnumerable<BsonDocument> source, BsonExpression expr, int order, int offset, int limit)
		{
			IEnumerable<KeyValuePair<BsonValue, PageAddress>> keyValues = source.Select((BsonDocument x) => new KeyValuePair<BsonValue, PageAddress>(expr.ExecuteScalar(x, _pragmas.Collation), x.RawId));
			using SortService sorter = new SortService(_tempDisk, order, _pragmas);
			sorter.Insert(keyValues);
			IEnumerable<KeyValuePair<BsonValue, PageAddress>> result = sorter.Sort().Skip(offset).Take(limit);
			foreach (KeyValuePair<BsonValue, PageAddress> keyValue in result)
			{
				yield return _lookup.Load(keyValue.Value);
			}
		}
	}
}
