using System;
using Microsoft.Xna.Framework;

namespace RaidClears.Features.Strikes.Services
{
	public class ResetsWatcherService : IDisposable
	{
		public DateTime NextDailyReset { get; private set; }

		public DateTime LastDailyReset { get; private set; }

		public DateTime NextWeeklyReset { get; private set; }

		public DateTime LastWeeklyReset { get; private set; }

		public event EventHandler<DateTime>? DailyReset;

		public event EventHandler<DateTime>? WeeklyReset;

		public ResetsWatcherService()
		{
			CalcNextDailyReset();
			CalcNextWeeklyReset();
		}

		public void CalcNextDailyReset()
		{
			NextDailyReset = DateTime.UtcNow.AddDays(1.0).Date;
			LastDailyReset = NextDailyReset.AddDays(-1.0);
		}

		public void CalcNextWeeklyReset()
		{
			NextWeeklyReset = NextDayOfWeek(DayOfWeek.Monday, 7, 30);
			LastWeeklyReset = NextWeeklyReset.AddDays(-7.0);
		}

		public static DateTime NextDayOfWeek(DayOfWeek weekday, int hour, int minute)
		{
			DateTime today = DateTime.UtcNow;
			if (today.Hour < hour && today.DayOfWeek == weekday)
			{
				return today.Date.AddHours(hour).AddMinutes(minute);
			}
			DateTime nextReset = today.AddDays(1.0);
			while (nextReset.DayOfWeek != weekday)
			{
				nextReset = nextReset.AddDays(1.0);
			}
			return nextReset.Date.AddHours(hour).AddMinutes(minute);
		}

		public void Update(GameTime gametime)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow >= NextDailyReset)
			{
				CalcNextDailyReset();
				this.DailyReset?.Invoke(this, NextDailyReset);
			}
			if (utcNow >= NextWeeklyReset)
			{
				CalcNextWeeklyReset();
				this.WeeklyReset?.Invoke(this, NextWeeklyReset);
			}
		}

		public void Dispose()
		{
		}
	}
}
