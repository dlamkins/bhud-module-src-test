using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class RollerBeetle : Mount
	{
		public RollerBeetle(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Roller", "Roller Beetle", "roller", isUnderwaterMount: false, isWvWMount: false, 6)
		{
		}
	}
}
