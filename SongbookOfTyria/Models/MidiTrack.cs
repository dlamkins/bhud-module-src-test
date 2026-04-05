using System.Collections.Generic;
using Newtonsoft.Json;

namespace SongbookOfTyria.Models
{
	public class MidiTrack
	{
		[JsonProperty("index")]
		public int Index { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("instrument")]
		public MidiInstrument Instrument { get; set; }

		[JsonProperty("is_bass")]
		public bool IsBass { get; set; }

		[JsonProperty("notation")]
		public string Notation { get; set; }

		[JsonProperty("note_count")]
		public int NoteCount { get; set; }

		[JsonProperty("notes")]
		public List<MidiNote> Notes { get; set; }

		public string GetDisplayName()
		{
			if (Instrument?.Gw2Name != null)
			{
				return Instrument.Gw2Name;
			}
			if (!string.IsNullOrEmpty(Name))
			{
				return Name;
			}
			return $"Track {Index + 1}";
		}
	}
}
