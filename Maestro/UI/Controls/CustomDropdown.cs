using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Maestro.UI.Controls
{
	public class CustomDropdown : Control
	{
		private class DropdownPanel : Control
		{
			private const int ITEM_HEIGHT = 24;

			private const int PADDING_X = 8;

			private const int MAX_VISIBLE_ITEMS = 8;

			private readonly CustomDropdown _owner;

			private int _highlightedIndex = -1;

			public DropdownPanel(CustomDropdown owner)
				: this()
			{
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				_owner = owner;
				int height = Math.Min(_owner._items.Count, 8) * 24 + 4;
				base._size = new Point(((Control)_owner).get_Width(), height);
				base._location = GetPanelLocation();
				base._zIndex = 2147483615;
				((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				Control.get_Input().get_Mouse().add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
			}

			private Point GetPanelLocation()
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				Rectangle absoluteBounds = ((Control)_owner).get_AbsoluteBounds();
				return ((Rectangle)(ref absoluteBounds)).get_Location() + new Point(0, ((Control)_owner).get_Height() - 1);
			}

			private void OnMouseButtonPressed(object sender, MouseEventArgs e)
			{
				if (!((Control)this).get_MouseOver())
				{
					((Control)this).Dispose();
				}
			}

			protected override void OnMouseMoved(MouseEventArgs e)
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_highlightedIndex = GetItemIndexAt(((Control)this).get_RelativeMousePosition().Y);
				if (_highlightedIndex >= 0 && _highlightedIndex < _owner._items.Count)
				{
					DropdownItem item = _owner._items[_highlightedIndex];
					int availableWidth = base._size.X - 16;
					string truncatedText = TruncateText(item.DisplayText, Control.get_Content().get_DefaultFont14(), availableWidth);
					((Control)this).set_BasicTooltipText((truncatedText != item.DisplayText) ? item.DisplayText : null);
				}
				else
				{
					((Control)this).set_BasicTooltipText((string)null);
				}
				((Control)this).OnMouseMoved(e);
			}

			private int GetItemIndexAt(int y)
			{
				int adjustedY = y - 2;
				if (adjustedY < 0)
				{
					return -1;
				}
				int index = adjustedY / 24;
				if (index >= _owner._items.Count)
				{
					return -1;
				}
				return index;
			}

			protected override void OnClick(MouseEventArgs e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				int index = GetItemIndexAt(((Control)this).get_RelativeMousePosition().Y);
				if (index >= 0 && index < _owner._items.Count)
				{
					_owner.SelectedIndex = index;
					((Control)this).Dispose();
				}
				((Control)this).OnClick(e);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(Point.get_Zero(), base._size), Color.get_Black());
				int y = 2;
				int visibleItems = Math.Min(_owner._items.Count, 8);
				for (int i = 0; i < visibleItems; i++)
				{
					DropdownItem item = _owner._items[i];
					bool isSelected = i == _owner.SelectedIndex;
					bool isHighlighted = _highlightedIndex == i;
					DrawItem(spriteBatch, item, y, isSelected, isHighlighted);
					y += 24;
				}
			}

			private void DrawItem(SpriteBatch spriteBatch, DropdownItem item, int y, bool isSelected, bool isHighlighted)
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				if (isHighlighted)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, y, base._size.X - 4, 24), new Color(45, 37, 25, 255));
				}
				Color textColor = (isHighlighted ? Colors.Chardonnay : (isSelected ? MaestroTheme.AmberGold : Color.FromNonPremultiplied(239, 240, 239, 255)));
				int availableWidth = base._size.X - 16;
				string displayText = TruncateText(item.DisplayText, Control.get_Content().get_DefaultFont14(), availableWidth);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, displayText, Control.get_Content().get_DefaultFont14(), new Rectangle(8, y, availableWidth, 24), textColor, false, false, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}

			private static string TruncateText(string text, BitmapFont font, int maxWidth)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				if (string.IsNullOrEmpty(text))
				{
					return text;
				}
				if ((int)font.MeasureString(text).Width <= maxWidth)
				{
					return text;
				}
				string ellipsis = "...";
				int ellipsisWidth = (int)font.MeasureString(ellipsis).Width;
				int targetWidth = maxWidth - ellipsisWidth;
				for (int i = text.Length - 1; i > 0; i--)
				{
					string truncated = text.Substring(0, i);
					if ((int)font.MeasureString(truncated).Width <= targetWidth)
					{
						return truncated + ellipsis;
					}
				}
				return ellipsis;
			}

			protected override void DisposeControl()
			{
				if (_owner != null)
				{
					_owner._panel = null;
				}
				Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				Control.get_Input().get_Mouse().remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				((Control)this).DisposeControl();
			}
		}

		public class DropdownItem
		{
			public string DisplayText { get; set; }

			public string TooltipText { get; set; }

			public object Value { get; set; }

			public DropdownItem(string displayText, string tooltipText = null, object value = null)
			{
				DisplayText = displayText;
				TooltipText = tooltipText ?? displayText;
				Value = value;
			}
		}

		private static readonly Texture2D TextureInputBox = Control.get_Content().GetTexture("input-box");

		private DropdownPanel _panel;

		private bool _hadPanel;

		private readonly List<DropdownItem> _items = new List<DropdownItem>();

		private int _selectedIndex = -1;

		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Expected O, but got Unknown
				if (_selectedIndex != value && value >= -1 && value < _items.Count)
				{
					string oldValue = ((_selectedIndex >= 0) ? _items[_selectedIndex].DisplayText : null);
					_selectedIndex = value;
					string newValue = ((_selectedIndex >= 0) ? _items[_selectedIndex].DisplayText : null);
					this.ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
				}
			}
		}

		public DropdownItem SelectedItem
		{
			get
			{
				if (_selectedIndex < 0 || _selectedIndex >= _items.Count)
				{
					return null;
				}
				return _items[_selectedIndex];
			}
		}

		public string SelectedValue => SelectedItem?.DisplayText;

		public bool PanelOpen => _panel != null;

		public int ItemCount => _items.Count;

		public event EventHandler<ValueChangedEventArgs> ValueChanged;

		public CustomDropdown()
			: this()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(200, 27));
		}

		public void AddItem(string displayText, string tooltipText = null, object value = null)
		{
			_items.Add(new DropdownItem(displayText, tooltipText, value));
			if (_selectedIndex == -1 && _items.Count == 1)
			{
				SelectedIndex = 0;
			}
		}

		public void AddItem(DropdownItem item)
		{
			_items.Add(item);
			if (_selectedIndex == -1 && _items.Count == 1)
			{
				SelectedIndex = 0;
			}
		}

		public void ClearItems()
		{
			_items.Clear();
			_selectedIndex = -1;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			if (((Control)this).get_Enabled())
			{
				if (_panel == null && !_hadPanel)
				{
					_panel = new DropdownPanel(this);
				}
				else
				{
					_hadPanel = false;
				}
			}
		}

		public void HidePanel()
		{
			_hadPanel = base._mouseOver;
			DropdownPanel panel = _panel;
			if (panel != null)
			{
				((Control)panel).Dispose();
			}
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
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TextureInputBox, RectangleExtension.Subtract(new Rectangle(Point.get_Zero(), base._size), new Rectangle(0, 0, 5, 0)), (Rectangle?)new Rectangle(0, 0, Math.Min(TextureInputBox.get_Width() - 5, ((Control)this).get_Width() - 5), TextureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TextureInputBox, new Rectangle(base._size.X - 5, 0, 5, base._size.Y), (Rectangle?)new Rectangle(TextureInputBox.get_Width() - 5, 0, 5, TextureInputBox.get_Height()));
			Color arrowColor = ((((Control)this).get_Enabled() && ((Control)this).get_MouseOver()) ? Colors.Chardonnay : MaestroTheme.MutedCream);
			int arrowX = base._size.X - 18;
			int arrowY = base._size.Y / 2 - 2;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(arrowX, arrowY, 8, 2), arrowColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(arrowX + 2, arrowY + 2, 4, 2), arrowColor);
			string fullText = SelectedItem?.DisplayText ?? "";
			Color textColor = (((Control)this).get_Enabled() ? Color.FromNonPremultiplied(239, 240, 239, 255) : StandardColors.get_DisabledText());
			int availableWidth = base._size.X - 30;
			string displayText = TruncateText(fullText, Control.get_Content().get_DefaultFont14(), availableWidth);
			((Control)this).set_BasicTooltipText((displayText != fullText) ? fullText : null);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, displayText, Control.get_Content().get_DefaultFont14(), new Rectangle(8, 0, availableWidth, base._size.Y), textColor, false, false, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		private static string TruncateText(string text, BitmapFont font, int maxWidth)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if ((int)font.MeasureString(text).Width <= maxWidth)
			{
				return text;
			}
			string ellipsis = "...";
			int ellipsisWidth = (int)font.MeasureString(ellipsis).Width;
			int targetWidth = maxWidth - ellipsisWidth;
			for (int i = text.Length - 1; i > 0; i--)
			{
				string truncated = text.Substring(0, i);
				if ((int)font.MeasureString(truncated).Width <= targetWidth)
				{
					return truncated + ellipsis;
				}
			}
			return ellipsis;
		}
	}
}
