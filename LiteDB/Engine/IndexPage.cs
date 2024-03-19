using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class IndexPage : BasePage
	{
		public IndexPage(PageBuffer buffer)
			: base(buffer)
		{
			Constants.ENSURE(base.PageType == PageType.Index, "page type must be index page");
			if (base.PageType != PageType.Index)
			{
				throw LiteException.InvalidPageType(PageType.Index, this);
			}
		}

		public IndexPage(PageBuffer buffer, uint pageID)
			: base(buffer, pageID, PageType.Index)
		{
		}

		public IndexNode GetIndexNode(byte index)
		{
			BufferSlice segment = Get(index);
			return new IndexNode(this, index, segment);
		}

		public IndexNode InsertIndexNode(byte slot, byte level, BsonValue key, PageAddress dataBlock, int bytesLength)
		{
			byte index;
			BufferSlice segment = Insert((ushort)bytesLength, out index);
			return new IndexNode(this, index, segment, slot, level, key, dataBlock);
		}

		public void DeleteIndexNode(byte index)
		{
			Delete(index);
		}

		public IEnumerable<IndexNode> GetIndexNodes()
		{
			foreach (byte index in GetUsedIndexs())
			{
				yield return GetIndexNode(index);
			}
		}

		public static byte FreeIndexSlot(int freeBytes)
		{
			Constants.ENSURE(freeBytes >= 0, "freeBytes must be positive");
			return (freeBytes < 1400) ? ((byte)1) : ((byte)0);
		}
	}
}
