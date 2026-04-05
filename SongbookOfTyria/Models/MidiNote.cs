using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class MidiNote
	{
		[JsonProperty("midi")]
		public int Midi { get; set; }

		[JsonProperty("time")]
		public double Time { get; set; }

		[JsonProperty("duration")]
		public double Duration { get; set; }

		[JsonProperty("velocity")]
		public int Velocity { get; set; }
	}
}
