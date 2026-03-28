using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class CoinSplitter
	{
		public const int GOLD_FAKE_API_ID = -3000;

		public const int SILVER_FAKE_API_ID = -2000;

		public const int COPPER_FAKE_API_ID = -1000;

		public static List<Stat> ReplaceCoinWithGoldSilverCopperStats(List<Stat> stats)
		{
			Stat coinStat = stats.SingleOrDefault((Stat c) => c.IsCoin);
			if (coinStat == null)
			{
				return stats;
			}
			string localizedCoinName = coinStat.Details.Name;
			Coin coin = new Coin(coinStat.Signed_Count.Value);
			if (coin.HasToDisplayCopper)
			{
				Stat copperStat = CreateCoinStat("Copper", coin.Sign * coin.Unsigned_Copper, -1000, StatApiDetailsState.CopperCoinCustomStat, localizedCoinName);
				stats.Insert(0, copperStat);
			}
			if (coin.HasToDisplaySilver)
			{
				Stat silverStat = CreateCoinStat("Silver", coin.Sign * coin.Unsigned_Silver, -2000, StatApiDetailsState.SilveCoinCustomStat, localizedCoinName);
				stats.Insert(0, silverStat);
			}
			if (coin.HasToDisplayGold)
			{
				Stat goldStat = CreateCoinStat("Gold", coin.Sign * coin.Unsigned_Gold, -3000, StatApiDetailsState.GoldCoinCustomStat, localizedCoinName);
				stats.Insert(0, goldStat);
			}
			stats.Remove(coinStat);
			return stats;
		}

		private static Stat CreateCoinStat(string name, long signed_count, int apiId, StatApiDetailsState statApiDetailsState, string localizedCoinName)
		{
			Stat stat = new Stat();
			stat.ApiId = apiId;
			stat.StatType = StatType.Currency;
			stat.Signed_Count.Value = signed_count;
			stat.Details.Name = name;
			stat.Details.WikiSearchTerm = localizedCoinName;
			stat.Details.State = statApiDetailsState;
			return stat;
		}
	}
}
