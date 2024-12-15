using Newtonsoft.Json;

namespace RaidClears.Features.Raids.Models
{
	public class Aerodrome
	{
		[JsonProperty("id")]
		public string Name = "undefined";

		[JsonProperty("abbriviation")]
		public string Abbriviation = "undefined";

		[JsonProperty("mapId")]
		public int MapId = -1;
	}
}
