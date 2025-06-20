using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class HelpScreenPanel
	{
		[JsonProperty("name")]
		public string Name { get; set; } = "";


		[JsonProperty("content")]
		public List<HelpScreenContent> Content { get; set; } = new List<HelpScreenContent>();

	}
}
