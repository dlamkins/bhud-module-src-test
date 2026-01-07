using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Maestro.Models;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services.Data
{
	public static class NoteParser
	{
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

		private static readonly Regex NotePattern = new Regex("([A-GR])(\\^|#)?([+-])?:(\\d+)(\\.)?", RegexOptions.Compiled);

		public static List<SongCommand> Parse(List<string> noteLines, int bpm)
		{
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			List<SongCommand> commands = new List<SongCommand>();
			int currentOctave = 0;
			foreach (string line in noteLines)
			{
				foreach (Match match in NotePattern.Matches(line))
				{
					string note = match.Groups[1].Value;
					string obj = (match.Groups[2].Success ? match.Groups[2].Value : null);
					bool isSharp = obj == "#";
					bool isHighC = obj == "^" && note == "C";
					string octaveModifier = (match.Groups[3].Success ? match.Groups[3].Value : null);
					int durationValue = int.Parse(match.Groups[4].Value);
					bool isDotted = match.Groups[5].Success;
					int durationMs = CalculateDurationMs(bpm, durationValue, isDotted);
					if (note == "R")
					{
						commands.Add(SongCommand.WaitCmd(durationMs));
						continue;
					}
					int targetOctave = 0;
					if (octaveModifier == "+")
					{
						targetOctave = 1;
					}
					else if (octaveModifier == "-")
					{
						targetOctave = -1;
					}
					if (targetOctave != currentOctave)
					{
						int num = targetOctave - currentOctave;
						Keys octaveKey = (Keys)((num > 0) ? 105 : 96);
						int pressCount = Math.Abs(num);
						for (int i = 0; i < pressCount; i++)
						{
							commands.Add(SongCommand.KeyDownCmd(octaveKey));
							commands.Add(SongCommand.KeyUpCmd(octaveKey));
						}
						currentOctave = targetOctave;
					}
					bool needsAlt = false;
					Keys noteKey;
					if (isHighC)
					{
						noteKey = (Keys)104;
					}
					else if (isSharp)
					{
						string sharpNote = note + "#";
						if (!SharpToKey.TryGetValue(sharpNote, out noteKey))
						{
							continue;
						}
						needsAlt = true;
					}
					else if (!NoteToKey.TryGetValue(note, out noteKey))
					{
						continue;
					}
					if (needsAlt)
					{
						commands.Add(SongCommand.KeyDownCmd((Keys)164));
					}
					commands.Add(SongCommand.KeyDownCmd(noteKey));
					commands.Add(SongCommand.KeyUpCmd(noteKey));
					if (needsAlt)
					{
						commands.Add(SongCommand.KeyUpCmd((Keys)164));
					}
					commands.Add(SongCommand.WaitCmd(durationMs));
				}
			}
			return commands;
		}

		private static int CalculateDurationMs(int bpm, int noteValue, bool isDotted)
		{
			double ms = 60000.0 / (double)bpm * (4.0 / (double)noteValue);
			if (isDotted)
			{
				ms *= 1.5;
			}
			return (int)Math.Round(ms);
		}
	}
}
