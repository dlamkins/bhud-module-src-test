using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace views
{
	internal class InventoryItem : Control
	{
		private AsyncTexture2D border;

		public AsyncTexture2D Texture = new AsyncTexture2D();

		public InventoryItem(Texture2D border_)
		{
			border = border_;
		}

		protected override void Paint(SpriteBatch spriteBatch_, Rectangle bounds_)
		{
			spriteBatch_.DrawOnCtrl(this, Texture, bounds_);
			spriteBatch_.DrawOnCtrl(this, border, bounds_);
		}
	}
}
