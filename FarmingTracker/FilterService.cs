using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class FilterService
	{
		public static bool IsUnknownFilterElement<T>(int currencyId)
		{
			return !Enum.IsDefined(typeof(T), currencyId);
		}

		public static (List<Stat> items, List<Stat> currencies) FilterStatsAndSetFunnelOpacity(List<Stat> items, List<Stat> currencies, StatsPanels statsPanels, SettingService settingService)
		{
			int currenciesCountBeforeFiltering = currencies.Count();
			int itemsCountBeforeFiltering = items.Count();
			currencies = FilterCurrencies(currencies, settingService);
			items = FilterItems(items, settingService);
			bool noCurrenciesHiddenByFilter = currencies.Count() == currenciesCountBeforeFiltering;
			bool noItemsHiddenByFilter = items.Count() == itemsCountBeforeFiltering;
			statsPanels.CurrencyFilterIcon.SetOpacity(noCurrenciesHiddenByFilter);
			statsPanels.ItemsFilterIcon.SetOpacity(noItemsHiddenByFilter);
			return (items, currencies);
		}

		private static List<Stat> FilterCurrencies(List<Stat> currencies, SettingService settingService)
		{
			List<KnownByApiFilter> knownByApi = settingService.KnownByApiFilterSetting.get_Value().ToList();
			if (knownByApi.Any())
			{
				currencies = currencies.Where((Stat s) => IsShownByKnownByApiFilter(s, knownByApi)).ToList();
			}
			List<CountFilter> countFilter = settingService.CountFilterSetting.get_Value().ToList();
			if (countFilter.Any())
			{
				currencies = currencies.Where((Stat s) => IsShownByCountSignFilter(s, countFilter)).ToList();
			}
			List<CurrencyFilter> currencyFilter = settingService.CurrencyFilterSetting.get_Value().ToList();
			if (currencyFilter.Any())
			{
				currencies = currencies.Where((Stat s) => IsShownByCurrencyFilter(s, currencyFilter)).ToList();
			}
			return currencies;
		}

		private static List<Stat> FilterItems(List<Stat> items, SettingService settingService)
		{
			List<KnownByApiFilter> knownByApi = settingService.KnownByApiFilterSetting.get_Value().ToList();
			if (knownByApi.Any())
			{
				items = items.Where((Stat s) => IsShownByKnownByApiFilter(s, knownByApi)).ToList();
			}
			List<CountFilter> countFilter = settingService.CountFilterSetting.get_Value().ToList();
			if (countFilter.Any())
			{
				items = items.Where((Stat s) => IsShownByCountSignFilter(s, countFilter)).ToList();
			}
			List<SellMethodFilter> sellMethodFilter = settingService.SellMethodFilterSetting.get_Value().ToList();
			if (sellMethodFilter.Any())
			{
				items = items.Where((Stat s) => IsShownBySellMethodFilter(s, sellMethodFilter)).ToList();
			}
			List<ItemRarity> rarityFilter = settingService.RarityStatsFilterSetting.get_Value().ToList();
			if (rarityFilter.Any())
			{
				items = items.Where((Stat s) => rarityFilter.Contains(s.Details.Rarity)).ToList();
			}
			List<ItemType> typeFilter = settingService.TypeStatsFilterSetting.get_Value().ToList();
			if (typeFilter.Any())
			{
				items = items.Where((Stat s) => typeFilter.Contains(s.Details.Type)).ToList();
			}
			List<ItemFlag> flagFilter = settingService.FlagStatsFilterSetting.get_Value().ToList();
			if (flagFilter.Any())
			{
				items = items.Where((Stat s) => IsShownByItemFlagFilter(s, flagFilter)).ToList();
			}
			return items;
		}

		private static bool IsShownByItemFlagFilter(Stat item, List<ItemFlag> flagFilter)
		{
			if (!((IEnumerable<ApiEnum<ItemFlag>>)item.Details.ItemFlags).Any())
			{
				return true;
			}
			return item.Details.ItemFlags.get_List().Any((ApiEnum<ItemFlag> f) => flagFilter.Contains(ApiEnum<ItemFlag>.op_Implicit(f)));
		}

		private static bool IsShownByCurrencyFilter(Stat c, List<CurrencyFilter> currencyFilter)
		{
			if (IsUnknownFilterElement<CurrencyFilter>(c.ApiId))
			{
				return true;
			}
			if (currencyFilter.Contains((CurrencyFilter)c.ApiId))
			{
				return true;
			}
			return false;
		}

		private static bool IsShownByKnownByApiFilter(Stat stat, List<KnownByApiFilter> knownByApi)
		{
			if (stat.Details.IsCustomCoinStat)
			{
				return true;
			}
			if (knownByApi.Contains(KnownByApiFilter.KnownByApi) && stat.Details.State == ApiStatDetailsState.SetByApi)
			{
				return true;
			}
			if (knownByApi.Contains(KnownByApiFilter.UnknownByApi) && stat.Details.State == ApiStatDetailsState.MissingBecauseUnknownByApi)
			{
				return true;
			}
			return false;
		}

		private static bool IsShownBySellMethodFilter(Stat stat, List<SellMethodFilter> sellMethodFilter)
		{
			if (sellMethodFilter.Contains(SellMethodFilter.SellableToVendor) && stat.Profits.CanBeSoldToVendor)
			{
				return true;
			}
			if (sellMethodFilter.Contains(SellMethodFilter.SellableOnTradingPost) && stat.Profits.CanBeSoldOnTp)
			{
				return true;
			}
			if (sellMethodFilter.Contains(SellMethodFilter.NotSellable) && stat.Profits.CanNotBeSold)
			{
				return true;
			}
			return false;
		}

		private static bool IsShownByCountSignFilter(Stat stat, List<CountFilter> countFilter)
		{
			if (countFilter.Contains(CountFilter.PositiveCount) && stat.Count > 0)
			{
				return true;
			}
			if (countFilter.Contains(CountFilter.NegativeCount) && stat.Count < 0)
			{
				return true;
			}
			if (stat.Details.IsCustomCoinStat)
			{
				return true;
			}
			return false;
		}
	}
}
