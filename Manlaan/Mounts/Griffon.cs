using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Griffon : Mount
	{
		public Griffon(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Griffon", "Griffon", "griffon", isUnderwaterMount: false, isWvWMount: false, 5)
		{
		}
	}
}
