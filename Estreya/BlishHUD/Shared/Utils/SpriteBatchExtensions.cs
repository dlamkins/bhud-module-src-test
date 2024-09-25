using System.IO;
using System.Linq;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class SpriteBatchExtensions
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(SpriteBatchExtensions));

		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, RectangleF destinationRectangle)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.Draw(texture, destinationRectangle, Color.get_White(), 0f);
		}

		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, RectangleF destinationRectangle, Color tint)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.Draw(texture, destinationRectangle, tint, 0f);
		}

		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, RectangleF destinationRectangle, Color tint, float angle)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(null, texture, destinationRectangle, tint, angle);
		}

		public static void DrawRectangle(this SpriteBatch spriteBatch, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawRectangleOnCtrl(null, baseTexture, coords, color);
		}

		public static void DrawRectangle(this SpriteBatch spriteBatch, Texture2D baseTexture, RectangleF coords, Color color, int borderSize, Color borderColor)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawRectangleOnCtrl(null, baseTexture, coords, color);
			if (borderSize > 0 && borderColor != Color.get_Transparent())
			{
				spriteBatch.DrawRectangleOnCtrl(null, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), coords.Width - (float)borderSize, (float)borderSize), borderColor);
				spriteBatch.DrawRectangleOnCtrl(null, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Right() - (float)borderSize, ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
				spriteBatch.DrawRectangleOnCtrl(null, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Bottom() - (float)borderSize, coords.Width, (float)borderSize), borderColor);
				spriteBatch.DrawRectangleOnCtrl(null, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
			}
		}

		public static void DrawString(this SpriteBatch spriteBatch, string text, SpriteFont font, RectangleF destinationRectangle, Color color, bool wrap = false, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawString(text, font, destinationRectangle, color, wrap, stroke: false, 1, horizontalAlignment, verticalAlignment);
		}

		public static void DrawString(this SpriteBatch spriteBatch, string text, SpriteFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance = 1, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(null, text, font, destinationRectangle, color, wrap, stroke, strokeDistance, horizontalAlignment, verticalAlignment);
		}

		public static void DrawString(this SpriteBatch spriteBatch, string text, SpriteFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance, Color strokeColor, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(null, text, font, destinationRectangle, color, wrap, stroke, strokeDistance, strokeColor, horizontalAlignment, verticalAlignment);
		}

		public static void DrawString(this SpriteBatch spriteBatch, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap = false, float scale = 1f, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawString(text, font, destinationRectangle, color, wrap, stroke: false, 1, scale, horizontalAlignment, verticalAlignment);
		}

		public static void DrawString(this SpriteBatch spriteBatch, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance = 1, float scale = 1f, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(null, text, font, destinationRectangle, color, wrap, stroke, strokeDistance, scale, horizontalAlignment, verticalAlignment);
		}

		public static void DrawString(this SpriteBatch spriteBatch, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance, Color strokeColor, float scale = 1f, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(null, text, font, destinationRectangle, color, wrap, stroke, strokeDistance, strokeColor, scale, horizontalAlignment, verticalAlignment);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Texture2D baseTexture, Rectangle coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawLineOnCtrl(null, baseTexture, coords, color);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawLineOnCtrl(null, baseTexture, coords, color);
		}

		public static void DrawCrossOut(this SpriteBatch spriteBatch, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawCrossOutOnCtrl(null, baseTexture, coords, color);
		}

		public static void DrawAngledLine(this SpriteBatch spriteBatch, Texture2D baseTexture, Point2 start, Point2 end, Color color, float thickness = 1f)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawAngledLineOnCtrl(null, baseTexture, start, end, color, thickness);
		}

		public static void DrawOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D texture, RectangleF destinationRectangle)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, texture, destinationRectangle, Color.get_White() * control.AbsoluteOpacity());
		}

		public static void DrawOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D texture, RectangleF destinationRectangle, Color tint)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(control, texture, destinationRectangle, tint, 0f);
		}

		public static void DrawOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D texture, RectangleF destinationRectangle, Color tint, float angle)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			RectangleF rectangle = ((control != null) ? destinationRectangle.ToBounds(RectangleF.op_Implicit(control.get_AbsoluteBounds())) : destinationRectangle);
			Vector2 scale = default(Vector2);
			((Vector2)(ref scale))._002Ector(rectangle.Width / (float)texture.get_Width(), rectangle.Height / (float)texture.get_Height());
			spriteBatch.Draw(texture, Point2.op_Implicit(((RectangleF)(ref rectangle)).get_Center() - ((RectangleF)(ref rectangle)).get_Size() / 2f), (Rectangle?)null, tint, angle, Vector2.get_Zero(), scale, (SpriteEffects)0, 0f);
		}

		public static void DrawRectangleOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (control != null)
			{
				spriteBatch.DrawOnCtrl(control, baseTexture, coords, color);
			}
			else
			{
				spriteBatch.Draw(baseTexture, coords, color);
			}
		}

		public static void DrawRectangleOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color, int borderSize, Color borderColor)
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
			spriteBatch.DrawRectangleOnCtrl(control, baseTexture, coords, color);
			if (borderSize > 0 && borderColor != Color.get_Transparent())
			{
				spriteBatch.DrawRectangleOnCtrl(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), coords.Width - (float)borderSize, (float)borderSize), borderColor);
				spriteBatch.DrawRectangleOnCtrl(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Right() - (float)borderSize, ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
				spriteBatch.DrawRectangleOnCtrl(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Bottom() - (float)borderSize, coords.Width, (float)borderSize), borderColor);
				spriteBatch.DrawRectangleOnCtrl(control, baseTexture, new RectangleF(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top(), (float)borderSize, coords.Height), borderColor);
			}
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, SpriteFont font, RectangleF destinationRectangle, Color color, bool wrap = false, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(ctrl, text, font, destinationRectangle, color, wrap, stroke: false, 1, horizontalAlignment, verticalAlignment);
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, SpriteFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance = 1, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(ctrl, text, font, destinationRectangle, color, wrap, stroke, strokeDistance, Color.get_Black(), horizontalAlignment, verticalAlignment);
		}

		private static string WrapTextSegment(SpriteFont spriteFont, string text, float maxLineWidth)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			string[] array = text.Split(' ');
			StringBuilder stringBuilder = new StringBuilder();
			float num = 0f;
			float width = spriteFont.MeasureString(" ").X;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				Vector2 vector = spriteFont.MeasureString(text2);
				if (num + vector.X < maxLineWidth)
				{
					stringBuilder.Append(text2 + " ");
					num += vector.X + width;
				}
				else
				{
					stringBuilder.Append("\n" + text2 + " ");
					num = vector.X + width;
				}
			}
			return stringBuilder.ToString();
		}

		private static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			return string.Join("\n", from s in text.Split('\n')
				select WrapTextSegment(spriteFont, s, maxLineWidth));
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, SpriteFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance, Color strokeColor, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
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
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Invalid comparison between Unknown and I4
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Invalid comparison between Unknown and I4
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Invalid comparison between Unknown and I4
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Invalid comparison between Unknown and I4
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			text = (wrap ? WrapText(font, text, destinationRectangle.Width) : text);
			if ((int)horizontalAlignment != 0 && (wrap || text.Contains("\n")))
			{
				using (StringReader stringReader = new StringReader(text))
				{
					for (int i = 0; destinationRectangle.Height - (float)i > 0f; i += font.get_LineSpacing())
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
			Vector2 vector = font.MeasureString(text);
			destinationRectangle = ((ctrl != null) ? destinationRectangle.ToBounds(RectangleF.op_Implicit(ctrl.get_AbsoluteBounds())) : destinationRectangle);
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
			float scale = ((ctrl != null) ? ctrl.AbsoluteOpacity() : 1f);
			if (stroke)
			{
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, 0f, (float)(-strokeDistance)), strokeColor * scale);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, (float)(-strokeDistance)), strokeColor * scale);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, 0f), strokeColor * scale);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, (float)strokeDistance), strokeColor * scale);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, 0f, (float)strokeDistance), strokeColor * scale);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), (float)strokeDistance), strokeColor * scale);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), 0f), strokeColor * scale);
				spriteBatch.DrawString(font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), (float)(-strokeDistance)), strokeColor * scale);
			}
			spriteBatch.DrawString(font, text, vector2, color * scale);
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap = false, float scale = 1f, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(ctrl, text, font, destinationRectangle, color, wrap, stroke: false, 1, scale, horizontalAlignment, verticalAlignment);
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance = 1, float scale = 1f, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(ctrl, text, font, destinationRectangle, color, wrap, stroke, strokeDistance, Color.get_Black(), scale, horizontalAlignment, verticalAlignment);
		}

		private static string WrapTextSegment(BitmapFont spriteFont, string text, float maxLineWidth)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			string[] array = text.Split(' ');
			StringBuilder stringBuilder = new StringBuilder();
			float num = 0f;
			float width = spriteFont.MeasureString(" ").Width;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				Vector2 vector = Size2.op_Implicit(spriteFont.MeasureString(text2));
				if (num + vector.X < maxLineWidth)
				{
					stringBuilder.Append(text2 + " ");
					num += vector.X + width;
				}
				else
				{
					stringBuilder.Append("\n" + text2 + " ");
					num = vector.X + width;
				}
			}
			return stringBuilder.ToString();
		}

		private static string WrapText(BitmapFont spriteFont, string text, float maxLineWidth)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			return string.Join("\n", from s in text.Split('\n')
				select WrapTextSegment(spriteFont, s, maxLineWidth));
		}

		public static void DrawStringOnCtrl(this SpriteBatch spriteBatch, Control ctrl, string text, BitmapFont font, RectangleF destinationRectangle, Color color, bool wrap, bool stroke, int strokeDistance, Color strokeColor, float scale = 1f, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Invalid comparison between Unknown and I4
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Invalid comparison between Unknown and I4
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Invalid comparison between Unknown and I4
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Invalid comparison between Unknown and I4
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			text = (wrap ? WrapText(font, text, destinationRectangle.Width) : text);
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
						spriteBatch.DrawStringOnCtrl(ctrl, text2, font, destinationRectangle.Add(0f, i, 0f, 0f), color, wrap, stroke, strokeDistance, scale, horizontalAlignment, verticalAlignment);
					}
				}
				return;
			}
			Vector2 vector = Size2.op_Implicit(font.MeasureString(text));
			destinationRectangle = ((ctrl != null) ? destinationRectangle.ToBounds(RectangleF.op_Implicit(ctrl.get_AbsoluteBounds())) : destinationRectangle);
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
			float opacity = ((ctrl != null) ? ctrl.AbsoluteOpacity() : 1f);
			float rotation = 0f;
			Vector2 origin = Vector2.get_Zero();
			if (stroke)
			{
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, 0f, (float)(-strokeDistance)), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, (float)(-strokeDistance)), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, 0f), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)strokeDistance, (float)strokeDistance), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, 0f, (float)strokeDistance), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), (float)strokeDistance), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), 0f), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, font, text, Vector2Extension.OffsetBy(vector2, (float)(-strokeDistance), (float)(-strokeDistance)), strokeColor * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
			}
			BitmapFontExtensions.DrawString(spriteBatch, font, text, vector2, color * opacity, rotation, origin, scale, (SpriteEffects)0, 1f, (Rectangle?)null);
		}

		public static void DrawLineOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Rectangle coords, Color color)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (control != null)
			{
				spriteBatch.DrawOnCtrl(control, baseTexture, RectangleF.op_Implicit(coords), color);
			}
			else
			{
				spriteBatch.Draw(baseTexture, RectangleF.op_Implicit(coords), color);
			}
		}

		public static void DrawLineOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (control != null)
			{
				spriteBatch.DrawOnCtrl(control, baseTexture, coords, color);
			}
			else
			{
				spriteBatch.Draw(baseTexture, coords, color);
			}
		}

		public static void DrawCrossOutOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, RectangleF coords, Color color)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			Point2 topLeft = default(Point2);
			((Point2)(ref topLeft))._002Ector(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Top());
			Point2 topRight = default(Point2);
			((Point2)(ref topRight))._002Ector(((RectangleF)(ref coords)).get_Right(), ((RectangleF)(ref coords)).get_Top());
			Point2 bottomLeft = default(Point2);
			((Point2)(ref bottomLeft))._002Ector(((RectangleF)(ref coords)).get_Left(), ((RectangleF)(ref coords)).get_Bottom() - 1.5f);
			Point2 bottomRight = default(Point2);
			((Point2)(ref bottomRight))._002Ector(((RectangleF)(ref coords)).get_Right(), ((RectangleF)(ref coords)).get_Bottom() - 1.5f);
			spriteBatch.DrawAngledLineOnCtrl(control, baseTexture, topLeft, bottomRight, color);
			spriteBatch.DrawAngledLineOnCtrl(control, baseTexture, bottomLeft, topRight, color);
		}

		public static void DrawAngledLineOnCtrl(this SpriteBatch spriteBatch, Control control, Texture2D baseTexture, Point2 start, Point2 end, Color color, float thickness = 1f)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			float length = MathHelper.CalculateDistance(start, end);
			RectangleF lineRectangle = default(RectangleF);
			((RectangleF)(ref lineRectangle))._002Ector(start.X, start.Y, length, thickness);
			float angle = MathHelper.CalculateAngle(start, end);
			if (control != null)
			{
				spriteBatch.DrawOnCtrl(control, baseTexture, lineRectangle, color, angle);
			}
			else
			{
				spriteBatch.Draw(baseTexture, lineRectangle, color, angle);
			}
		}
	}
}
