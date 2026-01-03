using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoreBridge.Controls
{
	public class FormattedLabelCustom : Control
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
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				Rectangle = rectangle;
				base._002Ector();
			}
		}

		private readonly List<(RectangleWrapper Rectangle, FormattedLabelPartCustom Text, object ToDraw)> _rectangles = new List<(RectangleWrapper, FormattedLabelPartCustom, object)>();

		private readonly IEnumerable<FormattedLabelPartCustom> _parts;

		private readonly bool _wrapText;

		private readonly bool _autoSizeWidth;

		private readonly bool _autoSizeHeight;

		private readonly bool _showShadow;

		private bool _finishedInitialization;

		public SpriteFontBase Font
		{
			set
			{
				foreach (FormattedLabelPartCustom part in _parts)
				{
					part.Font = value;
				}
				((Control)this).RecalculateLayout();
			}
		}

		internal FormattedLabelCustom(IEnumerable<FormattedLabelPartCustom> parts, bool wrapText, bool autoSizeWidth, bool autoSizeHeight, bool showShadow)
			: this()
		{
			_parts = parts;
			_wrapText = wrapText;
			_autoSizeWidth = autoSizeWidth;
			_autoSizeHeight = autoSizeHeight;
			_showShadow = showShadow;
		}

		public override void RecalculateLayout()
		{
			InitializeRectangles();
		}

		private Rectangle HandleFirstTextPart(FormattedLabelPartCustom item, string firstText)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			Vector2 textSize = item.Font.MeasureString(firstText);
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(0, 0, (int)Math.Floor(textSize.X) + 1, item.Font.FontSize);
			if (_rectangles.Count <= 0)
			{
				return rectangle;
			}
			(RectangleWrapper, FormattedLabelPartCustom, object) lastRectangle = _rectangles[_rectangles.Count - 1];
			rectangle.X = lastRectangle.Item1.X + lastRectangle.Item1.Width;
			rectangle.Y = lastRectangle.Item1.Y;
			return rectangle;
		}

		private Rectangle HandleMultiLineText(FormattedLabelPartCustom item, string text)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			Vector2 textSize = item.Font.MeasureString(text);
			IGrouping<int, (RectangleWrapper, FormattedLabelPartCustom, object)> possibleLastYRectangles = (from x in _rectangles
				orderby x.Rectangle.Y descending
				group x by x.Rectangle.Y).First();
			(RectangleWrapper, FormattedLabelPartCustom, object) lastYRectangle = possibleLastYRectangles.FirstOrDefault<(RectangleWrapper, FormattedLabelPartCustom, object)>(((RectangleWrapper Rectangle, FormattedLabelPartCustom Text, object ToDraw) x) => x.Rectangle.Height != 0);
			var (rectangleWrapper, formattedLabelPartCustom, obj) = lastYRectangle;
			if (rectangleWrapper == null && formattedLabelPartCustom == null && obj == null)
			{
				lastYRectangle = possibleLastYRectangles.First();
			}
			return new Rectangle(0, lastYRectangle.Item1.Y + lastYRectangle.Item1.Height, (int)Math.Floor(textSize.X) + 1, item.Font.FontSize);
		}

		private void InitializeRectangles()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Width() == 0 && !_autoSizeWidth)
			{
				return;
			}
			_finishedInitialization = false;
			_rectangles.Clear();
			foreach (FormattedLabelPartCustom item in _parts)
			{
				List<string> splittedText = item.Text.Split(new string[1] { "\n" }, StringSplitOptions.None).ToList();
				string firstText = splittedText[0];
				Rectangle rectangle = HandleFirstTextPart(item, firstText);
				bool wrapped = false;
				if (_wrapText && rectangle.X + rectangle.Width > ((Control)this).get_Width())
				{
					List<string> tempSplittedText = DrawUtilCustom.WrapText(item.Font, firstText, ((Control)this).get_Width() - rectangle.X).Split(new string[1] { "\n" }, StringSplitOptions.None).ToList();
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
						splittedText.InsertRange(i + 1, DrawUtilCustom.WrapText(item.Font, splittedText[i], ((Control)this).get_Width() - rectangle.X).Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries));
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
			}
			if (_autoSizeWidth)
			{
				((Control)this).set_Width((from x in _rectangles
					group x by x.Rectangle.Y into x
					select x.Select(((RectangleWrapper Rectangle, FormattedLabelPartCustom Text, object ToDraw) y) => y.Rectangle.Width).Sum()).Max());
			}
			if (_autoSizeHeight)
			{
				((Control)this).set_Height((from x in _rectangles
					group x by x.Rectangle.Y into x
					select x.Max(((RectangleWrapper Rectangle, FormattedLabelPartCustom Text, object ToDraw) x) => x.Rectangle.Height)).Sum());
			}
			HandleFontSizeDifferences();
			_finishedInitialization = true;
		}

		private void HandleFontSizeDifferences()
		{
			IGrouping<int, (RectangleWrapper, FormattedLabelPartCustom, object)>[] array = (from x in _rectangles
				group x by x.Rectangle.Y).ToArray();
			foreach (IGrouping<int, (RectangleWrapper, FormattedLabelPartCustom, object)> obj in array)
			{
				(RectangleWrapper, FormattedLabelPartCustom, object) maxHeightInRowRectangle = obj.OrderByDescending<(RectangleWrapper, FormattedLabelPartCustom, object), int>(((RectangleWrapper Rectangle, FormattedLabelPartCustom Text, object ToDraw) x) => x.Rectangle.Height).First();
				foreach (var rectangle in obj)
				{
					int offset = maxHeightInRowRectangle.Item1.Height - rectangle.Item1.Height;
					rectangle.Item1.Y += (int)Math.Floor((double)offset / 2.0);
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			if (!_finishedInitialization)
			{
				return;
			}
			float absoluteOpacity = ((Control)this).AbsoluteOpacity();
			foreach (var rectangle in _rectangles)
			{
				Rectangle destinationRectangle = RectangleExtension.ToBounds(rectangle.Rectangle.Rectangle, ((Control)this).get_AbsoluteBounds());
				Color textColor = rectangle.Text.TextColor;
				object item = rectangle.ToDraw;
				string stringText = item as string;
				if (stringText == null)
				{
					AsyncTexture2D texture = (AsyncTexture2D)((item is AsyncTexture2D) ? item : null);
					if (texture != null)
					{
						spriteBatch.Draw(AsyncTexture2D.op_Implicit(texture), destinationRectangle, Color.get_White() * absoluteOpacity);
					}
				}
				else
				{
					if (_showShadow)
					{
						spriteBatch.DrawStringOnCtrl((Control)(object)this, stringText, rectangle.Text.Font, RectangleExtension.OffsetBy(rectangle.Rectangle.Rectangle, 1, 1), Color.get_Black(), _wrapText, stroke: false, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
					spriteBatch.DrawStringOnCtrl((Control)(object)this, stringText, rectangle.Text.Font, rectangle.Rectangle.Rectangle, textColor, _wrapText, stroke: false, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		protected override void DisposeControl()
		{
			foreach (FormattedLabelPartCustom part in _parts)
			{
				part.Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
