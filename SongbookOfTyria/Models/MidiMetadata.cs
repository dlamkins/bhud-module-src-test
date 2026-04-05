using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class MidiMetadata
	{
		[JsonProperty("converter_version")]
		public string ConverterVersion { get; set; }

		[JsonProperty("track_count")]
		public int TrackCount { get; set; }

		[JsonProperty("piano_mode")]
		public bool PianoMode { get; set; }

		[JsonProperty("bars_per_row")]
		public int BarsPerRow { get; set; }
	}
}
