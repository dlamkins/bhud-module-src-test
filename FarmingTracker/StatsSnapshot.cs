using System.Collections.Generic;

namespace FarmingTracker
{
	public class StatsSnapshot
	{
		public IReadOnlyDictionary<int, Stat> ItemById { get; set; } = new Dictionary<int, Stat>();


		public IReadOnlyDictionary<int, Stat> CurrencyById { get; set; } = new Dictionary<int, Stat>();

	}
}
