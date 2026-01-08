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

		private static readonly Regex KeyDownPattern = new Regex("SendInput\\s*\\{(Numpad\\d)\\s+down\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex QuickPressPattern = new Regex("SendInput\\s*\\{(Numpad[09])\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex SleepPattern = new Regex("Sleep,\\s*(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static List<string> ParseToCompact(string ahkContent, int bpm)
		{
			string[] array = ahkContent.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			List<string> result = new List<string>();
			List<string> currentNotes = new List<string>();
			int currentOctave = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string trimmed = array2[i].Trim();
				Match keyDownMatch = KeyDownPattern.Match(trimmed);
				if (keyDownMatch.Success)
				{
					string numpad2 = keyDownMatch.Groups[1].Value;
					if (NumpadToNote.TryGetValue(numpad2, out var note))
					{
						string noteWithOctave = ApplyOctaveModifier(note, currentOctave);
						currentNotes.Add(noteWithOctave);
					}
					continue;
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
					continue;
				}
				Match sleepMatch = SleepPattern.Match(trimmed);
				if (sleepMatch.Success && currentNotes.Count > 0)
				{
					(int duration, bool isDotted) closestNoteDuration = GetClosestNoteDuration(int.Parse(sleepMatch.Groups[1].Value), bpm);
					int duration = closestNoteDuration.duration;
					bool isDotted = closestNoteDuration.isDotted;
					string durationStr = duration + (isDotted ? "." : "");
					string noteLine = string.Join(" ", currentNotes.ConvertAll((string n) => n + ":" + durationStr));
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
			if (note == "C^")
			{
				return "C^" + modifier;
			}
			return note + modifier;
		}

		private static (int duration, bool isDotted) GetClosestNoteDuration(int delayMs, int bpm)
		{
			int[] obj = new int[6] { 1, 2, 4, 8, 16, 32 };
			int closest = 4;
			bool isDotted = false;
			int minDiff = int.MaxValue;
			int[] array = obj;
			foreach (int dur in array)
			{
				int expectedMs = (int)(60000.0 / (double)bpm * (4.0 / (double)dur));
				int diff = Math.Abs(delayMs - expectedMs);
				if (diff < minDiff)
				{
					minDiff = diff;
					closest = dur;
					isDotted = false;
				}
				int dottedMs = (int)((double)expectedMs * 1.5);
				diff = Math.Abs(delayMs - dottedMs);
				if (diff < minDiff)
				{
					minDiff = diff;
					closest = dur;
					isDotted = true;
				}
			}
			return (closest, isDotted);
		}
	}
}
