using System;

namespace FarmingTracker
{
	public class Coin
	{
		public const int COIN_CURRENCY_ID = 1;

		public long Sign { get; }

		public long UnsignedGold { get; }

		public long UnsignedSilver { get; }

		public long UnsignedCopper { get; }

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
			int sign = Math.Sign(coinsInCopper);
			long num = Math.Abs(coinsInCopper);
			long unsignedGold = num / 10000;
			long unsignedSilver = (num - unsignedGold * 10000) / 100;
			long unsignedCopper = num % 100;
			UnsignedGold = unsignedGold;
			UnsignedSilver = unsignedSilver;
			UnsignedCopper = unsignedCopper;
			Gold = sign * unsignedGold;
			Silver = sign * unsignedSilver;
			Copper = sign * unsignedCopper;
			Sign = sign;
		}

		public object CreateCoinText()
		{
			string coinText = ((Sign == -1) ? "-" : "");
			if (Gold != 0L)
			{
				coinText += $"{UnsignedGold} g ";
			}
			if (Silver != 0L)
			{
				coinText += $"{UnsignedSilver} s ";
			}
			return coinText + $"{UnsignedCopper} c";
		}
	}
}
