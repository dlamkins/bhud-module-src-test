using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Springer : Mount
	{
		public Springer(SettingCollection settingCollection)
			: base(settingCollection, "Springer", "Springer", "springer", isUnderwaterMount: false, isWvWMount: false, 2)
		{
		}
	}
}
