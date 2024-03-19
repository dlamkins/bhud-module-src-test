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
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y, bounds.Width, lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y, lineWidth, bounds.Height), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y + bounds.Height - lineWidth, bounds.Width, lineWidth), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(bounds.X + bounds.Width - lineWidth, bounds.Y, lineWidth, bounds.Height), color);
		}

		public static void DrawRectangleOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, int lineWidth)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawRectangleOnCtrl(ctrl, bounds, lineWidth, Color.get_Black());
		}
	}
}
