using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models
{
	public class ScoreModel
	{
		[JsonProperty("min")]
		public int Min { get; set; }

		[JsonProperty("max")]
		public int Max { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; } = "missing value";


		[JsonProperty("color")]
		public string Color { get; set; } = "#FFFFFF";

	}
}
