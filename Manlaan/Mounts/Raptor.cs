using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Raptor : Mount
	{
		public Raptor(SettingCollection settingCollection)
			: base(settingCollection, "Raptor", "Raptor", "raptor", isUnderwaterMount: false, isWvWMount: false, 1)
		{
		}
	}
}
