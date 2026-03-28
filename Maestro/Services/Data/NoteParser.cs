using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Maestro.Models;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services.Data
{
	public static class NoteParser
	{
		private class ParsedNote
		{
			public Keys Key { get; set; }

			public int TargetOctave { get; set; }

			public int DurationMs { get; set; }

			public bool NeedsAlt { get; set; }

			public bool IsRest { get; set; }
		}

		public class ParseResult
		{
			public List<SongCommand> Commands { get; set; }

			public int[] CommandToNoteLineIndex { get; set; }
		}

		private static readonly Regex NotePattern = new Regex("([A-GR])(\\^|#)?([+-])?:(\\d+)", RegexOptions.Compiled);

		public static long CalculateDurationMs(List<string> noteLines)
		{
			long total = 0L;
			foreach (string line in noteLines)
			{
				MatchCollection matchCollection = NotePattern.Matches(line);
				int maxDuration = 0;
				foreach (Match item in matchCollection)
				{
					int duration = int.Parse(item.Groups[4].Value);
					if (duration > maxDuration)
					{
						maxDuration = duration;
					}
				}
				total += maxDuration;
			}
			return total;
		}

		public static ParseResult ParseWithMapping(List<string> noteLines)
		{
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			List<SongCommand> commands = new List<SongCommand>();
			List<int> mapping = new List<int>();
			int currentOctave = 0;
			int noteLineIndex = 0;
			foreach (string noteLine in noteLines)
			{
				List<ParsedNote> notes = ParseNotesFromLine(noteLine);
				if (notes.Count == 0)
				{
					noteLineIndex++;
					continue;
				}
				if (notes.Any((ParsedNote n) => n.IsRest))
				{
					commands.Add(SongCommand.WaitCmd(notes.Max((ParsedNote n) => n.DurationMs)));
					mapping.Add(noteLineIndex);
					noteLineIndex++;
					continue;
				}
				foreach (ParsedNote note2 in notes)
				{
					if (note2.TargetOctave != currentOctave)
					{
						int num = note2.TargetOctave - currentOctave;
						int absSteps = Math.Abs(num);
						Keys octaveKey = (Keys)((num > 0) ? 105 : 96);
						int delay = ((absSteps > 1) ? 150 : 50);
						for (int i = 0; i < absSteps; i++)
						{
							commands.Add(SongCommand.KeyDownCmd(octaveKey));
							mapping.Add(noteLineIndex);
							commands.Add(SongCommand.KeyUpCmd(octaveKey));
							mapping.Add(noteLineIndex);
							commands.Add(SongCommand.WaitCmd(delay));
							mapping.Add(noteLineIndex);
						}
						currentOctave = note2.TargetOctave;
					}
					if (note2.NeedsAlt)
					{
						commands.Add(SongCommand.KeyDownCmd((Keys)164));
						mapping.Add(noteLineIndex);
					}
					commands.Add(SongCommand.KeyDownCmd(note2.Key));
					mapping.Add(noteLineIndex);
				}
				commands.Add(SongCommand.WaitCmd(notes.Max((ParsedNote n) => n.DurationMs)));
				mapping.Add(noteLineIndex);
				foreach (ParsedNote note in notes.AsEnumerable().Reverse())
				{
					commands.Add(SongCommand.KeyUpCmd(note.Key));
					mapping.Add(noteLineIndex);
					if (note.NeedsAlt)
					{
						commands.Add(SongCommand.KeyUpCmd((Keys)164));
						mapping.Add(noteLineIndex);
					}
				}
				noteLineIndex++;
			}
			return new ParseResult
			{
				Commands = commands,
				CommandToNoteLineIndex = mapping.ToArray()
			};
		}

		public static List<SongCommand> Parse(List<string> noteLines)
		{
			return ParseWithMapping(noteLines).Commands;
		}

		public static SeekData ComputeSeekData(List<SongCommand> commands)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Invalid comparison between Unknown and I4
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Invalid comparison between Unknown and I4
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Invalid comparison between Unknown and I4
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Invalid comparison between Unknown and I4
			int count = commands.Count;
			long[] cumulativeTimeMs = new long[count];
			int[] octaveAtCommand = new int[count];
			long elapsed = 0L;
			int octave = 0;
			bool lastKeyUpWasOctave = false;
			for (int i = 0; i < count; i++)
			{
				SongCommand cmd = commands[i];
				if (cmd.Type == CommandType.KeyDown)
				{
					if ((int)cmd.Key == 105)
					{
						octave = Math.Min(octave + 1, 1);
					}
					else if ((int)cmd.Key == 96)
					{
						octave = Math.Max(octave - 1, -1);
					}
				}
				if (cmd.Type == CommandType.KeyUp)
				{
					lastKeyUpWasOctave = (int)cmd.Key == 105 || (int)cmd.Key == 96;
				}
				cumulativeTimeMs[i] = elapsed;
				octaveAtCommand[i] = octave;
				if (cmd.Type == CommandType.Wait)
				{
					if (!lastKeyUpWasOctave)
					{
						elapsed += cmd.Duration;
					}
					lastKeyUpWasOctave = false;
				}
			}
			return new SeekData
			{
				CumulativeTimeMs = cumulativeTimeMs,
				OctaveAtCommand = octaveAtCommand,
				TotalDurationMs = elapsed
			};
		}

		private static List<ParsedNote> ParseNotesFromLine(string line)
		{
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			List<ParsedNote> notes = new List<ParsedNote>();
			foreach (Match match in NotePattern.Matches(line))
			{
				string note = match.Groups[1].Value;
				string obj = (match.Groups[2].Success ? match.Groups[2].Value : null);
				bool isSharp = obj == "#";
				bool isHighC = obj == "^" && note == "C";
				string octaveModifier = (match.Groups[3].Success ? match.Groups[3].Value : null);
				int durationMs = int.Parse(match.Groups[4].Value);
				int targetOctave = ((octaveModifier == "+") ? 1 : ((octaveModifier == "-") ? (-1) : 0));
				if (note == "R")
				{
					notes.Add(new ParsedNote
					{
						IsRest = true,
						TargetOctave = targetOctave,
						DurationMs = durationMs
					});
					continue;
				}
				bool needsAlt = false;
				Keys noteKey;
				if (isHighC)
				{
					noteKey = (Keys)104;
				}
				else if (isSharp)
				{
					if (!NoteMapping.TryParse(note, out var sharpNoteName))
					{
						continue;
					}
					Keys? sharpKey = NoteMapping.GetSharpKey(sharpNoteName);
					if (!sharpKey.HasValue)
					{
						continue;
					}
					noteKey = sharpKey.Value;
					needsAlt = true;
				}
				else
				{
					if (!NoteMapping.TryParse(note, out var naturalNoteName))
					{
						continue;
					}
					Keys? naturalKey = NoteMapping.GetNaturalKey(naturalNoteName);
					if (!naturalKey.HasValue)
					{
						continue;
					}
					noteKey = naturalKey.Value;
				}
				notes.Add(new ParsedNote
				{
					Key = noteKey,
					TargetOctave = targetOctave,
					DurationMs = durationMs,
					NeedsAlt = needsAlt
				});
			}
			return notes;
		}
	}
}
