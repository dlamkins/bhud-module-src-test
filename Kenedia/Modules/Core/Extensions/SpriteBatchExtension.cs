using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Structs;
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
			spriteBatch.DrawOnCtrl(ctrl, textureImage, destination, sourceRectangle, color, rotationInRadians, originOffset, seffects);
		}

		public static void DrawFrame(this SpriteBatch spriteBatch, Control ctrl, Rectangle _selectorBounds, Color borderColor, int width = 1)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left() + width, ((Rectangle)(ref _selectorBounds)).get_Top(), _selectorBounds.Width - width * 2, width), Rectangle.get_Empty(), borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left() + width, ((Rectangle)(ref _selectorBounds)).get_Bottom() - width, _selectorBounds.Width - width * 2, width), Rectangle.get_Empty(), borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left(), ((Rectangle)(ref _selectorBounds)).get_Top(), width, _selectorBounds.Height), Rectangle.get_Empty(), borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Right() - width, ((Rectangle)(ref _selectorBounds)).get_Top(), width, _selectorBounds.Height), Rectangle.get_Empty(), borderColor * 0.8f);
		}

		public static void DrawFrame(this SpriteBatch spriteBatch, Control ctrl, Rectangle _selectorBounds, Color borderColor, RectangleDimensions? borderDimensions)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			RectangleDimensions border = borderDimensions ?? new RectangleDimensions(2);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left() + border.Left, ((Rectangle)(ref _selectorBounds)).get_Top(), _selectorBounds.Width - border.Horizontal, border.Top), Rectangle.get_Empty(), borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left() + border.Left, ((Rectangle)(ref _selectorBounds)).get_Bottom() - border.Bottom, _selectorBounds.Width - border.Horizontal, border.Bottom), Rectangle.get_Empty(), borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left(), ((Rectangle)(ref _selectorBounds)).get_Top(), border.Left, _selectorBounds.Height), Rectangle.get_Empty(), borderColor * 0.8f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref _selectorBounds)).get_Right() - border.Right, ((Rectangle)(ref _selectorBounds)).get_Top(), border.Right, _selectorBounds.Height), Rectangle.get_Empty(), borderColor * 0.8f);
		}
	}
}
