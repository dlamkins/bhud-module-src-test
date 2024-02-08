using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Extensions
{
	public static class SpriteBatchExtensions
	{
		public static void DrawFrame(this SpriteBatch spriteBatch, Control ctrl, Rectangle _selectorBounds, Color borderColor, int width = 1)
		{
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Left + width, _selectorBounds.Top, _selectorBounds.Width - width * 2, width), Rectangle.Empty, borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Left + width, _selectorBounds.Bottom - width, _selectorBounds.Width - width * 2, width), Rectangle.Empty, borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Left, _selectorBounds.Top, width, _selectorBounds.Height), Rectangle.Empty, borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Right - width, _selectorBounds.Top, width, _selectorBounds.Height), Rectangle.Empty, borderColor * 0.8f);
		}
	}
}
