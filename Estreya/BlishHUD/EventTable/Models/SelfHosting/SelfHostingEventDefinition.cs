using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.SelfHosting
{
	public class SelfHostingEventDefinition
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("wikiUrl")]
		public string WikiUrl { get; set; }
	}
}
