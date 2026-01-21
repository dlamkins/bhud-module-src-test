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

		private static readonly Regex NotePattern = new Regex("([A-GR])(\\^|#)?([+-])?:(\\d+)", RegexOptions.Compiled);

		public static List<SongCommand> Parse(List<string> noteLines)
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			List<SongCommand> commands = new List<SongCommand>();
			int currentOctave = 0;
			foreach (string noteLine in noteLines)
			{
				List<ParsedNote> notes = ParseNotesFromLine(noteLine);
				if (notes.Count == 0)
				{
					continue;
				}
				if (notes.Any((ParsedNote n) => n.IsRest))
				{
					commands.Add(SongCommand.WaitCmd(notes.Max((ParsedNote n) => n.DurationMs)));
					continue;
				}
				foreach (ParsedNote note2 in notes)
				{
					if (note2.TargetOctave != currentOctave)
					{
						int steps = note2.TargetOctave - currentOctave;
						Keys octaveKey = (Keys)((steps > 0) ? 105 : 96);
						for (int i = 0; i < Math.Abs(steps); i++)
						{
							commands.Add(SongCommand.KeyDownCmd(octaveKey));
							commands.Add(SongCommand.KeyUpCmd(octaveKey));
							commands.Add(SongCommand.WaitCmd(100));
						}
						currentOctave = note2.TargetOctave;
					}
					if (note2.NeedsAlt)
					{
						commands.Add(SongCommand.KeyDownCmd((Keys)164));
					}
					commands.Add(SongCommand.KeyDownCmd(note2.Key));
				}
				commands.Add(SongCommand.WaitCmd(notes.Max((ParsedNote n) => n.DurationMs)));
				foreach (ParsedNote note in notes.AsEnumerable().Reverse())
				{
					commands.Add(SongCommand.KeyUpCmd(note.Key));
					if (note.NeedsAlt)
					{
						commands.Add(SongCommand.KeyUpCmd((Keys)164));
					}
				}
			}
			return commands;
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
