using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class UiUpdater
	{
		public static void UpdateStatPanels(StatsPanels statsPanels, Model model, Services services)
		{
			Services services2 = services;
			List<Stat> source = (from s in model.Stats.GetStats()
				where s.Signed_Count.Value != 0
				where s.Details.State != StatApiDetailsState.MissingBecauseApiNotCalledYet
				where SearchService.IncludesSearchTerm(s, services2.SearchTerm)
				select s).ToList();
			List<Stat> items = (from s in source
				where s.IsItem
				where s.StatVisibility == StatVisibility.Regular
				select s).ToList();
			List<Stat> currencies = (from s in source
				where s.IsCurrency
				where s.StatVisibility == StatVisibility.Regular
				select s).ToList();
			List<Stat> favoriteStats = (from s in source
				where s.StatVisibility == StatVisibility.Favorite
				select s into f
				orderby f.StatType
				select f).ToList();
			currencies = CoinSplitter.ReplaceCoinWithGoldSilverCopperStats(currencies);
			favoriteStats = CoinSplitter.ReplaceCoinWithGoldSilverCopperStats(favoriteStats);
			(List<Stat> items, List<Stat> currencies) tuple = FilterService.FilterStatsAndSetFunnelOpacity(items, currencies, statsPanels, services2.SettingService);
			items = tuple.items;
			currencies = tuple.currencies;
			(List<Stat> items, List<Stat> currencies) tuple2 = SortService.SortStats(items, currencies, services2.SettingService);
			items = tuple2.items;
			currencies = tuple2.currencies;
			ControlCollection<Control> favoriteStatsControls = CreateStatControls(favoriteStats, PanelType.SummaryFavorites, model, services2);
			ControlCollection<Control> currencyControls = CreateStatControls(currencies, PanelType.SummaryCurrencies, model, services2);
			ControlCollection<Control> itemControls = CreateStatControls(items, PanelType.SummaryItems, model, services2);
			if (((IEnumerable<Control>)currencyControls).IsEmpty())
			{
				currencyControls.Add((Control)(object)new HintLabel("  No currency changes detected!"));
			}
			if (((IEnumerable<Control>)itemControls).IsEmpty())
			{
				itemControls.Add((Control)(object)new HintLabel("  No item changes detected!"));
			}
			if (((IEnumerable<Control>)favoriteStatsControls).IsEmpty())
			{
				if (favoriteStats.IsEmpty())
				{
					favoriteStatsControls.Add((Control)(object)new HintLabel("  Right click item or currency to add to favorites!"));
				}
				else
				{
					favoriteStatsControls.Add((Control)(object)new HintLabel("  No favorite item changes detected!"));
				}
			}
			Hacks.ClearAndAddChildrenWithoutUiFlickering(favoriteStatsControls, (Container)(object)statsPanels.FavoriteStatsFlowPanel);
			Hacks.ClearAndAddChildrenWithoutUiFlickering(itemControls, (Container)(object)statsPanels.ItemsFlowPanel);
			Hacks.ClearAndAddChildrenWithoutUiFlickering(currencyControls, (Container)(object)statsPanels.CurrenciesFlowPanel);
		}

		private static ControlCollection<Control> CreateStatControls(List<Stat> stats, PanelType panelType, Model model, Services services)
		{
			ControlCollection<Control> controls = new ControlCollection<Control>();
			foreach (Stat stat in stats)
			{
				controls.Add((Control)(object)new StatContainer(stat, panelType, model, services));
			}
			return controls;
		}
	}
}
