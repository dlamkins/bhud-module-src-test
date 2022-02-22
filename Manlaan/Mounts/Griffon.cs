using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Griffon : Mount
	{
		public Griffon(SettingCollection settingCollection)
			: base(settingCollection, "Griffon", "Griffon", "griffon", isUnderwaterMount: false, isWvWMount: false, 4)
		{
		}
	}
}
