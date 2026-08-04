using System;
using System.Collections.Generic;
using Oberyn.AnglerAssociate.Models;

namespace Oberyn.AnglerAssociate.Services
{
	public static class TyrianClock
	{
		private static readonly TimeSpan ReferenceUtcTimeOfDay = new TimeSpan(16, 30, 0);

		private const int TyrianSecondsAtReference = 21600;

		private const int CycleLengthRealSeconds = 7200;

		private const int TyrianSecondsPerRealSecond = 12;

		private const int SecondsPerDay = 86400;

		private static readonly (int Start, TimeOfDay State)[] TyriaSegments = new(int, TimeOfDay)[4]
		{
			(18000, TimeOfDay.Dawn),
			(21600, TimeOfDay.Day),
			(72000, TimeOfDay.Dusk),
			(75600, TimeOfDay.Night)
		};

		private static readonly (int Start, TimeOfDay State)[] CanthaCastoraSegments = new(int, TimeOfDay)[4]
		{
			(25200, TimeOfDay.Dawn),
			(28800, TimeOfDay.Day),
			(68400, TimeOfDay.Dusk),
			(72000, TimeOfDay.Night)
		};

		public static int GetCurrentTyrianSecondOfDay(DateTime utcNow)
		{
			return ((int)(((utcNow.TimeOfDay - ReferenceUtcTimeOfDay).TotalSeconds % 7200.0 + 7200.0) % 7200.0 * 12.0) + 21600) % 86400;
		}

		public static (TimeOfDay State, TimeSpan TimeRemaining) GetState(Cycle cycle, DateTime? utcNow = null)
		{
			if (cycle == Cycle.Global)
			{
				throw new ArgumentException("Global fish aren't tied to a day/night cycle.", "cycle");
			}
			int tyrianSecondOfDay = GetCurrentTyrianSecondOfDay(utcNow ?? DateTime.UtcNow);
			(int, TimeOfDay)[] segments = SegmentsFor(cycle);
			int index = FindSegmentIndex(tyrianSecondOfDay, segments);
			int elapsed = tyrianSecondOfDay - EffectiveStart(segments, index, tyrianSecondOfDay);
			int remaining = Duration(segments, index) - elapsed;
			return (segments[index].Item2, ToRealTimeSpan(remaining));
		}

		public static List<(TimeOfDay State, TimeSpan TimeUntilStart)> GetUpcomingStates(Cycle cycle, int count, DateTime? utcNow = null)
		{
			if (cycle == Cycle.Global)
			{
				throw new ArgumentException("Global fish aren't tied to a day/night cycle.", "cycle");
			}
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException("count", "Must request at least the current state.");
			}
			int tyrianSecondOfDay = GetCurrentTyrianSecondOfDay(utcNow ?? DateTime.UtcNow);
			(int, TimeOfDay)[] segments = SegmentsFor(cycle);
			int index = FindSegmentIndex(tyrianSecondOfDay, segments);
			int elapsed = tyrianSecondOfDay - EffectiveStart(segments, index, tyrianSecondOfDay);
			List<(TimeOfDay, TimeSpan)> results = new List<(TimeOfDay, TimeSpan)>(count);
			int cumulativeTyrianSeconds = Duration(segments, index) - elapsed;
			results.Add((segments[index].Item2, ToRealTimeSpan(cumulativeTyrianSeconds)));
			for (int step = 1; step < count; step++)
			{
				index = (index + 1) % segments.Length;
				results.Add((segments[index].Item2, ToRealTimeSpan(cumulativeTyrianSeconds)));
				cumulativeTyrianSeconds += Duration(segments, index);
			}
			return results;
		}

		private static (int Start, TimeOfDay State)[] SegmentsFor(Cycle cycle)
		{
			if (cycle != Cycle.CanthaCastora)
			{
				return TyriaSegments;
			}
			return CanthaCastoraSegments;
		}

		private static int FindSegmentIndex(int secondsOfDay, (int Start, TimeOfDay State)[] segments)
		{
			for (int i = segments.Length - 1; i >= 0; i--)
			{
				if (secondsOfDay >= segments[i].Start)
				{
					return i;
				}
			}
			return segments.Length - 1;
		}

		private static int EffectiveStart((int Start, TimeOfDay State)[] segments, int index, int secondsOfDay)
		{
			int start = segments[index].Start;
			if (start <= secondsOfDay)
			{
				return start;
			}
			return start - 86400;
		}

		private static int Duration((int Start, TimeOfDay State)[] segments, int index)
		{
			int nextIndex = (index + 1) % segments.Length;
			int nextStart = segments[nextIndex].Start;
			if (nextIndex == 0)
			{
				nextStart += 86400;
			}
			return nextStart - segments[index].Start;
		}

		private static TimeSpan ToRealTimeSpan(int tyrianSeconds)
		{
			return TimeSpan.FromSeconds((double)tyrianSeconds / 12.0);
		}
	}
}
