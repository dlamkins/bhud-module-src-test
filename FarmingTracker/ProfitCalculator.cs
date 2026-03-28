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

		public void CalculateProfits(Model model, TimeSpan elapsedFarmingTime)
		{
			long signed_profitInCopper = CalculateSignedProfitInCopper(model);
			Signed_ProfitPerHourInCopper = CalculateSignedProfitPerHourInCopper(signed_profitInCopper, elapsedFarmingTime);
			Signed_ProfitInCopper = signed_profitInCopper;
		}

		private static long CalculateSignedProfitInCopper(Model model)
		{
			List<Stat> stats = model.Stats.GetStats();
			IEnumerable<long> multiple_signed_statsSellProfitsInCopper = from s in stats
				where !s.IsCoinOrCustomCoin
				where s.StatVisibility != StatVisibility.Ignored
				select s.Signed_Count.Value * s.Profit.Unsigned_Max_ProfitInCopper;
			long total_signed_statsSellProfitInCopper = multiple_signed_statsSellProfitsInCopper.Sum();
			long signed_coinsInCopper = stats.SingleOrDefault((Stat s) => s.IsCoin)?.Signed_Count.Value ?? 0;
			long signed_totalProfit = signed_coinsInCopper + total_signed_statsSellProfitInCopper;
			if (DebugMode.DebugLoggingRequired)
			{
				Module.Logger.Debug($"totalProfit {signed_totalProfit} = " + $"coinsInCopper {signed_coinsInCopper} " + $"+ statsSellProfitInCopper {total_signed_statsSellProfitInCopper} " + "| maxAllProfits per Stat " + string.Join(" ", multiple_signed_statsSellProfitsInCopper));
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
	}
}
