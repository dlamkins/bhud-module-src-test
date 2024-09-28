using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class StatsService
	{
		public static (List<Stat> items, List<Stat> currencies) ShallowCopyStatsToPreventModification(StatsSnapshot snapshot)
		{
			List<Stat> item = snapshot.ItemById.Values.ToList();
			List<Stat> currencies = snapshot.CurrencyById.Values.ToList();
			return (item, currencies);
		}

		public static (List<Stat> items, List<Stat> currencies) RemoveStatsNotUpdatedYetDueToApiError(List<Stat> items, List<Stat> currencies)
		{
			items = items.Where((Stat s) => s.Details.State != ApiStatDetailsState.MissingBecauseApiNotCalledYet).ToList();
			currencies = currencies.Where((Stat s) => s.Details.State != ApiStatDetailsState.MissingBecauseApiNotCalledYet).ToList();
			return (items, currencies);
		}

		public static (List<Stat> items, List<Stat> currencies) RemoveZeroCountStats(List<Stat> items, List<Stat> currencies)
		{
			items = items.Where((Stat s) => s.Count != 0).ToList();
			currencies = currencies.Where((Stat s) => s.Count != 0).ToList();
			return (items, currencies);
		}

		public static void ResetCounts(Dictionary<int, Stat> statById)
		{
			foreach (Stat value in statById.Values)
			{
				value.Count = 0L;
			}
		}

		public static List<Stat> RemoveIgnoredItems(List<Stat> items, List<int> ignoredItemApiIds)
		{
			List<int> ignoredItemApiIds2 = ignoredItemApiIds;
			return items.Where((Stat s) => !ignoredItemApiIds2.Contains(s.ApiId)).ToList();
		}

		public static (List<Stat> favoriteItems, List<Stat> regularItems) SplitIntoFavoriteAndRegularItems(List<Stat> items, List<int> favoriteItemApiIds)
		{
			List<int> favoriteItemApiIds2 = favoriteItemApiIds;
			if (favoriteItemApiIds2.IsEmpty())
			{
				return (new List<Stat>(), items);
			}
			List<Stat> item = items.Where((Stat i) => favoriteItemApiIds2.Contains(i.ApiId)).ToList();
			List<Stat> regularItems = items.Where((Stat i) => !favoriteItemApiIds2.Contains(i.ApiId)).ToList();
			return (item, regularItems);
		}
	}
}
