using Newtonsoft.Json;

namespace FarmingTracker
{
	public class FileStat
	{
		[JsonProperty("ApiId")]
		public int ApiId { get; set; }

		[JsonProperty("StatType")]
		public StatType StatType { get; set; }

		[JsonProperty("Count")]
		public long Signed_Count { get; set; }

		[JsonProperty("Visibility")]
		public StatVisibility StatVisibility { get; set; }

		[JsonProperty("Unsigned_CustomProfitInCopper")]
		public long? Unsigned_CustomProfitInCopper { get; set; }
	}
}
