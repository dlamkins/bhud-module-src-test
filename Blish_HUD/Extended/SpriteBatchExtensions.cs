using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public static class SpriteBatchExtensions
	{
		public static void DrawBorderOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, Color color, int lineWidth)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			if (lineWidth > 0 && bounds.Width > 0 && bounds.Height > 0)
			{
				lineWidth = Math.Min(lineWidth, Math.Min(bounds.Width / 2, bounds.Height / 2));
				Rectangle localBounds = RectangleExtension.ToBounds(bounds, ctrl.get_AbsoluteBounds());
				Color col = color * ctrl.AbsoluteOpacity();
				spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(localBounds.X, localBounds.Y, localBounds.Width, lineWidth), col);
				spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(localBounds.X, ((Rectangle)(ref localBounds)).get_Bottom() - lineWidth, localBounds.Width, lineWidth), col);
				int innerHeight = localBounds.Height - lineWidth * 2;
				if (innerHeight > 0)
				{
					spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(localBounds.X, localBounds.Y + lineWidth, lineWidth, innerHeight), col);
					spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(((Rectangle)(ref localBounds)).get_Right() - lineWidth, localBounds.Y + lineWidth, lineWidth, innerHeight), col);
				}
			}
		}

		public static void DrawBorderOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, int lineWidth = 2)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawBorderOnCtrl(ctrl, bounds, Color.get_Black(), lineWidth);
		}
	}
}
