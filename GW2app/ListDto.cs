using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2app
{
	internal class ListDto
	{
		[JsonProperty("id")]
		public string Id;

		[JsonProperty("name")]
		public string Name;

		[JsonProperty("settings")]
		public JToken Settings;

		[JsonProperty("entries")]
		public List<EntryDto> Entries;

		[JsonProperty("is_loot_bag")]
		public bool IsLootBag;
	}
}
