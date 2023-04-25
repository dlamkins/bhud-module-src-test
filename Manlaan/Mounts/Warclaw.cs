using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class Warclaw : Mount
	{
		public Warclaw(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Warclaw", "Warclaw", "warclaw", (MountType)7, isUnderwaterMount: false, isFlyingMount: false, isWvWMount: true, 7)
		{
		}
	}
}
