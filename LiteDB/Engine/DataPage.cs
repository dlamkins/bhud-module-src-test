using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class DataPage : BasePage
	{
		private static readonly int[] _freePageSlots = new int[4] { 7344, 6120, 4896, 2448 };

		public DataPage(PageBuffer buffer)
			: base(buffer)
		{
			Constants.ENSURE(base.PageType == PageType.Data, "page type must be data page");
			if (base.PageType != PageType.Data)
			{
				throw LiteException.InvalidPageType(PageType.Data, this);
			}
		}

		public DataPage(PageBuffer buffer, uint pageID)
			: base(buffer, pageID, PageType.Data)
		{
		}

		public DataBlock GetBlock(byte index)
		{
			BufferSlice segment = Get(index);
			return new DataBlock(this, index, segment);
		}

		public DataBlock InsertBlock(int bytesLength, bool extend)
		{
			byte index;
			BufferSlice segment = Insert((ushort)(bytesLength + 6), out index);
			return new DataBlock(this, index, segment, extend, PageAddress.Empty);
		}

		public DataBlock UpdateBlock(DataBlock currentBlock, int bytesLength)
		{
			BufferSlice segment = Update(currentBlock.Position.Index, (ushort)(bytesLength + 6));
			return new DataBlock(this, currentBlock.Position.Index, segment, currentBlock.Extend, currentBlock.NextBlock);
		}

		public void DeleteBlock(byte index)
		{
			Delete(index);
		}

		public IEnumerable<PageAddress> GetBlocks()
		{
			foreach (byte index in GetUsedIndexs())
			{
				int slotPosition = BasePage.CalcPositionAddr(index);
				ushort position = _buffer.ReadUInt16(slotPosition);
				if (!_buffer.ReadBool(position))
				{
					yield return new PageAddress(base.PageID, index);
				}
			}
		}

		public static byte FreeIndexSlot(int freeBytes)
		{
			Constants.ENSURE(freeBytes >= 0, "freeBytes must be positive");
			for (int i = 0; i < _freePageSlots.Length; i++)
			{
				if (freeBytes >= _freePageSlots[i])
				{
					return (byte)i;
				}
			}
			return 4;
		}

		public static int GetMinimumIndexSlot(int length)
		{
			return FreeIndexSlot(length) - 1;
		}
	}
}
