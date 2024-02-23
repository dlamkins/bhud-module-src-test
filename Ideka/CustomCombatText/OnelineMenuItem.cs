using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	public class OnelineMenuItem : MenuItem
	{
		public OnelineMenuItem(string text)
			: this(text)
		{
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			string text = base._text;
			base._text = "";
			((MenuItem)this).PaintBeforeChildren(spriteBatch, bounds);
			int num = (((Container)this)._children.get_IsEmpty() ? 10 : 26);
			if (base._canCheck || base._icon != null)
			{
				num += 42;
			}
			else if (!((Container)this)._children.get_IsEmpty())
			{
				num += 10;
			}
			base._text = text;
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, base._text, Control.get_Content().get_DefaultFont16(), new Rectangle(num, 0, ((Control)this).get_Width() - (num - 10), ((MenuItem)this).get_MenuItemHeight()), Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
