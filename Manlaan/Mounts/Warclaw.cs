using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class Warclaw : Mount
	{
		public Warclaw(SettingCollection settingCollection)
			: base(settingCollection, "Warclaw", "Warclaw", "warclaw", isUnderwaterMount: false, isWvWMount: true, 6)
		{
		}
	}
}
