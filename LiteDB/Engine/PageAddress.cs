using System.Diagnostics;

namespace LiteDB.Engine
{
	[DebuggerStepThrough]
	internal struct PageAddress
	{
		public const int SIZE = 5;

		public static PageAddress Empty = new PageAddress(uint.MaxValue, byte.MaxValue);

		public readonly uint PageID;

		public readonly byte Index;

		public bool IsEmpty
		{
			get
			{
				if (PageID == uint.MaxValue)
				{
					return Index == byte.MaxValue;
				}
				return false;
			}
		}

		public override bool Equals(object obj)
		{
			PageAddress other = (PageAddress)obj;
			if (PageID == other.PageID)
			{
				return Index == other.Index;
			}
			return false;
		}

		public static bool operator ==(PageAddress lhs, PageAddress rhs)
		{
			if (lhs.PageID == rhs.PageID)
			{
				return lhs.Index == rhs.Index;
			}
			return false;
		}

		public static bool operator !=(PageAddress lhs, PageAddress rhs)
		{
			return !(lhs == rhs);
		}

		public override int GetHashCode()
		{
			return (int)((17 * 23 + PageID) * 23 + Index);
		}

		public PageAddress(uint pageID, byte index)
		{
			PageID = pageID;
			Index = index;
		}

		public override string ToString()
		{
			if (!IsEmpty)
			{
				return PageID.ToString().PadLeft(4, '0') + ":" + Index.ToString().PadLeft(2, '0');
			}
			return "(empty)";
		}

		public BsonValue ToBsonValue()
		{
			if (IsEmpty)
			{
				return BsonValue.Null;
			}
			return new BsonDocument
			{
				["pageID"] = (int)PageID,
				["index"] = Index
			};
		}
	}
}
