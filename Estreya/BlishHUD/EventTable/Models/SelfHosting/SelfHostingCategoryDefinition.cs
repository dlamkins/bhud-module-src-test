using System.Collections.Generic;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.SelfHosting
{
	public class SelfHostingCategoryDefinition
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("zones")]
		public List<SelfHostingZoneDefinition> Zones { get; set; }
	}
}
