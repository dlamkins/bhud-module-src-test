using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Raptor : Mount
	{
		protected override MountType MountType => (MountType)5;

		public Raptor(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Raptor", "Raptor", "raptor")
		{
		}
	}
}
