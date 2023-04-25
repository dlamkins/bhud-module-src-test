using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts
{
	public class SiegeTurtle : Mount
	{
		public SiegeTurtle(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Turtle", "Siege Turtle", "turtle", (MountType)10, isUnderwaterMount: true, isFlyingMount: false, isWvWMount: false, 9)
		{
		}
	}
}
