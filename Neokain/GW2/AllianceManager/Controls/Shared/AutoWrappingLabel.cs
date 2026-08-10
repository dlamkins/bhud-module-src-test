using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	public class AutoWrappingLabel : Control
	{
		private string _text = string.Empty;

		private string _wrappedText = string.Empty;

		private BitmapFont _font;

		private Color _textColor = Color.get_White();

		private bool _showShadow = true;

		private int _maxWidth;

		private int _lastWrapWidth;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (!(_text == value))
				{
					_text = value ?? string.Empty;
					UpdateWrappedText();
				}
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font ?? Control.get_Content().get_DefaultFont14();
			}
			set
			{
				if (_font != value)
				{
					_font = value;
					UpdateWrappedText();
				}
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _textColor, value, false, "TextColor");
			}
		}

		public bool ShowShadow
		{
			get
			{
				return _showShadow;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _showShadow, value, false, "ShowShadow");
			}
		}

		public int MaxWidth
		{
			get
			{
				return _maxWidth;
			}
			set
			{
				if (_maxWidth != value)
				{
					_maxWidth = value;
					UpdateWrappedText();
				}
			}
		}

		public AutoWrappingLabel()
			: this()
		{
		}//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)


		protected override void OnResized(ResizedEventArgs e)
		{
			((Control)this).OnResized(e);
			UpdateWrappedText();
		}

		public override void RecalculateLayout()
		{
			UpdateWrappedText();
		}

		private float MeasureTextWidth(string text)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return 0f;
			}
			StringGlyphEnumerable glyphs = Font.GetGlyphs(text, (Point2?)null);
			if (!((IEnumerable<BitmapFontGlyph>)(object)glyphs).Any())
			{
				return 0f;
			}
			BitmapFontGlyph lastGlyph = ((IEnumerable<BitmapFontGlyph>)(object)glyphs).Last();
			float x = lastGlyph.Position.X;
			BitmapFontRegion fontRegion = lastGlyph.FontRegion;
			return x + (float)((fontRegion != null) ? fontRegion.get_Width() : 0);
		}

		private string WrapTextAccurate(string text, float maxWidth)
		{
			if (string.IsNullOrEmpty(text) || maxWidth <= 0f)
			{
				return text ?? string.Empty;
			}
			StringBuilder result = new StringBuilder();
			StringBuilder currentLine = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					result.Append(currentLine);
					result.Append('\n');
					currentLine.Clear();
					continue;
				}
				string testLine = currentLine.ToString() + c;
				if (MeasureTextWidth(testLine) > maxWidth && currentLine.Length > 0)
				{
					result.Append(currentLine);
					result.Append('\n');
					currentLine.Clear();
				}
				currentLine.Append(c);
			}
			if (currentLine.Length > 0)
			{
				result.Append(currentLine);
			}
			return result.ToString();
		}

		private void UpdateWrappedText()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			int num;
			if (_maxWidth <= 0)
			{
				Container parent = ((Control)this).get_Parent();
				num = ((parent != null) ? parent.get_ContentRegion().Width : 400);
			}
			else
			{
				num = _maxWidth;
			}
			int wrapWidth = num;
			if (wrapWidth <= 0)
			{
				wrapWidth = 400;
			}
			_lastWrapWidth = wrapWidth;
			_wrappedText = WrapTextAccurate(_text, wrapWidth);
			Size2 size = Font.MeasureString(_wrappedText);
			((Control)this).set_Height((int)Math.Ceiling(size.Height) + (_showShadow ? 1 : 0));
			((Control)this).Invalidate();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			Textures.get_Pixel();
			if (!string.IsNullOrEmpty(_wrappedText))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _wrappedText, Font, bounds, TextColor, false, (HorizontalAlignment)0, (VerticalAlignment)0);
			}
		}
	}
}
