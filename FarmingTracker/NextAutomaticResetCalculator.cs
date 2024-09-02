using System;

namespace FarmingTracker
{
	public class NextAutomaticResetCalculator
	{
		public static readonly DateTime UNDEFINED_RESET_DATE_TIME = new DateTime(0L, DateTimeKind.Utc);

		private static readonly DateTime NEVER_OR_ON_MODULE_START_RESET_DATE_TIME = new DateTime(3155378975999999999L, DateTimeKind.Utc);

		public static DateTime GetNextResetDateTimeUtc(DateTime dateTimeUtc, AutomaticReset automaticReset, int minutesUntilResetAfterModuleShutdown)
		{
			switch (automaticReset)
			{
			case AutomaticReset.Never:
			case AutomaticReset.OnModuleStart:
				return NEVER_OR_ON_MODULE_START_RESET_DATE_TIME;
			case AutomaticReset.MinutesAfterModuleShutdown:
				return dateTimeUtc + TimeSpan.FromMinutes(minutesUntilResetAfterModuleShutdown);
			case AutomaticReset.OnDailyReset:
				return GetNextDailyResetDateTimeUtc(dateTimeUtc);
			case AutomaticReset.OnWeeklyReset:
				return GetNextWeeklyResetDateTimeUtc(dateTimeUtc, DayOfWeek.Monday, 7.0, 30.0);
			case AutomaticReset.OnWeeklyNaWvwReset:
				return GetNextWeeklyResetDateTimeUtc(dateTimeUtc, DayOfWeek.Saturday, 2.0);
			case AutomaticReset.OnWeeklyEuWvwReset:
				return GetNextWeeklyResetDateTimeUtc(dateTimeUtc, DayOfWeek.Friday, 18.0);
			case AutomaticReset.OnWeeklyMapBonusRewardsReset:
				return GetNextWeeklyResetDateTimeUtc(dateTimeUtc, DayOfWeek.Thursday, 20.0);
			default:
				Module.Logger.Error(Helper.CreateSwitchCaseNotFoundMessage(automaticReset, "AutomaticReset", "never reset"));
				return NEVER_OR_ON_MODULE_START_RESET_DATE_TIME;
			}
		}

		private static DateTime GetNextWeeklyResetDateTimeUtc(DateTime dateTimeUtc, DayOfWeek resetDayOfWeekUtc, double resetHoursUtc, double resetMinutesUtc = 0.0)
		{
			int daysUntilWeeklyReset = (resetDayOfWeekUtc - dateTimeUtc.DayOfWeek + 7) % 7;
			DateTime weeklyResetDateTimeUtc = dateTimeUtc.Date.AddDays(daysUntilWeeklyReset).AddHours(resetHoursUtc).AddMinutes(resetMinutesUtc);
			if (!(dateTimeUtc < weeklyResetDateTimeUtc))
			{
				return weeklyResetDateTimeUtc.AddDays(7.0);
			}
			return weeklyResetDateTimeUtc;
		}

		private static DateTime GetNextDailyResetDateTimeUtc(DateTime dateTimeUtc)
		{
			return dateTimeUtc.Date.AddDays(1.0);
		}
	}
}
