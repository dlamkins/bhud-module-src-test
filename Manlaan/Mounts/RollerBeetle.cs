using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class RollerBeetle : Mount
	{
		public RollerBeetle(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Roller", "Roller Beetle", "roller", (MountType)6, isUnderwaterMount: false, isFlyingMount: false, isWvWMount: false, 6)
		{
		}
	}
}
