using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class HelpScreenContent
	{
		[JsonProperty("type")]
		public string Type { get; set; } = "";


		[JsonProperty("value")]
		public string Value { get; set; } = "";

	}
}
