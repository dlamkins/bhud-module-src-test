namespace DavidRice.BlishHud.MidiControl.Core
{
	public readonly struct MidiNoteEvent
	{
		public int NoteNumber { get; }

		public bool IsNoteOn { get; }

		public MidiNoteEvent(int noteNumber, bool isNoteOn)
		{
			NoteNumber = noteNumber;
			IsNoteOn = isNoteOn;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1}, {2})", MidiNote.GetNoteName(NoteNumber), NoteNumber, IsNoteOn ? "on" : "off");
		}
	}
}
