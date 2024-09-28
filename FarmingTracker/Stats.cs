using System.Collections.Generic;
using Newtonsoft.Json;

namespace FarmingTracker
{
	public class Stats
	{
		public Dictionary<int, Stat> CurrencyById { get; } = new Dictionary<int, Stat>();


		public Dictionary<int, Stat> ItemById { get; } = new Dictionary<int, Stat>();


		public StatsSnapshot StatsSnapshot { get; set; } = new StatsSnapshot();


		public void UpdateStatsSnapshot()
		{
			StatsSnapshot statsSnapshot = JsonConvert.DeserializeObject<StatsSnapshot>(JsonConvert.SerializeObject((object)new StatsSnapshot
			{
				ItemById = ItemById,
				CurrencyById = CurrencyById
			}));
			if (statsSnapshot == null)
			{
				Module.Logger.Error("Failed to copy statsSnapshot.");
			}
			else
			{
				StatsSnapshot = statsSnapshot;
			}
		}
	}
}
