namespace LiteDB.Engine
{
	internal struct PagePosition
	{
		public uint PageID;

		public long Position;

		public static PagePosition Empty => new PagePosition(uint.MaxValue, long.MaxValue);

		public bool IsEmpty
		{
			get
			{
				if (PageID == uint.MaxValue)
				{
					return Position == long.MaxValue;
				}
				return false;
			}
		}

		public override bool Equals(object obj)
		{
			PagePosition other = (PagePosition)obj;
			if (PageID == other.PageID)
			{
				return Position == other.PageID;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (int)((17 * 23 + PageID) * 23) + (int)Position;
		}

		public PagePosition(uint pageID, long position)
		{
			PageID = pageID;
			Position = position;
		}

		public override string ToString()
		{
			if (!IsEmpty)
			{
				return ((PageID == uint.MaxValue) ? "----" : PageID.ToString()) + ":" + Position;
			}
			return "----:----";
		}
	}
}
