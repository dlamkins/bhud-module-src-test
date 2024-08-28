using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Warclaw : Mount
	{
		protected override MountType MountType => (MountType)7;

		public Warclaw(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Warclaw", "Warclaw", "warclaw")
		{
		}

		public override bool IsUsableInCombat()
		{
			return true;
		}
	}
}
