using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class FixedWidthHintLabel : Label
	{
		public FixedWidthHintLabel(Container parent, int width, string text)
			: this()
		{
			((Label)this).set_Text(text);
			((Label)this).set_WrapText(true);
			((Control)this).set_Width(width);
			((Label)this).set_AutoSizeHeight(true);
			((Control)this).set_Parent(parent);
		}
	}
}
