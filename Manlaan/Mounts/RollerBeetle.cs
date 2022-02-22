using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class RollerBeetle : Mount
	{
		public RollerBeetle(SettingCollection settingCollection)
			: base(settingCollection, "Roller", "Roller Beetle", "roller", isUnderwaterMount: false, isWvWMount: false, 5)
		{
		}
	}
}
