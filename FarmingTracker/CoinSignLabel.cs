using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class CoinSignLabel : Label
	{
		public CoinSignLabel(Tooltip? tooltip, BitmapFont font, Container parent)
			: this()
		{
			((Label)this).set_Text(" ");
			((Label)this).set_Font(font);
			((Control)this).set_Tooltip(tooltip);
			((Label)this).set_AutoSizeHeight(true);
			((Label)this).set_AutoSizeWidth(true);
			((Control)this).set_Parent(parent);
		}

		internal void SetSign(long sign)
		{
			((Label)this).set_Text((sign == -1) ? "-" : " ");
		}
	}
}
