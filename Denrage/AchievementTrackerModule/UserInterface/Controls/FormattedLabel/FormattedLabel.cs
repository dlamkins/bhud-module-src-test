using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel
{
	public class FormattedLabel : Control
	{
		private class RectangleWrapper
		{
			public Rectangle Rectangle { get; set; }

			public int X
			{
				get
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					return Rectangle.X;
				}
				set
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					Rectangle rectangle = Rectangle;
					rectangle.X = value;
					Rectangle = rectangle;
				}
			}

			public int Y
			{
				get
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					return Rectangle.Y;
				}
				set
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					Rectangle rectangle = Rectangle;
					rectangle.Y = value;
					Rectangle = rectangle;
				}
			}

			public int Width
			{
				get
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					return Rectangle.Width;
				}
				set
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					Rectangle rectangle = Rectangle;
					rectangle.Width = value;
					Rectangle = rectangle;
				}
			}

			public int Height
			{
				get
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					return Rectangle.Height;
				}
				set
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					Rectangle rectangle = Rectangle;
					rectangle.Height = value;
					Rectangle = rectangle;
				}
			}

			public RectangleWrapper(Rectangle rectangle)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				Rectangle = rectangle;
			}
		}

		private readonly List<(RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw)> _rectangles = new List<(RectangleWrapper, FormattedLabelPart, object)>();

		private readonly IEnumerable<FormattedLabelPart> _parts;

		private readonly bool _wrapText;

		private readonly bool _autoSizeWidth;

		private readonly bool _autoSizeHeight;

		private readonly HorizontalAlignment _horizontalAlignment;

		private readonly VerticalAlignment _verticalAlignment;

		private FormattedLabelPart _hoveredTextPart;

		internal FormattedLabel(IEnumerable<FormattedLabelPart> parts, bool wrapText, bool autoSizeWidth, bool autoSizeHeight, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
			: this()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			_parts = parts;
			_wrapText = wrapText;
			_autoSizeWidth = autoSizeWidth;
			_autoSizeHeight = autoSizeHeight;
			_horizontalAlignment = horizontalAlignment;
			_verticalAlignment = verticalAlignment;
		}

		public override void RecalculateLayout()
		{
			InitializeRectangles();
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			if (_hoveredTextPart != null)
			{
				_hoveredTextPart.Link?.Invoke();
			}
		}

		private Rectangle HandleFirstTextPart(FormattedLabelPart item, string firstText)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			Size2 textSize = item.Font.MeasureString(firstText);
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(0, 0, (int)Math.Ceiling(textSize.Width), (int)Math.Ceiling(textSize.Height));
			if (_rectangles.Count > 0)
			{
				(RectangleWrapper, FormattedLabelPart, object) lastRectangle = _rectangles[_rectangles.Count - 1];
				rectangle.X = lastRectangle.Item1.X + lastRectangle.Item1.Width;
				rectangle.Y = lastRectangle.Item1.Y;
			}
			return rectangle;
		}

		private Rectangle HandleMultiLineText(FormattedLabelPart item, string text)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			Size2 textSize = item.Font.MeasureString(text);
			IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)> possibleLastYRectangles = (from x in _rectangles
				orderby x.Rectangle.Y descending
				group x by x.Rectangle.Y).First();
			(RectangleWrapper, FormattedLabelPart, object) lastYRectangle = possibleLastYRectangles.FirstOrDefault<(RectangleWrapper, FormattedLabelPart, object)>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Height != 0);
			var (rectangleWrapper, formattedLabelPart, obj) = lastYRectangle;
			if (rectangleWrapper == null && formattedLabelPart == null && obj == null)
			{
				lastYRectangle = possibleLastYRectangles.First();
			}
			return new Rectangle(0, lastYRectangle.Item1.Y + lastYRectangle.Item1.Height, (int)Math.Ceiling(textSize.Width), (int)Math.Ceiling(textSize.Height));
		}

		private void InitializeRectangles()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Width() == 0)
			{
				return;
			}
			_rectangles.Clear();
			Rectangle imageRectangle = default(Rectangle);
			Rectangle imageRectangle2 = default(Rectangle);
			foreach (FormattedLabelPart item in _parts)
			{
				if (item.PrefixImage != null)
				{
					((Rectangle)(ref imageRectangle))._002Ector(0, 0, item.PrefixImageSize.X, item.PrefixImageSize.Y);
					if (_rectangles.Count > 0)
					{
						(RectangleWrapper, FormattedLabelPart, object) lastRectangle2 = _rectangles[_rectangles.Count - 1];
						imageRectangle.X = lastRectangle2.Item1.X + lastRectangle2.Item1.Width;
						imageRectangle.Y = lastRectangle2.Item1.Y;
					}
					if (_wrapText && imageRectangle.X + imageRectangle.Width > ((Control)this).get_Width())
					{
						IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)> possibleLastYRectangles2 = (from x in _rectangles
							orderby x.Rectangle.Y descending
							group x by x.Rectangle.Y).First();
						(RectangleWrapper, FormattedLabelPart, object) lastYRectangle2 = possibleLastYRectangles2.FirstOrDefault<(RectangleWrapper, FormattedLabelPart, object)>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Height != 0);
						var (rectangleWrapper, formattedLabelPart, obj) = lastYRectangle2;
						if (rectangleWrapper == null && formattedLabelPart == null && obj == null)
						{
							lastYRectangle2 = possibleLastYRectangles2.First();
						}
						imageRectangle.X = 0;
						imageRectangle.Y = lastYRectangle2.Item1.Y + lastYRectangle2.Item1.Height;
					}
					_rectangles.Add((new RectangleWrapper(imageRectangle), item, item.PrefixImage));
				}
				List<string> splittedText = item.Text.Split(new string[1] { "\n" }, StringSplitOptions.None).ToList();
				string firstText = splittedText[0];
				Rectangle rectangle = HandleFirstTextPart(item, firstText);
				bool wrapped = false;
				if (_wrapText && rectangle.X + rectangle.Width > ((Control)this).get_Width())
				{
					List<string> tempSplittedText = DrawUtil.WrapText(item.Font, firstText, (float)(((Control)this).get_Width() - rectangle.X)).Split(new string[1] { "\n" }, StringSplitOptions.None).ToList();
					splittedText = new string[1] { string.Join("", tempSplittedText.Skip(1)) }.Concat(splittedText.Skip(1)).ToList();
					firstText = tempSplittedText[0];
					rectangle = HandleFirstTextPart(item, firstText);
					wrapped = true;
				}
				_rectangles.Add((new RectangleWrapper(rectangle), item, firstText));
				for (int i = ((!wrapped) ? 1 : 0); i < splittedText.Count; i++)
				{
					rectangle = HandleMultiLineText(item, splittedText[i]);
					if (_wrapText && rectangle.X + rectangle.Width > ((Control)this).get_Width())
					{
						splittedText.InsertRange(i + 1, DrawUtil.WrapText(item.Font, splittedText[i], (float)(((Control)this).get_Width() - rectangle.X)).Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries));
						splittedText.RemoveAt(i);
						Rectangle newRectangle = HandleMultiLineText(item, splittedText[i]);
						if (newRectangle == rectangle)
						{
							return;
						}
						rectangle = newRectangle;
					}
					_rectangles.Add((new RectangleWrapper(rectangle), item, splittedText[i]));
				}
				if (item.SuffixImage == null)
				{
					continue;
				}
				((Rectangle)(ref imageRectangle2))._002Ector(0, 0, item.SuffixImageSize.X, item.SuffixImageSize.Y);
				if (_rectangles.Count > 0)
				{
					(RectangleWrapper, FormattedLabelPart, object) lastRectangle = _rectangles[_rectangles.Count - 1];
					imageRectangle2.X = lastRectangle.Item1.X + lastRectangle.Item1.Width;
					imageRectangle2.Y = lastRectangle.Item1.Y;
				}
				if (_wrapText && imageRectangle2.X + imageRectangle2.Width > ((Control)this).get_Width())
				{
					IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)> possibleLastYRectangles = (from x in _rectangles
						orderby x.Rectangle.Y descending
						group x by x.Rectangle.Y).First();
					(RectangleWrapper, FormattedLabelPart, object) lastYRectangle = possibleLastYRectangles.FirstOrDefault<(RectangleWrapper, FormattedLabelPart, object)>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Height != 0);
					var (rectangleWrapper, formattedLabelPart, obj) = lastYRectangle;
					if (rectangleWrapper == null && formattedLabelPart == null && obj == null)
					{
						lastYRectangle = possibleLastYRectangles.First();
					}
					imageRectangle2.X = 0;
					imageRectangle2.Y = lastYRectangle.Item1.Y + lastYRectangle.Item1.Height;
				}
				_rectangles.Add((new RectangleWrapper(imageRectangle2), item, item.SuffixImage));
			}
			if (_autoSizeWidth)
			{
				((Control)this).set_Width((from x in _rectangles
					group x by x.Rectangle.Y into x
					select x.Select(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) y) => y.Rectangle.Width).Sum()).Max());
			}
			if (_autoSizeHeight)
			{
				((Control)this).set_Height((from x in _rectangles
					group x by x.Rectangle.Y into x
					select x.Max(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) y) => y.Rectangle.Height)).Sum());
			}
			HandleHorizontalAlignment();
			HandleVerticalAlignment();
			HandleFontSizeDifferences();
		}

		private void HandleFontSizeDifferences()
		{
			IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)>[] array = (from x in _rectangles
				group x by x.Rectangle.Y).ToArray();
			foreach (IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)> obj in array)
			{
				(RectangleWrapper, FormattedLabelPart, object) maxHeightInRowRectangle = obj.OrderByDescending<(RectangleWrapper, FormattedLabelPart, object), int>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Height).First();
				foreach (var rectangle in obj)
				{
					int offset = maxHeightInRowRectangle.Item1.Height - rectangle.Item1.Height;
					rectangle.Item1.Y += (int)Math.Floor((double)offset / 2.0);
				}
			}
		}

		private void HandleHorizontalAlignment()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Invalid comparison between Unknown and I4
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Invalid comparison between Unknown and I4
			if ((int)_horizontalAlignment == 0)
			{
				return;
			}
			foreach (IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)> item in from x in _rectangles
				group x by x.Rectangle.Y)
			{
				if ((int)_horizontalAlignment == 1)
				{
					int combinedWidth = item.Sum<(RectangleWrapper, FormattedLabelPart, object)>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Width);
					int nextRectangleX2 = ((Control)this).get_Width() / 2 - combinedWidth / 2;
					foreach (var rectangle in item)
					{
						rectangle.Item1.X = nextRectangleX2;
						nextRectangleX2 = rectangle.Item1.X + rectangle.Item1.Width;
					}
				}
				else
				{
					if ((int)_horizontalAlignment != 2)
					{
						continue;
					}
					(RectangleWrapper, FormattedLabelPart, object)[] reversedOrder = item.Reverse().ToArray();
					int nextRectangleX = ((Control)this).get_Width() - reversedOrder.First().Item1.Width;
					for (int i = 0; i < reversedOrder.Length; i++)
					{
						reversedOrder[i].Item1.X = nextRectangleX;
						if (i != reversedOrder.Length - 1)
						{
							nextRectangleX = reversedOrder[i].Item1.X - reversedOrder[i + 1].Item1.Width;
						}
					}
				}
			}
		}

		private void HandleVerticalAlignment()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Invalid comparison between Unknown and I4
			if ((int)_verticalAlignment == 1)
			{
				IGrouping<int, (RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw)>[] array = (from x in _rectangles
					group x by x.Rectangle.Y).ToArray();
				int combinedHeight = array.Select((IGrouping<int, (RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw)> x) => x.OrderByDescending(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) y) => y.Rectangle.Height).First().Rectangle.Height).Sum();
				int nextRectangleY2 = ((Control)this).get_Height() / 2 - combinedHeight / 2;
				IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)>[] array2 = array;
				foreach (IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)> item in array2)
				{
					foreach (var item2 in item)
					{
						item2.Item1.Y = nextRectangleY2;
					}
					int maxHeightInRow2 = item.Max<(RectangleWrapper, FormattedLabelPart, object)>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Height);
					nextRectangleY2 += maxHeightInRow2;
				}
			}
			else
			{
				if ((int)_verticalAlignment != 2)
				{
					return;
				}
				IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)>[] yGroups = (from x in _rectangles
					group x by x.Rectangle.Y).Reverse().ToArray();
				int nextRectangleY = ((Control)this).get_Height() - yGroups.First().Max<(RectangleWrapper, FormattedLabelPart, object)>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Height);
				IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)>[] array2 = yGroups;
				foreach (IGrouping<int, (RectangleWrapper, FormattedLabelPart, object)> obj in array2)
				{
					int maxHeightInRow = obj.Max<(RectangleWrapper, FormattedLabelPart, object)>(((RectangleWrapper Rectangle, FormattedLabelPart Text, object ToDraw) x) => x.Rectangle.Height);
					foreach (var item3 in obj)
					{
						item3.Item1.Y = nextRectangleY;
					}
					nextRectangleY -= maxHeightInRow;
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			bool hoverSet = false;
			foreach (var rectangle in _rectangles)
			{
				Rectangle destinationRectangle = RectangleExtension.ToBounds(rectangle.Rectangle.Rectangle, ((Control)this).get_AbsoluteBounds());
				Point mousePosition = GameService.Input.get_Mouse().get_Position();
				if (rectangle.Text.Link != null && mousePosition.X > destinationRectangle.X && mousePosition.X < destinationRectangle.X + destinationRectangle.Width && mousePosition.Y > destinationRectangle.Y && mousePosition.Y < destinationRectangle.Y + destinationRectangle.Height)
				{
					_hoveredTextPart = rectangle.Text;
					hoverSet = true;
				}
			}
			if (!hoverSet)
			{
				_hoveredTextPart = null;
			}
			((Control)this).DoUpdate(gameTime);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			foreach (var rectangle in _rectangles)
			{
				Rectangle destinationRectangle = RectangleExtension.ToBounds(rectangle.Rectangle.Rectangle, ((Control)this).get_AbsoluteBounds());
				Color textColor = rectangle.Text.TextColor;
				if (_hoveredTextPart != null && rectangle.Text == _hoveredTextPart)
				{
					textColor = rectangle.Text.HoverColor;
				}
				string stringText = rectangle.ToDraw as string;
				if (stringText != null)
				{
					BitmapFont font = rectangle.Text.Font;
					Point location = ((Rectangle)(ref destinationRectangle)).get_Location();
					BitmapFontExtensions.DrawString(spriteBatch, font, stringText, ((Point)(ref location)).ToVector2(), textColor, (Rectangle?)null);
				}
				else
				{
					object item = rectangle.ToDraw;
					AsyncTexture2D texture = (AsyncTexture2D)((item is AsyncTexture2D) ? item : null);
					if (texture != null)
					{
						spriteBatch.Draw(AsyncTexture2D.op_Implicit(texture), destinationRectangle, Color.get_White());
					}
				}
				if (rectangle.Text.IsUnderlined)
				{
					ShapeExtensions.DrawLine(spriteBatch, new Vector2((float)destinationRectangle.X, (float)(destinationRectangle.Y + destinationRectangle.Height)), new Vector2((float)(destinationRectangle.X + destinationRectangle.Width), (float)(destinationRectangle.Y + destinationRectangle.Height)), textColor, 2f, 0f);
				}
				if (rectangle.Text.IsStrikeThrough)
				{
					ShapeExtensions.DrawLine(spriteBatch, new Vector2((float)destinationRectangle.X, (float)(destinationRectangle.Y + destinationRectangle.Height / 2)), new Vector2((float)(destinationRectangle.X + destinationRectangle.Width), (float)(destinationRectangle.Y + destinationRectangle.Height / 2)), textColor, 2f, 0f);
				}
			}
		}

		protected override void DisposeControl()
		{
			foreach (FormattedLabelPart part in _parts)
			{
				part.Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
