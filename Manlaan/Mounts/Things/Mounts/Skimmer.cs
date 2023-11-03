using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Skimmer : Mount
	{
		protected override MountType MountType => (MountType)4;

		public Skimmer(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Skimmer", "Skimmer", "skimmer")
		{
		}
	}
}
