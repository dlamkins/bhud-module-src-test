using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Extensions
{
	public static class SpriteBatchExtensions
	{
		public static SpriteBatchParameters Clone(this SpriteBatchParameters sbp)
		{
			return new SpriteBatchParameters
			{
				SortMode = sbp.SortMode,
				BlendState = sbp.BlendState,
				SamplerState = sbp.SamplerState,
				DepthStencilState = sbp.DepthStencilState,
				RasterizerState = sbp.RasterizerState,
				Effect = sbp.Effect,
				TransformMatrix = sbp.TransformMatrix
			};
		}

		public static void DrawFrame(this SpriteBatch spriteBatch, Control ctrl, Rectangle _selectorBounds, Color borderColor, int width = 1)
		{
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Left + width, _selectorBounds.Top, _selectorBounds.Width - width * 2, width), Rectangle.Empty, borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Left + width, _selectorBounds.Bottom - width, _selectorBounds.Width - width * 2, width), Rectangle.Empty, borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Left, _selectorBounds.Top, width, _selectorBounds.Height), Rectangle.Empty, borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(_selectorBounds.Right - width, _selectorBounds.Top, width, _selectorBounds.Height), Rectangle.Empty, borderColor * 0.8f);
		}
	}
}
