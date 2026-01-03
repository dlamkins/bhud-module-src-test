using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoreBridge
{
	public static class SpriteBatchExtensions
	{
		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, SpriteFontBase font, Rectangle destinationRectangle, Color color, bool wrap = false, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(ctrl, text, font, destinationRectangle, color, wrap, stroke: false, 1, horizontalAlignment, verticalAlignment);
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, SpriteFontBase font, Rectangle destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance = 1, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Invalid comparison between Unknown and I4
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Invalid comparison between Unknown and I4
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Invalid comparison between Unknown and I4
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Invalid comparison between Unknown and I4
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			text = (wrap ? DrawUtilCustom.WrapText(font, text, destinationRectangle.Width) : text);
			if ((int)horizontalAlignment != 0 && text.Contains("\n"))
			{
				using (StringReader stringReader = new StringReader(text))
				{
					int lineCount = 0;
					for (string text2 = stringReader.ReadLine(); text2 != null; text2 = stringReader.ReadLine())
					{
						spriteBatch.DrawStringOnCtrl(ctrl, text2, font, RectangleExtension.Add(destinationRectangle, 0, lineCount * font.FontSize, 0, 0), color, wrap, stroke, strokeDistance, horizontalAlignment, verticalAlignment);
						lineCount++;
					}
				}
				return;
			}
			Vector2 stringSize = font.MeasureString(text);
			float height = Math.Max(font.FontSize, stringSize.Y);
			Vector2 vector2_1 = default(Vector2);
			((Vector2)(ref vector2_1))._002Ector(stringSize.X, height);
			destinationRectangle = RectangleExtension.ToBounds(destinationRectangle, ctrl.get_AbsoluteBounds());
			int x = destinationRectangle.X;
			int y = destinationRectangle.Y;
			if ((int)horizontalAlignment != 1)
			{
				if ((int)horizontalAlignment == 2)
				{
					x += destinationRectangle.Width - (int)vector2_1.X;
				}
			}
			else
			{
				x += destinationRectangle.Width / 2 - (int)vector2_1.X / 2;
			}
			if ((int)verticalAlignment != 1)
			{
				if ((int)verticalAlignment == 2)
				{
					y += destinationRectangle.Height - (int)vector2_1.Y;
				}
			}
			else
			{
				y += destinationRectangle.Height / 2 - (int)vector2_1.Y / 2;
			}
			Vector2 vector2_2 = default(Vector2);
			((Vector2)(ref vector2_2))._002Ector((float)x, (float)y);
			float num = ctrl.AbsoluteOpacity();
			if (stroke)
			{
				Color color2 = Color.get_Black() * num;
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, 0f, (float)(-strokeDistance)), color2);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, (float)strokeDistance, (float)(-strokeDistance)), color2);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, (float)strokeDistance, 0f), color2);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, (float)strokeDistance, (float)strokeDistance), color2);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, 0f, (float)strokeDistance), color2);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, (float)(-strokeDistance), (float)strokeDistance), color2);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, (float)(-strokeDistance), 0f), color2);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2_2, (float)(-strokeDistance), (float)(-strokeDistance)), color2);
			}
			spriteBatch.DrawString(font, text, vector2_2, color * num);
		}
	}
}
