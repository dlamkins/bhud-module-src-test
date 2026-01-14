using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Maestro.Services.Data
{
	public static class AhkParser
	{
		private static readonly Dictionary<string, string> NumpadToNote = new Dictionary<string, string>
		{
			{ "Numpad1", "C" },
			{ "Numpad2", "D" },
			{ "Numpad3", "E" },
			{ "Numpad4", "F" },
			{ "Numpad5", "G" },
			{ "Numpad6", "A" },
			{ "Numpad7", "B" },
			{ "Numpad8", "C^" }
		};

		private static readonly Dictionary<string, string> NumpadToSharp = new Dictionary<string, string>
		{
			{ "Numpad1", "C#" },
			{ "Numpad2", "D#" },
			{ "Numpad3", "F#" },
			{ "Numpad4", "G#" },
			{ "Numpad5", "A#" }
		};

		private static readonly Regex KeyDownPattern = new Regex("\\{(Numpad[1-8])\\s+down\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex AltDownPattern = new Regex("LAlt\\s+down", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex AltUpPattern = new Regex("LAlt\\s+up", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex QuickPressPattern = new Regex("SendInput\\s*\\{(Numpad[09])\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex SleepPattern = new Regex("Sleep,\\s*(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static List<string> ParseToCompact(string ahkContent)
		{
			string[] array = ahkContent.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			List<string> result = new List<string>();
			List<string> currentNotes = new List<string>();
			int currentOctave = 0;
			bool altHeld = false;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string trimmed = array2[i].Trim();
				if (trimmed.StartsWith("PlaySong", StringComparison.OrdinalIgnoreCase) || trimmed == "{" || trimmed == "}")
				{
					continue;
				}
				if (AltDownPattern.IsMatch(trimmed))
				{
					altHeld = true;
				}
				if (AltUpPattern.IsMatch(trimmed))
				{
					altHeld = false;
				}
				MatchCollection matchCollection = KeyDownPattern.Matches(trimmed);
				bool hasAltOnLine = AltDownPattern.IsMatch(trimmed);
				foreach (Match item in matchCollection)
				{
					string numpad2 = item.Groups[1].Value;
					string note;
					if ((altHeld || hasAltOnLine) && NumpadToSharp.TryGetValue(numpad2, out var sharpNote))
					{
						note = sharpNote;
					}
					else
					{
						if (!NumpadToNote.TryGetValue(numpad2, out var naturalNote))
						{
							continue;
						}
						note = naturalNote;
					}
					string noteWithOctave = ApplyOctaveModifier(note, currentOctave);
					currentNotes.Add(noteWithOctave);
				}
				Match quickPressMatch = QuickPressPattern.Match(trimmed);
				if (quickPressMatch.Success)
				{
					string numpad = quickPressMatch.Groups[1].Value;
					if (numpad == "Numpad9")
					{
						currentOctave++;
					}
					else if (numpad == "Numpad0")
					{
						currentOctave--;
					}
				}
				Match sleepMatch = SleepPattern.Match(trimmed);
				if (sleepMatch.Success && currentNotes.Count > 0)
				{
					string durationMs = sleepMatch.Groups[1].Value;
					string noteLine = string.Join(" ", currentNotes.ConvertAll((string n) => n + ":" + durationMs));
					result.Add(noteLine);
					currentNotes.Clear();
				}
			}
			return result;
		}

		private static string ApplyOctaveModifier(string note, int octave)
		{
			if (octave == 0)
			{
				return note;
			}
			string modifier = ((octave > 0) ? "+" : "-");
			return note + modifier;
		}
	}
}
