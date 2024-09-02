using System.Collections.Generic;
using Newtonsoft.Json;

namespace FarmingTracker
{
	public class Model
	{
		public SafeList<int> IgnoredItemApiIds { get; set; } = new SafeList<int>();


		public SafeList<int> FavoriteItemApiIds { get; set; } = new SafeList<int>();


		public Dictionary<int, Stat> CurrencyById { get; } = new Dictionary<int, Stat>();


		public Dictionary<int, Stat> ItemById { get; } = new Dictionary<int, Stat>();


		public StatsSnapshot StatsSnapshot { get; set; } = new StatsSnapshot();


		public void UpdateStatsSnapshot()
		{
			StatsSnapshot newSnapshot = new StatsSnapshot
			{
				ItemById = ItemById,
				CurrencyById = CurrencyById
			};
			StatsSnapshot = JsonConvert.DeserializeObject<StatsSnapshot>(JsonConvert.SerializeObject((object)newSnapshot));
		}
	}
}
