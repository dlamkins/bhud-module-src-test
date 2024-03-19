namespace LiteDB.Engine
{
	internal class IndexNode
	{
		private const int INDEX_NODE_FIXED_SIZE = 12;

		private const int P_SLOT = 0;

		private const int P_LEVEL = 1;

		private const int P_DATA_BLOCK = 2;

		private const int P_NEXT_NODE = 7;

		private const int P_PREV_NEXT = 12;

		private readonly IndexPage _page;

		private readonly BufferSlice _segment;

		private int P_KEY => 12 + Level * 5 * 2;

		public PageAddress Position { get; }

		public byte Slot { get; }

		public byte Level { get; }

		public BsonValue Key { get; }

		public PageAddress DataBlock { get; }

		public PageAddress NextNode { get; private set; }

		public PageAddress[] Prev { get; private set; }

		public PageAddress[] Next { get; private set; }

		public IndexPage Page => _page;

		public static int GetNodeLength(byte level, BsonValue key, out int keyLength)
		{
			keyLength = GetKeyLength(key, recalc: true);
			return 12 + level * 2 * 5 + keyLength;
		}

		public static int GetKeyLength(BsonValue key, bool recalc)
		{
			return 1 + ((key.IsString || key.IsBinary) ? 1 : 0) + key.GetBytesCount(recalc);
		}

		public IndexNode(IndexPage page, byte index, BufferSlice segment)
		{
			_page = page;
			_segment = segment;
			Position = new PageAddress(page.PageID, index);
			Slot = segment.ReadByte(0);
			Level = segment.ReadByte(1);
			DataBlock = segment.ReadPageAddress(2);
			NextNode = segment.ReadPageAddress(7);
			Next = new PageAddress[Level];
			Prev = new PageAddress[Level];
			for (int i = 0; i < Level; i++)
			{
				Prev[i] = segment.ReadPageAddress(12 + i * 5 * 2);
				Next[i] = segment.ReadPageAddress(12 + i * 5 * 2 + 5);
			}
			Key = segment.ReadIndexKey(P_KEY);
		}

		public IndexNode(IndexPage page, byte index, BufferSlice segment, byte slot, byte level, BsonValue key, PageAddress dataBlock)
		{
			_page = page;
			_segment = segment;
			Position = new PageAddress(page.PageID, index);
			Slot = slot;
			Level = level;
			DataBlock = dataBlock;
			NextNode = PageAddress.Empty;
			Next = new PageAddress[level];
			Prev = new PageAddress[level];
			Key = key;
			segment.Write(slot, 0);
			segment.Write(level, 1);
			segment.Write(dataBlock, 2);
			segment.Write(NextNode, 7);
			for (int i = 0; i < level; i++)
			{
				SetPrev((byte)i, PageAddress.Empty);
				SetNext((byte)i, PageAddress.Empty);
			}
			segment.WriteIndexKey(key, P_KEY);
			page.IsDirty = true;
		}

		public IndexNode(BsonDocument doc)
		{
			_page = null;
			_segment = new BufferSlice(new byte[0], 0, 0);
			Position = new PageAddress(0u, 0);
			Slot = 0;
			Level = 0;
			DataBlock = PageAddress.Empty;
			NextNode = PageAddress.Empty;
			Next = new PageAddress[0];
			Prev = new PageAddress[0];
			Key = doc;
		}

		public void SetNextNode(PageAddress value)
		{
			NextNode = value;
			_segment.Write(value, 7);
			_page.IsDirty = true;
		}

		public void SetPrev(byte level, PageAddress value)
		{
			Constants.ENSURE(level <= Level, "out of index in level");
			Prev[level] = value;
			_segment.Write(value, 12 + level * 5 * 2);
			_page.IsDirty = true;
		}

		public void SetNext(byte level, PageAddress value)
		{
			Constants.ENSURE(level <= Level, "out of index in level");
			Next[level] = value;
			_segment.Write(value, 12 + level * 5 * 2 + 5);
			_page.IsDirty = true;
		}

		public PageAddress GetNextPrev(byte level, int order)
		{
			if (order != 1)
			{
				return Prev[level];
			}
			return Next[level];
		}

		public override string ToString()
		{
			return $"Pos: [{Position}] - Key: {Key}";
		}
	}
}
