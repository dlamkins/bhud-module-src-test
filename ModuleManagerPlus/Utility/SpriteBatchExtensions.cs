using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace ModuleManagerPlus.Utility
{
	internal static class SpriteBatchExtensions
	{
		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, BitmapFont font, Rectangle destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Rectangle? clippingRectangle)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Invalid comparison between Unknown and I4
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Invalid comparison between Unknown and I4
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Invalid comparison between Unknown and I4
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Invalid comparison between Unknown and I4
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			text = (wrap ? DrawUtil.WrapText(font, text, (float)destinationRectangle.Width) : text);
			if ((int)horizontalAlignment != 0 && (wrap || text.Contains("\n")))
			{
				using (StringReader reader = new StringReader(text))
				{
					for (int lineHeightDiff = 0; destinationRectangle.Height - lineHeightDiff > 0; lineHeightDiff += font.get_LineHeight())
					{
						string line;
						if ((line = reader.ReadLine()) == null)
						{
							break;
						}
						spriteBatch.DrawStringOnCtrl(ctrl, line, font, RectangleExtension.Add(destinationRectangle, 0, lineHeightDiff, 0, 0), color, wrap, stroke, strokeDistance, horizontalAlignment, verticalAlignment, clippingRectangle);
					}
				}
				return;
			}
			Vector2 textSize = Size2.op_Implicit(font.MeasureString(text));
			clippingRectangle = (clippingRectangle.HasValue ? new Rectangle?(RectangleExtension.ToBounds(clippingRectangle.GetValueOrDefault(), ctrl.get_AbsoluteBounds())) : null);
			destinationRectangle = RectangleExtension.ToBounds(destinationRectangle, ctrl.get_AbsoluteBounds());
			int xPos = destinationRectangle.X;
			int yPos = destinationRectangle.Y;
			if ((int)horizontalAlignment != 1)
			{
				if ((int)horizontalAlignment == 2)
				{
					xPos += destinationRectangle.Width - (int)textSize.X;
				}
			}
			else
			{
				xPos += destinationRectangle.Width / 2 - (int)textSize.X / 2;
			}
			if ((int)verticalAlignment != 1)
			{
				if ((int)verticalAlignment == 2)
				{
					yPos += destinationRectangle.Height - (int)textSize.Y;
				}
			}
			else
			{
				yPos += destinationRectangle.Height / 2 - (int)textSize.Y / 2;
			}
			Vector2 textPos = default(Vector2);
			((Vector2)(ref textPos))._002Ector((float)xPos, (float)yPos);
			float absoluteOpacity = ctrl.AbsoluteOpacity();
			if (stroke)
			{
				Color strokePreMultiplied = Color.get_Black() * absoluteOpacity;
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, 0f, (float)(-strokeDistance)), strokePreMultiplied, clippingRectangle);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, (float)strokeDistance, (float)(-strokeDistance)), strokePreMultiplied, clippingRectangle);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, (float)strokeDistance, 0f), strokePreMultiplied, clippingRectangle);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, (float)strokeDistance, (float)strokeDistance), strokePreMultiplied, clippingRectangle);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, 0f, (float)strokeDistance), strokePreMultiplied, clippingRectangle);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, (float)(-strokeDistance), (float)strokeDistance), strokePreMultiplied, clippingRectangle);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, (float)(-strokeDistance), 0f), strokePreMultiplied, clippingRectangle);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(textPos, (float)(-strokeDistance), (float)(-strokeDistance)), strokePreMultiplied, clippingRectangle);
			}
			BitmapFontExtensions.DrawString(spriteBatch, font, text, textPos, color * absoluteOpacity, clippingRectangle);
		}
	}
}
