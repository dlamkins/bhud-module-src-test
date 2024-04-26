using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public struct EventLocations
	{
		[JsonProperty("tooltip")]
		public string Tooltip;

		[JsonProperty("map")]
		public EventMapLocation Map;

		[JsonProperty("world")]
		public float[] World;
	}
}
