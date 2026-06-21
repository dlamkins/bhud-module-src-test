using System.Collections.Generic;
using DavidRice.BlishHud.MidiControl.Core;

namespace DavidRice.BlishHud.MidiControl.Keymaps.Visualization
{
	public static class KeybedLayoutCalculator
	{
		private static readonly int[] BlackKeySemitones = new int[5] { 1, 3, 6, 8, 10 };

		public static KeybedLayout Calculate(Keymap keymap)
		{
			List<(string, int, NoteDefinition)> mappedNotes = new List<(string, int, NoteDefinition)>();
			foreach (KeyValuePair<string, NoteDefinition> pair in keymap.Notes)
			{
				if (!string.IsNullOrWhiteSpace(pair.Value.Key) && MidiNote.TryParseNoteName(pair.Key, out var noteNumber))
				{
					mappedNotes.Add((pair.Key, noteNumber, pair.Value));
				}
			}
			if (mappedNotes.Count == 0)
			{
				return KeybedLayout.Empty;
			}
			int minNoteNumber = mappedNotes[0].Item2;
			int maxNoteNumber = mappedNotes[0].Item2;
			foreach (var mapped2 in mappedNotes)
			{
				if (mapped2.Item2 < minNoteNumber)
				{
					minNoteNumber = mapped2.Item2;
				}
				if (mapped2.Item2 > maxNoteNumber)
				{
					maxNoteNumber = mapped2.Item2;
				}
			}
			int startOctave = minNoteNumber / 12 - 1;
			int endOctave = maxNoteNumber / 12 - 1;
			int startNote = (startOctave + 1) * 12;
			int endNote = (endOctave + 1) * 12 + 11;
			Dictionary<int, (string, NoteDefinition)> mappedByNoteNumber = new Dictionary<int, (string, NoteDefinition)>();
			foreach (var mapped in mappedNotes)
			{
				mappedByNoteNumber[mapped.Item2] = (mapped.Item1, mapped.Item3);
			}
			List<KeybedKey> keys = new List<KeybedKey>();
			for (int note = startNote; note <= endNote; note++)
			{
				bool isBlackKey = IsBlackKey(note);
				(string, NoteDefinition) mappedEntry;
				bool isMapped = mappedByNoteNumber.TryGetValue(note, out mappedEntry);
				string text;
				if (!isMapped)
				{
					text = MidiNote.GetNoteName(note);
				}
				else
				{
					(text, _) = mappedEntry;
				}
				string noteName = text;
				string gw2Key = (isMapped ? mappedEntry.Item2.Key : null);
				int? octave = (isMapped ? mappedEntry.Item2.Octave : null);
				bool hasAltOctave = isMapped && mappedEntry.Item2.AltOctave.HasValue;
				int? altOctave = (hasAltOctave ? mappedEntry.Item2.AltOctave : null);
				string altOctaveKey = (hasAltOctave ? mappedEntry.Item2.AltOctaveKey : null);
				bool isKeySwitch = isMapped && ((!string.IsNullOrWhiteSpace(keymap.OctaveDownKey) && gw2Key == keymap.OctaveDownKey) || (!string.IsNullOrWhiteSpace(keymap.OctaveUpKey) && gw2Key == keymap.OctaveUpKey));
				keys.Add(new KeybedKey(note, noteName, isBlackKey, isMapped, gw2Key, isKeySwitch, octave, hasAltOctave, altOctave, altOctaveKey));
			}
			return new KeybedLayout(keys, startOctave, endOctave);
		}

		private static bool IsBlackKey(int noteNumber)
		{
			int semitone = noteNumber % 12;
			int[] blackKeySemitones = BlackKeySemitones;
			foreach (int blackSemitone in blackKeySemitones)
			{
				if (semitone == blackSemitone)
				{
					return true;
				}
			}
			return false;
		}
	}
}
