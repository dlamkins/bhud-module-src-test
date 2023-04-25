using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class Griffon : Mount
	{
		public Griffon(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Griffon", "Griffon", "griffon", (MountType)2, isUnderwaterMount: false, isFlyingMount: true, isWvWMount: false, 5)
		{
		}
	}
}
