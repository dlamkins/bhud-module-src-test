using System;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using MysticCrafting.Module.Strings;
using SemVer;

namespace MysticCrafting.Module.Settings
{
	public class ModuleSettings
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

		internal SettingEntry<KeyBinding> ToggleWindowSetting;

		internal SettingEntry<TradingPostOptions> TradingPostPreference;

		internal SettingEntry<Version> LastAcknowledgedUpdate;

		public ModuleSettings(SettingCollection settings)
		{
			Logger.Info("Initializing settings");
			DefineSettings(settings);
		}

		internal void DefineSettings(SettingCollection settings)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			ToggleWindowSetting = settings.DefineSetting<KeyBinding>("Toggle main window", new KeyBinding(), (Func<string>)(() => Common.ToggleWindow), (Func<string>)(() => "Open and close crafting window"));
			TradingPostPreference = settings.DefineSetting<TradingPostOptions>("Trading post preference", TradingPostOptions.Buy, (Func<string>)(() => Common.TradingPostDefault), (Func<string>)(() => "Default selection for items bought from the trading post"));
			LastAcknowledgedUpdate = settings.DefineSetting<Version>("LastAcknowledgedRelease", new Version(0, 0, 0, (string)null, (string)null), "Last version", (string)null, (SettingTypeRendererDelegate)null);
		}
	}
}
