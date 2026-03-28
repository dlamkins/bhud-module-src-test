using System.Collections.Generic;

namespace FarmingTracker
{
	public class StatsSnapshot
	{
		public IReadOnlyDictionary<int, Stat> StatById { get; set; } = new Dictionary<int, Stat>();

	}
}
