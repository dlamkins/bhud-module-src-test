using System;
using System.Collections.Generic;
using System.Linq;

namespace DavidRice.BlishHud.MidiControl.Keymaps
{
	public static class KeymapPreviewFormatter
	{
		public static IReadOnlyList<string> FormatLines(Keymap keymap)
		{
			Keymap keymap2 = keymap;
			if (keymap2 == null)
			{
				throw new ArgumentNullException("keymap");
			}
			List<string> list = (from kvp in keymap2.Notes
				select FormatLine(kvp.Key, kvp.Value, keymap2) into line
				where line != null
				select line).Cast<string>().ToList();
			list.Sort(CompareNoteLines);
			return list;
		}

		private static string? FormatLine(string noteName, NoteDefinition definition, Keymap keymap)
		{
			if (definition.ForceInternalOctave.HasValue)
			{
				return $"{noteName} → internal octave: {definition.ForceInternalOctave.Value}";
			}
			if (definition.Key == null)
			{
				return null;
			}
			if (!definition.Octave.HasValue)
			{
				if (definition.Key!.Equals(keymap.OctaveDownKey, StringComparison.OrdinalIgnoreCase) || definition.Key!.Equals(keymap.OctaveUpKey, StringComparison.OrdinalIgnoreCase))
				{
					return noteName + " → " + definition.Key + " (oct shift)";
				}
				return noteName + " → " + definition.Key;
			}
			string line = $"{noteName} → {definition.Key} (oct {definition.Octave.Value})";
			if (definition.AltOctave.HasValue && definition.AltOctaveKey != null)
			{
				line += $" | alt: {definition.AltOctaveKey} on oct {definition.AltOctave.Value}";
			}
			return line;
		}

		private static int CompareNoteLines(string a, string b)
		{
			(int, char, int) keyA = ParseSortKey(ExtractNoteName(a));
			(int, char, int) keyB = ParseSortKey(ExtractNoteName(b));
			if (keyA.Item1 != keyB.Item1)
			{
				return keyA.Item1.CompareTo(keyB.Item1);
			}
			if (keyA.Item2 != keyB.Item2)
			{
				return keyA.Item2.CompareTo(keyB.Item2);
			}
			return keyA.Item3.CompareTo(keyB.Item3);
		}

		private static string ExtractNoteName(string line)
		{
			int arrow = line.IndexOf(" → ", StringComparison.Ordinal);
			if (arrow <= 0)
			{
				return line;
			}
			return line.Substring(0, arrow);
		}

		private static (int octave, char letter, int accidental) ParseSortKey(string noteName)
		{
			int digitStart;
			for (digitStart = 0; digitStart < noteName.Length && !char.IsDigit(noteName[digitStart]); digitStart++)
			{
			}
			string notePart = noteName.Substring(0, digitStart);
			int octave = ((digitStart < noteName.Length) ? int.Parse(noteName.Substring(digitStart)) : int.MaxValue);
			char letter = notePart[0];
			string accidentalStr = ((notePart.Length > 1) ? notePart.Substring(1) : "");
			int num = ((!(accidentalStr == "b")) ? ((accidentalStr != null && accidentalStr.Length == 0) ? 1 : ((!(accidentalStr == "#")) ? ((!accidentalStr.StartsWith("b")) ? ((!accidentalStr.StartsWith("#")) ? 1 : 2) : 0) : 2)) : 0);
			int accidental = num;
			return (octave, letter, accidental);
		}
	}
}
