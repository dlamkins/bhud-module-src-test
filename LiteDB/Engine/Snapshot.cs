using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class Snapshot : IDisposable
	{
		private readonly HeaderPage _header;

		private readonly LockService _locker;

		private readonly DiskReader _reader;

		private readonly WalIndexService _walIndex;

		private readonly uint _transactionID;

		private readonly TransactionPages _transPages;

		private readonly int _readVersion;

		private readonly LockMode _mode;

		private readonly string _collectionName;

		private readonly CollectionPage _collectionPage;

		private readonly Dictionary<uint, BasePage> _localPages = new Dictionary<uint, BasePage>();

		public LockMode Mode => _mode;

		public string CollectionName => _collectionName;

		public CollectionPage CollectionPage => _collectionPage;

		public ICollection<BasePage> LocalPages => _localPages.Values;

		public int ReadVersion => _readVersion;

		public Snapshot(LockMode mode, string collectionName, HeaderPage header, uint transactionID, TransactionPages transPages, LockService locker, WalIndexService walIndex, DiskReader reader, bool addIfNotExists)
		{
			_mode = mode;
			_collectionName = collectionName;
			_header = header;
			_transactionID = transactionID;
			_transPages = transPages;
			_locker = locker;
			_walIndex = walIndex;
			_reader = reader;
			if (mode == LockMode.Write)
			{
				_locker.EnterLock(_collectionName);
			}
			_readVersion = _walIndex.CurrentReadVersion;
			new CollectionService(_header, this, _transPages).Get(_collectionName, addIfNotExists, ref _collectionPage);
			if (_collectionPage != null)
			{
				_localPages.Remove(_collectionPage.PageID);
			}
		}

		public IEnumerable<BasePage> GetWritablePages(bool dirty, bool includeCollectionPage)
		{
			if (_mode == LockMode.Read)
			{
				yield break;
			}
			foreach (BasePage page in _localPages.Values.Where((BasePage x) => x.IsDirty == dirty))
			{
				Constants.ENSURE(page.PageType != PageType.Header && page.PageType != PageType.Collection, "local cache cann't contains this page type");
				yield return page;
			}
			if (includeCollectionPage && _collectionPage != null && _collectionPage.IsDirty == dirty)
			{
				yield return _collectionPage;
			}
		}

		public void Clear()
		{
			if (_mode == LockMode.Read)
			{
				foreach (BasePage value in _localPages.Values)
				{
					value.Buffer.Release();
				}
			}
			_localPages.Clear();
		}

		public void Dispose()
		{
			Clear();
			if (_mode == LockMode.Read && _collectionPage != null)
			{
				_collectionPage.Buffer.Release();
			}
			if (_mode == LockMode.Write)
			{
				_locker.ExitLock(_collectionName);
			}
		}

		public T GetPage<T>(uint pageID) where T : BasePage
		{
			FileOrigin origin;
			long position;
			int walVersion;
			return GetPage<T>(pageID, out origin, out position, out walVersion);
		}

		public T GetPage<T>(uint pageID, out FileOrigin origin, out long position, out int walVersion) where T : BasePage
		{
			Constants.ENSURE(pageID <= _header.LastPageID, "request page must be less or equals lastest page in data file");
			if (pageID == 0)
			{
				origin = FileOrigin.None;
				position = 0L;
				walVersion = 0;
				return (T)(BasePage)_header;
			}
			if (_localPages.TryGetValue(pageID, out var page))
			{
				origin = FileOrigin.None;
				position = 0L;
				walVersion = 0;
				return (T)page;
			}
			page = ReadPage<T>(pageID, out origin, out position, out walVersion);
			_localPages[pageID] = page;
			_transPages.TransactionSize++;
			return (T)page;
		}

		private T ReadPage<T>(uint pageID, out FileOrigin origin, out long position, out int walVersion) where T : BasePage
		{
			if (_transPages.DirtyPages.TryGetValue(pageID, out var walPosition))
			{
				T val = BasePage.ReadPage<T>(_reader.ReadPage(walPosition.Position, _mode == LockMode.Write, FileOrigin.Log));
				origin = FileOrigin.Log;
				position = walPosition.Position;
				walVersion = _readVersion;
				Constants.ENSURE(val.TransactionID == _transactionID, "this page must came from same transaction");
				return val;
			}
			long pos = _walIndex.GetPageIndex(pageID, _readVersion, out walVersion);
			if (pos != long.MaxValue)
			{
				T val2 = BasePage.ReadPage<T>(_reader.ReadPage(pos, _mode == LockMode.Write, FileOrigin.Log));
				val2.TransactionID = 0u;
				val2.IsConfirmed = false;
				origin = FileOrigin.Log;
				position = pos;
				return val2;
			}
			long pagePosition = BasePage.GetPagePosition(pageID);
			T diskpage = BasePage.ReadPage<T>(_reader.ReadPage(pagePosition, _mode == LockMode.Write, FileOrigin.Data));
			origin = FileOrigin.Data;
			position = pagePosition;
			Constants.ENSURE(!diskpage.IsConfirmed || diskpage.TransactionID != 0, "page are not header-clear in data file");
			return diskpage;
		}

		public DataPage GetFreeDataPage(int bytesLength)
		{
			int length = bytesLength + 4;
			for (int currentSlot = DataPage.GetMinimumIndexSlot(length); currentSlot >= 0; currentSlot--)
			{
				uint freePageID = _collectionPage.FreeDataPageList[currentSlot];
				if (freePageID != uint.MaxValue)
				{
					DataPage page = GetPage<DataPage>(freePageID);
					Constants.ENSURE(page.PageListSlot == currentSlot, "stored slot must be same as called");
					Constants.ENSURE(page.FreeBytes >= length, "ensure selected page has space enough for this content");
					page.IsDirty = true;
					return page;
				}
			}
			return NewPage<DataPage>();
		}

		public IndexPage GetFreeIndexPage(int bytesLength, ref uint freeIndexPageList)
		{
			IndexPage page;
			if (freeIndexPageList == uint.MaxValue)
			{
				page = NewPage<IndexPage>();
			}
			else
			{
				page = GetPage<IndexPage>(freeIndexPageList);
				Constants.ENSURE(page.FreeBytes > bytesLength, "this page shout be space enouth for this new node");
				Constants.ENSURE(page.PageListSlot == 0, "this page should be in slot #0");
			}
			return page;
		}

		public T NewPage<T>() where T : BasePage
		{
			Constants.ENSURE(_collectionPage == null, typeof(T) == typeof(CollectionPage), "if no collection page defined yet, must be first request");
			Constants.ENSURE(typeof(T) == typeof(CollectionPage), _collectionPage == null, "there is no new collection page if page already exists");
			uint pageID = 0u;
			PageBuffer buffer;
			lock (_header)
			{
				if (_header.FreeEmptyPageList != uint.MaxValue)
				{
					BasePage free = GetPage<BasePage>(_header.FreeEmptyPageList);
					Constants.ENSURE(free.PageType == PageType.Empty, "empty page must be defined as empty type");
					_header.FreeEmptyPageList = free.NextPageID;
					free.NextPageID = uint.MaxValue;
					pageID = free.PageID;
					buffer = free.Buffer;
				}
				else
				{
					if ((_header.LastPageID + 1) * 8192 > _header.Pragmas.LimitSize)
					{
						throw new LiteException(0, "Maximum data file size has been reached: " + FileHelper.FormatFileSize(_header.Pragmas.LimitSize));
					}
					pageID = ++_header.LastPageID;
					buffer = _reader.NewPage();
				}
				_transPages.NewPages.Add(pageID);
			}
			T page = BasePage.CreatePage<T>(buffer, pageID);
			if (page.PageType != PageType.Collection)
			{
				_localPages[pageID] = page;
			}
			page.ColID = _collectionPage?.PageID ?? page.PageID;
			page.IsDirty = true;
			_transPages.TransactionSize++;
			return page;
		}

		public void AddOrRemoveFreeDataList(DataPage page)
		{
			byte newSlot = DataPage.FreeIndexSlot(page.FreeBytes);
			byte initialSlot = page.PageListSlot;
			if (newSlot != initialSlot || page.ItemsCount <= 0)
			{
				if (initialSlot != byte.MaxValue)
				{
					RemoveFreeList(page, ref _collectionPage.FreeDataPageList[initialSlot]);
				}
				if (page.ItemsCount == 0)
				{
					DeletePage(page);
					return;
				}
				AddFreeList(page, ref _collectionPage.FreeDataPageList[newSlot]);
				page.PageListSlot = newSlot;
			}
		}

		public void AddOrRemoveFreeIndexList(IndexPage page, ref uint startPageID)
		{
			byte newSlot = IndexPage.FreeIndexSlot(page.FreeBytes);
			bool isOnList = page.PageListSlot == 0;
			bool mustKeep = newSlot == 0;
			if (page.ItemsCount == 0)
			{
				if (isOnList)
				{
					RemoveFreeList(page, ref startPageID);
				}
				DeletePage(page);
				return;
			}
			if (isOnList && !mustKeep)
			{
				RemoveFreeList(page, ref startPageID);
			}
			else if (!isOnList && mustKeep)
			{
				AddFreeList(page, ref startPageID);
			}
			page.PageListSlot = newSlot;
			page.IsDirty = true;
		}

		private void AddFreeList<T>(T page, ref uint startPageID) where T : BasePage
		{
			Constants.ENSURE(page.PrevPageID == uint.MaxValue && page.NextPageID == uint.MaxValue, "only non-linked page can be added in linked list");
			if (startPageID != uint.MaxValue)
			{
				T page2 = GetPage<T>(startPageID);
				page2.PrevPageID = page.PageID;
				page2.IsDirty = true;
			}
			page.PrevPageID = uint.MaxValue;
			page.NextPageID = startPageID;
			page.IsDirty = true;
			Constants.ENSURE(page.PageType == PageType.Data || page.PageType == PageType.Index, "only data/index pages must be first on free stack");
			startPageID = page.PageID;
			_collectionPage.IsDirty = true;
		}

		private void RemoveFreeList<T>(T page, ref uint startPageID) where T : BasePage
		{
			if (page.PrevPageID != uint.MaxValue)
			{
				T page2 = GetPage<T>(page.PrevPageID);
				page2.NextPageID = page.NextPageID;
				page2.IsDirty = true;
			}
			if (page.NextPageID != uint.MaxValue)
			{
				T page3 = GetPage<T>(page.NextPageID);
				page3.PrevPageID = page.PrevPageID;
				page3.IsDirty = true;
			}
			if (startPageID == page.PageID)
			{
				startPageID = page.NextPageID;
				Constants.ENSURE(page.NextPageID == uint.MaxValue || GetPage<BasePage>(page.NextPageID).PageType != PageType.Empty, "first page on free stack must be non empty page");
				_collectionPage.IsDirty = true;
			}
			uint num3 = (page.PrevPageID = (page.NextPageID = uint.MaxValue));
			page.IsDirty = true;
		}

		private void DeletePage<T>(T page) where T : BasePage
		{
			Constants.ENSURE(page.PrevPageID == uint.MaxValue && page.NextPageID == uint.MaxValue, "before delete a page, no linked list with any another page");
			Constants.ENSURE(page.ItemsCount == 0 && page.UsedBytes == 0 && page.HighestIndex == byte.MaxValue && page.FragmentedBytes == 0, "no items on page when delete this page");
			Constants.ENSURE(page.PageType == PageType.Data || page.PageType == PageType.Index, "only data/index page can be deleted");
			page.MarkAsEmtpy();
			if (_transPages.FirstDeletedPageID == uint.MaxValue)
			{
				Constants.ENSURE(_transPages.DeletedPages == 0, "if has no firstDeletedPageID must has deleted pages");
				_transPages.FirstDeletedPageID = page.PageID;
				_transPages.LastDeletedPageID = page.PageID;
			}
			else
			{
				Constants.ENSURE(_transPages.DeletedPages > 0, "must have at least 1 deleted page");
				page.NextPageID = _transPages.FirstDeletedPageID;
				_transPages.FirstDeletedPageID = page.PageID;
			}
			_transPages.DeletedPages++;
		}

		public void DropCollection(Action safePoint)
		{
			IndexService indexer = new IndexService(this, _header.Pragmas.Collation);
			_transPages.FirstDeletedPageID = _collectionPage.PageID;
			_transPages.LastDeletedPageID = _collectionPage.PageID;
			_collectionPage.MarkAsEmtpy();
			_transPages.DeletedPages = 1;
			HashSet<uint> indexPages = new HashSet<uint>();
			foreach (CollectionIndex index in _collectionPage.GetCollectionIndexes())
			{
				indexPages.Add(index.Head.PageID);
				foreach (IndexNode node in indexer.FindAll(index, 1))
				{
					indexPages.Add(node.Page.PageID);
					safePoint();
				}
			}
			foreach (uint pageID in indexPages)
			{
				IndexPage page2 = GetPage<IndexPage>(pageID);
				page2.MarkAsEmtpy();
				page2.NextPageID = _transPages.FirstDeletedPageID;
				_transPages.FirstDeletedPageID = page2.PageID;
				_transPages.DeletedPages++;
				safePoint();
			}
			uint[] freeDataPageList = _collectionPage.FreeDataPageList;
			for (int i = 0; i < freeDataPageList.Length; i++)
			{
				uint next = freeDataPageList[i];
				while (next != uint.MaxValue)
				{
					DataPage page = GetPage<DataPage>(next);
					next = page.NextPageID;
					page.MarkAsEmtpy();
					page.NextPageID = _transPages.FirstDeletedPageID;
					_transPages.FirstDeletedPageID = page.PageID;
					_transPages.DeletedPages++;
					safePoint();
				}
			}
			_transPages.Commit += delegate(HeaderPage h)
			{
				h.DeleteCollection(_collectionName);
			};
		}

		public override string ToString()
		{
			return $"{_collectionName} (pages: {_localPages.Count})";
		}
	}
}
