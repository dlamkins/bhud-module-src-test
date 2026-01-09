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

		private static readonly Dictionary<string, Keys> NoteToKey = new Dictionary<string, Keys>
		{
			{
				"C",
				(Keys)97
			},
			{
				"D",
				(Keys)98
			},
			{
				"E",
				(Keys)99
			},
			{
				"F",
				(Keys)100
			},
			{
				"G",
				(Keys)101
			},
			{
				"A",
				(Keys)102
			},
			{
				"B",
				(Keys)103
			}
		};

		private static readonly Dictionary<string, Keys> SharpToKey = new Dictionary<string, Keys>
		{
			{
				"C#",
				(Keys)97
			},
			{
				"D#",
				(Keys)98
			},
			{
				"F#",
				(Keys)99
			},
			{
				"G#",
				(Keys)100
			},
			{
				"A#",
				(Keys)101
			}
		};

		private static readonly Regex NotePattern = new Regex("([A-GR])(\\^|#)?([+-])?:(\\d+)", RegexOptions.Compiled);

		public static List<SongCommand> Parse(List<string> noteLines)
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
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
					if (!SharpToKey.TryGetValue(note + "#", out noteKey))
					{
						continue;
					}
					needsAlt = true;
				}
				else if (!NoteToKey.TryGetValue(note, out noteKey))
				{
					continue;
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
