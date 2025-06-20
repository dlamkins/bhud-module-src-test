using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models
{
	public class HelpScreenModel
	{
		[JsonProperty("name")]
		public string Name { get; set; } = "missing name";


		[JsonProperty("content")]
		public List<HelpContentModel> Content { get; set; } = new List<HelpContentModel>();

	}
}
