using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Skyscale : Mount
	{
		protected override MountType MountType => (MountType)8;

		public Skyscale(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Skyscale", "Skyscale", "skyscale")
		{
		}

		public override bool IsUsableInCombat()
		{
			return _helper.IsCombatLaunchUnlocked();
		}
	}
}
