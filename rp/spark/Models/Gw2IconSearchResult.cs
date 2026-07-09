using System;
using System.Collections.Generic;

namespace rp.spark.Models
{
	public class Gw2IconSearchResult
	{
		public int AssetId { get; set; }

		public string Source { get; set; } = string.Empty;


		public string Name { get; set; } = string.Empty;


		public string Description { get; set; } = string.Empty;


		public IReadOnlyList<string> Keywords { get; set; } = Array.Empty<string>();

	}
}
