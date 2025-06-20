using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models
{
	public class HelpContentModel
	{
		[JsonProperty("type")]
		public string Type { get; set; } = "string";


		[JsonProperty("value")]
		public string Value { get; set; } = "missing value";

	}
}
