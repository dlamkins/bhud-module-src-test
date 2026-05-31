namespace DavidRice.BlishHud.MidiControl
{
	public class NoteDefinition
	{
		public string? Key { get; set; }

		public int? Octave { get; set; }

		public int? AltOctave { get; set; }

		public string? AltOctaveKey { get; set; }

		public int? ForceInternalOctave { get; set; }

		public NoteDefinition()
		{
		}

		public NoteDefinition(string? key = null, int? octave = null, int? altOctave = null, string? altOctaveKey = null, int? forceInternalOctave = null)
		{
			Key = key;
			Octave = octave;
			AltOctave = altOctave;
			AltOctaveKey = altOctaveKey;
			ForceInternalOctave = forceInternalOctave;
		}
	}
}
