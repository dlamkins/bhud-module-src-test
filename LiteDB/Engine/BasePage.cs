using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class BasePage
	{
		protected readonly PageBuffer _buffer;

		public const int SLOT_SIZE = 4;

		public const int P_PAGE_ID = 0;

		public const int P_PAGE_TYPE = 4;

		public const int P_PREV_PAGE_ID = 5;

		public const int P_NEXT_PAGE_ID = 9;

		public const int P_INITIAL_SLOT = 13;

		public const int P_TRANSACTION_ID = 14;

		public const int P_IS_CONFIRMED = 18;

		public const int P_COL_ID = 19;

		public const int P_ITEMS_COUNT = 23;

		public const int P_USED_BYTES = 24;

		public const int P_FRAGMENTED_BYTES = 26;

		public const int P_NEXT_FREE_POSITION = 28;

		public const int P_HIGHEST_INDEX = 30;

		private byte _startIndex;

		public uint PageID { get; }

		public PageType PageType { get; private set; }

		public uint PrevPageID { get; set; }

		public uint NextPageID { get; set; }

		public byte PageListSlot { get; set; }

		public byte ItemsCount { get; private set; }

		public ushort UsedBytes { get; private set; }

		public ushort FragmentedBytes { get; private set; }

		public ushort NextFreePosition { get; private set; }

		public byte HighestIndex { get; private set; }

		public int FreeBytes
		{
			get
			{
				if (ItemsCount != byte.MaxValue)
				{
					return 8160 - UsedBytes - FooterSize;
				}
				return 0;
			}
		}

		public int FooterSize
		{
			get
			{
				if (HighestIndex != byte.MaxValue)
				{
					return (HighestIndex + 1) * 4;
				}
				return 0;
			}
		}

		public uint ColID { get; set; }

		public uint TransactionID { get; set; }

		public bool IsConfirmed { get; set; }

		public bool IsDirty { get; set; }

		public PageBuffer Buffer => _buffer;

		public BasePage(PageBuffer buffer, uint pageID, PageType pageType)
		{
			_buffer = buffer;
			PageID = pageID;
			PageType = pageType;
			PrevPageID = uint.MaxValue;
			NextPageID = uint.MaxValue;
			PageListSlot = byte.MaxValue;
			ColID = uint.MaxValue;
			TransactionID = uint.MaxValue;
			IsConfirmed = false;
			ItemsCount = 0;
			UsedBytes = 0;
			FragmentedBytes = 0;
			NextFreePosition = 32;
			HighestIndex = byte.MaxValue;
			IsDirty = false;
			_buffer.Write(PageID, 0);
			_buffer.Write((byte)PageType, 4);
		}

		public BasePage(PageBuffer buffer)
		{
			_buffer = buffer;
			PageID = _buffer.ReadUInt32(0);
			PageType = (PageType)_buffer.ReadByte(4);
			PrevPageID = _buffer.ReadUInt32(5);
			NextPageID = _buffer.ReadUInt32(9);
			PageListSlot = _buffer.ReadByte(13);
			TransactionID = _buffer.ReadUInt32(14);
			IsConfirmed = _buffer.ReadBool(18);
			ColID = _buffer.ReadUInt32(19);
			ItemsCount = _buffer.ReadByte(23);
			UsedBytes = _buffer.ReadUInt16(24);
			FragmentedBytes = _buffer.ReadUInt16(26);
			NextFreePosition = _buffer.ReadUInt16(28);
			HighestIndex = _buffer.ReadByte(30);
		}

		public virtual PageBuffer UpdateBuffer()
		{
			Constants.ENSURE(PageID == _buffer.ReadUInt32(0), "pageID can't be changed");
			_buffer.Write(PrevPageID, 5);
			_buffer.Write(NextPageID, 9);
			_buffer.Write(PageListSlot, 13);
			_buffer.Write(TransactionID, 14);
			_buffer.Write(IsConfirmed, 18);
			_buffer.Write(ColID, 19);
			_buffer.Write(ItemsCount, 23);
			_buffer.Write(UsedBytes, 24);
			_buffer.Write(FragmentedBytes, 26);
			_buffer.Write(NextFreePosition, 28);
			_buffer.Write(HighestIndex, 30);
			return _buffer;
		}

		public void MarkAsEmtpy()
		{
			IsDirty = true;
			PageType = PageType.Empty;
			PrevPageID = uint.MaxValue;
			NextPageID = uint.MaxValue;
			PageListSlot = byte.MaxValue;
			ColID = uint.MaxValue;
			TransactionID = uint.MaxValue;
			IsConfirmed = false;
			ItemsCount = 0;
			UsedBytes = 0;
			FragmentedBytes = 0;
			NextFreePosition = 32;
			HighestIndex = byte.MaxValue;
			_buffer.Clear(32, 8160);
			_buffer.Write((byte)PageType, 4);
		}

		public BufferSlice Get(byte index)
		{
			Constants.ENSURE(ItemsCount > 0, "should have items in this page");
			Constants.ENSURE(HighestIndex != byte.MaxValue, "should have at least 1 index in this page");
			Constants.ENSURE(index <= HighestIndex, "get only index below highest index");
			int positionAddr = CalcPositionAddr(index);
			int lengthAddr = CalcLengthAddr(index);
			ushort position = _buffer.ReadUInt16(positionAddr);
			ushort length = _buffer.ReadUInt16(lengthAddr);
			Constants.ENSURE(IsValidPos(position), "invalid segment position");
			Constants.ENSURE(IsValidLen(length), "invalid segment length");
			return _buffer.Slice(position, length);
		}

		public BufferSlice Insert(ushort bytesLength, out byte index)
		{
			index = byte.MaxValue;
			return InternalInsert(bytesLength, ref index);
		}

		private BufferSlice InternalInsert(ushort bytesLength, ref byte index)
		{
			bool isNewInsert = index == byte.MaxValue;
			Constants.ENSURE(_buffer.ShareCounter == Constants.BUFFER_WRITABLE, "page must be writable to support changes");
			Constants.ENSURE(bytesLength > 0, "must insert more than 0 bytes");
			Constants.ENSURE(FreeBytes >= bytesLength + (isNewInsert ? 4 : 0), "length must be always lower than current free space");
			Constants.ENSURE(ItemsCount < byte.MaxValue, "page full");
			Constants.ENSURE(FreeBytes >= FragmentedBytes, "fragmented bytes must be at most free bytes");
			if (FreeBytes < bytesLength + (isNewInsert ? 4 : 0))
			{
				throw LiteException.InvalidFreeSpacePage(PageID, FreeBytes, bytesLength + (isNewInsert ? 4 : 0));
			}
			int continuousBlocks = FreeBytes - FragmentedBytes - (isNewInsert ? 4 : 0);
			Constants.ENSURE(continuousBlocks == 8192 - NextFreePosition - FooterSize - (isNewInsert ? 4 : 0), "continuousBlock must be same as from NextFreePosition");
			if (bytesLength > continuousBlocks)
			{
				Defrag();
			}
			if (index == byte.MaxValue)
			{
				index = GetFreeIndex();
			}
			if (index > HighestIndex || HighestIndex == byte.MaxValue)
			{
				Constants.ENSURE(index == (byte)(HighestIndex + 1), "new index must be next highest index");
				HighestIndex = index;
			}
			int positionAddr = CalcPositionAddr(index);
			int lengthAddr = CalcLengthAddr(index);
			Constants.ENSURE(_buffer.ReadUInt16(positionAddr) == 0, "slot position must be empty before use");
			Constants.ENSURE(_buffer.ReadUInt16(lengthAddr) == 0, "slot length must be empty before use");
			ushort position = NextFreePosition;
			_buffer.Write(position, positionAddr);
			_buffer.Write(bytesLength, lengthAddr);
			ItemsCount++;
			UsedBytes += bytesLength;
			NextFreePosition += bytesLength;
			IsDirty = true;
			Constants.ENSURE(position + bytesLength <= 8192 - (HighestIndex + 1) * 4, "new buffer slice could not override footer area");
			return _buffer.Slice(position, bytesLength);
		}

		public void Delete(byte index)
		{
			Constants.ENSURE(_buffer.ShareCounter == Constants.BUFFER_WRITABLE, "page must be writable to support changes");
			int positionAddr = CalcPositionAddr(index);
			int lengthAddr = CalcLengthAddr(index);
			ushort position = _buffer.ReadUInt16(positionAddr);
			ushort length = _buffer.ReadUInt16(lengthAddr);
			Constants.ENSURE(IsValidPos(position), "invalid segment position");
			Constants.ENSURE(IsValidLen(length), "invalid segment length");
			((BufferSlice)_buffer).Write((ushort)0, positionAddr);
			((BufferSlice)_buffer).Write((ushort)0, lengthAddr);
			ItemsCount--;
			UsedBytes -= length;
			_buffer.Array.Fill(0, _buffer.Offset + position, length);
			if (position + length == NextFreePosition)
			{
				NextFreePosition = position;
			}
			else
			{
				FragmentedBytes += length;
			}
			if (HighestIndex == index)
			{
				UpdateHighestIndex();
			}
			_startIndex = 0;
			if (ItemsCount == 0)
			{
				Constants.ENSURE(HighestIndex == byte.MaxValue, "if there is no items, HighestIndex must be clear");
				Constants.ENSURE(UsedBytes == 0, "should be no bytes used in clean page");
				NextFreePosition = 32;
				FragmentedBytes = 0;
			}
			IsDirty = true;
		}

		public BufferSlice Update(byte index, ushort bytesLength)
		{
			Constants.ENSURE(_buffer.ShareCounter == Constants.BUFFER_WRITABLE, "page must be writable to support changes");
			Constants.ENSURE(bytesLength > 0, "must update more than 0 bytes");
			int positionAddr = CalcPositionAddr(index);
			int lengthAddr = CalcLengthAddr(index);
			ushort position = _buffer.ReadUInt16(positionAddr);
			ushort length = _buffer.ReadUInt16(lengthAddr);
			Constants.ENSURE(IsValidPos(position), "invalid segment position");
			Constants.ENSURE(IsValidLen(length), "invalid segment length");
			bool isLastSegment = position + length == NextFreePosition;
			IsDirty = true;
			if (bytesLength == length)
			{
				return _buffer.Slice(position, length);
			}
			if (bytesLength < length)
			{
				ushort diff = (ushort)(length - bytesLength);
				if (isLastSegment)
				{
					NextFreePosition -= diff;
				}
				else
				{
					FragmentedBytes += diff;
				}
				UsedBytes -= diff;
				_buffer.Write(bytesLength, lengthAddr);
				_buffer.Clear(position + bytesLength, diff);
				return _buffer.Slice(position, bytesLength);
			}
			_buffer.Clear(position, length);
			ItemsCount--;
			UsedBytes -= length;
			if (isLastSegment)
			{
				NextFreePosition = position;
			}
			else
			{
				FragmentedBytes += length;
			}
			((BufferSlice)_buffer).Write((ushort)0, positionAddr);
			((BufferSlice)_buffer).Write((ushort)0, lengthAddr);
			return InternalInsert(bytesLength, ref index);
		}

		public void Defrag()
		{
			Constants.ENSURE(FragmentedBytes > 0, "do not call this when page has no fragmentation");
			Constants.ENSURE(_buffer.ShareCounter == Constants.BUFFER_WRITABLE, "page must be writable to support changes");
			Constants.ENSURE(HighestIndex < byte.MaxValue, "there is no items in this page to run defrag");
			SortedList<ushort, byte> segments = new SortedList<ushort, byte>();
			for (int index2 = 0; index2 <= HighestIndex; index2++)
			{
				int positionAddr = CalcPositionAddr((byte)index2);
				ushort position = _buffer.ReadUInt16(positionAddr);
				if (position != 0)
				{
					Constants.ENSURE(IsValidPos(position), "invalid segment position");
					segments.Add(position, (byte)index2);
				}
			}
			ushort next = 32;
			foreach (KeyValuePair<ushort, byte> slot in segments)
			{
				byte index = slot.Value;
				ushort position2 = slot.Key;
				int lengthAddr = CalcLengthAddr(index);
				ushort length = _buffer.ReadUInt16(lengthAddr);
				Constants.ENSURE(IsValidLen(length), "invalid segment length");
				if (position2 != next)
				{
					Constants.ENSURE(position2 > next, "current segment position must be greater than current empty space");
					System.Buffer.BlockCopy(_buffer.Array, _buffer.Offset + position2, _buffer.Array, _buffer.Offset + next, length);
					int positionAddr2 = CalcPositionAddr(index);
					_buffer.Write(next, positionAddr2);
				}
				next = (ushort)(next + length);
			}
			int emptyLength = 8192 - next - FooterSize;
			_buffer.Array.Fill(0, _buffer.Offset + next, emptyLength);
			FragmentedBytes = 0;
			NextFreePosition = next;
		}

		private byte GetFreeIndex()
		{
			for (byte index = _startIndex; index <= HighestIndex; index = (byte)(index + 1))
			{
				int positionAddr = CalcPositionAddr(index);
				if (_buffer.ReadUInt16(positionAddr) == 0)
				{
					_startIndex = (byte)(index + 1);
					return index;
				}
			}
			return (byte)(HighestIndex + 1);
		}

		public IEnumerable<byte> GetUsedIndexs()
		{
			if (ItemsCount == 0)
			{
				yield break;
			}
			Constants.ENSURE(HighestIndex != byte.MaxValue, "if has items count, Highest index should be not empty");
			for (byte index = 0; index <= HighestIndex; index = (byte)(index + 1))
			{
				int positionAddr = CalcPositionAddr(index);
				if (_buffer.ReadUInt16(positionAddr) != 0)
				{
					yield return index;
				}
			}
		}

		private void UpdateHighestIndex()
		{
			Constants.ENSURE(HighestIndex < byte.MaxValue, "can run only if contains a valid HighestIndex");
			if (HighestIndex == 0)
			{
				HighestIndex = byte.MaxValue;
				return;
			}
			for (int index = HighestIndex - 1; index >= 0; index--)
			{
				int positionAddr = CalcPositionAddr((byte)index);
				ushort position = _buffer.ReadUInt16(positionAddr);
				if (position != 0)
				{
					Constants.ENSURE(IsValidPos(position), "invalid segment position");
					HighestIndex = (byte)index;
					return;
				}
			}
			HighestIndex = byte.MaxValue;
		}

		private bool IsValidPos(ushort position)
		{
			if (position >= 32)
			{
				return position < 8192 - FooterSize;
			}
			return false;
		}

		private bool IsValidLen(ushort length)
		{
			if (length > 0)
			{
				return length <= 8160 - FooterSize;
			}
			return false;
		}

		public static int CalcPositionAddr(byte index)
		{
			return 8192 - (index + 1) * 4 + 2;
		}

		public static int CalcLengthAddr(byte index)
		{
			return 8192 - (index + 1) * 4;
		}

		public static long GetPagePosition(uint pageID)
		{
			checked
			{
				return unchecked((long)pageID) * 8192L;
			}
		}

		public static long GetPagePosition(int pageID)
		{
			Constants.ENSURE(pageID >= 0, "page could not be less than 0.");
			return GetPagePosition((uint)pageID);
		}

		public static T ReadPage<T>(PageBuffer buffer) where T : BasePage
		{
			if (typeof(T) == typeof(BasePage))
			{
				return (T)new BasePage(buffer);
			}
			if (typeof(T) == typeof(HeaderPage))
			{
				return (T)(BasePage)new HeaderPage(buffer);
			}
			if (typeof(T) == typeof(CollectionPage))
			{
				return (T)(BasePage)new CollectionPage(buffer);
			}
			if (typeof(T) == typeof(IndexPage))
			{
				return (T)(BasePage)new IndexPage(buffer);
			}
			if (typeof(T) == typeof(DataPage))
			{
				return (T)(BasePage)new DataPage(buffer);
			}
			throw new InvalidCastException();
		}

		public static T CreatePage<T>(PageBuffer buffer, uint pageID) where T : BasePage
		{
			if (typeof(T) == typeof(CollectionPage))
			{
				return (T)(BasePage)new CollectionPage(buffer, pageID);
			}
			if (typeof(T) == typeof(IndexPage))
			{
				return (T)(BasePage)new IndexPage(buffer, pageID);
			}
			if (typeof(T) == typeof(DataPage))
			{
				return (T)(BasePage)new DataPage(buffer, pageID);
			}
			throw new InvalidCastException();
		}

		public override string ToString()
		{
			return $"PageID: {PageID.ToString().PadLeft(4, '0')} : {PageType} ({ItemsCount} Items)";
		}
	}
}
