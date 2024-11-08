using System;

namespace FarmingTracker
{
	public class Coin
	{
		public const int COIN_CURRENCY_ID = 1;

		public long Sign { get; }

		public long Unsigned_Gold { get; }

		public long Unsigned_Silver { get; }

		public long Unsigned_Copper { get; }

		public bool HasToDisplayGold => Unsigned_Gold != 0;

		public bool HasToDisplaySilver
		{
			get
			{
				if (Unsigned_Gold == 0L)
				{
					return Unsigned_Silver != 0;
				}
				return true;
			}
		}

		public bool HasToDisplayCopper
		{
			get
			{
				if (Unsigned_Gold == 0L && Unsigned_Silver == 0L)
				{
					return Unsigned_Copper != 0;
				}
				return true;
			}
		}

		public Coin(long signed_coinsInCopper)
		{
			long unsigned_coinsInCopper = Math.Abs(signed_coinsInCopper);
			Sign = Math.Sign(signed_coinsInCopper);
			Unsigned_Gold = unsigned_coinsInCopper / 10000;
			Unsigned_Silver = unsigned_coinsInCopper % 10000 / 100;
			Unsigned_Copper = unsigned_coinsInCopper % 100;
		}

		public object CreateCoinText()
		{
			string coinText = ((Sign == -1) ? "-" : "");
			if (Unsigned_Gold != 0L)
			{
				coinText += $"{Unsigned_Gold} g ";
			}
			if (Unsigned_Silver != 0L)
			{
				coinText += $"{Unsigned_Silver} s ";
			}
			return coinText + $"{Unsigned_Copper} c";
		}
	}
}
