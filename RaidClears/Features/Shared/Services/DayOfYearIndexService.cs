using System;

namespace RaidClears.Features.Shared.Services
{
	public static class DayOfYearIndexService
	{
		public static int DayOfYearIndex()
		{
			return DayOfYearIndex(DateTime.UtcNow);
		}

		public static int DayOfYearIndex(DateTime date)
		{
			int day = date.DayOfYear - 1;
			if (DateTime.IsLeapYear(date.Year))
			{
				return day;
			}
			if (date.Month >= 3)
			{
				return day + 1;
			}
			return day;
		}
	}
}
