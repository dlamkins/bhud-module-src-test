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

			public string Key { get; set; }
		}

		private const int InstantNoteDurationMs = 1;

		private const int PianoTomasMaxPressDurationMs = 150;

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

		private static readonly Dictionary<string, string> FKeyToSharp = new Dictionary<string, string>
		{
			{ "F1", "C#" },
			{ "F2", "D#" },
			{ "F3", "F#" },
			{ "F4", "G#" },
			{ "F5", "A#" }
		};

		private static readonly Regex KeyDownPattern = new Regex("\\{(Numpad[1-8]|F[1-5])\\s+down\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex KeyUpPattern = new Regex("\\{(Numpad[1-8]|F[1-5])\\s+up\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex QuickPressPattern = new Regex("SendInput\\s*\\{(Numpad[09])\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex SleepPattern = new Regex("Sleep,\\s*(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex SendInputKeyDownPattern = new Regex("SendInput.*down", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex SendInputKeyUpPattern = new Regex("^SendInput.*up\\}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static string[] NormalizePianoTomas(string[] lines)
		{
			List<string> output = new List<string>();
			int i = 0;
			while (i < lines.Length)
			{
				string trimmed = lines[i].Trim();
				if (trimmed.StartsWith("PlaySong", StringComparison.OrdinalIgnoreCase) || trimmed == "{" || trimmed == "}" || trimmed == string.Empty || (trimmed.Length >= 2 && trimmed[1] == ':' && trimmed[0] != 'S') || trimmed.StartsWith("'::"))
				{
					output.Add(lines[i]);
					i++;
				}
				else if (SendInputKeyDownPattern.IsMatch(trimmed))
				{
					List<string> keyDownLines = new List<string>();
					for (; i < lines.Length && SendInputKeyDownPattern.IsMatch(lines[i].Trim()); i++)
					{
						keyDownLines.Add(lines[i]);
					}
					if (i < lines.Length)
					{
						Match sleepMatch = SleepPattern.Match(lines[i].Trim());
						if (sleepMatch.Success && int.Parse(sleepMatch.Groups[1].Value) <= 150)
						{
							int pressDuration = int.Parse(sleepMatch.Groups[1].Value);
							i++;
							List<string> keyUpLines = new List<string>();
							for (; i < lines.Length && SendInputKeyUpPattern.IsMatch(lines[i].Trim()); i++)
							{
								keyUpLines.Add(lines[i]);
							}
							if (i < lines.Length)
							{
								Match gapMatch = SleepPattern.Match(lines[i].Trim());
								if (gapMatch.Success)
								{
									int totalDuration = pressDuration + int.Parse(gapMatch.Groups[1].Value);
									i++;
									while (i < lines.Length)
									{
										string nextTrimmed = lines[i].Trim();
										if (SendInputKeyUpPattern.IsMatch(nextTrimmed))
										{
											i++;
											continue;
										}
										Match extraSleep = SleepPattern.Match(nextTrimmed);
										if (!extraSleep.Success)
										{
											break;
										}
										totalDuration += int.Parse(extraSleep.Groups[1].Value);
										i++;
									}
									output.AddRange(keyDownLines);
									output.Add($"Sleep, {totalDuration}");
									output.AddRange(keyUpLines);
									continue;
								}
							}
							output.AddRange(keyDownLines);
							output.Add($"Sleep, {pressDuration}");
							output.AddRange(keyUpLines);
							continue;
						}
					}
					output.AddRange(keyDownLines);
				}
				else
				{
					output.Add(lines[i]);
					i++;
				}
			}
			return output.ToArray();
		}

		public static List<string> ParseToCompact(string ahkContent)
		{
			string[] array = NormalizePianoTomas(ahkContent.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
			List<string> result = new List<string>();
			List<PendingNote> heldNotes = new List<PendingNote>();
			List<string> instantNotes = new List<string>();
			int currentOctave = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string trimmed = array2[i].Trim();
				if (trimmed.StartsWith("PlaySong", StringComparison.OrdinalIgnoreCase) || trimmed == "{" || trimmed == "}")
				{
					continue;
				}
				foreach (Match item in KeyDownPattern.Matches(trimmed))
				{
					string key2 = item.Groups[1].Value;
					string note;
					if (FKeyToSharp.TryGetValue(key2, out var sharpNote))
					{
						note = sharpNote;
					}
					else
					{
						if (!NumpadToNote.TryGetValue(key2, out var naturalNote))
						{
							continue;
						}
						note = naturalNote;
					}
					string noteWithOctave = ApplyOctaveModifier(note, currentOctave);
					heldNotes.Add(new PendingNote
					{
						Note = noteWithOctave,
						Key = key2
					});
				}
				foreach (Match keyUpMatch in KeyUpPattern.Matches(trimmed))
				{
					string key = keyUpMatch.Groups[1].Value;
					PendingNote matchingNote = heldNotes.Find((PendingNote n) => n.Key == key);
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
						currentOctave = Math.Min(currentOctave + 1, 1);
					}
					else if (numpad == "Numpad0")
					{
						currentOctave = Math.Max(currentOctave - 1, -1);
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
