using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class BorderContainer : Container
	{
		public BorderContainer(Point location, Point size, Color borderColor, Tooltip tooltip, Container parent)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(location);
			((Control)this).set_Size(size);
			((Control)this).set_BackgroundColor(borderColor);
			((Control)this).set_Tooltip(tooltip);
			((Control)this).set_Parent(parent);
		}
	}
}
