using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace Blish_HUD.Extended
{
	public class TextDropdown<T> : BaseDropdown<T>
	{
		private static readonly Texture2D _textureInputBox = Control.get_Content().GetTexture("input-box");

		private static readonly TextureRegion2D _textureArrow = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow");

		private static readonly TextureRegion2D _textureArrowActive = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow-active");

		private readonly SortedList<T, string> _itemTexts;

		private readonly SortedList<T, Color> _itemColors;

		private string _selectedItemText;

		private Color _selectedItemColor;

		private readonly Color _defaultColor;

		private readonly Color _placeholderColor;

		private string _placeholderText;

		private bool _autoSizeWidth;

		private BitmapFont _font;

		public string PlaceholderText
		{
			get
			{
				return _placeholderText;
			}
			set
			{
				if (((Control)this).SetProperty<string>(ref _placeholderText, value, false, "PlaceholderText"))
				{
					OnItemsUpdated();
					((Control)this).Invalidate();
				}
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
				if (((Control)this).SetProperty<bool>(ref _autoSizeWidth, value, false, "AutoSizeWidth"))
				{
					OnItemsUpdated();
					((Control)this).Invalidate();
				}
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (((Control)this).SetProperty<BitmapFont>(ref _font, value, false, "Font"))
				{
					OnItemsUpdated();
					((Control)this).Invalidate();
				}
			}
		}

		public TextDropdown()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			_itemTexts = new SortedList<T, string>();
			_itemColors = new SortedList<T, Color>();
			_placeholderText = string.Empty;
			_selectedItemText = string.Empty;
			_defaultColor = Color.FromNonPremultiplied(239, 240, 239, 255);
			_selectedItemColor = _defaultColor;
			_placeholderColor = Color.FromNonPremultiplied(209, 210, 209, 255);
			_font = Control.get_Content().get_DefaultFont14();
		}

		public void AddItem(T value, string text, Func<string> tooltip = null, Color color = default(Color))
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (AddItem(value, tooltip ?? ((Func<string>)(() => text))))
			{
				_itemTexts.Add(value, text);
				_itemColors.Add(value, ((Color)(ref color)).Equals(default(Color)) ? _defaultColor : color);
				OnItemsUpdated();
			}
		}

		public void AddItem(T value, string text, Color color)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			AddItem(value, text, null, color);
		}

		protected override void OnDropdownMenuShown(DropdownMenu menu)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)menu).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)menu).set_OuterControlPadding(new Vector2(2f, 2f));
		}

		protected override void OnItemRemoved(T value)
		{
			_itemTexts.Remove(value);
			_itemColors.Remove(value);
		}

		protected override void OnItemsCleared()
		{
			_itemTexts.Clear();
			_itemColors.Clear();
		}

		protected override void OnItemsUpdated()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			if (base.HasSelected && _itemTexts != null && _itemColors != null)
			{
				_selectedItemText = (_itemTexts.TryGetValue(base.SelectedItem, out var displayText) ? displayText : string.Empty);
				_selectedItemColor = (_itemColors.TryGetValue(base.SelectedItem, out var color) ? color : _defaultColor);
			}
			else
			{
				_selectedItemText = string.Empty;
				_selectedItemColor = _defaultColor;
			}
			if (AutoSizeWidth)
			{
				int width = ((Control)this).get_Width();
				if (!string.IsNullOrEmpty(_selectedItemText))
				{
					width = (int)Math.Round(_font.MeasureString(_selectedItemText).Width);
				}
				else if (!string.IsNullOrEmpty(_placeholderText))
				{
					width = (int)Math.Round(_font.MeasureString(_placeholderText).Width);
				}
				((Control)this).set_Width(width + 13 + _textureArrow.get_Width());
			}
		}

		private Color GetItemColor(T item)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (_itemColors.TryGetValue(item, out var color))
			{
				return color;
			}
			return _defaultColor;
		}

		private string GetItemText(T item)
		{
			if (_itemTexts.TryGetValue(item, out var text))
			{
				return text;
			}
			return string.Empty;
		}

		protected override void OnSelectedItemChanged(T previous, T current)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			_selectedItemText = GetItemText(current);
			_selectedItemColor = GetItemColor(current);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, RectangleExtension.Subtract(new Rectangle(Point.get_Zero(), ((Control)this)._size), new Rectangle(0, 0, 5, 0)), (Rectangle?)new Rectangle(0, 0, Math.Min(_textureInputBox.get_Width() - 5, ((Control)this).get_Width() - 5), _textureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, new Rectangle(((Control)this)._size.X - 5, 0, 5, ((Control)this)._size.Y), (Rectangle?)new Rectangle(_textureInputBox.get_Width() - 5, 0, 5, _textureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (((Control)this).get_Enabled() && ((Control)this).get_MouseOver()) ? _textureArrowActive : _textureArrow, new Rectangle(((Control)this)._size.X - _textureArrow.get_Width() - 5, ((Control)this)._size.Y / 2 - _textureArrow.get_Height() / 2, _textureArrow.get_Width(), _textureArrow.get_Height()));
			if (string.IsNullOrEmpty(_selectedItemText))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _placeholderText, _font, new Rectangle(5, 0, ((Control)this)._size.X - 10 - _textureArrow.get_Width(), ((Control)this)._size.Y), ((Control)this).get_Enabled() ? _placeholderColor : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			else
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _selectedItemText, _font, new Rectangle(5, 0, ((Control)this)._size.X - 10 - _textureArrow.get_Width(), ((Control)this)._size.Y), ((Control)this).get_Enabled() ? _selectedItemColor : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		protected override void PaintDropdownItem(DropdownMenu.DropdownItem ctrl, SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)ctrl).get_MouseOver())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)ctrl, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height), new Color(45, 37, 25, 255));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)ctrl, GetItemText(ctrl.Item), _font, new Rectangle(base.Margin, bounds.Y, bounds.Width - base.Margin, bounds.Height), _itemColors.TryGetValue(ctrl.Item, out var color2) ? color2 : Colors.Chardonnay, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			else
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)ctrl, GetItemText(ctrl.Item), _font, new Rectangle(base.Margin, bounds.Y, bounds.Width - base.Margin, bounds.Height), _itemColors.TryGetValue(ctrl.Item, out var color) ? (color * 0.95f) : _defaultColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		protected override Point GetDropdownSize()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			Point itemSize = GetDropdownItemSize();
			return new Point(((Control)this).get_Width(), (base.MenuHeight > itemSize.Y + 4) ? base.MenuHeight : (itemSize.Y * _itemTexts.Count + 4));
		}
	}
}
