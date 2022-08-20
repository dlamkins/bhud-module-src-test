using System;

namespace Ideka.RacingMeter
{
	internal static class TimeSpanExtensions
	{
		public static TimeSpan Multiply(this TimeSpan a, double b)
		{
			return TimeSpan.FromTicks((long)Math.Round((double)a.Ticks * b));
		}
	}
}
