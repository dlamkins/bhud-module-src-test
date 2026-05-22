using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class KpRequirement
	{
		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("mode")]
		public string Mode { get; set; }
	}
}
