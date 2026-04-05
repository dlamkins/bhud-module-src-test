using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class MidiTimeSignature
	{
		[JsonProperty("ticks")]
		public int Ticks { get; set; }

		[JsonProperty("numerator")]
		public int Numerator { get; set; }

		[JsonProperty("denominator")]
		public int Denominator { get; set; }
	}
}
