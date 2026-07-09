using System;
using System.Collections.Generic;

namespace rp.spark.UI.Controls
{
	internal sealed class PageList
	{
		public const int DefaultPageSize = 50;

		public int PageIndex { get; private set; }

		public int PageSize { get; }

		public PageList(int pageSize = 50)
		{
			PageSize = Math.Max(1, pageSize);
		}

		public int GetPageCount(int itemCount)
		{
			itemCount = Math.Max(0, itemCount);
			return Math.Max(1, (itemCount + PageSize - 1) / PageSize);
		}

		public void Reset()
		{
			PageIndex = 0;
		}

		public void Clamp(int itemCount)
		{
			PageIndex = Math.Max(0, Math.Min(PageIndex, GetPageCount(itemCount) - 1));
		}

		public bool Previous()
		{
			if (PageIndex <= 0)
			{
				return false;
			}
			PageIndex--;
			return true;
		}

		public bool Next(int itemCount)
		{
			if (PageIndex + 1 >= GetPageCount(itemCount))
			{
				return false;
			}
			PageIndex++;
			return true;
		}

		public int GetFirstItemNumber(int itemCount)
		{
			if (itemCount > 0)
			{
				return PageIndex * PageSize + 1;
			}
			return 0;
		}

		public int GetLastItemNumber(int itemCount)
		{
			if (itemCount > 0)
			{
				return Math.Min((PageIndex + 1) * PageSize, itemCount);
			}
			return 0;
		}

		public IReadOnlyList<T> GetPage<T>(IReadOnlyList<T> items)
		{
			int itemCount = items?.Count ?? 0;
			Clamp(itemCount);
			if (itemCount == 0)
			{
				return new List<T>();
			}
			int start = PageIndex * PageSize;
			int count = Math.Min(PageSize, itemCount - start);
			List<T> page = new List<T>(count);
			for (int index = start; index < start + count; index++)
			{
				page.Add(items[index]);
			}
			return page;
		}
	}
}
