using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class Skimmer : Mount
	{
		public Skimmer(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Skimmer", "Skimmer", "skimmer", (MountType)4, isUnderwaterMount: true, isFlyingMount: false, isWvWMount: false, 3)
		{
		}
	}
}
