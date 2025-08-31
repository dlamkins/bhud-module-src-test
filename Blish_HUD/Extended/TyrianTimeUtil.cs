using System;
using System.Collections.Generic;

namespace Blish_HUD.Extended
{
	public static class TyrianTimeUtil
	{
		private static IReadOnlyDictionary<TyrianTime, (TimeSpan, TimeSpan)> _dayCycleIntervals = new Dictionary<TyrianTime, (TimeSpan, TimeSpan)>
		{
			{
				TyrianTime.DAWN,
				(new TimeSpan(5, 0, 0), new TimeSpan(6, 0, 0))
			},
			{
				TyrianTime.DAY,
				(new TimeSpan(6, 0, 0), new TimeSpan(20, 0, 0))
			},
			{
				TyrianTime.DUSK,
				(new TimeSpan(20, 0, 0), new TimeSpan(21, 0, 0))
			},
			{
				TyrianTime.NIGHT,
				(new TimeSpan(21, 0, 0), new TimeSpan(5, 0, 0))
			}
		};

		private static IReadOnlyDictionary<TyrianTime, (TimeSpan, TimeSpan)> _canthanDayCycleIntervals = new Dictionary<TyrianTime, (TimeSpan, TimeSpan)>
		{
			{
				TyrianTime.DAWN,
				(new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0))
			},
			{
				TyrianTime.DAY,
				(new TimeSpan(8, 0, 0), new TimeSpan(19, 0, 0))
			},
			{
				TyrianTime.DUSK,
				(new TimeSpan(19, 0, 0), new TimeSpan(20, 0, 0))
			},
			{
				TyrianTime.NIGHT,
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
			if (GameService.Gw2Mumble.IsAvailable)
			{
				double x = GameService.Gw2Mumble.UI.MapPosition.X;
				double y = GameService.Gw2Mumble.UI.MapPosition.Y;
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
			return TyrianTime.NONE;
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
