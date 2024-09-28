using System.Collections.Generic;
using Blish_HUD.Settings;

namespace FarmingTracker
{
	public static class MigrateBlishSettingsService
	{
		public static void MigrateSettings(SettingEntry<int> settingsVersionSetting, SettingEntry<List<CurrencyFilter>> currencyFilterSetting, SettingEntry<List<SellMethodFilter>> sellMethodFilterSetting)
		{
			if (settingsVersionSetting.get_Value() == 1)
			{
				currencyFilterSetting.get_Value().Add(CurrencyFilter.UrsusOblige);
				sellMethodFilterSetting.get_Value().Add(SellMethodFilter.CustomProfitIsSet);
				settingsVersionSetting.set_Value(2);
				Module.Logger.Info("Migrated blish settings version 1 to 2.");
			}
		}
	}
}
