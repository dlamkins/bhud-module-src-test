using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Maestro.Services.Data
{
	public static class AhkParser
	{
		private class PendingNote
		{
			public string Note { get; set; }

			public string NumpadKey { get; set; }

			public bool IsSharp { get; set; }
		}

		private const int InstantNoteDurationMs = 1;

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

		private static readonly Regex KeyUpPattern = new Regex("\\{(Numpad[1-8])\\s+up\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex AltDownPattern = new Regex("LAlt\\s+down", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex AltUpPattern = new Regex("LAlt\\s+up", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex QuickPressPattern = new Regex("SendInput\\s*\\{(Numpad[09])\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex SleepPattern = new Regex("Sleep,\\s*(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static List<string> ParseToCompact(string ahkContent)
		{
			string[] array = ahkContent.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			List<string> result = new List<string>();
			List<PendingNote> heldNotes = new List<PendingNote>();
			List<string> instantNotes = new List<string>();
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
				bool hasAltOnLine = AltDownPattern.IsMatch(trimmed);
				foreach (Match item in KeyDownPattern.Matches(trimmed))
				{
					string numpad3 = item.Groups[1].Value;
					bool isSharp = altHeld || hasAltOnLine;
					string note;
					if (isSharp && NumpadToSharp.TryGetValue(numpad3, out var sharpNote))
					{
						note = sharpNote;
					}
					else
					{
						if (!NumpadToNote.TryGetValue(numpad3, out var naturalNote))
						{
							continue;
						}
						note = naturalNote;
					}
					string noteWithOctave = ApplyOctaveModifier(note, currentOctave);
					heldNotes.Add(new PendingNote
					{
						Note = noteWithOctave,
						NumpadKey = numpad3,
						IsSharp = isSharp
					});
				}
				foreach (Match keyUpMatch in KeyUpPattern.Matches(trimmed))
				{
					string numpad2 = keyUpMatch.Groups[1].Value;
					PendingNote matchingNote = heldNotes.Find((PendingNote n) => n.NumpadKey == numpad2);
					if (matchingNote != null)
					{
						heldNotes.Remove(matchingNote);
						instantNotes.Add(matchingNote.Note);
					}
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
				if (!sleepMatch.Success)
				{
					continue;
				}
				string durationMs = sleepMatch.Groups[1].Value;
				foreach (string instantNote in instantNotes)
				{
					result.Add($"{instantNote}:{1}");
				}
				instantNotes.Clear();
				if (heldNotes.Count > 0)
				{
					string noteLine = string.Join(" ", heldNotes.ConvertAll((PendingNote n) => n.Note + ":" + durationMs));
					result.Add(noteLine);
					heldNotes.Clear();
				}
			}
			foreach (string instantNote2 in instantNotes)
			{
				result.Add($"{instantNote2}:{1}");
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
