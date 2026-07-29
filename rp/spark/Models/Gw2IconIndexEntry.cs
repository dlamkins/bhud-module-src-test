using System.Collections.Generic;
using Newtonsoft.Json;

namespace rp.spark.Models
{
	public class Gw2IconIndexEntry
	{
		[JsonProperty("s")]
		public string Source { get; set; } = string.Empty;


		[JsonProperty("n")]
		public string Name { get; set; } = string.Empty;


		[JsonProperty("q")]
		public List<string> Aliases { get; set; } = new List<string>();


		[JsonProperty("d")]
		public string Description { get; set; } = string.Empty;


		[JsonProperty("a")]
		public int AssetId { get; set; }

		[JsonProperty("k")]
		public List<string> Keywords { get; set; } = new List<string>();

	}
}
