using System.Collections.Generic;
using System.Linq;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Extensions
{
	public static class ListExtensions
	{
		public static IEnumerable<MysticCurrencyQuantity> CombineQuantities(this IEnumerable<MysticCurrencyQuantity> quantities)
		{
			List<MysticCurrencyQuantity> newList = new List<MysticCurrencyQuantity>();
			foreach (IGrouping<int, MysticCurrencyQuantity> quantityGroup in from p in quantities
				where p?.Currency != null
				select p into q
				group q by q.Currency.GameId)
			{
				newList.Add(new MysticCurrencyQuantity
				{
					Currency = quantityGroup.FirstOrDefault().Currency,
					Count = quantityGroup.Sum((MysticCurrencyQuantity q) => q.Count)
				});
			}
			return newList;
		}

		public static IEnumerable<MysticCurrencyQuantity> MapToCurrencyQuantities(this IEnumerable<VendorSellsItem> vendorPrices, int multiplyBy = 1)
		{
			return (from p in vendorPrices.SelectMany((VendorSellsItem p) => p.Prices)
				select new MysticCurrencyQuantity
				{
					Currency = ServiceContainer.CurrencyRepository.GetCurrency(p.Currency),
					Count = p.Value.Value * multiplyBy
				}).CombineQuantities().ToList();
		}

		public static IEnumerable<MysticCurrencyQuantity> MapToCurrencyQuantities(this VendorSellsItem vendorPrices, int multiplyBy = 1)
		{
			return vendorPrices.Prices.Select((VendorSellsItemCost p) => new MysticCurrencyQuantity
			{
				Currency = ServiceContainer.CurrencyRepository.GetCurrency(p.Currency),
				Count = p.Value.Value * multiplyBy
			}).CombineQuantities().ToList();
		}

		public static int CoinCount(this IEnumerable<MysticCurrencyQuantity> prices)
		{
			return prices.Where((MysticCurrencyQuantity p) => p.Currency != null && p.Currency.GameId == 1)?.Sum((MysticCurrencyQuantity p) => p.Count) ?? 0;
		}

		public static MysticCurrencyQuantity CoinQuantity(this IEnumerable<MysticCurrencyQuantity> prices)
		{
			return prices.Where((MysticCurrencyQuantity p) => p.Currency != null && p.Currency.GameId == 1)?.CombineQuantities()?.FirstOrDefault();
		}

		public static IEnumerable<MysticCurrencyQuantity> ExcludingCoins(this IEnumerable<MysticCurrencyQuantity> prices)
		{
			return prices.Where((MysticCurrencyQuantity p) => p.Currency != null && p.Currency.GameId != 1);
		}
	}
}
