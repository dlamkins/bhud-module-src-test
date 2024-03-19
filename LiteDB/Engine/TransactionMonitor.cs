using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LiteDB.Engine
{
	internal class TransactionMonitor : IDisposable
	{
		private readonly Dictionary<uint, TransactionService> _transactions = new Dictionary<uint, TransactionService>();

		private readonly ThreadLocal<TransactionService> _slot = new ThreadLocal<TransactionService>();

		private readonly HeaderPage _header;

		private readonly LockService _locker;

		private readonly DiskService _disk;

		private readonly WalIndexService _walIndex;

		private int _freePages;

		private readonly int _initialSize;

		public ICollection<TransactionService> Transactions => _transactions.Values;

		public int FreePages => _freePages;

		public int InitialSize => _initialSize;

		public TransactionMonitor(HeaderPage header, LockService locker, DiskService disk, WalIndexService walIndex)
		{
			_header = header;
			_locker = locker;
			_disk = disk;
			_walIndex = walIndex;
			_freePages = 100000;
			_initialSize = 1000;
		}

		public TransactionService GetTransaction(bool create, bool queryOnly, out bool isNew)
		{
			TransactionService transaction = _slot.Value;
			if (create && transaction == null)
			{
				isNew = true;
				bool alreadyLock;
				lock (_transactions)
				{
					if (_transactions.Count >= 100)
					{
						throw new LiteException(0, "Maximum number of transactions reached");
					}
					int initialSize = GetInitialSize();
					alreadyLock = _transactions.Values.Any((TransactionService x) => x.ThreadID == Environment.CurrentManagedThreadId);
					transaction = new TransactionService(_header, _locker, _disk, _walIndex, initialSize, this, queryOnly);
					_transactions[transaction.TransactionID] = transaction;
				}
				if (!alreadyLock)
				{
					try
					{
						_locker.EnterTransaction();
					}
					catch
					{
						transaction.Dispose();
						lock (_transactions)
						{
							_freePages += transaction.MaxTransactionSize;
							_transactions.Remove(transaction.TransactionID);
						}
						throw;
					}
				}
				if (!queryOnly)
				{
					_slot.Value = transaction;
				}
			}
			else
			{
				isNew = false;
			}
			return transaction;
		}

		public void ReleaseTransaction(TransactionService transaction)
		{
			transaction.Dispose();
			bool keepLocked;
			lock (_transactions)
			{
				_transactions.Remove(transaction.TransactionID);
				_freePages += transaction.MaxTransactionSize;
				keepLocked = _transactions.Values.Any((TransactionService x) => x.ThreadID == Environment.CurrentManagedThreadId);
			}
			if (!keepLocked)
			{
				_locker.ExitTransaction();
			}
			if (!transaction.QueryOnly)
			{
				Constants.ENSURE(_slot.Value == transaction, "current thread must contains transaction parameter");
				_slot.Value = null;
			}
		}

		public TransactionService GetThreadTransaction()
		{
			lock (_transactions)
			{
				return _slot.Value ?? _transactions.Values.FirstOrDefault((TransactionService x) => x.ThreadID == Environment.CurrentManagedThreadId);
			}
		}

		private int GetInitialSize()
		{
			if (_freePages >= _initialSize)
			{
				_freePages -= _initialSize;
				return _initialSize;
			}
			int sum = 0;
			foreach (TransactionService value in _transactions.Values)
			{
				int reduce = value.MaxTransactionSize / _initialSize;
				value.MaxTransactionSize -= reduce;
				sum += reduce;
			}
			return sum;
		}

		private bool TryExtend(TransactionService trans)
		{
			lock (_transactions)
			{
				if (_freePages >= _initialSize)
				{
					trans.MaxTransactionSize += _initialSize;
					_freePages -= _initialSize;
					return true;
				}
				return false;
			}
		}

		public bool CheckSafepoint(TransactionService trans)
		{
			if (trans.Pages.TransactionSize >= trans.MaxTransactionSize)
			{
				return !TryExtend(trans);
			}
			return false;
		}

		public void Dispose()
		{
			if (_transactions.Count <= 0)
			{
				return;
			}
			foreach (TransactionService value in _transactions.Values)
			{
				value.Dispose();
			}
			_transactions.Clear();
		}
	}
}
