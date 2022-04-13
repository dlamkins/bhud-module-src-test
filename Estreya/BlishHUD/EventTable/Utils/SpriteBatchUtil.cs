using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.Utils
{
	public static class SpriteBatchUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(SpriteBatchUtil));

		public static void DrawOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D texture, RectangleF destinationRectangle)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, texture, destinationRectangle, Color.get_White() * control.AbsoluteOpacity(), 0f);
		}

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
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Invalid comparison between Unknown and I4
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Invalid comparison between Unknown and I4
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Invalid comparison between Unknown and I4
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Invalid comparison between Unknown and I4
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
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

		public static void DrawRectangle(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, baseTexture, coords, color);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Rectangle coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, baseTexture, RectangleF.op_Implicit(coords), color);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, baseTexture, coords, color);
		}

		public static void DrawCrossOut(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			Point2 topLeft = default(Point2);
			((Point2)(ref topLeft))._002Ector(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top());
			Point2 topRight = default(Point2);
			((Point2)(ref topRight))._002Ector(((RectangleF)(ref coords)).get_Right(), ((RectangleF)(ref coords)).get_Top());
			Point2 bottomLeft = default(Point2);
			((Point2)(ref bottomLeft))._002Ector(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Bottom() - 1.5f);
			Point2 bottomRight = default(Point2);
			((Point2)(ref bottomRight))._002Ector(((RectangleF)(ref coords)).get_Right(), ((RectangleF)(ref coords)).get_Bottom() - 1.5f);
			spriteBatch.DrawAngledLine(control, baseTexture, topLeft, bottomRight, color);
			spriteBatch.DrawAngledLine(control, baseTexture, bottomLeft, topRight, color);
		}

		public static void DrawAngledLine(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Point2 start, Point2 end, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			float length = MathHelper.CalculeDistance(start, end);
			RectangleF lineRectangle = default(RectangleF);
			((RectangleF)(ref lineRectangle))._002Ector(start.X, start.Y, length, 1f);
			float angle = MathHelper.CalculeAngle(start, end);
			spriteBatch.DrawOnCtrl(control, baseTexture, lineRectangle, color, angle);
		}

		public static void DrawRectangle(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color, int borderSize, Color borderColor)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawRectangle(control, baseTexture, coords, color);
			if (borderSize > 0 && borderColor != Color.get_Transparent())
			{
				spriteBatch.DrawRectangle(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), coords.Width - (float)borderSize, (float)borderSize), borderColor);
				spriteBatch.DrawRectangle(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Right() - (float)borderSize, ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
				spriteBatch.DrawRectangle(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Bottom() - (float)borderSize, coords.Width, (float)borderSize), borderColor);
				spriteBatch.DrawRectangle(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
			}
		}
	}
}
