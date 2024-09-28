using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class SortService
	{
		public static (List<Stat> items, List<Stat> currencies) SortStats(List<Stat> items, List<Stat> currencies, SettingService settingService)
		{
			currencies = SortCurrencies(currencies);
			items = SortItems(items, settingService);
			return (items, currencies);
		}

		private static List<Stat> SortCurrencies(List<Stat> currencies)
		{
			return currencies.OrderBy((Stat c) => c.ApiId).ToList();
		}

		private static List<Stat> SortItems(List<Stat> items, SettingService settingService)
		{
			List<SortByWithDirection> sortByWithDirectionList = settingService.SortByWithDirectionListSetting.get_Value().ToList();
			if (!sortByWithDirectionList.Any())
			{
				return items;
			}
			IOrderedEnumerable<Stat> orderedItems = items.OrderBy((Stat i) => 0);
			foreach (SortByWithDirection sortByWithDirection in sortByWithDirectionList)
			{
				orderedItems = ItemOrderBy(orderedItems, sortByWithDirection);
			}
			return orderedItems.ToList();
		}

		private static IOrderedEnumerable<Stat> ItemOrderBy(IOrderedEnumerable<Stat> items, SortByWithDirection sortByWithDirection)
		{
			switch (sortByWithDirection)
			{
			case SortByWithDirection.Name_Ascending:
				return items.ThenBy<Stat, string>((Stat i) => i.Details.Name);
			case SortByWithDirection.Name_Descending:
				return items.ThenByDescending<Stat, string>((Stat i) => i.Details.Name);
			case SortByWithDirection.Rarity_Ascending:
				return items.ThenBy<Stat, ItemRarity>((Stat i) => i.Details.Rarity);
			case SortByWithDirection.Rarity_Descending:
				return items.ThenByDescending<Stat, ItemRarity>((Stat i) => i.Details.Rarity);
			case SortByWithDirection.Count_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.Count);
			case SortByWithDirection.Count_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.Count);
			case SortByWithDirection.PositiveAndNegativeCount_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.CountSign);
			case SortByWithDirection.PositiveAndNegativeCount_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.CountSign);
			case SortByWithDirection.ApiId_Ascending:
				return items.ThenBy<Stat, int>((Stat i) => i.ApiId);
			case SortByWithDirection.ApiId_Descending:
				return items.ThenByDescending<Stat, int>((Stat i) => i.ApiId);
			case SortByWithDirection.ItemType_Ascending:
				return items.ThenBy<Stat, ItemType>((Stat i) => i.Details.Type);
			case SortByWithDirection.ItemType_Descending:
				return items.ThenByDescending<Stat, ItemType>((Stat i) => i.Details.Type);
			case SortByWithDirection.ProfitAll_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.CountSign * i.Profits.All.MaxProfitInCopper);
			case SortByWithDirection.ProfitAll_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.CountSign * i.Profits.All.MaxProfitInCopper);
			case SortByWithDirection.ProfitPerItem_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.CountSign * i.Profits.Each.MaxProfitInCopper);
			case SortByWithDirection.ProfitPerItem_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.CountSign * i.Profits.Each.MaxProfitInCopper);
			case SortByWithDirection.VendorProfitAll_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.CountSign * i.Profits.All.VendorProfitInCopper);
			case SortByWithDirection.VendorProfitAll_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.CountSign * i.Profits.All.VendorProfitInCopper);
			case SortByWithDirection.VendorProfitPerItem_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.CountSign * i.Profits.Each.VendorProfitInCopper);
			case SortByWithDirection.VendorProfitPerItem_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.CountSign * i.Profits.Each.VendorProfitInCopper);
			case SortByWithDirection.TradingPostProfitAll_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.CountSign * i.Profits.All.MaxTpProfitInCopper);
			case SortByWithDirection.TradingPostProfitAll_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.CountSign * i.Profits.All.MaxTpProfitInCopper);
			case SortByWithDirection.TradingPostProfitPerItem_Ascending:
				return items.ThenBy<Stat, long>((Stat i) => i.CountSign * i.Profits.Each.MaxTpProfitInCopper);
			case SortByWithDirection.TradingPostProfitPerItem_Descending:
				return items.ThenByDescending<Stat, long>((Stat i) => i.CountSign * i.Profits.Each.MaxTpProfitInCopper);
			default:
				Module.Logger.Error(Helper.CreateSwitchCaseNotFoundMessage(sortByWithDirection, "SortByWithDirection", "dont sort"));
				return items;
			}
		}
	}
}
