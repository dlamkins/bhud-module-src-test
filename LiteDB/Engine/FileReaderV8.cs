using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiteDB.Engine
{
	internal class FileReaderV8 : IFileReader
	{
		private readonly Dictionary<string, uint> _collections;

		private readonly Stream _stream;

		private readonly byte[] _buffer = new byte[8192];

		private BasePage _cachedPage;

		public FileReaderV8(HeaderPage header, DiskService disk)
		{
			_collections = header.GetCollections().ToDictionary((KeyValuePair<string, uint> x) => x.Key, (KeyValuePair<string, uint> x) => x.Value);
			_stream = disk.GetPool(FileOrigin.Data).Writer;
		}

		public IEnumerable<string> GetCollections()
		{
			return _collections.Keys;
		}

		public IEnumerable<IndexInfo> GetIndexes(string collection)
		{
			CollectionPage page = ReadPage<CollectionPage>(_collections[collection]);
			foreach (CollectionIndex index in from x in page.GetCollectionIndexes()
				where x.Name != "_id"
				select x)
			{
				yield return new IndexInfo
				{
					Collection = collection,
					Name = index.Name,
					Expression = index.Expression,
					Unique = index.Unique
				};
			}
		}

		public IEnumerable<BsonDocument> GetDocuments(string collection)
		{
			CollectionPage colPage = ReadPage<CollectionPage>(_collections[collection]);
			for (int slot = 0; slot < 5; slot++)
			{
				uint next = colPage.FreeDataPageList[slot];
				while (next != uint.MaxValue)
				{
					DataPage page = ReadPage<DataPage>(next);
					PageAddress[] array = page.GetBlocks().ToArray();
					foreach (PageAddress block in array)
					{
						using BufferReader r = new BufferReader(ReadBlocks(block));
						yield return r.ReadDocument();
					}
					next = page.NextPageID;
				}
			}
		}

		private T ReadPage<T>(uint pageID) where T : BasePage
		{
			long position = BasePage.GetPagePosition(pageID);
			BasePage cachedPage = _cachedPage;
			if (cachedPage != null && cachedPage.PageID == pageID)
			{
				return (T)_cachedPage;
			}
			_stream.Position = position;
			_stream.Read(_buffer, 0, 8192);
			PageBuffer buffer = new PageBuffer(_buffer, 0, 0);
			return (T)(_cachedPage = BasePage.ReadPage<T>(buffer));
		}

		public IEnumerable<BufferSlice> ReadBlocks(PageAddress address)
		{
			while (address != PageAddress.Empty)
			{
				DataPage dataPage = ReadPage<DataPage>(address.PageID);
				DataBlock block = dataPage.GetBlock(address.Index);
				yield return block.Buffer;
				address = block.NextBlock;
			}
		}
	}
}
