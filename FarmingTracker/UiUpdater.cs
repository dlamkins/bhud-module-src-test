using System.Collections.Generic;
using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class UiUpdater
	{
		public static void UpdateStatPanels(StatsPanels statsPanels, StatsSnapshot snapshot, Model model, Services services)
		{
			List<int> favoriteItemApiIds = model.FavoriteItemApiIds.ToListSafe();
			List<CustomStatProfit> customStatProfits = model.CustomStatProfits.ToListSafe();
			(List<Stat> items, List<Stat> currencies) tuple = StatsService.ShallowCopyStatsToPreventModification(snapshot);
			List<Stat> items = tuple.items;
			List<Stat> currencies = tuple.currencies;
			(List<Stat> items, List<Stat> currencies) tuple2 = StatsService.RemoveZeroCountStats(items, currencies);
			items = tuple2.items;
			currencies = tuple2.currencies;
			(List<Stat> items, List<Stat> currencies) tuple3 = StatsService.RemoveStatsNotUpdatedYetDueToApiError(items, currencies);
			items = tuple3.items;
			currencies = tuple3.currencies;
			(List<Stat> favoriteItems, List<Stat> regularItems) tuple4 = StatsService.SplitIntoFavoriteAndRegularItems(items, favoriteItemApiIds);
			List<Stat> favoriteItems = tuple4.favoriteItems;
			items = tuple4.regularItems;
			items = StatsService.RemoveIgnoredItems(items, model.IgnoredItemApiIds.ToListSafe());
			currencies = CoinSplitter.ReplaceCoinWithGoldSilverCopperStats(currencies);
			(List<Stat> items, List<Stat> currencies) tuple5 = SearchService.FilterBySearchTerm(items, currencies, services.SearchTerm);
			items = tuple5.items;
			currencies = tuple5.currencies;
			(List<Stat> items, List<Stat> currencies) tuple6 = FilterService.FilterStatsAndSetFunnelOpacity(items, currencies, customStatProfits, statsPanels, services.SettingService);
			items = tuple6.items;
			currencies = tuple6.currencies;
			(List<Stat> items, List<Stat> currencies) tuple7 = SortService.SortStats(items, currencies, services.SettingService);
			items = tuple7.items;
			currencies = tuple7.currencies;
			ControlCollection<Control> currencyControls = CreateStatControls(currencies, PanelType.SummaryCurrencies, model.IgnoredItemApiIds, model.FavoriteItemApiIds, model.CustomStatProfits, services);
			ControlCollection<Control> favoriteItemsControls = CreateStatControls(favoriteItems, PanelType.SummaryFavoriteItems, model.IgnoredItemApiIds, model.FavoriteItemApiIds, model.CustomStatProfits, services);
			ControlCollection<Control> itemControls = CreateStatControls(items, PanelType.SummaryRegularItems, model.IgnoredItemApiIds, model.FavoriteItemApiIds, model.CustomStatProfits, services);
			if (((IEnumerable<Control>)currencyControls).IsEmpty())
			{
				currencyControls.Add((Control)(object)new HintLabel("  No currency changes detected!"));
			}
			if (((IEnumerable<Control>)itemControls).IsEmpty())
			{
				itemControls.Add((Control)(object)new HintLabel("  No item changes detected!"));
			}
			if (((IEnumerable<Control>)favoriteItemsControls).IsEmpty())
			{
				if (favoriteItemApiIds.IsEmpty())
				{
					favoriteItemsControls.Add((Control)(object)new HintLabel("  Right click item to add to favorites!"));
				}
				else
				{
					favoriteItemsControls.Add((Control)(object)new HintLabel("  No favorite item changes detected!"));
				}
			}
			Hacks.ClearAndAddChildrenWithoutUiFlickering(itemControls, (Container)(object)statsPanels.ItemsFlowPanel);
			Hacks.ClearAndAddChildrenWithoutUiFlickering(favoriteItemsControls, (Container)(object)statsPanels.FavoriteItemsFlowPanel);
			Hacks.ClearAndAddChildrenWithoutUiFlickering(currencyControls, (Container)(object)statsPanels.CurrenciesFlowPanel);
		}

		private static ControlCollection<Control> CreateStatControls(List<Stat> stats, PanelType panelType, SafeList<int> ignoredItemApiIds, SafeList<int> favoriteItemApiIds, SafeList<CustomStatProfit> customStatProfits, Services services)
		{
			ControlCollection<Control> controls = new ControlCollection<Control>();
			foreach (Stat stat in stats)
			{
				controls.Add((Control)(object)new StatContainer(stat, panelType, ignoredItemApiIds, favoriteItemApiIds, customStatProfits, services));
			}
			return controls;
		}
	}
}
