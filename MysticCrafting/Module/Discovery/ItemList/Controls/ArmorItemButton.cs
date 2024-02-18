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
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, base.Type, Control.Content.DefaultFont16, new Rectangle(_size.Y + 350, 0, _size.X - _size.Y - 35, base.Height), Color.White, wrap: true, stroke: true);
		}
	}
}
