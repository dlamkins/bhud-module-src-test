using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class PaginatedResponse<T>
	{
		public List<T> Items { get; set; } = new List<T>();


		public int TotalCount { get; set; }

		public int Page { get; set; }

		public int PageSize { get; set; }

		public int TotalPages
		{
			get
			{
				if (PageSize <= 0)
				{
					return 0;
				}
				return (int)Math.Ceiling((double)TotalCount / (double)PageSize);
			}
		}

		public bool HasPreviousPage => Page > 1;

		public bool HasNextPage => Page < TotalPages;
	}
}
