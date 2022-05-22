using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Warclaw : Mount
	{
		public Warclaw(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Warclaw", "Warclaw", "warclaw", isUnderwaterMount: false, isWvWMount: true, 7)
		{
		}
	}
}
