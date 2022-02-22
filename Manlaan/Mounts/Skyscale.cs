using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Skyscale : Mount
	{
		public Skyscale(SettingCollection settingCollection)
			: base(settingCollection, "Skyscale", "Skyscale", "skyscale", isUnderwaterMount: false, isWvWMount: false, 7)
		{
		}
	}
}
