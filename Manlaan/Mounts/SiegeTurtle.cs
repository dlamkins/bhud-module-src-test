using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class SiegeTurtle : Mount
	{
		public SiegeTurtle(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Turtle", "Siege Turtle", "turtle", isUnderwaterMount: true, isWvWMount: false, 9)
		{
		}
	}
}
