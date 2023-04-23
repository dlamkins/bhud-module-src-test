using System;
using Blish_HUD.Settings;

namespace KpRefresher
{
	public class ModuleSettings
	{
		public SettingEntry<bool> EnableAutoRetry { get; set; }

		public SettingEntry<bool> ShowScheduleNotification { get; set; }

		public SettingEntry<bool> EnableRefreshOnKill { get; set; }

		public SettingEntry<bool> RefreshOnKillOnlyBoss { get; set; }

		public SettingEntry<bool> RefreshOnMapChange { get; set; }

		public SettingEntry<int> DelayBeforeRefreshOnMapChange { get; set; }

		public ModuleSettings(SettingCollection settings)
		{
			SettingCollection internalSettings = settings.AddSubCollection("Internal", false);
			EnableAutoRetry = internalSettings.DefineSetting<bool>("EnableAutoRetry", true, (Func<string>)null, (Func<string>)null);
			ShowScheduleNotification = internalSettings.DefineSetting<bool>("ShowScheduleNotification", true, (Func<string>)null, (Func<string>)null);
			EnableRefreshOnKill = internalSettings.DefineSetting<bool>("EnableRefreshOnKill", false, (Func<string>)null, (Func<string>)null);
			RefreshOnKillOnlyBoss = internalSettings.DefineSetting<bool>("RefreshOnKillOnlyBoss", true, (Func<string>)null, (Func<string>)null);
			RefreshOnMapChange = internalSettings.DefineSetting<bool>("RefreshOnMapChange", false, (Func<string>)null, (Func<string>)null);
			DelayBeforeRefreshOnMapChange = internalSettings.DefineSetting<int>("DelayBeforeRefreshOnMapChange", 10, (Func<string>)null, (Func<string>)null);
		}
	}
}
