using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class SysPageList : SystemCollection
	{
		private readonly HeaderPage _header;

		private readonly TransactionMonitor _monitor;

		private Dictionary<uint, string> _collections;

		public SysPageList(HeaderPage header, TransactionMonitor monitor)
			: base("$page_list")
		{
			_header = header;
			_monitor = monitor;
		}

		public override IEnumerable<BsonDocument> Input(BsonValue options)
		{
			BsonValue pageID = SystemCollection.GetOption(options, "pageID");
			TransactionService transaction = _monitor.GetThreadTransaction();
			Snapshot snapshot = transaction.CreateSnapshot(LockMode.Read, "$", addIfNotExists: false);
			_collections = _header.GetCollections().ToDictionary((KeyValuePair<string, uint> x) => x.Value, (KeyValuePair<string, uint> x) => x.Key);
			IEnumerable<BsonDocument> result = ((pageID != null) ? GetList((uint)pageID.AsInt32, null, transaction, snapshot) : GetAllList(transaction, snapshot));
			foreach (BsonDocument item in result)
			{
				yield return item;
			}
		}

		private IEnumerable<BsonDocument> GetAllList(TransactionService transaction, Snapshot snapshot)
		{
			foreach (BsonDocument item in GetList(_header.FreeEmptyPageList, null, transaction, snapshot))
			{
				yield return item;
			}
			foreach (KeyValuePair<uint, string> collection in _collections)
			{
				Snapshot snap = transaction.CreateSnapshot(LockMode.Read, collection.Value, addIfNotExists: false);
				for (int slot2 = 0; slot2 < 5; slot2++)
				{
					IEnumerable<BsonDocument> result = GetList(snap.CollectionPage.FreeDataPageList[slot2], null, transaction, snap);
					foreach (BsonDocument item2 in result)
					{
						yield return item2;
					}
				}
				CollectionIndex[] indexes = snap.CollectionPage.GetCollectionIndexes().ToArray();
				CollectionIndex[] array = indexes;
				foreach (CollectionIndex index in array)
				{
					IEnumerable<BsonDocument> result2 = GetList(index.FreeIndexPageList, index.Name, transaction, snap);
					foreach (BsonDocument item3 in result2)
					{
						yield return item3;
					}
				}
			}
		}

		private IEnumerable<BsonDocument> GetList(uint pageID, string indexName, TransactionService transaction, Snapshot snapshot)
		{
			if (pageID == uint.MaxValue)
			{
				yield break;
			}
			BasePage page = snapshot.GetPage<BasePage>(pageID);
			while (page != null)
			{
				_collections.TryGetValue(page.ColID, out var collection);
				yield return new BsonDocument
				{
					["pageID"] = (int)page.PageID,
					["pageType"] = page.PageType.ToString(),
					["slot"] = page.PageListSlot,
					["collection"] = collection,
					["index"] = indexName,
					["freeBytes"] = page.FreeBytes,
					["itemsCount"] = page.ItemsCount
				};
				if (page.NextPageID != uint.MaxValue)
				{
					transaction.Safepoint();
					page = snapshot.GetPage<BasePage>(page.NextPageID);
					continue;
				}
				break;
			}
		}
	}
}
