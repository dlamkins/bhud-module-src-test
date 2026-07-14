using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Maestro.Services.Practice
{
	public class NoteTimeline
	{
		private static readonly Regex NotePattern = new Regex("([A-GR])(\\^|#)?([+-])?:(\\d+)", RegexOptions.Compiled);

		public IReadOnlyList<TimelineNote> Notes { get; }

		public IReadOnlyList<OctaveShiftPoint> OctaveShiftPoints { get; }

		public int TotalDurationMs { get; }

		private NoteTimeline(List<TimelineNote> notes, List<OctaveShiftPoint> shifts, int total)
		{
			Notes = notes;
			OctaveShiftPoints = shifts;
			TotalDurationMs = total;
		}

		public static NoteTimeline Build(IReadOnlyList<string> noteLines)
		{
			List<TimelineNote> notes = new List<TimelineNote>();
			List<OctaveShiftPoint> shifts = new List<OctaveShiftPoint>();
			int currentOctave = 0;
			int currentMs = 0;
			foreach (string line in noteLines)
			{
				MatchCollection matches = NotePattern.Matches(line);
				if (matches.Count == 0)
				{
					continue;
				}
				int maxDuration = 0;
				foreach (Match i in matches)
				{
					string pitch = i.Groups[1].Value;
					string modifier = (i.Groups[2].Success ? i.Groups[2].Value : null);
					string octaveMarker = (i.Groups[3].Success ? i.Groups[3].Value : null);
					int duration = int.Parse(i.Groups[4].Value);
					if (duration > maxDuration)
					{
						maxDuration = duration;
					}
					if (pitch == "R")
					{
						continue;
					}
					int targetOctave = ((octaveMarker == "+") ? 1 : ((octaveMarker == "-") ? (-1) : 0));
					if (targetOctave != currentOctave)
					{
						int delta = ((targetOctave > currentOctave) ? 1 : (-1));
						int steps = Math.Abs(targetOctave - currentOctave);
						for (int s = 0; s < steps; s++)
						{
							shifts.Add(new OctaveShiftPoint
							{
								AtMs = currentMs,
								Delta = delta
							});
						}
						currentOctave = targetOctave;
					}
					bool isSharp = modifier == "#";
					int lane = ((modifier == "^" && pitch == "C") ? 8 : PitchToLane(pitch[0]));
					if (lane != 0 && (!isSharp || SharpExistsOnLane(lane)))
					{
						notes.Add(new TimelineNote
						{
							StartMs = currentMs,
							DurationMs = duration,
							OctaveAtPlay = currentOctave,
							IsSharp = isSharp,
							Lane = lane
						});
					}
				}
				currentMs += maxDuration;
			}
			return new NoteTimeline(notes, shifts, currentMs);
		}

		public static char LaneLetter(int lane)
		{
			switch (lane)
			{
			case 1:
			case 8:
				return 'C';
			case 2:
				return 'D';
			case 3:
				return 'E';
			case 4:
				return 'F';
			case 5:
				return 'G';
			case 6:
				return 'A';
			case 7:
				return 'B';
			default:
				return '?';
			}
		}

		private static int PitchToLane(char pitch)
		{
			return pitch switch
			{
				'C' => 1, 
				'D' => 2, 
				'E' => 3, 
				'F' => 4, 
				'G' => 5, 
				'A' => 6, 
				'B' => 7, 
				_ => 0, 
			};
		}

		private static bool SharpExistsOnLane(int lane)
		{
			if (lane != 1 && lane != 2 && lane != 4 && lane != 5)
			{
				return lane == 6;
			}
			return true;
		}

		public List<int> GetNoteIndicesInWindow(int fromMs, int toMs)
		{
			List<int> result = new List<int>();
			for (int i = LowerBound(fromMs); i < Notes.Count && Notes[i].StartMs <= toMs; i++)
			{
				result.Add(i);
			}
			return result;
		}

		private int LowerBound(int fromMs)
		{
			int lo = 0;
			int hi = Notes.Count;
			while (lo < hi)
			{
				int mid = lo + hi >> 1;
				if (Notes[mid].StartMs < fromMs)
				{
					lo = mid + 1;
				}
				else
				{
					hi = mid;
				}
			}
			return lo;
		}
	}
}
