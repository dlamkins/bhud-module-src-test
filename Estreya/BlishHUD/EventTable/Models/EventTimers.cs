using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class EventTimers
	{
		[JsonProperty("mapId")]
		public int MapID;

		[JsonProperty("map")]
		public EventMapTimer[] Map;

		[JsonProperty("world")]
		public EventWorldTimer[] World;
	}
}
