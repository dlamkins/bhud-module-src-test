using System;
using System.Collections.Generic;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;

namespace RaidClears.Features.Shared.Services
{
	public class WeeklyBountyEncountersService
	{
		private readonly HashSet<string> _weeklyBountyApiIds = new HashSet<string>();

		private readonly object _lock = new object();

		public WeeklyBountyEncountersService()
		{
			Rebuild();
		}

		public void Rebuild()
		{
			lock (_lock)
			{
				_weeklyBountyApiIds.Clear();
				DailyBountyData bountyData = Service.DailyBountyData;
				if (bountyData == null || !bountyData.Enabled)
				{
					return;
				}
				DateTime nextWeekly = Service.ResetWatcher.NextWeeklyReset;
				DateTime today = DateTime.UtcNow.Date;
				DateTime lastDayOfWeek = nextWeekly.Date.AddDays(-1.0);
				if (today > lastDayOfWeek)
				{
					return;
				}
				DateTime date = today;
				while (date <= lastDayOfWeek)
				{
					foreach (string apiId in DailyBountyService.GetBountyEncounterApiIdsForDay(PriorityRotationService.DayOfYearIndex(date)))
					{
						_weeklyBountyApiIds.Add(apiId);
					}
					date = date.AddDays(1.0);
				}
			}
		}

		public bool IsWeeklyBounty(string encounterApiId)
		{
			if (string.IsNullOrEmpty(encounterApiId))
			{
				return false;
			}
			lock (_lock)
			{
				return _weeklyBountyApiIds.Contains(encounterApiId);
			}
		}
	}
}
