using System;
using System.Collections.Concurrent;
using System.Threading;

namespace LiteDB.Engine
{
	internal class LockService : IDisposable
	{
		private readonly EnginePragmas _pragmas;

		private readonly ReaderWriterLockSlim _transaction = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		private readonly ConcurrentDictionary<string, object> _collections = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

		public bool IsInTransaction
		{
			get
			{
				if (!_transaction.IsReadLockHeld)
				{
					return _transaction.IsWriteLockHeld;
				}
				return true;
			}
		}

		public int TransactionsCount => _transaction.CurrentReadCount;

		internal LockService(EnginePragmas pragmas)
		{
			_pragmas = pragmas;
		}

		public void EnterTransaction()
		{
			if (_transaction.IsWriteLockHeld || _transaction.TryEnterReadLock(_pragmas.Timeout))
			{
				return;
			}
			throw LiteException.LockTimeout("transaction", _pragmas.Timeout);
		}

		public void ExitTransaction()
		{
			if (!_transaction.IsWriteLockHeld && _transaction.IsReadLockHeld)
			{
				try
				{
					_transaction.ExitReadLock();
				}
				catch
				{
				}
			}
		}

		public void EnterLock(string collectionName)
		{
			Constants.ENSURE(_transaction.IsReadLockHeld || _transaction.IsWriteLockHeld, "Use EnterTransaction() before EnterLock(name)");
			if (!Monitor.TryEnter(_collections.GetOrAdd(collectionName, (string s) => new object()), _pragmas.Timeout))
			{
				throw LiteException.LockTimeout("write", collectionName, _pragmas.Timeout);
			}
		}

		public void ExitLock(string collectionName)
		{
			if (!_collections.TryGetValue(collectionName, out var collection))
			{
				throw LiteException.CollectionLockerNotFound(collectionName);
			}
			Monitor.Exit(collection);
		}

		public bool EnterExclusive()
		{
			if (_transaction.IsWriteLockHeld)
			{
				return false;
			}
			if (!_transaction.TryEnterWriteLock(_pragmas.Timeout))
			{
				throw LiteException.LockTimeout("exclusive", _pragmas.Timeout);
			}
			return true;
		}

		public bool TryEnterExclusive(out bool mustExit)
		{
			if (_transaction.IsWriteLockHeld)
			{
				mustExit = false;
				return true;
			}
			if (_transaction.IsReadLockHeld || _transaction.CurrentReadCount > 0)
			{
				mustExit = false;
				return false;
			}
			if (!_transaction.TryEnterWriteLock(10))
			{
				mustExit = false;
				return false;
			}
			Constants.ENSURE(_transaction.RecursiveReadCount == 0, "must have no other transaction here");
			mustExit = true;
			return true;
		}

		public void ExitExclusive()
		{
			_transaction.ExitWriteLock();
		}

		public void Dispose()
		{
			try
			{
				_transaction.Dispose();
			}
			catch (SynchronizationLockException)
			{
			}
		}
	}
}
