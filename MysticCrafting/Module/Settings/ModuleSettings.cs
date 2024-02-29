using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace MysticCrafting.Module.Settings
{
	public class ModuleSettings
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

		internal SettingEntry<KeyBinding> ToggleWindowSetting;

		internal SettingEntry<TradingPostOptions> TradingPostPreference;

		public ModuleSettings(SettingCollection settings)
		{
			Logger.Info("Initializing settings");
			InitGeneral(settings);
		}

		private void InitGeneral(SettingCollection settings)
		{
		}
	}
}
