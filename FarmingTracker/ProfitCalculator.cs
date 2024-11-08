using System;
using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class ProfitCalculator
	{
		public long Signed_ProfitInCopper { get; private set; }

		public long Signed_ProfitPerHourInCopper { get; private set; }

		public void CalculateProfitPerHour(TimeSpan elapsedFarmingTime)
		{
			Signed_ProfitPerHourInCopper = CalculateSignedProfitPerHourInCopper(Signed_ProfitInCopper, elapsedFarmingTime);
		}

		public void CalculateProfits(StatsSnapshot snapshot, SafeList<CustomStatProfit> customStatProfits, SafeList<int> ignoredItemApiIds, TimeSpan elapsedFarmingTime)
		{
			long signed_profitInCopper = CalculateSignedProfitInCopper(snapshot, customStatProfits, ignoredItemApiIds);
			Signed_ProfitPerHourInCopper = CalculateSignedProfitPerHourInCopper(signed_profitInCopper, elapsedFarmingTime);
			Signed_ProfitInCopper = signed_profitInCopper;
		}

		private static long CalculateSignedProfitInCopper(StatsSnapshot snapshot, SafeList<CustomStatProfit> customStatProfits, SafeList<int> ignoredItemApiIds)
		{
			List<CustomStatProfit> customStatProfitsCopy = customStatProfits.ToListSafe();
			List<int> ignoredItemApiIdsCopy = ignoredItemApiIds.ToListSafe();
			long signed_itemsSellProfitInCopper = snapshot.ItemById.Values.Where((Stat i) => !ignoredItemApiIdsCopy.Contains(i.ApiId)).Sum((Stat i) => GetSignedStatProfit(customStatProfitsCopy, i));
			long signed_currenciesSellProfitInCopper = snapshot.CurrencyById.Values.Where((Stat c) => !c.IsCoinOrCustomCoin).Sum((Stat c) => GetSignedStatProfit(customStatProfitsCopy, c));
			long signed_coinsInCopper = snapshot.CurrencyById.Values.SingleOrDefault((Stat s) => s.IsCoin)?.Signed_Count ?? 0;
			long signed_totalProfit = signed_coinsInCopper + signed_itemsSellProfitInCopper + signed_currenciesSellProfitInCopper;
			if (DebugMode.DebugLoggingRequired)
			{
				Module.Logger.Debug($"totalProfit {signed_totalProfit} = " + $"coinsInCopper {signed_coinsInCopper} " + $"+ itemsSellProfitInCopper {signed_itemsSellProfitInCopper} " + $"+ currenciesSellProfitInCopper {signed_currenciesSellProfitInCopper} " + "| maxAllProfits per Item (including ignored) " + string.Join(" ", snapshot.ItemById.Values.Select((Stat i) => GetSignedStatProfit(customStatProfitsCopy, i))) + "| maxAllProfits per Currency " + string.Join(" ", snapshot.CurrencyById.Values.Select((Stat c) => GetSignedStatProfit(customStatProfitsCopy, c))));
			}
			return signed_totalProfit;
		}

		private static long CalculateSignedProfitPerHourInCopper(long signed_totalProfitInCopper, TimeSpan elapsedFarmingTime)
		{
			if (signed_totalProfitInCopper == 0L)
			{
				return 0L;
			}
			if (elapsedFarmingTime.TotalSeconds < 1.0)
			{
				return 0L;
			}
			double signed_profitPerHourInCopper = (double)signed_totalProfitInCopper / elapsedFarmingTime.TotalHours;
			if (signed_profitPerHourInCopper > 9.223372036854776E+18)
			{
				return long.MaxValue;
			}
			if (signed_profitPerHourInCopper <= -9.223372036854776E+18)
			{
				return -9223372036854775807L;
			}
			return (long)signed_profitPerHourInCopper;
		}

		private static long GetSignedStatProfit(List<CustomStatProfit> customStatProfits, Stat s)
		{
			Stat s2 = s;
			CustomStatProfit customStatProfit = customStatProfits.SingleOrDefault((CustomStatProfit c) => c.BelongsToStat(s2));
			if (customStatProfit != null)
			{
				return s2.Signed_Count * customStatProfit.Unsigned_CustomProfitInCopper;
			}
			return s2.CountSign * s2.Profits.All.Unsigned_MaxProfitInCopper;
		}
	}
}
