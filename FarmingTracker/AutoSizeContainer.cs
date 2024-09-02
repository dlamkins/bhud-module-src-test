using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class AutoSizeContainer : Container
	{
		public AutoSizeContainer(Container parent)
			: this()
		{
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
		}
	}
}
