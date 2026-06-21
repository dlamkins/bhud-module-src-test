using System;

namespace DavidRice.BlishHud.MidiControl.Core
{
	public static class MidiNote
	{
		private static readonly string[] NoteNames = new string[12]
		{
			"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A",
			"A#", "B"
		};

		public static string GetNoteName(int noteNumber)
		{
			string noteName = NoteNames[noteNumber % 12];
			int octave = noteNumber / 12 - 1;
			return $"{noteName}{octave}";
		}

		public static bool TryParseNoteName(string? noteName, out int noteNumber)
		{
			noteNumber = 0;
			if (string.IsNullOrWhiteSpace(noteName))
			{
				return false;
			}
			noteName = noteName!.Trim();
			if (noteName!.Length < 2)
			{
				return false;
			}
			char letter = char.ToUpperInvariant(noteName![0]);
			if (letter < 'A' || letter > 'G')
			{
				return false;
			}
			int accidentalOffset = 0;
			int octaveStartIndex = 1;
			if (noteName!.Length > 1)
			{
				switch (noteName![1])
				{
				case '#':
					accidentalOffset = 1;
					octaveStartIndex = 2;
					break;
				case 'B':
				case 'b':
					accidentalOffset = -1;
					octaveStartIndex = 2;
					break;
				}
			}
			if (octaveStartIndex >= noteName!.Length)
			{
				return false;
			}
			if (!int.TryParse(noteName!.Substring(octaveStartIndex), out var octave))
			{
				return false;
			}
			int semitone = letter switch
			{
				'C' => 0, 
				'D' => 2, 
				'E' => 4, 
				'F' => 5, 
				'G' => 7, 
				'A' => 9, 
				'B' => 11, 
				_ => throw new InvalidOperationException("Unexpected note letter."), 
			} + accidentalOffset;
			noteNumber = (octave + 1) * 12 + semitone;
			if (noteNumber < 0 || noteNumber > 127)
			{
				return false;
			}
			return true;
		}
	}
}
