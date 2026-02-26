using Newtonsoft.Json;

namespace Gorthax.Gilledwars
{
	public class FishData
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("rarity")]
		public string Rarity { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("time")]
		public string Time { get; set; }

		[JsonProperty("bait")]
		public string Bait { get; set; }

		[JsonProperty("fishing_hole")]
		public string FishingHole { get; set; }

		[JsonProperty("item_id")]
		public int ItemId { get; set; }

		[JsonProperty("min_weight")]
		public double MinW { get; set; } = 1.0;


		[JsonProperty("max_weight")]
		public double MaxW { get; set; } = 10.0;


		[JsonProperty("min_length")]
		public double MinL { get; set; } = 5.0;


		[JsonProperty("max_length")]
		public double MaxL { get; set; } = 20.0;

	}
}
