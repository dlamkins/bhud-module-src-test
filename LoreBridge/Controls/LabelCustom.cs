using Blish_HUD.Controls;
using FontStashSharp;
using Microsoft.Xna.Framework;

namespace LoreBridge.Controls
{
	public class LabelCustom : LabelBaseCustom
	{
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (((Control)this).SetProperty<string>(ref _text, value, true, "Text") && (_autoSizeWidth || _autoSizeHeight))
				{
					((Control)this).RecalculateLayout();
				}
			}
		}

		public SpriteFontBase Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (((Control)this).SetProperty<SpriteFontBase>(ref _font, value, true, "Font") && (_autoSizeWidth || _autoSizeHeight))
				{
					((Control)this).RecalculateLayout();
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

		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _horizontalAlignment;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<HorizontalAlignment>(ref _horizontalAlignment, value, false, "HorizontalAlignment");
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _verticalAlignment;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<VerticalAlignment>(ref _verticalAlignment, value, false, "VerticalAlignment");
			}
		}

		public bool WrapText
		{
			get
			{
				return _wrapText;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _wrapText, value, true, "WrapText");
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
				((Control)this).SetProperty<bool>(ref _showShadow, value, true, "ShowShadow");
			}
		}

		public bool StrokeText
		{
			get
			{
				return _strokeText;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _strokeText, value, true, "StrokeText");
			}
		}

		public Color ShadowColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _shadowColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _shadowColor, value, false, "ShadowColor");
			}
		}

		public bool AutoSizeWidth
		{
			get
			{
				return _autoSizeWidth;
			}
			set
			{
				if (((Control)this).SetProperty<bool>(ref _autoSizeWidth, value, true, "AutoSizeWidth") && (_autoSizeWidth || _autoSizeHeight))
				{
					((Control)this).RecalculateLayout();
				}
			}
		}

		public bool AutoSizeHeight
		{
			get
			{
				return _autoSizeHeight;
			}
			set
			{
				if (((Control)this).SetProperty<bool>(ref _autoSizeHeight, value, true, "AutoSizeHeight") && (_autoSizeWidth || _autoSizeHeight))
				{
					((Control)this).RecalculateLayout();
				}
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			((Control)this).set_Size(LabelRegion);
		}
	}
}
