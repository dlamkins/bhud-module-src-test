using System;

namespace Nekres.Stream_Out
{
	public static class DateTimeExtensions
	{
		public static bool IsBetween(this DateTime time, DateTime start, DateTime end)
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
