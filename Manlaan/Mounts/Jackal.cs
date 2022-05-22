using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Jackal : Mount
	{
		public Jackal(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Jackal", "Jackal", "jackal", isUnderwaterMount: false, isWvWMount: false, 4)
		{
		}
	}
}
