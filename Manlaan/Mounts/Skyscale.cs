using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Skyscale : Mount
	{
		public Skyscale(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Skyscale", "Skyscale", "skyscale", isUnderwaterMount: false, isWvWMount: false, 8)
		{
		}
	}
}
