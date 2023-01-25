using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public static class SpriteBatchExtensions
	{
		public static void DrawRectangleOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, int lineWidth, Color color)
		{
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X - lineWidth, bounds.Y - lineWidth, bounds.Width + lineWidth, lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X - lineWidth, bounds.Y, lineWidth, bounds.Height + lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y + bounds.Height, bounds.Width + lineWidth, lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X + bounds.Width, bounds.Y - lineWidth, lineWidth, bounds.Height + lineWidth), color);
		}

		public static void DrawRectangleOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, int lineWidth)
		{
			spriteBatch.DrawRectangleOnCtrl(ctrl, bounds, lineWidth, Color.Black);
		}
	}
}
