using System.Collections.Generic;
using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class MidiData
	{
		[JsonProperty("tracks")]
		public List<MidiTrack> Tracks { get; set; }

		[JsonProperty("notation")]
		public string Notation { get; set; }

		[JsonProperty("duration")]
		public double Duration { get; set; }

		[JsonProperty("ppq")]
		public int Ppq { get; set; }

		[JsonProperty("tempos")]
		public List<MidiTempo> Tempos { get; set; }

		[JsonProperty("time_signatures")]
		public List<MidiTimeSignature> TimeSignatures { get; set; }

		[JsonProperty("metadata")]
		public MidiMetadata Metadata { get; set; }
	}
}
