using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class Skyscale : Mount
	{
		public Skyscale(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Skyscale", "Skyscale", "skyscale", (MountType)8, isUnderwaterMount: false, isFlyingMount: true, isWvWMount: false, 8)
		{
		}
	}
}
