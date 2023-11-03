using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class RollerBeetle : Mount
	{
		protected override MountType MountType => (MountType)6;

		public RollerBeetle(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Roller", "Roller Beetle", "roller")
		{
		}
	}
}
