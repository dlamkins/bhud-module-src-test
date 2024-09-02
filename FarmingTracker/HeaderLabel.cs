using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class HeaderLabel : Label
	{
		public HeaderLabel(Container parent, string text, BitmapFont font)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			((Label)this).set_Text(text);
			((Label)this).set_TextColor(Color.get_DeepSkyBlue());
			((Label)this).set_Font(font);
			((Label)this).set_AutoSizeHeight(true);
			((Label)this).set_AutoSizeWidth(true);
			((Control)this).set_Parent(parent);
		}
	}
}
