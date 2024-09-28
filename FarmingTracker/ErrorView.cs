using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class ErrorView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			new HintLabel(buildPanel, "Module failed loading :(. Report in BlishHUD Discord please!");
			((Control)new OpenUrlInBrowserButton("https://discord.com/invite/FYKN3qh", "Open BlishHUD Discord in your default browser", "", null, buildPanel)).set_Location(new Point(0, 30));
		}

		public ErrorView()
			: this()
		{
		}
	}
}
