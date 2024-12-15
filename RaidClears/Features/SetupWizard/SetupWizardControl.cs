using Blish_HUD;
using Blish_HUD.Controls;

namespace RaidClears.Features.SetupWizard
{
	public class SetupWizardControl : Container
	{
		private SetupWizardControl()
			: this()
		{
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		}
	}
}
