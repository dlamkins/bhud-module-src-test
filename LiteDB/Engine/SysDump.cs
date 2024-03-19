using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class SysDump : SystemCollection
	{
		private readonly HeaderPage _header;

		private readonly TransactionMonitor _monitor;

		public SysDump(HeaderPage header, TransactionMonitor monitor)
			: base("$dump")
		{
			_header = header;
			_monitor = monitor;
		}

		public override IEnumerable<BsonDocument> Input(BsonValue options)
		{
			BsonValue pageID = SystemCollection.GetOption(options, "pageID");
			return DumpPages((pageID == null) ? null : new uint?((uint)pageID.AsInt32));
		}

		private IEnumerable<BsonDocument> DumpPages(uint? pageID)
		{
			Dictionary<uint, string> collections = _header.GetCollections().ToDictionary((KeyValuePair<string, uint> x) => x.Value, (KeyValuePair<string, uint> x) => x.Key);
			TransactionService transaction = _monitor.GetThreadTransaction();
			Snapshot snapshot = transaction.CreateSnapshot(LockMode.Read, "$", addIfNotExists: false);
			uint start = (pageID.HasValue ? pageID.Value : 0u);
			uint end = (pageID.HasValue ? pageID.Value : _header.LastPageID);
			for (uint i = start; i <= Math.Min(end, _header.LastPageID); i++)
			{
				FileOrigin origin;
				long position;
				int walVersion;
				BasePage page = snapshot.GetPage<BasePage>(i, out origin, out position, out walVersion);
				BsonDocument doc = new BsonDocument
				{
					["pageID"] = (int)page.PageID,
					["pageType"] = page.PageType.ToString(),
					["_position"] = position,
					["_origin"] = origin.ToString(),
					["_version"] = walVersion,
					["prevPageID"] = (int)page.PrevPageID,
					["nextPageID"] = (int)page.NextPageID,
					["slot"] = page.PageListSlot,
					["collection"] = collections.GetOrDefault(page.ColID, "-"),
					["itemsCount"] = page.ItemsCount,
					["freeBytes"] = page.FreeBytes,
					["usedBytes"] = page.UsedBytes,
					["fragmentedBytes"] = page.FragmentedBytes,
					["nextFreePosition"] = page.NextFreePosition,
					["highestIndex"] = page.HighestIndex
				};
				if (page.PageType == PageType.Collection)
				{
					CollectionPage collectionPage = new CollectionPage(page.Buffer);
					doc["dataPageList"] = new BsonArray(collectionPage.FreeDataPageList.Select((uint x) => new BsonValue((int)x)));
					doc["indexes"] = new BsonArray(from x in collectionPage.GetCollectionIndexes()
						select new BsonDocument
						{
							["slot"] = x.Slot,
							["empty"] = x.IsEmpty,
							["indexType"] = x.IndexType,
							["name"] = x.Name,
							["expression"] = x.Expression,
							["unique"] = x.Unique,
							["head"] = x.Head.ToBsonValue(),
							["tail"] = x.Tail.ToBsonValue(),
							["maxLevel"] = x.MaxLevel,
							["freeIndexPageList"] = (int)x.FreeIndexPageList
						});
				}
				if (pageID.HasValue)
				{
					doc["buffer"] = page.Buffer.ToArray();
				}
				yield return doc;
				transaction.Safepoint();
			}
		}
	}
}
