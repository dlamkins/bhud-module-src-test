using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LiteDB.Engine
{
	internal class MemoryCache : IDisposable
	{
		private readonly ConcurrentQueue<PageBuffer> _free = new ConcurrentQueue<PageBuffer>();

		private readonly ConcurrentDictionary<long, PageBuffer> _readable = new ConcurrentDictionary<long, PageBuffer>();

		private int _extends;

		private readonly int[] _segmentSizes;

		public int PagesInUse => _readable.Values.Where((PageBuffer x) => x.ShareCounter != 0).Count();

		public int FreePages => _free.Count;

		public int ExtendSegments => _extends;

		public int ExtendPages => (from x in Enumerable.Range(0, _extends)
			select _segmentSizes[Math.Min(_segmentSizes.Length - 1, x)]).Sum();

		public int WritablePages => ExtendPages - _free.Count - _readable.Count;

		public MemoryCache(int[] memorySegmentSizes)
		{
			_segmentSizes = memorySegmentSizes;
			Extend();
		}

		public PageBuffer GetReadablePage(long position, FileOrigin origin, Action<long, BufferSlice> factory)
		{
			long key = GetReadableKey(position, origin);
			PageBuffer orAdd = _readable.GetOrAdd(key, delegate
			{
				PageBuffer freePage = GetFreePage();
				freePage.Position = position;
				freePage.Origin = origin;
				factory(position, freePage);
				return freePage;
			});
			Interlocked.Exchange(ref orAdd.Timestamp, DateTime.UtcNow.Ticks);
			Interlocked.Increment(ref orAdd.ShareCounter);
			return orAdd;
		}

		private long GetReadableKey(long position, FileOrigin origin)
		{
			Constants.ENSURE(origin != FileOrigin.None, "file origin must be defined");
			if (origin == FileOrigin.Data)
			{
				return position;
			}
			if (position == 0L)
			{
				return long.MinValue;
			}
			return -position;
		}

		public PageBuffer GetWritablePage(long position, FileOrigin origin, Action<long, BufferSlice> factory)
		{
			long key = GetReadableKey(position, origin);
			PageBuffer writable = NewPage(position, origin);
			if (_readable.TryGetValue(key, out var clean))
			{
				Buffer.BlockCopy(clean.Array, clean.Offset, writable.Array, writable.Offset, 8192);
			}
			else
			{
				factory(position, writable);
			}
			return writable;
		}

		public PageBuffer NewPage()
		{
			return NewPage(long.MaxValue, FileOrigin.None);
		}

		private PageBuffer NewPage(long position, FileOrigin origin)
		{
			PageBuffer page = GetFreePage();
			page.Position = position;
			page.ShareCounter = Constants.BUFFER_WRITABLE;
			if (page.Timestamp > 0)
			{
				page.Clear();
			}
			page.Origin = origin;
			page.Timestamp = DateTime.UtcNow.Ticks;
			return page;
		}

		public bool TryMoveToReadable(PageBuffer page)
		{
			Constants.ENSURE(page.Position != long.MaxValue, "page must have a position");
			Constants.ENSURE(page.ShareCounter == Constants.BUFFER_WRITABLE, "page must be writable");
			Constants.ENSURE(page.Origin != FileOrigin.None, "page must have origin defined");
			long key = GetReadableKey(page.Position, page.Origin);
			page.ShareCounter = 0;
			bool num = _readable.TryAdd(key, page);
			if (!num)
			{
				page.ShareCounter = Constants.BUFFER_WRITABLE;
			}
			return num;
		}

		public PageBuffer MoveToReadable(PageBuffer page)
		{
			Constants.ENSURE(page.Position != long.MaxValue, "page must have position to be readable");
			Constants.ENSURE(page.Origin != FileOrigin.None, "page should be a source before move to readable");
			Constants.ENSURE(page.ShareCounter == Constants.BUFFER_WRITABLE, "page must be writable before move to readable dict");
			long key = GetReadableKey(page.Position, page.Origin);
			bool added = true;
			page.ShareCounter = 1;
			PageBuffer result = _readable.AddOrUpdate(key, page, delegate(long newKey, PageBuffer current)
			{
				Constants.ENSURE(current.ShareCounter == 0, "user must ensure this page is not in use when marked as read only");
				Constants.ENSURE(current.Origin == page.Origin, "origin must be same");
				current.ShareCounter = 1;
				Buffer.BlockCopy(page.Array, page.Offset, current.Array, current.Offset, 8192);
				added = false;
				page.ShareCounter = Constants.BUFFER_WRITABLE;
				return current;
			});
			if (!added)
			{
				DiscardPage(page);
			}
			return result;
		}

		public void DiscardPage(PageBuffer page)
		{
			Constants.ENSURE(page.ShareCounter == Constants.BUFFER_WRITABLE, "discarded page must be writable");
			page.ShareCounter = 0;
			page.Position = long.MaxValue;
			page.Origin = FileOrigin.None;
			_free.Enqueue(page);
		}

		private PageBuffer GetFreePage()
		{
			if (_free.TryDequeue(out var page))
			{
				Constants.ENSURE(page.Position == long.MaxValue, "pages in memory store must have no position defined");
				Constants.ENSURE(page.ShareCounter == 0, "pages in memory store must be non-shared");
				Constants.ENSURE(page.Origin == FileOrigin.None, "page in memory must have no page origin");
				return page;
			}
			lock (_free)
			{
				if (_free.Count > 0)
				{
					return GetFreePage();
				}
				Extend();
			}
			return GetFreePage();
		}

		private void Extend()
		{
			int num = _readable.Values.Count((PageBuffer x) => x.ShareCounter == 0);
			int segmentSize = _segmentSizes[Math.Min(_segmentSizes.Length - 1, _extends)];
			if (num > segmentSize)
			{
				long[] array = (from x in _readable
					where x.Value.ShareCounter == 0
					orderby x.Value.Timestamp
					select x.Key).Take(segmentSize).ToArray();
				foreach (long key in array)
				{
					Constants.ENSURE(_readable.TryRemove(key, out var page), "page should be in readable list before moving to free list");
					if (page.ShareCounter > 0)
					{
						if (!_readable.TryAdd(key, page))
						{
							throw new LiteException(0, "MemoryCache: removed in-use memory page. This situation has no way to fix (yet). Throwing exception to avoid database corruption. No other thread can read/write from database now.");
						}
					}
					else
					{
						Constants.ENSURE(page.ShareCounter == 0, "page should not be in use by anyone");
						page.Position = long.MaxValue;
						page.Origin = FileOrigin.None;
						_free.Enqueue(page);
					}
				}
			}
			else
			{
				byte[] buffer = new byte[8192 * segmentSize];
				int uniqueID = ExtendPages + 1;
				for (int i = 0; i < segmentSize; i++)
				{
					_free.Enqueue(new PageBuffer(buffer, i * 8192, uniqueID++));
				}
				_extends++;
			}
		}

		public ICollection<PageBuffer> GetPages()
		{
			return _readable.Values;
		}

		public int Clear()
		{
			int counter = 0;
			Constants.ENSURE(PagesInUse == 0, "must have no pages in use when call Clear() cache");
			foreach (PageBuffer page in _readable.Values)
			{
				page.Position = long.MaxValue;
				page.Origin = FileOrigin.None;
				_free.Enqueue(page);
				counter++;
			}
			_readable.Clear();
			return counter;
		}

		public void Dispose()
		{
		}
	}
}
