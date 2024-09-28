using System;
using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class ProfitCalculator
	{
		public long ProfitInCopper { get; private set; }

		public long ProfitPerHourInCopper { get; private set; }

		public void CalculateProfitPerHour(TimeSpan elapsedFarmingTime)
		{
			ProfitPerHourInCopper = CalculateProfitPerHourInCopper(ProfitInCopper, elapsedFarmingTime);
		}

		public void CalculateProfits(StatsSnapshot snapshot, SafeList<CustomStatProfit> customStatProfits, SafeList<int> ignoredItemApiIds, TimeSpan elapsedFarmingTime)
		{
			long profitInCopper = CalculateProfitInCopper(snapshot, customStatProfits, ignoredItemApiIds);
			ProfitPerHourInCopper = CalculateProfitPerHourInCopper(profitInCopper, elapsedFarmingTime);
			ProfitInCopper = profitInCopper;
		}

		public static long CalculateProfitInCopper(StatsSnapshot snapshot, SafeList<CustomStatProfit> customStatProfits, SafeList<int> ignoredItemApiIds)
		{
			List<CustomStatProfit> customStatProfitsCopy = customStatProfits.ToListSafe();
			List<int> ignoredItemApiIdsCopy = ignoredItemApiIds.ToListSafe();
			long itemsSellProfitInCopper = snapshot.ItemById.Values.Where((Stat i) => !ignoredItemApiIdsCopy.Contains(i.ApiId)).Sum((Stat i) => GetStatProfit(customStatProfitsCopy, i));
			long currenciesSellProfitInCopper = snapshot.CurrencyById.Values.Where((Stat c) => !c.IsCoinOrCustomCoin).Sum((Stat c) => GetStatProfit(customStatProfitsCopy, c));
			long coinsInCopper = snapshot.CurrencyById.Values.SingleOrDefault((Stat s) => s.IsCoin)?.Count ?? 0;
			long totalProfit = coinsInCopper + itemsSellProfitInCopper + currenciesSellProfitInCopper;
			if (DebugMode.DebugLoggingRequired)
			{
				Module.Logger.Debug($"totalProfit {totalProfit} = " + $"coinsInCopper {coinsInCopper} " + $"+ itemsSellProfitInCopper {itemsSellProfitInCopper} " + $"+ currenciesSellProfitInCopper {currenciesSellProfitInCopper} " + "| maxAllProfits per Item (including ignored) " + string.Join(" ", snapshot.ItemById.Values.Select((Stat i) => GetStatProfit(customStatProfitsCopy, i))) + "| maxAllProfits per Currency " + string.Join(" ", snapshot.CurrencyById.Values.Select((Stat c) => GetStatProfit(customStatProfitsCopy, c))));
			}
			return totalProfit;
		}

		public static long CalculateProfitPerHourInCopper(long totalProfitInCopper, TimeSpan elapsedFarmingTime)
		{
			if (totalProfitInCopper == 0L)
			{
				return 0L;
			}
			if (elapsedFarmingTime.TotalSeconds < 1.0)
			{
				return 0L;
			}
			double profitPerHourInCopper = (double)totalProfitInCopper / elapsedFarmingTime.TotalHours;
			if (profitPerHourInCopper > 9.223372036854776E+18)
			{
				return long.MaxValue;
			}
			if (profitPerHourInCopper <= -9.223372036854776E+18)
			{
				return -9223372036854775807L;
			}
			return (long)profitPerHourInCopper;
		}

		private static long GetStatProfit(List<CustomStatProfit> customStatProfits, Stat s)
		{
			Stat s2 = s;
			CustomStatProfit customStatProfit = customStatProfits.SingleOrDefault((CustomStatProfit c) => c.BelongsToStat(s2));
			if (customStatProfit != null)
			{
				return s2.CountSign * s2.Count * customStatProfit.CustomProfitInCopper;
			}
			return s2.CountSign * s2.Profits.All.MaxProfitInCopper;
		}
	}
}
