using Newtonsoft.Json;

namespace Gorthax.Gilledwars
{
	public class LeaderboardEntry
	{
		[JsonProperty("player_name")]
		public string PlayerName { get; set; }

		[JsonProperty("fish_name")]
		public string FishName { get; set; }

		[JsonProperty("weight")]
		public double Weight { get; set; }

		[JsonProperty("length")]
		public double Length { get; set; }

		[JsonProperty("record_type")]
		public string RecordType { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }
	}
}
