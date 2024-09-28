using System;

namespace FarmingTracker
{
	public class Coin
	{
		public const int COIN_CURRENCY_ID = 1;

		public long Sign { get; }

		public long Gold { get; }

		public long Silver { get; }

		public long Copper { get; }

		public bool HasToDisplayGold => Gold != 0;

		public bool HasToDisplaySilver
		{
			get
			{
				if (Gold == 0L)
				{
					return Silver != 0;
				}
				return true;
			}
		}

		public bool HasToDisplayCopper
		{
			get
			{
				if (Gold == 0L && Silver == 0L)
				{
					return Copper != 0;
				}
				return true;
			}
		}

		public Coin(long coinsInCopper)
		{
			long unsignedCoinsInCopper = Math.Abs(coinsInCopper);
			Sign = Math.Sign(coinsInCopper);
			Gold = unsignedCoinsInCopper / 10000;
			Silver = unsignedCoinsInCopper % 10000 / 100;
			Copper = unsignedCoinsInCopper % 100;
		}

		public object CreateCoinText()
		{
			string coinText = ((Sign == -1) ? "-" : "");
			if (Gold != 0L)
			{
				coinText += $"{Gold} g ";
			}
			if (Silver != 0L)
			{
				coinText += $"{Silver} s ";
			}
			return coinText + $"{Copper} c";
		}
	}
}
