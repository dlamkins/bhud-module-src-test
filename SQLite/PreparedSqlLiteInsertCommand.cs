using System;

namespace SQLite
{
	internal class PreparedSqlLiteInsertCommand : IDisposable
	{
		private bool Initialized;

		private SQLiteConnection Connection;

		private string CommandText;

		private IntPtr Statement;

		private static readonly IntPtr NullStatement;

		public PreparedSqlLiteInsertCommand(SQLiteConnection conn, string commandText)
		{
			Connection = conn;
			CommandText = commandText;
		}

		public int ExecuteNonQuery(object[] source)
		{
			if (Initialized && Statement == NullStatement)
			{
				throw new ObjectDisposedException("PreparedSqlLiteInsertCommand");
			}
			if (Connection.Trace)
			{
				Connection.Tracer?.Invoke("Executing: " + CommandText);
			}
			SQLite3.Result r = SQLite3.Result.OK;
			if (!Initialized)
			{
				Statement = SQLite3.Prepare2(Connection.Handle, CommandText);
				Initialized = true;
			}
			if (source != null)
			{
				for (int i = 0; i < source.Length; i++)
				{
					SQLiteCommand.BindParameter(Statement, i + 1, source[i], Connection.StoreDateTimeAsTicks, Connection.DateTimeStringFormat, Connection.StoreTimeSpanAsTicks);
				}
			}
			r = SQLite3.Step(Statement);
			switch (r)
			{
			case SQLite3.Result.Done:
			{
				int result = SQLite3.Changes(Connection.Handle);
				SQLite3.Reset(Statement);
				return result;
			}
			case SQLite3.Result.Error:
			{
				string msg = SQLite3.GetErrmsg(Connection.Handle);
				SQLite3.Reset(Statement);
				throw SQLiteException.New(r, msg);
			}
			case SQLite3.Result.Constraint:
				if (SQLite3.ExtendedErrCode(Connection.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
				{
					SQLite3.Reset(Statement);
					throw NotNullConstraintViolationException.New(r, SQLite3.GetErrmsg(Connection.Handle));
				}
				break;
			}
			SQLite3.Reset(Statement);
			throw SQLiteException.New(r, r.ToString());
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			IntPtr s = Statement;
			Statement = NullStatement;
			Connection = null;
			if (s != NullStatement)
			{
				SQLite3.Finalize(s);
			}
		}

		~PreparedSqlLiteInsertCommand()
		{
			Dispose(disposing: false);
		}
	}
}
