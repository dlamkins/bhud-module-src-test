using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.Utils
{
	public static class SpriteBatchUtil
	{
		public static void DrawOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D texture, RectangleF destinationRectangle, Color tint)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, texture, destinationRectangle, tint, 0f);
		}

		public static void DrawOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D texture, RectangleF destinationRectangle, Color tint, float angle)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			RectangleF rectangle = destinationRectangle.ToBounds(RectangleF.op_Implicit(control.get_AbsoluteBounds()));
			Vector2 scale = default(Vector2);
			((Vector2)(ref scale))._002Ector(rectangle.Width / (float)texture.get_Width(), rectangle.Height / (float)texture.get_Height());
			spriteBatch.Draw(texture, Point2.op_Implicit(((RectangleF)(ref rectangle)).get_Center() - ((RectangleF)(ref rectangle)).get_Size() / 2f), (Rectangle?)null, tint, angle, Vector2.get_Zero(), scale, (SpriteEffects)0, 0f);
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap = false, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(ctrl, text, font, destinationRectangle, color, wrap, stroke: false, 1, horizontalAlignment, verticalAlignment);
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance = 1, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Invalid comparison between Unknown and I4
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Invalid comparison between Unknown and I4
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Invalid comparison between Unknown and I4
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Invalid comparison between Unknown and I4
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			text = (wrap ? DrawUtil.WrapText(font, text, destinationRectangle.Width) : text);
			if ((int)horizontalAlignment != 0 && (wrap || text.Contains("\n")))
			{
				using (StringReader stringReader = new StringReader(text))
				{
					for (int i = 0; destinationRectangle.Height - (float)i > 0f; i += font.get_LineHeight())
					{
						string text2;
						if ((text2 = stringReader.ReadLine()) == null)
						{
							break;
						}
						spriteBatch.DrawStringOnCtrl(ctrl, text2, font, destinationRectangle.Add(0f, i, 0f, 0f), color, wrap, stroke, strokeDistance, horizontalAlignment, verticalAlignment);
					}
				}
				return;
			}
			Vector2 vector = Size2.op_Implicit(font.MeasureString(text));
			destinationRectangle = destinationRectangle.ToBounds(RectangleF.op_Implicit(ctrl.get_AbsoluteBounds()));
			float num = destinationRectangle.X;
			float num2 = destinationRectangle.Y;
			if ((int)horizontalAlignment != 1)
			{
				if ((int)horizontalAlignment == 2)
				{
					num += destinationRectangle.Width - vector.X;
				}
			}
			else
			{
				num += destinationRectangle.Width / 2f - vector.X / 2f;
			}
			if ((int)verticalAlignment != 1)
			{
				if ((int)verticalAlignment == 2)
				{
					num2 += destinationRectangle.Height - vector.Y;
				}
			}
			else
			{
				num2 += destinationRectangle.Height / 2f - vector.Y / 2f;
			}
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector(num, num2);
			float scale = ctrl.AbsoluteOpacity();
			if (stroke)
			{
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, 0f, (float)(-strokeDistance)), Color.get_Black() * scale, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, (float)(-strokeDistance)), Color.get_Black() * scale, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, 0f), Color.get_Black() * scale, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, (float)strokeDistance), Color.get_Black() * scale, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, 0f, (float)strokeDistance), Color.get_Black() * scale, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), (float)strokeDistance), Color.get_Black() * scale, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), 0f), Color.get_Black() * scale, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), (float)(-strokeDistance)), Color.get_Black() * scale, (Rectangle?)null);
			}
			BitmapFontExtensions.DrawString(spriteBatch, font, text, vector2, color * scale, (Rectangle?)null);
		}
	}
}
