using System;
using System.Globalization;

namespace SQLite
{
	public class SQLiteConnectionString
	{
		private const string DateTimeSqliteDefaultFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";

		public string UniqueKey { get; }

		public string DatabasePath { get; }

		public bool StoreDateTimeAsTicks { get; }

		public bool StoreTimeSpanAsTicks { get; }

		public string DateTimeStringFormat { get; }

		public DateTimeStyles DateTimeStyle { get; }

		public object Key { get; }

		public SQLiteOpenFlags OpenFlags { get; }

		public Action<SQLiteConnection> PreKeyAction { get; }

		public Action<SQLiteConnection> PostKeyAction { get; }

		public string VfsName { get; }

		public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks = true)
			: this(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks)
		{
		}

		public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks, object key = null, Action<SQLiteConnection> preKeyAction = null, Action<SQLiteConnection> postKeyAction = null, string vfsName = null)
			: this(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks, key, preKeyAction, postKeyAction, vfsName)
		{
		}

		public SQLiteConnectionString(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks, object key = null, Action<SQLiteConnection> preKeyAction = null, Action<SQLiteConnection> postKeyAction = null, string vfsName = null, string dateTimeStringFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", bool storeTimeSpanAsTicks = true)
		{
			if (key != null && !(key is byte[]) && !(key is string))
			{
				throw new ArgumentException("Encryption keys must be strings or byte arrays", "key");
			}
			UniqueKey = $"{databasePath}_{(uint)openFlags:X8}";
			StoreDateTimeAsTicks = storeDateTimeAsTicks;
			StoreTimeSpanAsTicks = storeTimeSpanAsTicks;
			DateTimeStringFormat = dateTimeStringFormat;
			DateTimeStyle = (("o".Equals(DateTimeStringFormat, StringComparison.OrdinalIgnoreCase) || "r".Equals(DateTimeStringFormat, StringComparison.OrdinalIgnoreCase)) ? DateTimeStyles.RoundtripKind : DateTimeStyles.None);
			Key = key;
			PreKeyAction = preKeyAction;
			PostKeyAction = postKeyAction;
			OpenFlags = openFlags;
			VfsName = vfsName;
			DatabasePath = databasePath;
		}
	}
}
