using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class Jackal : Mount
	{
		public Jackal(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Jackal", "Jackal", "jackal", (MountType)1, isUnderwaterMount: false, isFlyingMount: false, isWvWMount: false, 4)
		{
		}
	}
}
