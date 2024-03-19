using System;
using System.Collections.Generic;
using System.Threading;

namespace LiteDB.Engine
{
	internal class WalIndexService
	{
		private readonly DiskService _disk;

		private readonly LockService _locker;

		private readonly Dictionary<uint, List<KeyValuePair<int, long>>> _index = new Dictionary<uint, List<KeyValuePair<int, long>>>();

		private readonly ReaderWriterLockSlim _indexLock = new ReaderWriterLockSlim();

		private readonly HashSet<uint> _confirmTransactions = new HashSet<uint>();

		private int _currentReadVersion;

		private int _lastTransactionID;

		public int CurrentReadVersion => _currentReadVersion;

		public int LastTransactionID => _lastTransactionID;

		public WalIndexService(DiskService disk, LockService locker)
		{
			_disk = disk;
			_locker = locker;
		}

		public void Clear()
		{
			_indexLock.TryEnterWriteLock(-1);
			try
			{
				_confirmTransactions.Clear();
				_index.Clear();
				_lastTransactionID = 0;
				_currentReadVersion = 0;
				_disk.Cache.Clear();
				_disk.SetLength(0L, FileOrigin.Log);
			}
			finally
			{
				_indexLock.ExitWriteLock();
			}
		}

		public uint NextTransactionID()
		{
			return (uint)Interlocked.Increment(ref _lastTransactionID);
		}

		public long GetPageIndex(uint pageID, int version, out int walVersion)
		{
			if (version == 0)
			{
				walVersion = 0;
				return long.MaxValue;
			}
			_indexLock.TryEnterReadLock(-1);
			try
			{
				if (_index.TryGetValue(pageID, out var list))
				{
					int idx = list.Count;
					long position = long.MaxValue;
					walVersion = version;
					while (idx > 0)
					{
						idx--;
						KeyValuePair<int, long> v = list[idx];
						if (v.Key <= version)
						{
							walVersion = v.Key;
							position = v.Value;
							break;
						}
					}
					return position;
				}
				walVersion = int.MaxValue;
				return long.MaxValue;
			}
			finally
			{
				_indexLock.ExitReadLock();
			}
		}

		public void ConfirmTransaction(uint transactionID, ICollection<PagePosition> pagePositions)
		{
			_indexLock.TryEnterWriteLock(-1);
			try
			{
				_currentReadVersion++;
				foreach (PagePosition pos in pagePositions)
				{
					if (!_index.TryGetValue(pos.PageID, out var slot))
					{
						slot = new List<KeyValuePair<int, long>>();
						_index.Add(pos.PageID, slot);
					}
					slot.Add(new KeyValuePair<int, long>(_currentReadVersion, pos.Position));
				}
				_confirmTransactions.Add(transactionID);
			}
			finally
			{
				_indexLock.ExitWriteLock();
			}
		}

		public void RestoreIndex(ref HeaderPage header)
		{
			Dictionary<long, List<PagePosition>> positions = new Dictionary<long, List<PagePosition>>();
			long current = 0L;
			foreach (PageBuffer buffer in _disk.ReadFull(FileOrigin.Log))
			{
				if (buffer.IsBlank())
				{
					current += 8192;
					continue;
				}
				uint pageID = buffer.ReadUInt32(0);
				bool num = buffer.ReadBool(18);
				uint transactionID = buffer.ReadUInt32(14);
				PagePosition position = new PagePosition(pageID, current);
				if (positions.TryGetValue(transactionID, out var list))
				{
					list.Add(position);
				}
				else
				{
					positions[transactionID] = new List<PagePosition> { position };
				}
				if (num)
				{
					ConfirmTransaction(transactionID, positions[transactionID]);
					if (buffer.ReadByte(4) == 1)
					{
						PageBuffer headerBuffer = header.Buffer;
						Buffer.BlockCopy(buffer.Array, buffer.Offset, headerBuffer.Array, headerBuffer.Offset, 8192);
						header = new HeaderPage(headerBuffer);
						header.TransactionID = uint.MaxValue;
						header.IsConfirmed = false;
					}
				}
				_lastTransactionID = (int)transactionID;
				current += 8192;
			}
		}

		public int Checkpoint()
		{
			if (_disk.GetVirtualLength(FileOrigin.Log) == 0L || _confirmTransactions.Count == 0)
			{
				return 0;
			}
			bool mustExit = _locker.EnterExclusive();
			try
			{
				return CheckpointInternal();
			}
			finally
			{
				if (mustExit)
				{
					_locker.ExitExclusive();
				}
			}
		}

		public int TryCheckpoint()
		{
			if (_disk.GetVirtualLength(FileOrigin.Log) == 0L || _confirmTransactions.Count == 0)
			{
				return 0;
			}
			if (!_locker.TryEnterExclusive(out var mustExit))
			{
				return 0;
			}
			try
			{
				return CheckpointInternal();
			}
			finally
			{
				if (mustExit)
				{
					_locker.ExitExclusive();
				}
			}
		}

		private int CheckpointInternal()
		{
			_disk.Queue.Wait();
			int counter = 0;
			Constants.ENSURE(_disk.Queue.Length == 0, "no pages on queue when checkpoint");
			_disk.Write(source(), FileOrigin.Data);
			Clear();
			return counter;
			IEnumerable<PageBuffer> source()
			{
				foreach (PageBuffer buffer in _disk.ReadFull(FileOrigin.Log))
				{
					if (!buffer.IsBlank())
					{
						uint transactionID = buffer.ReadUInt32(14);
						if (_confirmTransactions.Contains(transactionID))
						{
							uint pageID = buffer.ReadUInt32(0);
							buffer.Write(uint.MaxValue, 14);
							buffer.Write(value: false, 18);
							buffer.Position = BasePage.GetPagePosition(pageID);
							counter++;
							yield return buffer;
						}
					}
				}
			}
		}
	}
}
