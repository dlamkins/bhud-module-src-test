using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class Springer : Mount
	{
		protected override MountType MountType => (MountType)3;

		public Springer(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Springer", "Springer", "springer")
		{
		}
	}
}
