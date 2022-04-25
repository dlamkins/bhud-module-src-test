using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SQLite
{
	[Preserve(AllMembers = true)]
	public class SQLiteConnection : IDisposable
	{
		private struct IndexedColumn
		{
			public int Order;

			public string ColumnName;
		}

		private struct IndexInfo
		{
			public string IndexName;

			public string TableName;

			public bool Unique;

			public List<IndexedColumn> Columns;
		}

		[Preserve(AllMembers = true)]
		public class ColumnInfo
		{
			[Column("name")]
			public string Name { get; set; }

			public int notnull { get; set; }

			public override string ToString()
			{
				return Name;
			}
		}

		private bool _open;

		private TimeSpan _busyTimeout;

		private static readonly Dictionary<string, TableMapping> _mappings = new Dictionary<string, TableMapping>();

		private Stopwatch _sw;

		private long _elapsedMilliseconds;

		private int _transactionDepth;

		private Random _rand = new Random();

		private static readonly IntPtr NullHandle = default(IntPtr);

		private static readonly IntPtr NullBackupHandle = default(IntPtr);

		private readonly Dictionary<Tuple<string, string>, PreparedSqlLiteInsertCommand> _insertCommandMap = new Dictionary<Tuple<string, string>, PreparedSqlLiteInsertCommand>();

		public IntPtr Handle { get; private set; }

		public string DatabasePath { get; private set; }

		public int LibVersionNumber { get; private set; }

		public bool TimeExecution { get; set; }

		public bool Trace { get; set; }

		public Action<string> Tracer { get; set; }

		public bool StoreDateTimeAsTicks { get; private set; }

		public bool StoreTimeSpanAsTicks { get; private set; }

		public string DateTimeStringFormat { get; private set; }

		internal DateTimeStyles DateTimeStyle { get; private set; }

		public TimeSpan BusyTimeout
		{
			get
			{
				return _busyTimeout;
			}
			set
			{
				_busyTimeout = value;
				if (Handle != NullHandle)
				{
					SQLite3.BusyTimeout(Handle, (int)_busyTimeout.TotalMilliseconds);
				}
			}
		}

		public IEnumerable<TableMapping> TableMappings
		{
			get
			{
				lock (_mappings)
				{
					return new List<TableMapping>(_mappings.Values);
				}
			}
		}

		public bool IsInTransaction => _transactionDepth > 0;

		public event EventHandler<NotifyTableChangedEventArgs> TableChanged;

		public SQLiteConnection(string databasePath, bool storeDateTimeAsTicks = true)
			: this(new SQLiteConnectionString(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks))
		{
		}

		public SQLiteConnection(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = true)
			: this(new SQLiteConnectionString(databasePath, openFlags, storeDateTimeAsTicks))
		{
		}

		public SQLiteConnection(SQLiteConnectionString connectionString)
		{
			if (connectionString == null)
			{
				throw new ArgumentNullException("connectionString");
			}
			if (connectionString.DatabasePath == null)
			{
				throw new InvalidOperationException("DatabasePath must be specified");
			}
			DatabasePath = connectionString.DatabasePath;
			LibVersionNumber = SQLite3.LibVersionNumber();
			IntPtr handle;
			SQLite3.Result r = SQLite3.Open(GetNullTerminatedUtf8(connectionString.DatabasePath), out handle, (int)connectionString.OpenFlags, connectionString.VfsName);
			Handle = handle;
			if (r != 0)
			{
				throw SQLiteException.New(r, $"Could not open database file: {DatabasePath} ({r})");
			}
			_open = true;
			StoreDateTimeAsTicks = connectionString.StoreDateTimeAsTicks;
			DateTimeStringFormat = connectionString.DateTimeStringFormat;
			DateTimeStyle = connectionString.DateTimeStyle;
			BusyTimeout = TimeSpan.FromSeconds(0.1);
			Tracer = delegate
			{
			};
			connectionString.PreKeyAction?.Invoke(this);
			string stringKey = connectionString.Key as string;
			if (stringKey != null)
			{
				SetKey(stringKey);
			}
			else
			{
				byte[] bytesKey = connectionString.Key as byte[];
				if (bytesKey != null)
				{
					SetKey(bytesKey);
				}
				else if (connectionString.Key != null)
				{
					throw new InvalidOperationException("Encryption keys must be strings or byte arrays");
				}
			}
			connectionString.PostKeyAction?.Invoke(this);
		}

		public void EnableWriteAheadLogging()
		{
			ExecuteScalar<string>("PRAGMA journal_mode=WAL", Array.Empty<object>());
		}

		private static string Quote(string unsafeString)
		{
			if (unsafeString == null)
			{
				return "NULL";
			}
			string safe = unsafeString.Replace("'", "''");
			return "'" + safe + "'";
		}

		private void SetKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			string q = Quote(key);
			Execute("pragma key = " + q);
		}

		private void SetKey(byte[] key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key.Length != 32)
			{
				throw new ArgumentException("Key must be 32 bytes (256-bit)", "key");
			}
			string s = string.Join("", key.Select((byte x) => x.ToString("X2")));
			Execute("pragma key = \"x'" + s + "'\"");
		}

		public void EnableLoadExtension(bool enabled)
		{
			SQLite3.Result r = SQLite3.EnableLoadExtension(Handle, enabled ? 1 : 0);
			if (r != 0)
			{
				string msg = SQLite3.GetErrmsg(Handle);
				throw SQLiteException.New(r, msg);
			}
		}

		private static byte[] GetNullTerminatedUtf8(string s)
		{
			byte[] bytes = new byte[Encoding.UTF8.GetByteCount(s) + 1];
			Encoding.UTF8.GetBytes(s, 0, s.Length, bytes, 0);
			return bytes;
		}

		public TableMapping GetMapping(Type type, CreateFlags createFlags = CreateFlags.None)
		{
			string key = type.FullName;
			lock (_mappings)
			{
				if (_mappings.TryGetValue(key, out var map))
				{
					if (createFlags != 0)
					{
						if (createFlags != map.CreateFlags)
						{
							map = new TableMapping(type, createFlags);
							_mappings[key] = map;
							return map;
						}
						return map;
					}
					return map;
				}
				map = new TableMapping(type, createFlags);
				_mappings.Add(key, map);
				return map;
			}
		}

		public TableMapping GetMapping<T>(CreateFlags createFlags = CreateFlags.None)
		{
			return GetMapping(typeof(T), createFlags);
		}

		public int DropTable<T>()
		{
			return DropTable(GetMapping(typeof(T)));
		}

		public int DropTable(TableMapping map)
		{
			string query = $"drop table if exists \"{map.TableName}\"";
			return Execute(query);
		}

		public CreateTableResult CreateTable<T>(CreateFlags createFlags = CreateFlags.None)
		{
			return CreateTable(typeof(T), createFlags);
		}

		public CreateTableResult CreateTable(Type ty, CreateFlags createFlags = CreateFlags.None)
		{
			TableMapping map = GetMapping(ty, createFlags);
			if (map.Columns.Length == 0)
			{
				throw new Exception($"Cannot create a table without columns (does '{ty.FullName}' have public properties?)");
			}
			CreateTableResult result = CreateTableResult.Created;
			List<ColumnInfo> existingCols = GetTableInfo(map.TableName);
			if (existingCols.Count == 0)
			{
				bool num = (createFlags & CreateFlags.FullTextSearch3) != 0;
				bool fts4 = (createFlags & CreateFlags.FullTextSearch4) != 0;
				string @virtual = ((num || fts4) ? "virtual " : string.Empty);
				string @using = (num ? "using fts3 " : (fts4 ? "using fts4 " : string.Empty));
				string query = "create " + @virtual + "table if not exists \"" + map.TableName + "\" " + @using + "(\n";
				IEnumerable<string> decls = map.Columns.Select((TableMapping.Column p) => Orm.SqlDecl(p, StoreDateTimeAsTicks, StoreTimeSpanAsTicks));
				string decl = string.Join(",\n", decls.ToArray());
				query += decl;
				query += ")";
				if (map.WithoutRowId)
				{
					query += " without rowid";
				}
				Execute(query);
			}
			else
			{
				result = CreateTableResult.Migrated;
				MigrateTable(map, existingCols);
			}
			Dictionary<string, IndexInfo> indexes = new Dictionary<string, IndexInfo>();
			TableMapping.Column[] columns2 = map.Columns;
			foreach (TableMapping.Column c in columns2)
			{
				foreach (IndexedAttribute j in c.Indices)
				{
					string iname = j.Name ?? (map.TableName + "_" + c.Name);
					if (!indexes.TryGetValue(iname, out var iinfo))
					{
						IndexInfo indexInfo = default(IndexInfo);
						indexInfo.IndexName = iname;
						indexInfo.TableName = map.TableName;
						indexInfo.Unique = j.Unique;
						indexInfo.Columns = new List<IndexedColumn>();
						iinfo = indexInfo;
						indexes.Add(iname, iinfo);
					}
					if (j.Unique != iinfo.Unique)
					{
						throw new Exception("All the columns in an index must have the same value for their Unique property");
					}
					iinfo.Columns.Add(new IndexedColumn
					{
						Order = j.Order,
						ColumnName = c.Name
					});
				}
			}
			foreach (string indexName in indexes.Keys)
			{
				IndexInfo index = indexes[indexName];
				string[] columns = (from i in index.Columns
					orderby i.Order
					select i.ColumnName).ToArray();
				CreateIndex(indexName, index.TableName, columns, index.Unique);
			}
			return result;
		}

		public CreateTablesResult CreateTables<T, T2>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()
		{
			return CreateTables(createFlags, typeof(T), typeof(T2));
		}

		public CreateTablesResult CreateTables<T, T2, T3>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()where T3 : new()
		{
			return CreateTables(createFlags, typeof(T), typeof(T2), typeof(T3));
		}

		public CreateTablesResult CreateTables<T, T2, T3, T4>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()where T3 : new()where T4 : new()
		{
			return CreateTables(createFlags, typeof(T), typeof(T2), typeof(T3), typeof(T4));
		}

		public CreateTablesResult CreateTables<T, T2, T3, T4, T5>(CreateFlags createFlags = CreateFlags.None) where T : new()where T2 : new()where T3 : new()where T4 : new()where T5 : new()
		{
			return CreateTables(createFlags, typeof(T), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}

		public CreateTablesResult CreateTables(CreateFlags createFlags = CreateFlags.None, params Type[] types)
		{
			CreateTablesResult result = new CreateTablesResult();
			foreach (Type type in types)
			{
				CreateTableResult aResult = CreateTable(type, createFlags);
				result.Results[type] = aResult;
			}
			return result;
		}

		public int CreateIndex(string indexName, string tableName, string[] columnNames, bool unique = false)
		{
			string sql = string.Format("create {2} index if not exists \"{3}\" on \"{0}\"(\"{1}\")", tableName, string.Join("\", \"", columnNames), unique ? "unique" : "", indexName);
			return Execute(sql);
		}

		public int CreateIndex(string indexName, string tableName, string columnName, bool unique = false)
		{
			return CreateIndex(indexName, tableName, new string[1] { columnName }, unique);
		}

		public int CreateIndex(string tableName, string columnName, bool unique = false)
		{
			return CreateIndex(tableName + "_" + columnName, tableName, columnName, unique);
		}

		public int CreateIndex(string tableName, string[] columnNames, bool unique = false)
		{
			return CreateIndex(tableName + "_" + string.Join("_", columnNames), tableName, columnNames, unique);
		}

		public int CreateIndex<T>(Expression<Func<T, object>> property, bool unique = false)
		{
			MemberExpression mx = ((property.Body.NodeType != ExpressionType.Convert) ? (property.Body as MemberExpression) : (((UnaryExpression)property.Body).Operand as MemberExpression));
			PropertyInfo obj = mx.Member as PropertyInfo;
			if (obj == null)
			{
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
			}
			string propName = obj.Name;
			TableMapping map = GetMapping<T>();
			string colName = map.FindColumnWithPropertyName(propName).Name;
			return CreateIndex(map.TableName, colName, unique);
		}

		public List<ColumnInfo> GetTableInfo(string tableName)
		{
			string query = "pragma table_info(\"" + tableName + "\")";
			return Query<ColumnInfo>(query, Array.Empty<object>());
		}

		private void MigrateTable(TableMapping map, List<ColumnInfo> existingCols)
		{
			List<TableMapping.Column> toBeAdded = new List<TableMapping.Column>();
			TableMapping.Column[] columns = map.Columns;
			foreach (TableMapping.Column p in columns)
			{
				bool found = false;
				foreach (ColumnInfo c in existingCols)
				{
					found = string.Compare(p.Name, c.Name, StringComparison.OrdinalIgnoreCase) == 0;
					if (found)
					{
						break;
					}
				}
				if (!found)
				{
					toBeAdded.Add(p);
				}
			}
			foreach (TableMapping.Column p2 in toBeAdded)
			{
				string addCol = "alter table \"" + map.TableName + "\" add column " + Orm.SqlDecl(p2, StoreDateTimeAsTicks, StoreTimeSpanAsTicks);
				Execute(addCol);
			}
		}

		protected virtual SQLiteCommand NewCommand()
		{
			return new SQLiteCommand(this);
		}

		public SQLiteCommand CreateCommand(string cmdText, params object[] ps)
		{
			if (!_open)
			{
				throw SQLiteException.New(SQLite3.Result.Error, "Cannot create commands from unopened database");
			}
			SQLiteCommand cmd = NewCommand();
			cmd.CommandText = cmdText;
			foreach (object o in ps)
			{
				cmd.Bind(o);
			}
			return cmd;
		}

		public int Execute(string query, params object[] args)
		{
			SQLiteCommand sQLiteCommand = CreateCommand(query, args);
			if (TimeExecution)
			{
				if (_sw == null)
				{
					_sw = new Stopwatch();
				}
				_sw.Reset();
				_sw.Start();
			}
			int result = sQLiteCommand.ExecuteNonQuery();
			if (TimeExecution)
			{
				_sw.Stop();
				_elapsedMilliseconds += _sw.ElapsedMilliseconds;
				Action<string> tracer = Tracer;
				if (tracer == null)
				{
					return result;
				}
				tracer($"Finished in {_sw.ElapsedMilliseconds} ms ({(double)_elapsedMilliseconds / 1000.0:0.0} s total)");
			}
			return result;
		}

		public T ExecuteScalar<T>(string query, params object[] args)
		{
			SQLiteCommand sQLiteCommand = CreateCommand(query, args);
			if (TimeExecution)
			{
				if (_sw == null)
				{
					_sw = new Stopwatch();
				}
				_sw.Reset();
				_sw.Start();
			}
			T result = sQLiteCommand.ExecuteScalar<T>();
			if (TimeExecution)
			{
				_sw.Stop();
				_elapsedMilliseconds += _sw.ElapsedMilliseconds;
				Action<string> tracer = Tracer;
				if (tracer == null)
				{
					return result;
				}
				tracer($"Finished in {_sw.ElapsedMilliseconds} ms ({(double)_elapsedMilliseconds / 1000.0:0.0} s total)");
			}
			return result;
		}

		public List<T> Query<T>(string query, params object[] args) where T : new()
		{
			return CreateCommand(query, args).ExecuteQuery<T>();
		}

		public IEnumerable<T> DeferredQuery<T>(string query, params object[] args) where T : new()
		{
			return CreateCommand(query, args).ExecuteDeferredQuery<T>();
		}

		public List<object> Query(TableMapping map, string query, params object[] args)
		{
			return CreateCommand(query, args).ExecuteQuery<object>(map);
		}

		public IEnumerable<object> DeferredQuery(TableMapping map, string query, params object[] args)
		{
			return CreateCommand(query, args).ExecuteDeferredQuery<object>(map);
		}

		public TableQuery<T> Table<T>() where T : new()
		{
			return new TableQuery<T>(this);
		}

		public T Get<T>(object pk) where T : new()
		{
			TableMapping map = GetMapping(typeof(T));
			return Query<T>(map.GetByPrimaryKeySql, new object[1] { pk }).First();
		}

		public object Get(object pk, TableMapping map)
		{
			return Query(map, map.GetByPrimaryKeySql, pk).First();
		}

		public T Get<T>(Expression<Func<T, bool>> predicate) where T : new()
		{
			return Table<T>().Where(predicate).First();
		}

		public T Find<T>(object pk) where T : new()
		{
			TableMapping map = GetMapping(typeof(T));
			return Query<T>(map.GetByPrimaryKeySql, new object[1] { pk }).FirstOrDefault();
		}

		public object Find(object pk, TableMapping map)
		{
			return Query(map, map.GetByPrimaryKeySql, pk).FirstOrDefault();
		}

		public T Find<T>(Expression<Func<T, bool>> predicate) where T : new()
		{
			return Table<T>().Where(predicate).FirstOrDefault();
		}

		public T FindWithQuery<T>(string query, params object[] args) where T : new()
		{
			return Query<T>(query, args).FirstOrDefault();
		}

		public object FindWithQuery(TableMapping map, string query, params object[] args)
		{
			return Query(map, query, args).FirstOrDefault();
		}

		public void BeginTransaction()
		{
			if (Interlocked.CompareExchange(ref _transactionDepth, 1, 0) == 0)
			{
				try
				{
					Execute("begin transaction");
				}
				catch (Exception ex)
				{
					SQLiteException sqlExp = ex as SQLiteException;
					if (sqlExp != null)
					{
						switch (sqlExp.Result)
						{
						case SQLite3.Result.Busy:
						case SQLite3.Result.NoMem:
						case SQLite3.Result.Interrupt:
						case SQLite3.Result.IOError:
						case SQLite3.Result.Full:
							RollbackTo(null, noThrow: true);
							break;
						}
					}
					else
					{
						Interlocked.Decrement(ref _transactionDepth);
					}
					throw;
				}
				return;
			}
			throw new InvalidOperationException("Cannot begin a transaction while already in a transaction.");
		}

		public string SaveTransactionPoint()
		{
			string retVal = string.Concat(str3: (Interlocked.Increment(ref _transactionDepth) - 1).ToString(), str0: "S", str1: _rand.Next(32767).ToString(), str2: "D");
			try
			{
				Execute("savepoint " + retVal);
				return retVal;
			}
			catch (Exception ex)
			{
				SQLiteException sqlExp = ex as SQLiteException;
				if (sqlExp != null)
				{
					switch (sqlExp.Result)
					{
					case SQLite3.Result.Busy:
					case SQLite3.Result.NoMem:
					case SQLite3.Result.Interrupt:
					case SQLite3.Result.IOError:
					case SQLite3.Result.Full:
						RollbackTo(null, noThrow: true);
						break;
					}
				}
				else
				{
					Interlocked.Decrement(ref _transactionDepth);
				}
				throw;
			}
		}

		public void Rollback()
		{
			RollbackTo(null, noThrow: false);
		}

		public void RollbackTo(string savepoint)
		{
			RollbackTo(savepoint, noThrow: false);
		}

		private void RollbackTo(string savepoint, bool noThrow)
		{
			try
			{
				if (string.IsNullOrEmpty(savepoint))
				{
					if (Interlocked.Exchange(ref _transactionDepth, 0) > 0)
					{
						Execute("rollback");
					}
				}
				else
				{
					DoSavePointExecute(savepoint, "rollback to ");
				}
			}
			catch (SQLiteException)
			{
				if (!noThrow)
				{
					throw;
				}
			}
		}

		public void Release(string savepoint)
		{
			try
			{
				DoSavePointExecute(savepoint, "release ");
			}
			catch (SQLiteException ex)
			{
				if (ex.Result == SQLite3.Result.Busy)
				{
					try
					{
						Execute("rollback");
					}
					catch
					{
					}
				}
				throw;
			}
		}

		private void DoSavePointExecute(string savepoint, string cmd)
		{
			int firstLen = savepoint.IndexOf('D');
			if (firstLen >= 2 && savepoint.Length > firstLen + 1 && int.TryParse(savepoint.Substring(firstLen + 1), out var depth) && 0 <= depth && depth < _transactionDepth)
			{
				Thread.VolatileWrite(ref _transactionDepth, depth);
				Execute(cmd + savepoint);
				return;
			}
			throw new ArgumentException("savePoint is not valid, and should be the result of a call to SaveTransactionPoint.", "savePoint");
		}

		public void Commit()
		{
			if (Interlocked.Exchange(ref _transactionDepth, 0) == 0)
			{
				return;
			}
			try
			{
				Execute("commit");
			}
			catch
			{
				try
				{
					Execute("rollback");
				}
				catch
				{
				}
				throw;
			}
		}

		public void RunInTransaction(Action action)
		{
			try
			{
				string savePoint = SaveTransactionPoint();
				action();
				Release(savePoint);
			}
			catch (Exception)
			{
				Rollback();
				throw;
			}
		}

		public int InsertAll(IEnumerable objects, bool runInTransaction = true)
		{
			int c = 0;
			if (runInTransaction)
			{
				RunInTransaction(delegate
				{
					foreach (object current in objects)
					{
						c += Insert(current);
					}
				});
			}
			else
			{
				foreach (object r in objects)
				{
					c += Insert(r);
				}
			}
			return c;
		}

		public int InsertAll(IEnumerable objects, string extra, bool runInTransaction = true)
		{
			int c = 0;
			if (runInTransaction)
			{
				RunInTransaction(delegate
				{
					foreach (object current in objects)
					{
						c += Insert(current, extra);
					}
				});
			}
			else
			{
				foreach (object r in objects)
				{
					c += Insert(r);
				}
			}
			return c;
		}

		public int InsertAll(IEnumerable objects, Type objType, bool runInTransaction = true)
		{
			int c = 0;
			if (runInTransaction)
			{
				RunInTransaction(delegate
				{
					foreach (object current in objects)
					{
						c += Insert(current, objType);
					}
				});
			}
			else
			{
				foreach (object r in objects)
				{
					c += Insert(r, objType);
				}
			}
			return c;
		}

		public int Insert(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return Insert(obj, "", Orm.GetType(obj));
		}

		public int InsertOrReplace(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return Insert(obj, "OR REPLACE", Orm.GetType(obj));
		}

		public int Insert(object obj, Type objType)
		{
			return Insert(obj, "", objType);
		}

		public int InsertOrReplace(object obj, Type objType)
		{
			return Insert(obj, "OR REPLACE", objType);
		}

		public int Insert(object obj, string extra)
		{
			if (obj == null)
			{
				return 0;
			}
			return Insert(obj, extra, Orm.GetType(obj));
		}

		public int Insert(object obj, string extra, Type objType)
		{
			if (obj == null || objType == null)
			{
				return 0;
			}
			TableMapping map = GetMapping(objType);
			if (map.PK != null && map.PK.IsAutoGuid && map.PK.GetValue(obj).Equals(Guid.Empty))
			{
				map.PK.SetValue(obj, Guid.NewGuid());
			}
			TableMapping.Column[] cols = ((string.Compare(extra, "OR REPLACE", StringComparison.OrdinalIgnoreCase) == 0) ? map.InsertOrReplaceColumns : map.InsertColumns);
			object[] vals = new object[cols.Length];
			for (int i = 0; i < vals.Length; i++)
			{
				vals[i] = cols[i].GetValue(obj);
			}
			PreparedSqlLiteInsertCommand insertCmd = GetInsertCommand(map, extra);
			int count;
			lock (insertCmd)
			{
				try
				{
					count = insertCmd.ExecuteNonQuery(vals);
				}
				catch (SQLiteException ex)
				{
					if (SQLite3.ExtendedErrCode(Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
					{
						throw NotNullConstraintViolationException.New(ex.Result, ex.Message, map, obj);
					}
					throw;
				}
				if (map.HasAutoIncPK)
				{
					long id = SQLite3.LastInsertRowid(Handle);
					map.SetAutoIncPK(obj, id);
				}
			}
			if (count > 0)
			{
				OnTableChanged(map, NotifyTableChangedAction.Insert);
			}
			return count;
		}

		private PreparedSqlLiteInsertCommand GetInsertCommand(TableMapping map, string extra)
		{
			Tuple<string, string> key = Tuple.Create(map.MappedType.FullName, extra);
			PreparedSqlLiteInsertCommand prepCmd;
			lock (_insertCommandMap)
			{
				if (_insertCommandMap.TryGetValue(key, out prepCmd))
				{
					return prepCmd;
				}
			}
			prepCmd = CreateInsertCommand(map, extra);
			lock (_insertCommandMap)
			{
				if (_insertCommandMap.TryGetValue(key, out var existing))
				{
					prepCmd.Dispose();
					return existing;
				}
				_insertCommandMap.Add(key, prepCmd);
				return prepCmd;
			}
		}

		private PreparedSqlLiteInsertCommand CreateInsertCommand(TableMapping map, string extra)
		{
			TableMapping.Column[] cols = map.InsertColumns;
			string insertSql;
			if (cols.Length == 0 && map.Columns.Length == 1 && map.Columns[0].IsAutoInc)
			{
				insertSql = string.Format("insert {1} into \"{0}\" default values", map.TableName, extra);
			}
			else
			{
				if (string.Compare(extra, "OR REPLACE", StringComparison.OrdinalIgnoreCase) == 0)
				{
					cols = map.InsertOrReplaceColumns;
				}
				insertSql = string.Format("insert {3} into \"{0}\"({1}) values ({2})", map.TableName, string.Join(",", cols.Select((TableMapping.Column c) => "\"" + c.Name + "\"").ToArray()), string.Join(",", cols.Select((TableMapping.Column c) => "?").ToArray()), extra);
			}
			return new PreparedSqlLiteInsertCommand(this, insertSql);
		}

		public int Update(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return Update(obj, Orm.GetType(obj));
		}

		public int Update(object obj, Type objType)
		{
			int rowsAffected = 0;
			if (obj == null || objType == null)
			{
				return 0;
			}
			TableMapping map = GetMapping(objType);
			TableMapping.Column pk = map.PK;
			if (pk == null)
			{
				throw new NotSupportedException("Cannot update " + map.TableName + ": it has no PK");
			}
			IEnumerable<TableMapping.Column> cols = map.Columns.Where((TableMapping.Column p) => p != pk);
			List<object> ps = new List<object>(cols.Select((TableMapping.Column c) => c.GetValue(obj)));
			if (ps.Count == 0)
			{
				cols = map.Columns;
				ps = new List<object>(cols.Select((TableMapping.Column c) => c.GetValue(obj)));
			}
			ps.Add(pk.GetValue(obj));
			string q = string.Format("update \"{0}\" set {1} where {2} = ? ", map.TableName, string.Join(",", cols.Select((TableMapping.Column c) => "\"" + c.Name + "\" = ? ").ToArray()), pk.Name);
			try
			{
				rowsAffected = Execute(q, ps.ToArray());
			}
			catch (SQLiteException ex)
			{
				if (ex.Result == SQLite3.Result.Constraint && SQLite3.ExtendedErrCode(Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
				{
					throw NotNullConstraintViolationException.New(ex, map, obj);
				}
				throw ex;
			}
			if (rowsAffected > 0)
			{
				OnTableChanged(map, NotifyTableChangedAction.Update);
			}
			return rowsAffected;
		}

		public int UpdateAll(IEnumerable objects, bool runInTransaction = true)
		{
			int c = 0;
			if (runInTransaction)
			{
				RunInTransaction(delegate
				{
					foreach (object current in objects)
					{
						c += Update(current);
					}
				});
			}
			else
			{
				foreach (object r in objects)
				{
					c += Update(r);
				}
			}
			return c;
		}

		public int Delete(object objectToDelete)
		{
			TableMapping map = GetMapping(Orm.GetType(objectToDelete));
			TableMapping.Column pk = map.PK;
			if (pk == null)
			{
				throw new NotSupportedException("Cannot delete " + map.TableName + ": it has no PK");
			}
			string q = $"delete from \"{map.TableName}\" where \"{pk.Name}\" = ?";
			int num = Execute(q, pk.GetValue(objectToDelete));
			if (num > 0)
			{
				OnTableChanged(map, NotifyTableChangedAction.Delete);
			}
			return num;
		}

		public int Delete<T>(object primaryKey)
		{
			return Delete(primaryKey, GetMapping(typeof(T)));
		}

		public int Delete(object primaryKey, TableMapping map)
		{
			TableMapping.Column pk = map.PK;
			if (pk == null)
			{
				throw new NotSupportedException("Cannot delete " + map.TableName + ": it has no PK");
			}
			string q = $"delete from \"{map.TableName}\" where \"{pk.Name}\" = ?";
			int num = Execute(q, primaryKey);
			if (num > 0)
			{
				OnTableChanged(map, NotifyTableChangedAction.Delete);
			}
			return num;
		}

		public int DeleteAll<T>()
		{
			TableMapping map = GetMapping(typeof(T));
			return DeleteAll(map);
		}

		public int DeleteAll(TableMapping map)
		{
			string query = $"delete from \"{map.TableName}\"";
			int num = Execute(query);
			if (num > 0)
			{
				OnTableChanged(map, NotifyTableChangedAction.Delete);
			}
			return num;
		}

		public void Backup(string destinationDatabasePath, string databaseName = "main")
		{
			SQLite3.Result r = SQLite3.Open(destinationDatabasePath, out var destHandle);
			if (r != 0)
			{
				throw SQLiteException.New(r, "Failed to open destination database");
			}
			IntPtr intPtr = SQLite3.BackupInit(destHandle, databaseName, Handle, databaseName);
			if (intPtr == NullBackupHandle)
			{
				SQLite3.Close(destHandle);
				throw new Exception("Failed to create backup");
			}
			SQLite3.BackupStep(intPtr, -1);
			SQLite3.BackupFinish(intPtr);
			r = SQLite3.GetResult(destHandle);
			string msg = "";
			if (r != 0)
			{
				msg = SQLite3.GetErrmsg(destHandle);
			}
			SQLite3.Close(destHandle);
			if (r != 0)
			{
				throw SQLiteException.New(r, msg);
			}
		}

		~SQLiteConnection()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public void Close()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
			bool useClose2 = LibVersionNumber >= 3007014;
			if (!_open || !(Handle != NullHandle))
			{
				return;
			}
			try
			{
				if (disposing)
				{
					lock (_insertCommandMap)
					{
						foreach (PreparedSqlLiteInsertCommand value in _insertCommandMap.Values)
						{
							value.Dispose();
						}
						_insertCommandMap.Clear();
					}
					SQLite3.Result r = (useClose2 ? SQLite3.Close2(Handle) : SQLite3.Close(Handle));
					if (r != 0)
					{
						string msg = SQLite3.GetErrmsg(Handle);
						throw SQLiteException.New(r, msg);
					}
				}
				else if (!useClose2)
				{
					SQLite3.Close(Handle);
				}
				else
				{
					SQLite3.Close2(Handle);
				}
			}
			finally
			{
				Handle = NullHandle;
				_open = false;
			}
		}

		private void OnTableChanged(TableMapping table, NotifyTableChangedAction action)
		{
			this.TableChanged?.Invoke(this, new NotifyTableChangedEventArgs(table, action));
		}
	}
}
