using System;
using System.Collections.Generic;
using Gw2Sharp.Models;

namespace Blish_HUD.Extended
{
	public static class TyrianTimeUtil
	{
		private static IReadOnlyDictionary<TyrianTime, (TimeSpan, TimeSpan)> _dayCycleIntervals = new Dictionary<TyrianTime, (TimeSpan, TimeSpan)>
		{
			{
				TyrianTime.Dawn,
				(new TimeSpan(5, 0, 0), new TimeSpan(6, 0, 0))
			},
			{
				TyrianTime.Day,
				(new TimeSpan(6, 0, 0), new TimeSpan(20, 0, 0))
			},
			{
				TyrianTime.Dusk,
				(new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0))
			},
			{
				TyrianTime.Night,
				(new TimeSpan(21, 0, 0), new TimeSpan(5, 0, 0))
			}
		};

		private static IReadOnlyDictionary<TyrianTime, (TimeSpan, TimeSpan)> _canthanDayCycleIntervals = new Dictionary<TyrianTime, (TimeSpan, TimeSpan)>
		{
			{
				TyrianTime.Dawn,
				(new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0))
			},
			{
				TyrianTime.Day,
				(new TimeSpan(8, 0, 0), new TimeSpan(19, 0, 0))
			},
			{
				TyrianTime.Dusk,
				(new TimeSpan(19, 0, 0), new TimeSpan(20, 0, 0))
			},
			{
				TyrianTime.Night,
				(new TimeSpan(20, 0, 0), new TimeSpan(7, 0, 0))
			}
		};

		public static TyrianTime GetCurrentDayCycle()
		{
			return GetDayCycle(GetCurrentTyrianTime());
		}

		public static TimeSpan GetCurrentTyrianTime()
		{
			return FromRealDateTime(DateTime.UtcNow);
		}

		public static TyrianTime GetDayCycle(TimeSpan tyrianTime)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.Gw2Mumble.get_IsAvailable())
			{
				Coordinates2 mapPosition = GameService.Gw2Mumble.get_UI().get_MapPosition();
				double x = ((Coordinates2)(ref mapPosition)).get_X();
				mapPosition = GameService.Gw2Mumble.get_UI().get_MapPosition();
				double y = ((Coordinates2)(ref mapPosition)).get_Y();
				if (x > 20000.0 && x < 365000.0 && y > 97000.0 && y < 115000.0)
				{
					return GetDayCycleFromRegion(_canthanDayCycleIntervals, tyrianTime);
				}
			}
			return GetDayCycleFromRegion(_dayCycleIntervals, tyrianTime);
		}

		private static TyrianTime GetDayCycleFromRegion(IReadOnlyDictionary<TyrianTime, (TimeSpan, TimeSpan)> _dayCycles, TimeSpan tyrianTime)
		{
			foreach (KeyValuePair<TyrianTime, (TimeSpan, TimeSpan)> timePair in _dayCycleIntervals)
			{
				TyrianTime key = timePair.Key;
				(TimeSpan, TimeSpan) value = timePair.Value;
				if (TimeBetween(tyrianTime, value.Item1, value.Item2))
				{
					return key;
				}
			}
			return TyrianTime.None;
		}

		public static TimeSpan FromRealDateTime(DateTime realTime)
		{
			return TimeSpan.FromSeconds((realTime - realTime.Date).TotalSeconds % 7200.0 * 12.0);
		}

		public static bool TimeBetween(TimeSpan time, TimeSpan start, TimeSpan end)
		{
			if (start < end)
			{
				if (start <= time)
				{
					return time <= end;
				}
				return false;
			}
			if (end < time)
			{
				return !(time < start);
			}
			return true;
		}
	}
}
