using System;

namespace LiteDB
{
	internal static class DateExtensions
	{
		public static DateTime Truncate(this DateTime dt)
		{
			if (dt == DateTime.MaxValue || dt == DateTime.MinValue)
			{
				return dt;
			}
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);
		}

		public static int MonthDifference(this DateTime startDate, DateTime endDate)
		{
			int num = endDate.Month + endDate.Year * 12 - (startDate.Month + startDate.Year * 12);
			double daysInEndMonth = (endDate - endDate.AddMonths(1)).Days;
			return Convert.ToInt32(Math.Truncate((double)num + (double)(startDate.Day - endDate.Day) / daysInEndMonth));
		}

		public static int YearDifference(this DateTime startDate, DateTime endDate)
		{
			int years = endDate.Year - startDate.Year;
			if (startDate.Month == endDate.Month && endDate.Day < startDate.Day)
			{
				years--;
			}
			else if (endDate.Month < startDate.Month)
			{
				years--;
			}
			return years;
		}
	}
}
