using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class HintLabel : Label
	{
		public HintLabel(string text)
			: this(null, text)
		{
		}

		public HintLabel(Container? parent, string text)
			: this()
		{
			((Label)this).set_Text(text);
			((Label)this).set_AutoSizeHeight(true);
			((Label)this).set_AutoSizeWidth(true);
			((Control)this).set_Parent(parent);
		}
	}
}
