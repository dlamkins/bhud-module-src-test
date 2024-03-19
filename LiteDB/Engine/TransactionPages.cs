using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class TransactionPages
	{
		public int TransactionSize { get; set; }

		public Dictionary<uint, PagePosition> DirtyPages { get; } = new Dictionary<uint, PagePosition>();


		public List<uint> NewPages { get; } = new List<uint>();


		public uint FirstDeletedPageID { get; set; } = uint.MaxValue;


		public uint LastDeletedPageID { get; set; } = uint.MaxValue;


		public int DeletedPages { get; set; }

		public bool HeaderChanged
		{
			get
			{
				if (NewPages.Count <= 0 && DeletedPages <= 0)
				{
					return this.Commit != null;
				}
				return true;
			}
		}

		public event Action<HeaderPage> Commit;

		public void OnCommit(HeaderPage header)
		{
			this.Commit?.Invoke(header);
		}
	}
}
