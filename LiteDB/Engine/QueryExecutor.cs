using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class QueryExecutor
	{
		private readonly LiteEngine _engine;

		private readonly TransactionMonitor _monitor;

		private readonly SortDisk _sortDisk;

		private readonly EnginePragmas _pragmas;

		private readonly CursorInfo _cursor;

		private readonly string _collection;

		private readonly Query _query;

		private readonly IEnumerable<BsonDocument> _source;

		public QueryExecutor(LiteEngine engine, TransactionMonitor monitor, SortDisk sortDisk, EnginePragmas pragmas, string collection, Query query, IEnumerable<BsonDocument> source)
		{
			_engine = engine;
			_monitor = monitor;
			_sortDisk = sortDisk;
			_pragmas = pragmas;
			_collection = collection;
			_query = query;
			_cursor = new CursorInfo(collection, query);
			_source = source;
		}

		public BsonDataReader ExecuteQuery()
		{
			if (_query.Into == null)
			{
				return ExecuteQuery(_query.ExplainPlan);
			}
			return ExecuteQueryInto(_query.Into, _query.IntoAutoId);
		}

		internal BsonDataReader ExecuteQuery(bool executionPlan)
		{
			bool isNew;
			TransactionService transaction = _monitor.GetTransaction(create: true, queryOnly: true, out isNew);
			transaction.OpenCursors.Add(_cursor);
			return new BsonDataReader(RunQuery(), _collection);
			IEnumerable<BsonDocument> RunQuery()
			{
				Snapshot snapshot = transaction.CreateSnapshot(_query.ForUpdate ? LockMode.Write : LockMode.Read, _collection, addIfNotExists: false);
				if (snapshot.CollectionPage == null && _source == null)
				{
					if (_query.Select.UseSource)
					{
						yield return _query.Select.ExecuteScalar(_pragmas.Collation).AsDocument;
					}
					transaction.OpenCursors.Remove(_cursor);
					if (isNew)
					{
						_monitor.ReleaseTransaction(transaction);
					}
				}
				else
				{
					QueryPlan queryPlan = new QueryOptimization(snapshot, _query, _source, _pragmas.Collation).ProcessQuery();
					if (executionPlan)
					{
						yield return queryPlan.GetExecutionPlan();
						transaction.OpenCursors.Remove(_cursor);
						if (isNew)
						{
							_monitor.ReleaseTransaction(transaction);
						}
					}
					else
					{
						IEnumerable<IndexNode> nodes = queryPlan.Index.Run(snapshot.CollectionPage, new IndexService(snapshot, _pragmas.Collation));
						BasePipe pipe = queryPlan.GetPipe(transaction, snapshot, _sortDisk, _pragmas);
						try
						{
							_cursor.Elapsed.Start();
							foreach (BsonDocument doc in pipe.Pipe(nodes, queryPlan))
							{
								_cursor.Fetched++;
								_cursor.Elapsed.Stop();
								yield return doc;
								if (transaction.State != 0)
								{
									throw new LiteException(0, "There is no more active transaction for this cursor: " + _cursor.Query.ToSQL(_cursor.Collection));
								}
								_cursor.Elapsed.Start();
							}
						}
						finally
						{
							_cursor.Elapsed.Stop();
							transaction.OpenCursors.Remove(_cursor);
							if (isNew)
							{
								_monitor.ReleaseTransaction(transaction);
							}
						}
					}
				}
			}
		}

		internal BsonDataReader ExecuteQueryInto(string into, BsonAutoId autoId)
		{
			int result;
			if (into.StartsWith("$"))
			{
				SqlParser.ParseCollection(new Tokenizer(into), out var name, out var options);
				result = _engine.GetSystemCollection(name).Output(GetResultset(), options);
			}
			else
			{
				result = _engine.Insert(into, GetResultset(), autoId);
			}
			return new BsonDataReader(result);
			IEnumerable<BsonDocument> GetResultset()
			{
				using BsonDataReader reader = ExecuteQuery(executionPlan: false);
				while (reader.Read())
				{
					yield return reader.Current.AsDocument;
				}
			}
		}
	}
}
