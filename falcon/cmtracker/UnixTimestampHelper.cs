using System;

namespace falcon.cmtracker
{
	public class UnixTimestampHelper
	{
		public static long getLatestWeeklyServerResetTimestampInSeconds()
		{
			DateTimeOffset nowUtc = DateTimeOffset.UtcNow;
			int daysSinceMonday = (int)(nowUtc.DayOfWeek - 1);
			if (daysSinceMonday < 0)
			{
				daysSinceMonday = 7;
			}
			DateTimeOffset latestMondayUtc = nowUtc.AddDays(-daysSinceMonday);
			TimeSpan utcMinus6Offset = TimeSpan.FromHours(-6.0);
			return new DateTimeOffset(latestMondayUtc.Year, latestMondayUtc.Month, latestMondayUtc.Day, 1, 30, 0, utcMinus6Offset).ToUnixTimeSeconds();
		}
	}
}
