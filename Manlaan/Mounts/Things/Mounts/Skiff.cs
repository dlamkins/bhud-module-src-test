using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Skiff : Mount
	{
		protected override MountType MountType => (MountType)9;

		public Skiff(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Skiff", "Skiff", "skiff")
		{
		}
	}
}
