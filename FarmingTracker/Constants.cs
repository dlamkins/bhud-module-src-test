using System;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class Constants
	{
		public static class TabTitles
		{
			public const string SUMMARY = "Summary";

			public const string TIMELINE = "Timeline";

			public const string FILTER = "Filter";

			public const string SORT = "Sort Items";

			public const string IGNORED = "Ignored Items";

			public const string CUSTOM_STAT_PROFIT = "Custom Profit";

			public const string SETTINGS = "Settings";

			public const string DEBUG = "Debug";
		}

		public const int PANEL_WIDTH = 500;

		public const int LABEL_WIDTH = 480;

		public const int SCROLLBAR_WIDTH_OFFSET = 30;

		public const string DRF_CONNECTION_LABEL_TEXT = "DRF Server Connection";

		public const string UPDATING_HINT_TEXT = "Updating...";

		public const string RESETTING_HINT_TEXT = "Resetting...";

		public const string GW2_API_ERROR_HINT = "GW2 API error";

		public const string FAVORITE_ITEMS_PANEL_TITLE = "Favorite Items";

		public const string ITEMS_PANEL_TITLE = "Items";

		public const string CURRENCIES_PANEL_TITLE = "Currencies";

		public const string FULL_HEIGHT_EMPTY_LABEL = " ";

		public const string ZERO_HEIGHT_EMPTY_LABEL = "";

		public const string HINT_IN_PANEL_PADDING = "  ";

		public const int PROFIT_PER_HOUR_UPDATE_INTERVAL_IN_SECONDS = 5;

		public static ItemRarity[] ALL_ITEM_RARITIES => (ItemRarity[])Enum.GetValues(typeof(ItemRarity));

		public static SellMethodFilter[] ALL_SELL_METHODS => (SellMethodFilter[])Enum.GetValues(typeof(SellMethodFilter));

		public static CountFilter[] ALL_COUNTS => (CountFilter[])Enum.GetValues(typeof(CountFilter));

		public static KnownByApiFilter[] ALL_KNOWN_BY_API => (KnownByApiFilter[])Enum.GetValues(typeof(KnownByApiFilter));

		public static ItemType[] ALL_ITEM_TYPES => (ItemType[])Enum.GetValues(typeof(ItemType));

		public static ItemFlag[] ALL_ITEM_FLAGS => (ItemFlag[])Enum.GetValues(typeof(ItemFlag));

		public static CurrencyFilter[] ALL_CURRENCIES => (CurrencyFilter[])Enum.GetValues(typeof(CurrencyFilter));
	}
}
