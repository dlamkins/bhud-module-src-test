using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class DataService
	{
		public const int MAX_DATA_BYTES_PER_PAGE = 8150;

		private readonly Snapshot _snapshot;

		public DataService(Snapshot snapshot)
		{
			_snapshot = snapshot;
		}

		public PageAddress Insert(BsonDocument doc)
		{
			int bytesLeft = doc.GetBytesCount(recalc: true);
			if (bytesLeft > 16683050)
			{
				throw new LiteException(0, "Document size exceed {0} limit", 16683050);
			}
			PageAddress firstBlock = PageAddress.Empty;
			using (BufferWriter w = new BufferWriter(source()))
			{
				w.WriteDocument(doc, recalc: false);
				w.Consume();
			}
			return firstBlock;
			IEnumerable<BufferSlice> source()
			{
				int blockIndex = 0;
				DataBlock lastBlock = null;
				int bytesToCopy;
				for (; bytesLeft > 0; bytesLeft -= bytesToCopy)
				{
					bytesToCopy = Math.Min(bytesLeft, 8150);
					DataPage dataPage = _snapshot.GetFreeDataPage(bytesToCopy + 6);
					DataBlock dataBlock = dataPage.InsertBlock(bytesToCopy, blockIndex++ > 0);
					lastBlock?.SetNextBlock(dataBlock.Position);
					if (firstBlock.IsEmpty)
					{
						firstBlock = dataBlock.Position;
					}
					_snapshot.AddOrRemoveFreeDataList(dataPage);
					yield return dataBlock.Buffer;
					lastBlock = dataBlock;
				}
			}
		}

		public void Update(CollectionPage col, PageAddress blockAddress, BsonDocument doc)
		{
			int bytesLeft = doc.GetBytesCount(recalc: true);
			if (bytesLeft > 16683050)
			{
				throw new LiteException(0, "Document size exceed {0} limit", 16683050);
			}
			DataBlock lastBlock = null;
			PageAddress updateAddress = blockAddress;
			using (BufferWriter w = new BufferWriter(source()))
			{
				w.WriteDocument(doc, recalc: false);
				w.Consume();
			}
			IEnumerable<BufferSlice> source()
			{
				int bytesToCopy;
				for (; bytesLeft > 0; bytesLeft -= bytesToCopy)
				{
					if (!updateAddress.IsEmpty)
					{
						DataPage dataPage2 = _snapshot.GetPage<DataPage>(updateAddress.PageID);
						DataBlock currentBlock = dataPage2.GetBlock(updateAddress.Index);
						bytesToCopy = Math.Min(bytesLeft, dataPage2.FreeBytes + currentBlock.Buffer.Count);
						DataBlock updateBlock2 = dataPage2.UpdateBlock(currentBlock, bytesToCopy);
						_snapshot.AddOrRemoveFreeDataList(dataPage2);
						yield return updateBlock2.Buffer;
						lastBlock = updateBlock2;
						updateAddress = updateBlock2.NextBlock;
					}
					else
					{
						bytesToCopy = Math.Min(bytesLeft, 8150);
						DataPage dataPage = _snapshot.GetFreeDataPage(bytesToCopy + 6);
						DataBlock updateBlock2 = dataPage.InsertBlock(bytesToCopy, extend: true);
						if (lastBlock != null)
						{
							lastBlock.SetNextBlock(updateBlock2.Position);
						}
						_snapshot.AddOrRemoveFreeDataList(dataPage);
						yield return updateBlock2.Buffer;
						lastBlock = updateBlock2;
					}
				}
				if (!lastBlock.NextBlock.IsEmpty)
				{
					PageAddress nextBlockAddress = lastBlock.NextBlock;
					lastBlock.SetNextBlock(PageAddress.Empty);
					Delete(nextBlockAddress);
				}
			}
		}

		public IEnumerable<BufferSlice> Read(PageAddress address)
		{
			while (address != PageAddress.Empty)
			{
				DataPage dataPage = _snapshot.GetPage<DataPage>(address.PageID);
				DataBlock block = dataPage.GetBlock(address.Index);
				yield return block.Buffer;
				address = block.NextBlock;
			}
		}

		public void Delete(PageAddress blockAddress)
		{
			while (blockAddress != PageAddress.Empty)
			{
				DataPage page = _snapshot.GetPage<DataPage>(blockAddress.PageID);
				DataBlock block = page.GetBlock(blockAddress.Index);
				page.DeleteBlock(blockAddress.Index);
				_snapshot.AddOrRemoveFreeDataList(page);
				blockAddress = block.NextBlock;
			}
		}
	}
}
