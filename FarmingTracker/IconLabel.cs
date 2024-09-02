using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class IconLabel : Container
	{
		public IconLabel(string text, AsyncTexture2D texture, int height, BitmapFont font, Container parent)
			: this()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Width(77);
			((Control)this).set_Parent(parent);
			Image val = new Image(texture);
			((Control)val).set_Size(new Point(height));
			((Control)val).set_Parent((Container)(object)this);
			Label val2 = new Label();
			val2.set_Text(text);
			val2.set_Font(font);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Height(height);
			((Control)val2).set_Left(height + 5);
			((Control)val2).set_Parent((Container)(object)this);
		}
	}
}
