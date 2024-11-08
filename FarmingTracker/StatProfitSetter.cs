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
				item.Profits = CreateProfits(item.Signed_Count, item.Details);
			}
		}

		private static Profits CreateProfits(long signed_count, ApiStatDetails details)
		{
			long unsigned_tpSellProfitInCopper = details.Unsigned_SellsUnitPriceInCopper * 85 / 100;
			long unsigned_tpBuyProfitInCopper = details.Unsigned_BuysUnitPriceInCopper * 85 / 100;
			bool canBeSoldOnTp = unsigned_tpSellProfitInCopper > 0 || unsigned_tpBuyProfitInCopper > 0;
			bool canBeSoldToVendor = details.Unsigned_VendorValueInCopper != 0L && !((IEnumerable<ApiEnum<ItemFlag>>)details.ItemFlags).Any((ApiEnum<ItemFlag> f) => f == ApiEnum<ItemFlag>.op_Implicit((ItemFlag)11));
			long unsigned_vendorProfitInCopper = (canBeSoldToVendor ? details.Unsigned_VendorValueInCopper : 0);
			return new Profits
			{
				Each = CreateProfit(unsigned_vendorProfitInCopper, unsigned_tpSellProfitInCopper, unsigned_tpBuyProfitInCopper),
				All = CreateProfit(signed_count * unsigned_vendorProfitInCopper, signed_count * unsigned_tpSellProfitInCopper, signed_count * unsigned_tpBuyProfitInCopper),
				CanBeSoldOnTp = canBeSoldOnTp,
				CanBeSoldToVendor = canBeSoldToVendor,
				CanNotBeSold = (!canBeSoldToVendor && !canBeSoldOnTp)
			};
		}

		private static Profit CreateProfit(long unsigned_vendorProfitInCopper, long unsigned_tpSellProfitInCopper, long unsigned_tpBuyProfitInCopper)
		{
			unsigned_vendorProfitInCopper = Math.Abs(unsigned_vendorProfitInCopper);
			unsigned_tpSellProfitInCopper = Math.Abs(unsigned_tpSellProfitInCopper);
			unsigned_tpBuyProfitInCopper = Math.Abs(unsigned_tpBuyProfitInCopper);
			long unsigned_maxTpProfitInCopper = Math.Max(unsigned_tpSellProfitInCopper, unsigned_tpBuyProfitInCopper);
			long unsigned_maxProfitInCopper = Math.Max(unsigned_vendorProfitInCopper, unsigned_maxTpProfitInCopper);
			return new Profit
			{
				Unsigned_VendorProfitInCopper = unsigned_vendorProfitInCopper,
				Unsigned_TpSellProfitInCopper = unsigned_tpSellProfitInCopper,
				Unsigned_TpBuyProfitInCopper = unsigned_tpBuyProfitInCopper,
				Unsigned_MaxTpProfitInCopper = unsigned_maxTpProfitInCopper,
				Unsigned_MaxProfitInCopper = unsigned_maxProfitInCopper
			};
		}
	}
}
