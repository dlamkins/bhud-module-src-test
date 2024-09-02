using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class StatProfitSetter
	{
		public static void SetProfits(Dictionary<int, Stat> itemById)
		{
			foreach (Stat item in itemById.Values)
			{
				item.Profits = CreateProfits(item.Count, item.Details);
			}
		}

		private static Profits CreateProfits(long count, ApiStatDetails details)
		{
			long tpSellProfitInCopper = details.SellsUnitPriceInCopper * 85 / 100;
			long tpBuyProfitInCopper = details.BuysUnitPriceInCopper * 85 / 100;
			bool canBeSoldOnTp = tpSellProfitInCopper > 0 || tpBuyProfitInCopper > 0;
			bool canBeSoldToVendor = details.VendorValueInCopper != 0L && !((IEnumerable<ApiEnum<ItemFlag>>)details.ItemFlags).Any((ApiEnum<ItemFlag> f) => f == ApiEnum<ItemFlag>.op_Implicit((ItemFlag)11));
			long vendorProfitInCopper = (canBeSoldToVendor ? details.VendorValueInCopper : 0);
			return new Profits
			{
				Each = CreateProfit(vendorProfitInCopper, tpSellProfitInCopper, tpBuyProfitInCopper),
				All = CreateProfit(count * vendorProfitInCopper, count * tpSellProfitInCopper, count * tpBuyProfitInCopper),
				CanBeSoldOnTp = canBeSoldOnTp,
				CanBeSoldToVendor = canBeSoldToVendor,
				CanNotBeSold = (!canBeSoldToVendor && !canBeSoldOnTp)
			};
		}

		private static Profit CreateProfit(long vendorProfitInCopper, long tpSellProfitInCopper, long tpBuyProfitInCopper)
		{
			vendorProfitInCopper = Math.Abs(vendorProfitInCopper);
			tpSellProfitInCopper = Math.Abs(tpSellProfitInCopper);
			tpBuyProfitInCopper = Math.Abs(tpBuyProfitInCopper);
			long maxTpProfitInCopper = Math.Max(tpSellProfitInCopper, tpBuyProfitInCopper);
			long maxProfitInCopper = Math.Max(vendorProfitInCopper, maxTpProfitInCopper);
			return new Profit
			{
				VendorProfitInCopper = vendorProfitInCopper,
				TpSellProfitInCopper = tpSellProfitInCopper,
				TpBuyProfitInCopper = tpBuyProfitInCopper,
				MaxTpProfitInCopper = maxTpProfitInCopper,
				MaxProfitInCopper = maxProfitInCopper
			};
		}
	}
}
