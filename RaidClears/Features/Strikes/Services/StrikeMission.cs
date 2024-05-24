using System.Collections.Generic;
using Newtonsoft.Json;

namespace RaidClears.Features.Strikes.Services
{
	public class StrikeMission
	{
		[JsonProperty("id")]
		public string Id = "undefined";

		[JsonProperty("name")]
		public string Name = "undefined";

		[JsonProperty("abbriviation")]
		public string Abbriviation = "undefined";

		[JsonProperty("mapIds")]
		public List<int> MapIds = new List<int>();
	}
}
