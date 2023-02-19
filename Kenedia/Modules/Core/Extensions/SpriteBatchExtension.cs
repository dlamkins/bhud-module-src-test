using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Extensions
{
	public static class SpriteBatchExtension
	{
		public static void DrawRectangleCenteredRotation(this SpriteBatch spriteBatch, Texture2D textureImage, Rectangle rectangleAreaToDrawAt, Color color, float rotationInRadians, bool flipVertically, bool flipHorizontally)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			SpriteEffects seffects = (SpriteEffects)0;
			if (flipHorizontally)
			{
				seffects = (SpriteEffects)(seffects | 1);
			}
			if (flipVertically)
			{
				seffects = (SpriteEffects)(seffects | 2);
			}
			Rectangle destination = default(Rectangle);
			((Rectangle)(ref destination))._002Ector(rectangleAreaToDrawAt.X + rectangleAreaToDrawAt.Width / 2, rectangleAreaToDrawAt.Y + rectangleAreaToDrawAt.Height / 2, rectangleAreaToDrawAt.Width, rectangleAreaToDrawAt.Height);
			Vector2 originOffset = default(Vector2);
			((Vector2)(ref originOffset))._002Ector((float)(textureImage.get_Width() / 2), (float)(textureImage.get_Height() / 2));
			spriteBatch.Draw(textureImage, destination, (Rectangle?)new Rectangle(0, 0, textureImage.get_Width(), textureImage.get_Height()), color, rotationInRadians, originOffset, seffects, 0f);
		}
	}
}
