using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;

namespace RaidClears.Features.Strikes.Services
{
	public static class PriorityRotationService
	{
		public static IEnumerable<BoxModel> GetPriorityEncounters()
		{
			return from e in GetPriorityStrikes()
				select new BoxModel("priority_" + e.Encounter.id, e.Encounter.name + "\n\n" + Strings.Strike_Tooltip_tomorrow + "\n" + e.TomorrowEncounter.Name, e.Encounter.shortName);
		}

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

		public static IEnumerable<StrikeInfo> GetPriorityStrikes()
		{
			return Service.StrikeData.GetPriorityStrikes(DayOfYearIndex());
		}
	}
}
