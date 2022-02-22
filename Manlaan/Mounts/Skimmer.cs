using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Skimmer : Mount
	{
		public Skimmer(SettingCollection settingCollection)
			: base(settingCollection, "Skimmer", "Skimmer", "skimmer", isUnderwaterMount: true, isWvWMount: false, 3)
		{
		}
	}
}
