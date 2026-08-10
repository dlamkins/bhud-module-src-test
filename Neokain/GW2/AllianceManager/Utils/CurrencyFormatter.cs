using System.Globalization;

namespace Neokain.GW2.AllianceManager.Utils
{
	public static class CurrencyFormatter
	{
		private const int CoinCurrencyId = 1;

		public static string FormatCurrency(int currencyId, int amount)
		{
			if (currencyId == 1)
			{
				return FormatCoins(amount);
			}
			return FormatStandardCurrency(amount);
		}

		public static string FormatCoins(int totalCopper)
		{
			if (totalCopper == 0)
			{
				return "00g00s00c";
			}
			int gold = totalCopper / 10000;
			int silver = totalCopper % 10000 / 100;
			int copper = totalCopper % 100;
			return $"{gold:D2}g{silver:D2}s{copper:D2}c";
		}

		public static string FormatStandardCurrency(int amount)
		{
			return amount.ToString("N0", CultureInfo.CurrentCulture);
		}

		public static string FormatStandardCurrency(int amount, CultureInfo culture)
		{
			return amount.ToString("N0", culture);
		}
	}
}
