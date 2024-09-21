using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Atzie.MysticCrafting.Models.Currencies;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD.Controls;
using MysticCrafting.Models.Vendor;

namespace MysticCrafting.Module.Extensions
{
	public static class ListExtensions
	{
		public static IEnumerable<CurrencyQuantity> CombineQuantities(this IEnumerable<CurrencyQuantity> quantities)
		{
			List<CurrencyQuantity> newList = new List<CurrencyQuantity>();
			foreach (IGrouping<int, CurrencyQuantity> quantityGroup in from p in quantities
				where p?.Currency != null
				select p into q
				group q by q.Currency.Id)
			{
				newList.Add(new CurrencyQuantity
				{
					Currency = quantityGroup.FirstOrDefault().Currency,
					Count = quantityGroup.Sum((CurrencyQuantity q) => q.Count)
				});
			}
			return newList;
		}

		public static IEnumerable<CurrencyQuantity> MapToCurrencyQuantities(this IEnumerable<VendorSellsItem> vendorPrices, int multiplyBy = 1)
		{
			return (from p in vendorPrices.SelectMany((VendorSellsItem p) => p.ItemCosts)
				select new CurrencyQuantity
				{
					Currency = p.Currency,
					Count = p.UnitPrice * multiplyBy
				}).CombineQuantities().ToList();
		}

		public static IEnumerable<CurrencyQuantity> MapToCurrencyQuantities(this VendorSellsItem vendorPrices, int multiplyBy = 1)
		{
			return vendorPrices.ItemCosts.Select((VendorSellsItemCost p) => new CurrencyQuantity
			{
				Currency = p.Currency,
				Count = p.UnitPrice * multiplyBy
			}).CombineQuantities().ToList();
		}

		public static IEnumerable<CurrencyQuantity> MapToCurrencyQuantities(this IList<VendorSellsItemCost> costs, int multiplyBy = 1)
		{
			return costs.Select((VendorSellsItemCost p) => new CurrencyQuantity
			{
				Currency = p.Currency,
				Count = p.UnitPrice * multiplyBy
			}).CombineQuantities().ToList();
		}

		public static IList<VendorSellsItemGroup> CombineVendorSellsItems(this IList<VendorSellsItem> vendorSellsItems)
		{
			return (from v in vendorSellsItems
				group v by string.Join(",", from c in v.ItemCosts
					orderby c.GetComparableId()
					select c.GetComparableId()) into v
				select new VendorSellsItemGroup
				{
					VendorSellsItems = v.ToList()
				}).ToList();
		}

		public static string GetComparableId(this VendorSellsItemCost cost)
		{
			return $"{cost.UnitPrice}-{cost.CurrencyId}";
		}

		public static long CoinCount(this IEnumerable<CurrencyQuantity> prices)
		{
			return prices.Where((CurrencyQuantity p) => p.Currency != null && p.Currency.Id == 1)?.Sum((CurrencyQuantity p) => p.Count) ?? 0;
		}

		public static int CoinCount(this IEnumerable<VendorSellsItemCost> prices)
		{
			return prices.Where((VendorSellsItemCost p) => p.Currency != null && p.Currency.Id == 1)?.Sum((VendorSellsItemCost p) => p.UnitPrice) ?? 0;
		}

		public static CurrencyQuantity CoinQuantity(this IEnumerable<CurrencyQuantity> prices)
		{
			return prices.Where((CurrencyQuantity p) => p.Currency != null && p.Currency.Id == 1)?.CombineQuantities()?.FirstOrDefault();
		}

		public static IEnumerable<CurrencyQuantity> ExcludingCoins(this IEnumerable<CurrencyQuantity> prices)
		{
			return prices.Where((CurrencyQuantity p) => p.Currency != null && p.Currency.Id != 1);
		}

		public static IEnumerable<VendorSellsItemCost> ExcludingCoins(this IEnumerable<VendorSellsItemCost> prices)
		{
			return prices.Where((VendorSellsItemCost p) => p.CurrencyId != 1);
		}

		public static void SafeDispose(this IEnumerable<Control> controls, CancellationTokenSource token = null)
		{
			if (controls == null)
			{
				return;
			}
			foreach (Control control in controls.ToList())
			{
				if (token != null && token.IsCancellationRequested)
				{
					break;
				}
				if (control != null)
				{
					control.Dispose();
				}
			}
		}
	}
}
