using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Maestro.Models;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services.Data
{
	public static class DrumParser
	{
		private class ParsedHit
		{
			public DrumSoundInfo Info { get; set; }

			public int DurationMs { get; set; }

			public bool IsRest { get; set; }
		}

		private static readonly Regex DrumPattern = new Regex("(ht|mt|ft|cr|rd|hc|ho|hf|b|s|x|g|R)\\s*:\\s*(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static long CalculateDurationMs(List<string> noteLines)
		{
			long total = 0L;
			foreach (string line in noteLines)
			{
				int maxDuration = 0;
				foreach (Match item in DrumPattern.Matches(line))
				{
					int d = int.Parse(item.Groups[2].Value);
					if (d > maxDuration)
					{
						maxDuration = d;
					}
				}
				total += maxDuration;
			}
			return total;
		}

		public static NoteParser.ParseResult ParseWithMapping(List<string> noteLines)
		{
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			List<SongCommand> commands = new List<SongCommand>();
			List<int> mapping = new List<int>();
			Dictionary<DrumSound, bool> useSecondary = new Dictionary<DrumSound, bool>();
			int noteLineIndex = 0;
			foreach (string noteLine in noteLines)
			{
				List<ParsedHit> hits = ParseHitsFromLine(noteLine);
				if (hits.Count == 0)
				{
					noteLineIndex++;
					continue;
				}
				if (hits.Any((ParsedHit h) => h.IsRest))
				{
					commands.Add(SongCommand.WaitCmd(hits.Max((ParsedHit h) => h.DurationMs)));
					mapping.Add(noteLineIndex);
					noteLineIndex++;
					continue;
				}
				foreach (ParsedHit item in hits)
				{
					DrumSoundInfo info = item.Info;
					Keys key = info.PrimaryKey;
					if (info.HasPair)
					{
						bool s;
						bool second = useSecondary.TryGetValue(info.Sound, out s) && s;
						key = (second ? info.SecondaryKey : info.PrimaryKey);
						useSecondary[info.Sound] = !second;
					}
					if (info.NeedsAlt)
					{
						commands.Add(SongCommand.KeyDownCmd((Keys)164));
						mapping.Add(noteLineIndex);
					}
					commands.Add(SongCommand.KeyDownCmd(key));
					mapping.Add(noteLineIndex);
					commands.Add(SongCommand.KeyUpCmd(key));
					mapping.Add(noteLineIndex);
					if (info.NeedsAlt)
					{
						commands.Add(SongCommand.KeyUpCmd((Keys)164));
						mapping.Add(noteLineIndex);
					}
				}
				commands.Add(SongCommand.WaitCmd(hits.Max((ParsedHit h) => h.DurationMs)));
				mapping.Add(noteLineIndex);
				noteLineIndex++;
			}
			return new NoteParser.ParseResult
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
			int count = commands.Count;
			long[] cumulativeTimeMs = new long[count];
			int[] octaveAtCommand = new int[count];
			long elapsed = 0L;
			for (int i = 0; i < count; i++)
			{
				cumulativeTimeMs[i] = elapsed;
				octaveAtCommand[i] = 0;
				if (commands[i].Type == CommandType.Wait)
				{
					elapsed += commands[i].Duration;
				}
			}
			return new SeekData
			{
				CumulativeTimeMs = cumulativeTimeMs,
				OctaveAtCommand = octaveAtCommand,
				TotalDurationMs = elapsed
			};
		}

		private static List<ParsedHit> ParseHitsFromLine(string line)
		{
			List<ParsedHit> hits = new List<ParsedHit>();
			foreach (Match item in DrumPattern.Matches(line))
			{
				string code = item.Groups[1].Value;
				int duration = int.Parse(item.Groups[2].Value);
				DrumSoundInfo info;
				if (string.Equals(code, "R", StringComparison.OrdinalIgnoreCase))
				{
					hits.Add(new ParsedHit
					{
						IsRest = true,
						DurationMs = duration
					});
				}
				else if (DrumMapping.TryFromCode(code.ToLowerInvariant(), out info))
				{
					hits.Add(new ParsedHit
					{
						Info = info,
						DurationMs = duration
					});
				}
			}
			return hits;
		}
	}
}
