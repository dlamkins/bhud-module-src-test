using System;
using Blish_HUD;
using Blish_HUD.Controls;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LoreBridge.Controls
{
	public abstract class LabelBaseCustom : Control
	{
		protected bool _autoSizeHeight;

		protected bool _autoSizeWidth;

		protected SpriteFontBase _font;

		protected HorizontalAlignment _horizontalAlignment;

		protected Color _shadowColor = Color.get_Black();

		protected bool _showShadow;

		protected bool _strokeText;

		protected string _text;

		protected Color _textColor = Color.get_White();

		protected VerticalAlignment _verticalAlignment = (VerticalAlignment)1;

		protected bool _wrapText;

		protected Point LabelRegion = Point.get_Zero();

		public override void RecalculateLayout()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			int lblRegionWidth = base._size.X;
			int lblRegionHeight = base._size.Y;
			if (_autoSizeWidth || _autoSizeHeight)
			{
				Size2 textSize = GetTextDimensions();
				if (_autoSizeWidth)
				{
					lblRegionWidth = (int)Math.Ceiling(textSize.Width + (float)((_showShadow || _strokeText) ? 1 : 0));
				}
				if (_autoSizeHeight)
				{
					lblRegionHeight = (int)Math.Ceiling(textSize.Height + (float)((_showShadow || _strokeText) ? 1 : 0));
				}
			}
			LabelRegion = new Point(lblRegionWidth, lblRegionHeight);
		}

		protected Size2 GetTextDimensions(string text = null)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			text = text ?? _text;
			if (_font == null)
			{
				return new Size2(0f, 0f);
			}
			if (!_autoSizeWidth && _wrapText)
			{
				text = DrawUtilCustom.WrapText(_font, text, (LabelRegion.X > 0) ? LabelRegion.X : base._size.X);
			}
			return Size2.op_Implicit(_font.MeasureString(text ?? _text));
		}

		protected void DrawText(SpriteBatch spriteBatch, Rectangle bounds, string text = null)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			text = text ?? _text;
			if (_font != null && !string.IsNullOrEmpty(text))
			{
				if (_showShadow && !_strokeText)
				{
					spriteBatch.DrawStringOnCtrl((Control)(object)this, text, _font, RectangleExtension.OffsetBy(bounds, 1, 1), _shadowColor, _wrapText, _horizontalAlignment, _verticalAlignment);
				}
				spriteBatch.DrawStringOnCtrl((Control)(object)this, text, _font, bounds, _textColor, _wrapText, _strokeText, 1, _horizontalAlignment, _verticalAlignment);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			DrawText(spriteBatch, bounds, _text);
		}

		protected LabelBaseCustom()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)

	}
}
