using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Griffon : Mount
	{
		protected override MountType MountType => (MountType)2;

		public Griffon(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Griffon", "Griffon", "griffon")
		{
		}
	}
}
