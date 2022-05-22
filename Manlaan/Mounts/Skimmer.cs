using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Skimmer : Mount
	{
		public Skimmer(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Skimmer", "Skimmer", "skimmer", isUnderwaterMount: true, isWvWMount: false, 3)
		{
		}
	}
}
