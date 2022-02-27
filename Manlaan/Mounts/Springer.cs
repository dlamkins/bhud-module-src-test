using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Springer : Mount
	{
		public Springer(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Springer", "Springer", "springer", isUnderwaterMount: false, isWvWMount: false, 2)
		{
		}
	}
}
