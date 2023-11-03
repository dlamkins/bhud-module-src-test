using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public class SiegeTurtle : Mount
	{
		protected override MountType MountType => (MountType)10;

		public SiegeTurtle(SettingCollection settingCollection, Helper helper)
			: base(settingCollection, helper, "Turtle", "Siege Turtle", "turtle")
		{
		}
	}
}
