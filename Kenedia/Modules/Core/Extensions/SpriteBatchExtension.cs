using Blish_HUD;
using Blish_HUD.Controls;
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

		public static void DrawCenteredRotationOnCtrl(this SpriteBatch spriteBatch, Control ctrl, Texture2D textureImage, Rectangle rectangleAreaToDrawAt, Rectangle sourceRectangle, Color color, float rotationInRadians, bool flipVertically, bool flipHorizontally)
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
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
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
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, textureImage, destination, (Rectangle?)sourceRectangle, color, rotationInRadians, originOffset, seffects);
		}

		public static void DrawFrame(this SpriteBatch spriteBatch, Control ctrl, Rectangle _selectorBounds, Color borderColor, int width = 1)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left(), ((Rectangle)(ref _selectorBounds)).get_Top(), _selectorBounds.Width, width), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left(), ((Rectangle)(ref _selectorBounds)).get_Bottom() - width, _selectorBounds.Width, width), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left(), ((Rectangle)(ref _selectorBounds)).get_Top(), width, _selectorBounds.Height), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Right() - width, ((Rectangle)(ref _selectorBounds)).get_Top(), width, _selectorBounds.Height), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
		}
	}
}
