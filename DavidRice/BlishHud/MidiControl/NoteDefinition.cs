namespace DavidRice.BlishHud.MidiControl
{
	public class NoteDefinition
	{
		public string? Key { get; }

		public int? Octave { get; }

		public int? AltOctave { get; }

		public string? AltOctaveKey { get; }

		public int? ForceInternalOctave { get; }

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
