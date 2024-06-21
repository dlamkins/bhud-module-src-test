using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class EventMapTimer
	{
		[JsonProperty("x")]
		public float X;

		[JsonProperty("y")]
		public float Y;

		[JsonProperty("radius")]
		public float Radius;
	}
}
