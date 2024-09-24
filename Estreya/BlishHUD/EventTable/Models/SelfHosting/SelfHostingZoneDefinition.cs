using System.Collections.Generic;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.SelfHosting
{
	public class SelfHostingZoneDefinition
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("events")]
		public List<SelfHostingEventDefinition> Events { get; set; }
	}
}
