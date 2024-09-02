using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class DrfResultAdder
	{
		public static void UpdateCountsOrAddNewStats(List<DrfMessage> drfMessages, Dictionary<int, Stat> itemById, Dictionary<int, Stat> currencyById)
		{
			InternalUpdateCountsOrAddNewStats(drfMessages.SelectMany((DrfMessage d) => d.Payload.Drop.Items), itemById);
			InternalUpdateCountsOrAddNewStats(drfMessages.SelectMany((DrfMessage d) => d.Payload.Drop.Currencies), currencyById);
		}

		private static void InternalUpdateCountsOrAddNewStats(IEnumerable<KeyValuePair<int, long>> statIdAndCounts, Dictionary<int, Stat> statById)
		{
			foreach (KeyValuePair<int, long> statIdAndCount in statIdAndCounts)
			{
				if (statById.TryGetValue(statIdAndCount.Key, out var stat))
				{
					stat.Count += statIdAndCount.Value;
					continue;
				}
				statById[statIdAndCount.Key] = new Stat
				{
					ApiId = statIdAndCount.Key,
					Count = statIdAndCount.Value
				};
			}
		}
	}
}
