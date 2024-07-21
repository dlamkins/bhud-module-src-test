using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Discovery.ItemList.Controls
{
	public class ArmorItemButton : ItemButton
	{
		public string ArmorType { get; set; }

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, base.Type, Control.get_Content().get_DefaultFont16(), new Rectangle(((Control)this)._size.Y + 350, 0, ((Control)this)._size.X - ((Control)this)._size.Y - 35, ((Control)this).get_Height()), Color.get_White(), true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
