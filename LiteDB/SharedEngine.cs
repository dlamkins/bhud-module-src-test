using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using LiteDB.Engine;

namespace LiteDB
{
	public class SharedEngine : ILiteEngine, IDisposable
	{
		private readonly EngineSettings _settings;

		private readonly Mutex _mutex;

		private LiteEngine _engine;

		private bool _transactionRunning;

		public SharedEngine(EngineSettings settings)
		{
			_settings = settings;
			string name = Path.GetFullPath(settings.Filename).ToLower().Sha1();
			try
			{
				_mutex = new Mutex(initiallyOwned: false, "Global\\" + name + ".Mutex");
			}
			catch (NotSupportedException ex)
			{
				throw new PlatformNotSupportedException("Shared mode is not supported in platforms that do not implement named mutex.", ex);
			}
		}

		private void OpenDatabase()
		{
			try
			{
				_mutex.WaitOne();
			}
			catch (AbandonedMutexException)
			{
			}
			if (!_transactionRunning && _engine == null)
			{
				try
				{
					_engine = new LiteEngine(_settings);
				}
				catch
				{
					_mutex.ReleaseMutex();
					throw;
				}
			}
		}

		private void CloseDatabase()
		{
			if (!_transactionRunning && _engine != null)
			{
				_engine.Dispose();
				_engine = null;
			}
			_mutex.ReleaseMutex();
		}

		public bool BeginTrans()
		{
			OpenDatabase();
			try
			{
				_transactionRunning = _engine.BeginTrans();
				return _transactionRunning;
			}
			catch
			{
				CloseDatabase();
				throw;
			}
		}

		public bool Commit()
		{
			if (_engine == null)
			{
				return false;
			}
			try
			{
				return _engine.Commit();
			}
			finally
			{
				_transactionRunning = false;
				CloseDatabase();
			}
		}

		public bool Rollback()
		{
			if (_engine == null)
			{
				return false;
			}
			try
			{
				return _engine.Rollback();
			}
			finally
			{
				_transactionRunning = false;
				CloseDatabase();
			}
		}

		public IBsonDataReader Query(string collection, Query query)
		{
			OpenDatabase();
			return new SharedDataReader(_engine.Query(collection, query), delegate
			{
				CloseDatabase();
			});
		}

		public BsonValue Pragma(string name)
		{
			OpenDatabase();
			try
			{
				return _engine.Pragma(name);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public bool Pragma(string name, BsonValue value)
		{
			OpenDatabase();
			try
			{
				return _engine.Pragma(name, value);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public int Checkpoint()
		{
			OpenDatabase();
			try
			{
				return _engine.Checkpoint();
			}
			finally
			{
				CloseDatabase();
			}
		}

		public long Rebuild(RebuildOptions options)
		{
			OpenDatabase();
			try
			{
				return _engine.Rebuild(options);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public int Insert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId)
		{
			OpenDatabase();
			try
			{
				return _engine.Insert(collection, docs, autoId);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public int Update(string collection, IEnumerable<BsonDocument> docs)
		{
			OpenDatabase();
			try
			{
				return _engine.Update(collection, docs);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public int UpdateMany(string collection, BsonExpression extend, BsonExpression predicate)
		{
			OpenDatabase();
			try
			{
				return _engine.UpdateMany(collection, extend, predicate);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public int Upsert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId)
		{
			OpenDatabase();
			try
			{
				return _engine.Upsert(collection, docs, autoId);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public int Delete(string collection, IEnumerable<BsonValue> ids)
		{
			OpenDatabase();
			try
			{
				return _engine.Delete(collection, ids);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public int DeleteMany(string collection, BsonExpression predicate)
		{
			OpenDatabase();
			try
			{
				return _engine.DeleteMany(collection, predicate);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public bool DropCollection(string name)
		{
			OpenDatabase();
			try
			{
				return _engine.DropCollection(name);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public bool RenameCollection(string name, string newName)
		{
			OpenDatabase();
			try
			{
				return _engine.RenameCollection(name, newName);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public bool DropIndex(string collection, string name)
		{
			OpenDatabase();
			try
			{
				return _engine.DropIndex(collection, name);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public bool EnsureIndex(string collection, string name, BsonExpression expression, bool unique)
		{
			OpenDatabase();
			try
			{
				return _engine.EnsureIndex(collection, name, expression, unique);
			}
			finally
			{
				CloseDatabase();
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~SharedEngine()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && _engine != null)
			{
				_engine.Dispose();
				_engine = null;
				_mutex.ReleaseMutex();
			}
		}
	}
}
