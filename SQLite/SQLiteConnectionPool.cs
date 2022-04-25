using System;
using System.Collections.Generic;

namespace SQLite
{
	internal class SQLiteConnectionPool
	{
		private class Entry
		{
			private WeakReference<SQLiteConnectionWithLock> connection;

			public SQLiteConnectionString ConnectionString { get; }

			public Entry(SQLiteConnectionString connectionString)
			{
				ConnectionString = connectionString;
			}

			public SQLiteConnectionWithLock Connect()
			{
				SQLiteConnectionWithLock c = null;
				WeakReference<SQLiteConnectionWithLock> wc = connection;
				if (wc == null || !wc.TryGetTarget(out c))
				{
					c = new SQLiteConnectionWithLock(ConnectionString);
					if (ConnectionString.OpenFlags.HasFlag(SQLiteOpenFlags.FullMutex))
					{
						c.SkipLock = true;
					}
					connection = new WeakReference<SQLiteConnectionWithLock>(c);
				}
				return c;
			}

			public void Close()
			{
				WeakReference<SQLiteConnectionWithLock> wc = connection;
				if (wc != null && wc.TryGetTarget(out var c))
				{
					c.Close();
				}
				connection = null;
			}
		}

		private readonly Dictionary<string, Entry> _entries = new Dictionary<string, Entry>();

		private readonly object _entriesLock = new object();

		private static readonly SQLiteConnectionPool _shared = new SQLiteConnectionPool();

		public static SQLiteConnectionPool Shared => _shared;

		public SQLiteConnectionWithLock GetConnection(SQLiteConnectionString connectionString)
		{
			string key = connectionString.UniqueKey;
			Entry entry;
			lock (_entriesLock)
			{
				if (!_entries.TryGetValue(key, out entry))
				{
					entry = new Entry(connectionString);
					_entries[key] = entry;
				}
			}
			return entry.Connect();
		}

		public void CloseConnection(SQLiteConnectionString connectionString)
		{
			string key = connectionString.UniqueKey;
			Entry entry;
			lock (_entriesLock)
			{
				if (_entries.TryGetValue(key, out entry))
				{
					_entries.Remove(key);
				}
			}
			entry?.Close();
		}

		public void Reset()
		{
			List<Entry> entries;
			lock (_entriesLock)
			{
				entries = new List<Entry>(_entries.Values);
				_entries.Clear();
			}
			foreach (Entry item in entries)
			{
				item.Close();
			}
		}
	}
}
