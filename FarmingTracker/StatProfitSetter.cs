using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class StatProfitSetter
	{
		public static void SetProfits(Stats Stats)
		{
			foreach (Stat stat in Stats.GetStats())
			{
				SetProfit(stat.Profit, stat.Details);
			}
		}

		private static void SetProfit(Profit profit, StatApiDetails details)
		{
			bool canBeSoldToVendor = details.Unsigned_VendorValueInCopper != 0L && !((IEnumerable<ApiEnum<ItemFlag>>)details.ItemFlags).Any((ApiEnum<ItemFlag> f) => f == ApiEnum<ItemFlag>.op_Implicit((ItemFlag)11));
			long unsigned_vendor_ProfitInCopper = (canBeSoldToVendor ? details.Unsigned_VendorValueInCopper : 0);
			long unsigned_tpSell_ProfitInCopper = details.Unsigned_SellsUnitPriceInCopper * 85 / 100;
			long unsigned_tpBuy_ProfitInCopper = details.Unsigned_BuysUnitPriceInCopper * 85 / 100;
			bool canBeSoldOnTp = unsigned_tpSell_ProfitInCopper > 0 || unsigned_tpBuy_ProfitInCopper > 0;
			long unsigned_maxTp_ProfitInCopper = Math.Max(unsigned_tpSell_ProfitInCopper, unsigned_tpBuy_ProfitInCopper);
			long unsigned_maxTpAndVendor_ProfitInCopper = Math.Max(unsigned_vendor_ProfitInCopper, unsigned_maxTp_ProfitInCopper);
			profit.CanBeSoldOnTp = canBeSoldOnTp;
			profit.CanBeSoldToVendor = canBeSoldToVendor;
			profit.CanNotBeSold = !canBeSoldToVendor && !canBeSoldOnTp;
			profit.Unsigned_Vendor_ProfitInCopper.Value = unsigned_vendor_ProfitInCopper;
			profit.Unsigned_TpSell_ProfitInCopper.Value = unsigned_tpSell_ProfitInCopper;
			profit.Unsigned_TpBuy_ProfitInCopper.Value = unsigned_tpBuy_ProfitInCopper;
			profit.Unsigned_MaxTp_ProfitInCopper.Value = unsigned_maxTp_ProfitInCopper;
			profit.Unsigned_MaxTpAndVendor_ProfitInCopper.Value = unsigned_maxTpAndVendor_ProfitInCopper;
		}
	}
}
