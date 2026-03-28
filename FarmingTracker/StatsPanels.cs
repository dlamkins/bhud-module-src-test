using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class StatsPanels
	{
		public FlowPanel CurrenciesFlowPanel { get; }

		public FlowPanel ItemsFlowPanel { get; }

		public FlowPanel FavoriteStatsFlowPanel { get; }

		public ClickThroughImage CurrencyFilterIcon { get; }

		public ClickThroughImage ItemsFilterIcon { get; }

		public StatsPanels(FlowPanel currenciesFlowPanel, FlowPanel favoriteStatsFlowPanel, FlowPanel itemsFlowPanel, ClickThroughImage currencyFilterIcon, ClickThroughImage itemsFilterIcon)
		{
			CurrenciesFlowPanel = currenciesFlowPanel;
			FavoriteStatsFlowPanel = favoriteStatsFlowPanel;
			ItemsFlowPanel = itemsFlowPanel;
			CurrencyFilterIcon = currencyFilterIcon;
			ItemsFilterIcon = itemsFilterIcon;
		}
	}
}
