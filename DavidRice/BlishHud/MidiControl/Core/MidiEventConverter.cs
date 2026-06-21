using NAudio.Midi;

namespace DavidRice.BlishHud.MidiControl.Core
{
	public static class MidiEventConverter
	{
		public static MidiNoteEvent? TryConvertToMidiNoteEvent(MidiEvent? midiEvent)
		{
			NoteEvent noteEvent = midiEvent as NoteEvent;
			if (noteEvent == null)
			{
				return null;
			}
			if (MidiEvent.IsNoteOn(noteEvent))
			{
				return new MidiNoteEvent(noteEvent.NoteNumber, isNoteOn: true);
			}
			if (MidiEvent.IsNoteOff(noteEvent))
			{
				return new MidiNoteEvent(noteEvent.NoteNumber, isNoteOn: false);
			}
			return null;
		}
	}
}
