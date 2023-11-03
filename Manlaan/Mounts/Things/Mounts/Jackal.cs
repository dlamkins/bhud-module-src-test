using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Jackal : Mount
	{
		protected override MountType MountType => (MountType)1;

		public Jackal(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Jackal", "Jackal", "jackal")
		{
		}
	}
}
