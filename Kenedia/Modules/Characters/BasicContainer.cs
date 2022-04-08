using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters
{
	public class BasicContainer : Container
	{
		private const int PADDING = 2;

		public Texture2D Texture;

		public bool showBackground = true;

		public Color FrameColor = Color.Black;

		public Color TextureColor = Color.White;

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (showBackground)
			{
				if (Texture == null)
				{
					Texture = Textures.Backgrounds[2];
				}
				spriteBatch.DrawOnCtrl(this, Texture, bounds, new Rectangle(3, 4, _size.X, _size.Y), TextureColor * 0.98f);
			}
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(1, 0, _size.X - 2, 3).Add(-2, -2, 4, 0), FrameColor * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(1, 1, _size.X - 2, 1).Add(-2, -2, 4, 0), FrameColor * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(_size.X - 3, 1, 3, _size.Y - 2).Add(2, -2, 0, 4), FrameColor * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(_size.X - 2, 1, 1, _size.Y - 2).Add(2, -2, 0, 4), FrameColor * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(1, _size.Y - 3, _size.X - 2, 3).Add(-2, 2, 4, 0), FrameColor * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(1, _size.Y - 2, _size.X - 2, 1).Add(-2, 2, 4, 0), FrameColor * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, 1, 3, _size.Y - 2).Add(-2, -2, 0, 4), FrameColor * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(1, 1, 1, _size.Y - 2).Add(-2, -2, 0, 4), FrameColor * 0.6f);
		}
	}
}
