using System.Collections.Generic;
using System.Linq;

namespace FarmingTracker
{
	public class CoinSplitter
	{
		public const int GOLD_FAKE_API_ID = -3;

		public const int SILVER_FAKE_API_ID = -2;

		public const int COPPER_FAKE_API_ID = -1;

		public static List<Stat> ReplaceCoinWithGoldSilverCopperStats(List<Stat> currencies)
		{
			Stat coinStat = currencies.SingleOrDefault((Stat c) => c.IsCoin);
			if (coinStat == null)
			{
				return currencies;
			}
			string localizedCoinName = coinStat.Details.Name;
			Coin coin = new Coin(coinStat.Count);
			if (coin.HasToDisplayGold)
			{
				Stat goldStat = CreateCoinStat("Gold", coin.Sign * coin.Gold, -3, ApiStatDetailsState.GoldCoinCustomStat, localizedCoinName);
				currencies.Add(goldStat);
			}
			if (coin.HasToDisplaySilver)
			{
				Stat silverStat = CreateCoinStat("Silver", coin.Sign * coin.Silver, -2, ApiStatDetailsState.SilveCoinCustomStat, localizedCoinName);
				currencies.Add(silverStat);
			}
			if (coin.HasToDisplayCopper)
			{
				Stat copperStat = CreateCoinStat("Copper", coin.Sign * coin.Copper, -1, ApiStatDetailsState.CopperCoinCustomStat, localizedCoinName);
				currencies.Add(copperStat);
			}
			currencies.Remove(coinStat);
			return currencies;
		}

		private static Stat CreateCoinStat(string name, long count, int apiId, ApiStatDetailsState apiStatDetailsState, string localizedCoinName)
		{
			return new Stat
			{
				ApiId = apiId,
				StatType = StatType.Currency,
				Count = count,
				Details = 
				{
					Name = name,
					WikiSearchTerm = localizedCoinName,
					State = apiStatDetailsState
				}
			};
		}
	}
}
