using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SQLite
{
	public class SQLiteAsyncConnection
	{
		private readonly SQLiteConnectionString _connectionString;

		public string DatabasePath => GetConnection().DatabasePath;

		public int LibVersionNumber => GetConnection().LibVersionNumber;

		public string DateTimeStringFormat => GetConnection().DateTimeStringFormat;

		public bool StoreDateTimeAsTicks => GetConnection().StoreDateTimeAsTicks;

		public bool StoreTimeSpanAsTicks => GetConnection().StoreTimeSpanAsTicks;

		public bool Trace
		{
			get
			{
				return GetConnection().Trace;
			}
			set
			{
				GetConnection().Trace = value;
			}
		}

		public Action<string> Tracer
		{
			get
			{
				return GetConnection().Tracer;
			}
			set
			{
				GetConnection().Tracer = value;
			}
		}

		public bool TimeExecution
		{
			get
			{
				return GetConnection().TimeExecution;
			}
			set
			{
				GetConnection().TimeExecution = value;
			}
		}

		public IEnumerable<TableMapping> TableMappings => GetConnection().TableMappings;

		public SQLiteAsyncConnection(string databasePath, bool storeDateTimeAsTicks = true)
			: this(new SQLiteConnectionString(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, storeDateTimeAsTicks))
		{
		}

		public SQLiteAsyncConnection(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = true)
			: this(new SQLiteConnectionString(databasePath, openFlags, storeDateTimeAsTicks))
		{
		}

		public SQLiteAsyncConnection(SQLiteConnectionString connectionString)
		{
			_connectionString = connectionString;
		}

		public TimeSpan GetBusyTimeout()
		{
			return GetConnection().BusyTimeout;
		}

		public Task SetBusyTimeoutAsync(TimeSpan value)
		{
			return ReadAsync((Func<SQLiteConnectionWithLock, object>)delegate(SQLiteConnectionWithLock conn)
			{
				conn.BusyTimeout = value;
				return null;
			});
		}

		public Task EnableWriteAheadLoggingAsync()
		{
			return WriteAsync((Func<SQLiteConnectionWithLock, object>)delegate(SQLiteConnectionWithLock conn)
			{
				conn.EnableWriteAheadLogging();
				return null;
			});
		}

		public static void ResetPool()
		{
			SQLiteConnectionPool.Shared.Reset();
		}

		public SQLiteConnectionWithLock GetConnection()
		{
			return SQLiteConnectionPool.Shared.GetConnection(_connectionString);
		}

		public Task CloseAsync()
		{
			return Task.Factory.StartNew(delegate
			{
				SQLiteConnectionPool.Shared.CloseConnection(_connectionString);
			}, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		private Task<T> ReadAsync<T>(Func<SQLiteConnectionWithLock, T> read)
		{
			return Task.Factory.StartNew(delegate
			{
				SQLiteConnectionWithLock connection = GetConnection();
				using (connection.Lock())
				{
					return read(connection);
				}
			}, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		private Task<T> WriteAsync<T>(Func<SQLiteConnectionWithLock, T> write)
		{
			return Task.Factory.StartNew(delegate
			{
				SQLiteConnectionWithLock connection = GetConnection();
				using (connection.Lock())
				{
					return write(connection);
				}
			}, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		public Task EnableLoadExtensionAsync(bool enabled)
		{
			return WriteAsync((Func<SQLiteConnectionWithLock, object>)delegate(SQLiteConnectionWithLock conn)
			{
				conn.EnableLoadExtension(enabled);
				return null;
			});
		}

		public Task<CreateTableResult> CreateTableAsync<T>(CreateFlags createFlags = CreateFlags.None) where T : new()
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateTable<T>(createFlags));
		}

		public Task<CreateTableResult> CreateTableAsync(Type ty, CreateFlags createFlags = CreateFlags.None)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateTable(ty, createFlags));
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()
		{
			return CreateTablesAsync(createFlags, typeof(T), typeof(T2));
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()where T3 : new()
		{
			return CreateTablesAsync(createFlags, typeof(T), typeof(T2), typeof(T3));
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3, T4>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()where T3 : new()where T4 : new()
		{
			return CreateTablesAsync(createFlags, typeof(T), typeof(T2), typeof(T3), typeof(T4));
		}

		public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3, T4, T5>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()where T3 : new()where T4 : new()where T5 : new()
		{
			return CreateTablesAsync(createFlags, typeof(T), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}

		public Task<CreateTablesResult> CreateTablesAsync(CreateFlags createFlags = CreateFlags.None, params Type[] types)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateTables(createFlags, types));
		}

		public Task<int> DropTableAsync<T>() where T : new()
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.DropTable<T>());
		}

		public Task<int> DropTableAsync(TableMapping map)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.DropTable(map));
		}

		public Task<int> CreateIndexAsync(string tableName, string columnName, bool unique = false)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateIndex(tableName, columnName, unique));
		}

		public Task<int> CreateIndexAsync(string indexName, string tableName, string columnName, bool unique = false)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateIndex(indexName, tableName, columnName, unique));
		}

		public Task<int> CreateIndexAsync(string tableName, string[] columnNames, bool unique = false)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateIndex(tableName, columnNames, unique));
		}

		public Task<int> CreateIndexAsync(string indexName, string tableName, string[] columnNames, bool unique = false)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateIndex(indexName, tableName, columnNames, unique));
		}

		public Task<int> CreateIndexAsync<T>(Expression<Func<T, object>> property, bool unique = false)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateIndex(property, unique));
		}

		public Task<int> InsertAsync(object obj)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Insert(obj));
		}

		public Task<int> InsertAsync(object obj, Type objType)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Insert(obj, objType));
		}

		public Task<int> InsertAsync(object obj, string extra)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Insert(obj, extra));
		}

		public Task<int> InsertAsync(object obj, string extra, Type objType)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Insert(obj, extra, objType));
		}

		public Task<int> InsertOrReplaceAsync(object obj)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.InsertOrReplace(obj));
		}

		public Task<int> InsertOrReplaceAsync(object obj, Type objType)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.InsertOrReplace(obj, objType));
		}

		public Task<int> UpdateAsync(object obj)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Update(obj));
		}

		public Task<int> UpdateAsync(object obj, Type objType)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Update(obj, objType));
		}

		public Task<int> UpdateAllAsync(IEnumerable objects, bool runInTransaction = true)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.UpdateAll(objects, runInTransaction));
		}

		public Task<int> DeleteAsync(object objectToDelete)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Delete(objectToDelete));
		}

		public Task<int> DeleteAsync<T>(object primaryKey)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Delete<T>(primaryKey));
		}

		public Task<int> DeleteAsync(object primaryKey, TableMapping map)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Delete(primaryKey, map));
		}

		public Task<int> DeleteAllAsync<T>()
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.DeleteAll<T>());
		}

		public Task<int> DeleteAllAsync(TableMapping map)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.DeleteAll(map));
		}

		public Task BackupAsync(string destinationDatabasePath, string databaseName = "main")
		{
			return WriteAsync(delegate(SQLiteConnectionWithLock conn)
			{
				conn.Backup(destinationDatabasePath, databaseName);
				return 0;
			});
		}

		public Task<T> GetAsync<T>(object pk) where T : new()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Get<T>(pk));
		}

		public Task<object> GetAsync(object pk, TableMapping map)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Get(pk, map));
		}

		public Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : new()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Get(predicate));
		}

		public Task<T> FindAsync<T>(object pk) where T : new()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Find<T>(pk));
		}

		public Task<object> FindAsync(object pk, TableMapping map)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Find(pk, map));
		}

		public Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : new()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Find(predicate));
		}

		public Task<T> FindWithQueryAsync<T>(string query, params object[] args) where T : new()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.FindWithQuery<T>(query, args));
		}

		public Task<object> FindWithQueryAsync(TableMapping map, string query, params object[] args)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.FindWithQuery(map, query, args));
		}

		public Task<TableMapping> GetMappingAsync(Type type, CreateFlags createFlags = CreateFlags.None)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.GetMapping(type, createFlags));
		}

		public Task<TableMapping> GetMappingAsync<T>(CreateFlags createFlags = CreateFlags.None) where T : new()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.GetMapping<T>(createFlags));
		}

		public Task<List<SQLiteConnection.ColumnInfo>> GetTableInfoAsync(string tableName)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.GetTableInfo(tableName));
		}

		public Task<int> ExecuteAsync(string query, params object[] args)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.Execute(query, args));
		}

		public Task<int> InsertAllAsync(IEnumerable objects, bool runInTransaction = true)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.InsertAll(objects, runInTransaction));
		}

		public Task<int> InsertAllAsync(IEnumerable objects, string extra, bool runInTransaction = true)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.InsertAll(objects, extra, runInTransaction));
		}

		public Task<int> InsertAllAsync(IEnumerable objects, Type objType, bool runInTransaction = true)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.InsertAll(objects, objType, runInTransaction));
		}

		public Task RunInTransactionAsync(Action<SQLiteConnection> action)
		{
			return WriteAsync((Func<SQLiteConnectionWithLock, object>)delegate(SQLiteConnectionWithLock conn)
			{
				conn.BeginTransaction();
				try
				{
					action(conn);
					conn.Commit();
					return null;
				}
				catch (Exception)
				{
					conn.Rollback();
					throw;
				}
			});
		}

		public AsyncTableQuery<T> Table<T>() where T : new()
		{
			return new AsyncTableQuery<T>(GetConnection().Table<T>());
		}

		public Task<T> ExecuteScalarAsync<T>(string query, params object[] args)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => conn.CreateCommand(query, args).ExecuteScalar<T>());
		}

		public Task<List<T>> QueryAsync<T>(string query, params object[] args) where T : new()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Query<T>(query, args));
		}

		public Task<List<object>> QueryAsync(TableMapping map, string query, params object[] args)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => conn.Query(map, query, args));
		}

		public Task<IEnumerable<T>> DeferredQueryAsync<T>(string query, params object[] args) where T : new()
		{
			return ReadAsync((Func<SQLiteConnectionWithLock, IEnumerable<T>>)((SQLiteConnectionWithLock conn) => conn.DeferredQuery<T>(query, args).ToList()));
		}

		public Task<IEnumerable<object>> DeferredQueryAsync(TableMapping map, string query, params object[] args)
		{
			return ReadAsync((Func<SQLiteConnectionWithLock, IEnumerable<object>>)((SQLiteConnectionWithLock conn) => conn.DeferredQuery(map, query, args).ToList()));
		}
	}
}
