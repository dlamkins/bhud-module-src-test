using Blish_HUD.Controls;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ControlExtensions
	{
		public static bool ToggleVisibility(this Control c, bool? visible = null)
		{
			c.set_Visible(visible ?? (!c.get_Visible()));
			return c.get_Visible();
		}
	}
}
