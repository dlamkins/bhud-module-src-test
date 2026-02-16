using Blish_HUD.Settings;

namespace RaidWeekPlanner
{
	public class ModuleSettings
	{
		public ModuleSettings(SettingCollection settings)
		{
			settings.AddSubCollection("Internal", false);
		}
	}
}
