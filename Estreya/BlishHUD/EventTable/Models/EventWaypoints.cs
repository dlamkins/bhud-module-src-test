using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public struct EventWaypoints
	{
		[JsonProperty("EU")]
		public string EU;

		[JsonProperty("NA")]
		public string NA;
	}
}
