using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public static class SpriteBatchExtensions
	{
		public static void DrawRectangleOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, int lineWidth, Color color)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X - lineWidth, bounds.Y - lineWidth, bounds.Width + lineWidth, lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X - lineWidth, bounds.Y, lineWidth, bounds.Height + lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y + bounds.Height, bounds.Width + lineWidth, lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X + bounds.Width, bounds.Y - lineWidth, lineWidth, bounds.Height + lineWidth), color);
		}

		public static void DrawRectangleOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, int lineWidth)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawRectangleOnCtrl(ctrl, bounds, lineWidth, Color.get_Black());
		}
	}
}
