namespace LiteDB.Engine
{
	internal class DataBlock
	{
		public const int DATA_BLOCK_FIXED_SIZE = 6;

		public const int P_EXTEND = 0;

		public const int P_NEXT_BLOCK = 1;

		public const int P_BUFFER = 6;

		private readonly DataPage _page;

		private readonly BufferSlice _segment;

		public PageAddress Position { get; }

		public bool Extend { get; }

		public PageAddress NextBlock { get; private set; }

		public BufferSlice Buffer { get; }

		public DataBlock(DataPage page, byte index, BufferSlice segment)
		{
			_page = page;
			_segment = segment;
			Position = new PageAddress(page.PageID, index);
			Extend = segment.ReadBool(0);
			NextBlock = segment.ReadPageAddress(1);
			Buffer = segment.Slice(6, segment.Count - 6);
		}

		public DataBlock(DataPage page, byte index, BufferSlice segment, bool extend, PageAddress nextBlock)
		{
			_page = page;
			_segment = segment;
			Position = new PageAddress(page.PageID, index);
			NextBlock = nextBlock;
			Extend = extend;
			segment.Write(extend, 0);
			segment.Write(nextBlock, 1);
			Buffer = segment.Slice(6, segment.Count - 6);
			page.IsDirty = true;
		}

		public void SetNextBlock(PageAddress nextBlock)
		{
			NextBlock = nextBlock;
			_segment.Write(nextBlock, 1);
			_page.IsDirty = true;
		}

		public override string ToString()
		{
			return $"Pos: [{Position}] - Ext: [{Extend}] - Next: [{NextBlock}] - Buffer: [{Buffer}]";
		}
	}
}
