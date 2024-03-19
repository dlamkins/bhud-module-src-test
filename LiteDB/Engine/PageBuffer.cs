using System.Threading;

namespace LiteDB.Engine
{
	internal class PageBuffer : BufferSlice
	{
		public readonly int UniqueID;

		public long Position;

		public FileOrigin Origin;

		public int ShareCounter;

		public long Timestamp;

		public PageBuffer(byte[] buffer, int offset, int uniqueID)
			: base(buffer, offset, 8192)
		{
			UniqueID = uniqueID;
			Position = long.MaxValue;
			Origin = FileOrigin.None;
			ShareCounter = 0;
			Timestamp = 0L;
		}

		public void Release()
		{
			Constants.ENSURE(ShareCounter > 0, "share counter must be > 0 in Release()");
			Interlocked.Decrement(ref ShareCounter);
		}

		public override string ToString()
		{
			string p = ((Position == long.MaxValue) ? "<empty>" : Position.ToString());
			string s = ((ShareCounter == Constants.BUFFER_WRITABLE) ? "<writable>" : ShareCounter.ToString());
			uint pageID = this.ReadUInt32(0);
			byte pageType = base[4];
			return string.Format("ID: {0} - Position: {1}/{2} - Shared: {3} - ({4}) :: Content: [{5}/{6}]", UniqueID, p, Origin, s, base.ToString(), pageID.ToString("0:0000"), (PageType)pageType);
		}

		public unsafe bool IsBlank()
		{
			fixed (byte* arrayPtr = base.Array)
			{
				ulong* ptr = (ulong*)(arrayPtr + base.Offset);
				if (*ptr == 0L)
				{
					return ptr[1] == 0;
				}
				return false;
			}
		}
	}
}
