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

		public void CalculateProfits(StatsSnapshot snapshot, SafeList<int> ignoredItemApiIds, TimeSpan elapsedFarmingTime)
		{
			long profitInCopper = CalculateTotalProfitInCopper(snapshot, ignoredItemApiIds);
			ProfitPerHourInCopper = CalculateProfitPerHourInCopper(profitInCopper, elapsedFarmingTime);
			ProfitInCopper = profitInCopper;
		}

		public static long CalculateTotalProfitInCopper(StatsSnapshot snapshot, SafeList<int> ignoredItemApiIds)
		{
			long coinsInCopper = snapshot.CurrencyById.Values.SingleOrDefault((Stat s) => s.IsCoin)?.Count ?? 0;
			List<int> ignoredItemApiIdsCopy = ignoredItemApiIds.ToListSafe();
			long itemsSellProfitInCopper = snapshot.ItemById.Values.Where((Stat s) => !ignoredItemApiIdsCopy.Contains(s.ApiId)).Sum((Stat s) => s.CountSign * s.Profits.All.MaxProfitInCopper);
			long totalProfit = coinsInCopper + itemsSellProfitInCopper;
			if (Module.DebugEnabled)
			{
				Module.Logger.Debug($"totalProfit {totalProfit} = coinsInCopper {coinsInCopper} + itemsSellProfitInCopper {itemsSellProfitInCopper} | " + "maxProfitsPerItem " + string.Join(" ", snapshot.ItemById.Values.Select((Stat s) => s.CountSign * s.Profits.All.MaxProfitInCopper)));
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
	}
}
