using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class TransactionService : IDisposable
	{
		private readonly HeaderPage _header;

		private readonly LockService _locker;

		private readonly DiskService _disk;

		private readonly DiskReader _reader;

		private readonly WalIndexService _walIndex;

		private readonly TransactionMonitor _monitor;

		private readonly Dictionary<string, Snapshot> _snapshots = new Dictionary<string, Snapshot>(StringComparer.OrdinalIgnoreCase);

		private readonly TransactionPages _transPages = new TransactionPages();

		private readonly int _threadID = Environment.CurrentManagedThreadId;

		private readonly uint _transactionID;

		private readonly DateTime _startTime;

		private LockMode _mode;

		private TransactionState _state;

		public int ThreadID => _threadID;

		public uint TransactionID => _transactionID;

		public TransactionState State => _state;

		public LockMode Mode => _mode;

		public TransactionPages Pages => _transPages;

		public DateTime StartTime => _startTime;

		public IEnumerable<Snapshot> Snapshots => _snapshots.Values;

		public bool QueryOnly { get; }

		public int MaxTransactionSize { get; set; }

		public List<CursorInfo> OpenCursors { get; } = new List<CursorInfo>();


		public bool ExplicitTransaction { get; set; }

		public TransactionService(HeaderPage header, LockService locker, DiskService disk, WalIndexService walIndex, int maxTransactionSize, TransactionMonitor monitor, bool queryOnly)
		{
			_header = header;
			_locker = locker;
			_disk = disk;
			_walIndex = walIndex;
			_monitor = monitor;
			QueryOnly = queryOnly;
			MaxTransactionSize = maxTransactionSize;
			_transactionID = walIndex.NextTransactionID();
			_startTime = DateTime.UtcNow;
			_reader = _disk.GetReader();
		}

		~TransactionService()
		{
			Dispose(dispose: false);
		}

		public Snapshot CreateSnapshot(LockMode mode, string collection, bool addIfNotExists)
		{
			Constants.ENSURE(_state == TransactionState.Active, "transaction must be active to create new snapshot");
			if (!_snapshots.TryGetValue(collection, out var snapshot))
			{
				snapshot = (_snapshots[collection] = create());
			}
			else if ((mode == LockMode.Write && snapshot.Mode == LockMode.Read) || (addIfNotExists && snapshot.CollectionPage == null))
			{
				snapshot.Dispose();
				_snapshots.Remove(collection);
				snapshot = (_snapshots[collection] = create());
			}
			if (mode == LockMode.Write)
			{
				_mode = LockMode.Write;
			}
			return snapshot;
			Snapshot create()
			{
				return new Snapshot(mode, collection, _header, _transactionID, _transPages, _locker, _walIndex, _reader, addIfNotExists);
			}
		}

		public void Safepoint()
		{
			if (_state != 0)
			{
				throw new LiteException(0, "This transaction are invalid state");
			}
			if (!_monitor.CheckSafepoint(this))
			{
				return;
			}
			if (_mode == LockMode.Write)
			{
				PersistDirtyPages(commit: false);
			}
			foreach (Snapshot value in _snapshots.Values)
			{
				value.Clear();
			}
			_transPages.TransactionSize = 0;
		}

		private int PersistDirtyPages(bool commit)
		{
			int dirty = 0;
			int result = _disk.WriteAsync(source());
			_disk.DiscardCleanPages(from x in _snapshots.Values.Where((Snapshot x) => x.Mode == LockMode.Write).SelectMany((Snapshot x) => x.GetWritablePages(dirty: false, commit))
				select x.Buffer);
			return result;
			IEnumerable<PageBuffer> source()
			{
				IEnumerable<BasePage> pages = _snapshots.Values.Where((Snapshot x) => x.Mode == LockMode.Write).SelectMany((Snapshot x) => x.GetWritablePages(dirty: true, commit));
				bool markLastAsConfirmed = commit && !_transPages.HeaderChanged;
				foreach (LastItem<BasePage> page in pages.IsLast())
				{
					page.Item.TransactionID = _transactionID;
					if (page.IsLast)
					{
						page.Item.IsConfirmed = markLastAsConfirmed;
					}
					if (_transPages.LastDeletedPageID == page.Item.PageID && commit)
					{
						Constants.ENSURE(_transPages.HeaderChanged, "must header be in lock");
						Constants.ENSURE(page.Item.PageType == PageType.Empty, "must be marked as deleted page");
						page.Item.NextPageID = _header.FreeEmptyPageList;
						_header.FreeEmptyPageList = _transPages.FirstDeletedPageID;
					}
					PageBuffer buffer2 = page.Item.UpdateBuffer();
					yield return buffer2;
					dirty++;
					_transPages.DirtyPages[page.Item.PageID] = new PagePosition(page.Item.PageID, buffer2.Position);
				}
				if (commit && _transPages.HeaderChanged)
				{
					lock (_header)
					{
						_header.TransactionID = _transactionID;
						_header.IsConfirmed = true;
						_transPages.OnCommit(_header);
						PageBuffer buffer = _header.UpdateBuffer();
						PageBuffer clone = _disk.NewPage();
						Buffer.BlockCopy(buffer.Array, buffer.Offset, clone.Array, clone.Offset, clone.Count);
						yield return clone;
					}
				}
			}
		}

		public void Commit()
		{
			Constants.ENSURE(_state == TransactionState.Active, $"transaction must be active to commit (current state: {_state})");
			if ((_mode == LockMode.Write || _transPages.HeaderChanged) && PersistDirtyPages(commit: true) > 0)
			{
				_walIndex.ConfirmTransaction(_transactionID, _transPages.DirtyPages.Values);
			}
			foreach (Snapshot value in _snapshots.Values)
			{
				value.Dispose();
			}
			_state = TransactionState.Committed;
		}

		public void Rollback()
		{
			Constants.ENSURE(_state == TransactionState.Active, $"transaction must be active to rollback (current state: {_state})");
			if (_transPages.NewPages.Count > 0)
			{
				ReturnNewPages();
			}
			foreach (Snapshot snapshot in _snapshots.Values)
			{
				if (snapshot.Mode == LockMode.Write)
				{
					_disk.DiscardDirtyPages(from x in snapshot.GetWritablePages(dirty: true, includeCollectionPage: true)
						select x.Buffer);
					_disk.DiscardCleanPages(from x in snapshot.GetWritablePages(dirty: false, includeCollectionPage: true)
						select x.Buffer);
				}
				snapshot.Dispose();
			}
			_state = TransactionState.Aborted;
		}

		private void ReturnNewPages()
		{
			uint transactionID = _walIndex.NextTransactionID();
			Dictionary<uint, PagePosition> pagePositions;
			lock (_header)
			{
				pagePositions = new Dictionary<uint, PagePosition>();
				PageBuffer safepoint = _header.Savepoint();
				try
				{
					_disk.WriteAsync(source());
				}
				catch
				{
					_header.Restore(safepoint);
					throw;
				}
				_walIndex.ConfirmTransaction(transactionID, pagePositions.Values);
			}
			IEnumerable<PageBuffer> source()
			{
				for (int i = 0; i < _transPages.NewPages.Count; i++)
				{
					uint pageID = _transPages.NewPages[i];
					uint next = ((i < _transPages.NewPages.Count - 1) ? _transPages.NewPages[i + 1] : _header.FreeEmptyPageList);
					PageBuffer buffer = _disk.NewPage();
					BasePage page = new BasePage(buffer, pageID, PageType.Empty)
					{
						NextPageID = next,
						TransactionID = transactionID
					};
					yield return page.UpdateBuffer();
					pagePositions[pageID] = new PagePosition(pageID, buffer.Position);
				}
				_header.TransactionID = transactionID;
				_header.FreeEmptyPageList = _transPages.NewPages[0];
				_header.IsConfirmed = true;
				PageBuffer buf = _header.UpdateBuffer();
				PageBuffer clone = _disk.NewPage();
				Buffer.BlockCopy(buf.Array, buf.Offset, clone.Array, clone.Offset, clone.Count);
				yield return clone;
			}
		}

		public void Dispose()
		{
			Dispose(dispose: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool dispose)
		{
			if (_state == TransactionState.Disposed)
			{
				return;
			}
			Constants.ENSURE(_state != TransactionState.Disposed, "transaction must be active before call Done");
			if (_state == TransactionState.Active && _snapshots.Count > 0)
			{
				foreach (Snapshot snapshot2 in _snapshots.Values.Where((Snapshot x) => x.Mode == LockMode.Write))
				{
					_disk.DiscardDirtyPages(from x in snapshot2.GetWritablePages(dirty: true, includeCollectionPage: true)
						select x.Buffer);
					_disk.DiscardCleanPages(from x in snapshot2.GetWritablePages(dirty: false, includeCollectionPage: true)
						select x.Buffer);
				}
				foreach (Snapshot snapshot in _snapshots.Values.Where((Snapshot x) => x.Mode == LockMode.Read))
				{
					foreach (BasePage localPage in snapshot.LocalPages)
					{
						localPage.Buffer.Release();
					}
					snapshot.CollectionPage?.Buffer.Release();
				}
			}
			_reader.Dispose();
			_state = TransactionState.Disposed;
			if (!dispose)
			{
				_monitor.ReleaseTransaction(this);
			}
		}
	}
}
