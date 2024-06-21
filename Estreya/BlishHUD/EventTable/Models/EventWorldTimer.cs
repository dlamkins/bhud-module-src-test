using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class EventWorldTimer
	{
		[JsonProperty("x")]
		public float X;

		[JsonProperty("y")]
		public float Y;

		[JsonProperty("z")]
		public float Z;

		[JsonProperty("rotation")]
		public float Rotation;
	}
}
