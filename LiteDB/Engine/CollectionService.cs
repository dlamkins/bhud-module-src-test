using System.Text;

namespace LiteDB.Engine
{
	internal class CollectionService
	{
		private readonly HeaderPage _header;

		private readonly Snapshot _snapshot;

		private readonly TransactionPages _transPages;

		public CollectionService(HeaderPage header, Snapshot snapshot, TransactionPages transPages)
		{
			_snapshot = snapshot;
			_header = header;
			_transPages = transPages;
		}

		public static void CheckName(string name, HeaderPage header)
		{
			if (Encoding.UTF8.GetByteCount(name) > header.GetAvailableCollectionSpace())
			{
				throw LiteException.InvalidCollectionName(name, "There is no space in header this collection name");
			}
			if (!name.IsWord())
			{
				throw LiteException.InvalidCollectionName(name, "Use only [a-Z$_]");
			}
			if (name.StartsWith("$"))
			{
				throw LiteException.InvalidCollectionName(name, "Collection can't starts with `$` (reserved for system collections)");
			}
		}

		public void Get(string name, bool addIfNotExists, ref CollectionPage collectionPage)
		{
			uint pageID = _header.GetCollectionPageID(name);
			if (pageID != uint.MaxValue)
			{
				collectionPage = _snapshot.GetPage<CollectionPage>(pageID);
			}
			else if (addIfNotExists)
			{
				Add(name, ref collectionPage);
			}
		}

		private void Add(string name, ref CollectionPage collectionPage)
		{
			CheckName(name, _header);
			collectionPage = _snapshot.NewPage<CollectionPage>();
			uint pageID = collectionPage.PageID;
			_transPages.Commit += delegate(HeaderPage h)
			{
				h.InsertCollection(name, pageID);
			};
			new IndexService(_snapshot, _header.Pragmas.Collation).CreateIndex("_id", "$._id", unique: true);
		}
	}
}
