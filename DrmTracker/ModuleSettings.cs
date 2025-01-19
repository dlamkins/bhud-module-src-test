using Blish_HUD.Settings;

namespace DrmTracker
{
	public class ModuleSettings
	{
		public ModuleSettings(SettingCollection settings)
		{
			settings.AddSubCollection("Internal", false);
		}
	}
}
