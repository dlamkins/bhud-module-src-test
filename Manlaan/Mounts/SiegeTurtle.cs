using Blish_HUD.Settings;

namespace Manlaan.Mounts
{
	public class SiegeTurtle : Mount
	{
		public SiegeTurtle(SettingCollection settingCollection)
			: base(settingCollection, "Turtle", "Siege Turtle", "turtle", isUnderwaterMount: true, isWvWMount: false, 8)
		{
		}
	}
}
