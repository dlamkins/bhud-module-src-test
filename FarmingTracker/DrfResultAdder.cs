using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class DrfResultAdder
	{
		public static void UpdateCountsOrAddNewStats(List<DrfMessage> drfMessages, Stats stats)
		{
			InternalUpdateCountsOrAddNewStats(drfMessages.SelectMany((DrfMessage d) => d.Payload.Drop.Items), StatType.Item, stats);
			InternalUpdateCountsOrAddNewStats(drfMessages.SelectMany((DrfMessage d) => d.Payload.Drop.Currencies), StatType.Currency, stats);
		}

		private static void InternalUpdateCountsOrAddNewStats(IEnumerable<KeyValuePair<int, long>> statIdAndCounts, StatType statType, Stats stats)
		{
			foreach (KeyValuePair<int, long> statIdAndCount in statIdAndCounts)
			{
				Stat stat2 = new Stat();
				stat2.ApiId = statIdAndCount.Key;
				stat2.StatType = statType;
				stat2.Signed_Count.Value = statIdAndCount.Value;
				Stat stat = stat2;
				stats.UpdateCountOrAddNewStat(stat);
			}
		}
	}
}
