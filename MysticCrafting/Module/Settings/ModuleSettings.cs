using Blish_HUD;
using Blish_HUD.Settings;

namespace MysticCrafting.Module.Settings
{
	public class ModuleSettings
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

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
