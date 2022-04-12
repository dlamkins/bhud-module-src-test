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
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Texture, bounds, (Rectangle?)new Rectangle(3, 4, ((Control)this)._size.X, ((Control)this)._size.Y), TextureColor * 0.98f);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(1, 0, ((Control)this)._size.X - 2, 3), -2, -2, 4, 0), FrameColor * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(1, 1, ((Control)this)._size.X - 2, 1), -2, -2, 4, 0), FrameColor * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(((Control)this)._size.X - 3, 1, 3, ((Control)this)._size.Y - 2), 2, -2, 0, 4), FrameColor * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(((Control)this)._size.X - 2, 1, 1, ((Control)this)._size.Y - 2), 2, -2, 0, 4), FrameColor * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(1, ((Control)this)._size.Y - 3, ((Control)this)._size.X - 2, 3), -2, 2, 4, 0), FrameColor * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(1, ((Control)this)._size.Y - 2, ((Control)this)._size.X - 2, 1), -2, 2, 4, 0), FrameColor * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(0, 1, 3, ((Control)this)._size.Y - 2), -2, -2, 0, 4), FrameColor * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(new Rectangle(1, 1, 1, ((Control)this)._size.Y - 2), -2, -2, 0, 4), FrameColor * 0.6f);
		}

		public BasicContainer()
			: this()
		{
		}
	}
}
