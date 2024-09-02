using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class FixedWidthContainer : Container
	{
		public FixedWidthContainer(Container parent)
			: this()
		{
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
		}
	}
}
