using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class MidiTempo
	{
		[JsonProperty("ticks")]
		public int Ticks { get; set; }

		[JsonProperty("bpm")]
		public double Bpm { get; set; }

		[JsonProperty("microseconds_per_beat")]
		public int MicrosecondsPerBeat { get; set; }
	}
}
