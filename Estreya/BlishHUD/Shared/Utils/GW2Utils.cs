using Microsoft.Win32;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class GW2Utils
	{
		public static string FormatCoins(int coins)
		{
			(int, int, int) splitCoins = SplitCoins(coins);
			if (splitCoins.Item1 <= 0)
			{
				if (splitCoins.Item2 <= 0)
				{
					return $"{splitCoins.Item3}c";
				}
				return $"{splitCoins.Item2}s {splitCoins.Item3}c";
			}
			return $"{splitCoins.Item1}g {splitCoins.Item2}s {splitCoins.Item3}c";
		}

		public static (int Gold, int Silver, int Copper) SplitCoins(int coins)
		{
			int copper = coins % 100;
			coins = (coins - copper) / 100;
			int silver = coins % 100;
			return ((coins - silver) / 100, silver, copper);
		}

		public static int ToCoins(int gold, int silver, int copper)
		{
			return copper + silver * 100 + gold * 10000;
		}

		public static string GetInstallPath()
		{
			return (string)RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey("SOFTWARE\\ArenaNet\\Guild Wars 2").GetValue("Path");
		}
	}
}
