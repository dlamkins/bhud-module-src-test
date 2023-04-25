using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class Raptor : Mount
	{
		public Raptor(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Raptor", "Raptor", "raptor", (MountType)5, isUnderwaterMount: false, isFlyingMount: false, isWvWMount: false, 1)
		{
		}
	}
}
