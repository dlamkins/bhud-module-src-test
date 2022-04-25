using System;
using System.Threading;

namespace SQLite
{
	public class SQLiteConnectionWithLock : SQLiteConnection
	{
		private class LockWrapper : IDisposable
		{
			private object _lockPoint;

			public LockWrapper(object lockPoint)
			{
				_lockPoint = lockPoint;
				Monitor.Enter(_lockPoint);
			}

			public void Dispose()
			{
				Monitor.Exit(_lockPoint);
			}
		}

		private class FakeLockWrapper : IDisposable
		{
			public void Dispose()
			{
			}
		}

		private readonly object _lockPoint = new object();

		public bool SkipLock { get; set; }

		public SQLiteConnectionWithLock(SQLiteConnectionString connectionString)
			: base(connectionString)
		{
		}

		public IDisposable Lock()
		{
			if (!SkipLock)
			{
				return new LockWrapper(_lockPoint);
			}
			return new FakeLockWrapper();
		}
	}
}
