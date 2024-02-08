using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	public class ImageButton : Control
	{
		public AsyncTexture2D Texture { get; set; }

		public AsyncTexture2D HoverTexture { get; set; }

		public AsyncTexture2D Icon { get; set; }

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (base.MouseOver && HoverTexture != null)
			{
				spriteBatch.DrawOnCtrl(this, HoverTexture, new Rectangle((int)base.Padding.Left, (int)base.Padding.Top, base.Size.X, base.Size.Y), Color.White);
			}
			else if (Texture != null)
			{
				spriteBatch.DrawOnCtrl(this, Texture, new Rectangle((int)base.Padding.Left, (int)base.Padding.Top, base.Size.X, base.Size.Y), Color.White);
			}
		}
	}
}
