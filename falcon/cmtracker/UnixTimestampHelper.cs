using System;

namespace falcon.cmtracker
{
	public class UnixTimestampHelper
	{
		public static long getLatestWeeklyServerResetTimestampInSeconds()
		{
			TimeSpan utcMinus6Offset = TimeSpan.FromHours(-6.0);
			DateTimeOffset nowInUtcMinus6 = DateTime.UtcNow.Add(utcMinus6Offset);
			int daysSinceMonday = (int)(nowInUtcMinus6.DayOfWeek - 1);
			if (daysSinceMonday < 0)
			{
				daysSinceMonday = 7;
			}
			DateTimeOffset latestMonday = nowInUtcMinus6.AddDays(-daysSinceMonday);
			return new DateTimeOffset(latestMonday.Year, latestMonday.Month, latestMonday.Day, 1, 30, 0, utcMinus6Offset).ToUnixTimeSeconds();
		}
	}
}
