using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class Springer : Mount
	{
		public Springer(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Springer", "Springer", "springer", (MountType)3, isUnderwaterMount: false, isFlyingMount: false, isWvWMount: false, 2)
		{
		}
	}
}
