using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Nekres.ChatMacros.Core.UI
{
	public class KeyValueDropdown<T> : Control
	{
		private class DropdownPanel : Control
		{
			private const int TOOLTIP_HOVER_DELAY = 800;

			private const int SCROLL_CLOSE_THRESHOLD = 20;

			private KeyValueDropdown<T> _assocDropdown;

			private int _highlightedItemIndex = -1;

			private double _hoverTime;

			private int _startTop;

			private int HighlightedItemIndex
			{
				get
				{
					return _highlightedItemIndex;
				}
				set
				{
					if (((Control)this).SetProperty<int>(ref _highlightedItemIndex, value, false, "HighlightedItemIndex"))
					{
						_hoverTime = 0.0;
					}
				}
			}

			private DropdownPanel(KeyValueDropdown<T> assocDropdown)
				: this()
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				_assocDropdown = assocDropdown;
				base._size = new Point(((Control)_assocDropdown).get_Width(), ((Control)_assocDropdown).get_Height() * _assocDropdown._items.Count);
				base._location = GetPanelLocation();
				base._zIndex = 2147483615;
				_startTop = base._location.Y;
				((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
				Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				Control.get_Input().get_Mouse().add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
			}

			private Point GetPanelLocation()
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				Rectangle absoluteBounds = ((Control)_assocDropdown).get_AbsoluteBounds();
				Point dropdownLocation = ((Rectangle)(ref absoluteBounds)).get_Location();
				int yUnderDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Bottom() - (dropdownLocation.Y + ((Control)_assocDropdown).get_Height() + base._size.Y);
				int yAboveDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Top() + (dropdownLocation.Y - base._size.Y);
				if (yUnderDef <= 0 && yUnderDef <= yAboveDef)
				{
					return dropdownLocation - new Point(0, base._size.Y + 1);
				}
				return dropdownLocation + new Point(0, ((Control)_assocDropdown).get_Height() - 1);
			}

			public static DropdownPanel ShowPanel(KeyValueDropdown<T> assocDropdown)
			{
				return new DropdownPanel(assocDropdown);
			}

			private void InputOnMousedOffDropdownPanel(object sender, MouseEventArgs e)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Invalid comparison between Unknown and I4
				if (!((Control)this).get_MouseOver())
				{
					if ((int)e.get_EventType() == 516)
					{
						_assocDropdown.HideDropdownPanelWithoutDebounce();
					}
					else
					{
						_assocDropdown.HideDropdownPanel();
					}
				}
			}

			protected override void OnMouseMoved(MouseEventArgs e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				HighlightedItemIndex = ((Control)this).get_RelativeMousePosition().Y / ((Control)_assocDropdown).get_Height();
				((Control)this).OnMouseMoved(e);
			}

			private KeyValuePair<T, string> GetActiveItem()
			{
				if (_highlightedItemIndex <= 0 || _highlightedItemIndex >= _assocDropdown._items.Count)
				{
					return default(KeyValuePair<T, string>);
				}
				return _assocDropdown._items.ElementAt(_highlightedItemIndex);
			}

			private void UpdateHoverTimer(double elapsedMilliseconds)
			{
				if (base._mouseOver)
				{
					_hoverTime += elapsedMilliseconds;
				}
				else
				{
					_hoverTime = 0.0;
				}
				((Control)this).set_BasicTooltipText((_hoverTime > 800.0) ? GetActiveItem().Value : string.Empty);
			}

			private void UpdateDropdownLocation()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				base._location = GetPanelLocation();
				if (Math.Abs(base._location.Y - _startTop) > 20)
				{
					((Control)this).Dispose();
				}
			}

			public override void DoUpdate(GameTime gameTime)
			{
				UpdateHoverTimer(gameTime.get_ElapsedGameTime().TotalMilliseconds);
				UpdateDropdownLocation();
			}

			protected override void OnClick(MouseEventArgs e)
			{
				_assocDropdown.SelectedItem = _assocDropdown._items.ElementAt(HighlightedItemIndex).Key;
				((Control)this).OnClick(e);
				((Control)this).Dispose();
			}

			private int GetMinimumWidth()
			{
				if (_assocDropdown._items == null || _assocDropdown._items.Count == 0)
				{
					return 0;
				}
				return (int)Math.Round(_assocDropdown._items.Max((KeyValuePair<T, string> i) => Control.get_Content().get_DefaultFont14().MeasureString(i.Value)
					.Width)) + 10 + ((!_assocDropdown.AutoSizeWidth) ? KeyValueDropdown<T>._textureArrow.get_Width() : 0);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_0134: Unknown result type (might be due to invalid IL or missing references)
				//IL_0153: Unknown result type (might be due to invalid IL or missing references)
				//IL_0187: Unknown result type (might be due to invalid IL or missing references)
				//IL_018e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0195: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Width(GetMinimumWidth());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(Point.get_Zero(), base._size), Color.get_Black());
				int index = 0;
				foreach (KeyValuePair<T, string> item in _assocDropdown._items)
				{
					if (index == HighlightedItemIndex)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, 2 + ((Control)_assocDropdown).get_Height() * index, ((Control)this).get_Width() - 4, ((Control)_assocDropdown).get_Height() - 4), new Color(45, 37, 25, 255));
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, item.Value, Control.get_Content().get_DefaultFont14(), new Rectangle(6, ((Control)_assocDropdown).get_Height() * index, bounds.Width - 13 - KeyValueDropdown<T>._textureArrow.get_Width(), ((Control)_assocDropdown).get_Height()), _assocDropdown._itemColors.TryGetValue(item.Key, out var color2) ? color2 : Colors.Chardonnay, false, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
					else
					{
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, item.Value, Control.get_Content().get_DefaultFont14(), new Rectangle(6, ((Control)_assocDropdown).get_Height() * index, bounds.Width - 13 - KeyValueDropdown<T>._textureArrow.get_Width(), ((Control)_assocDropdown).get_Height()), _assocDropdown._itemColors.TryGetValue(item.Key, out var color) ? (color * 0.95f) : Color.FromNonPremultiplied(239, 240, 239, 255), false, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
					index++;
				}
			}

			protected override void DisposeControl()
			{
				if (_assocDropdown != null)
				{
					_assocDropdown._lastPanel = null;
					_assocDropdown = null;
				}
				Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				Control.get_Input().get_Mouse().remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				((Control)this).DisposeControl();
			}
		}

		private static readonly Texture2D _textureInputBox = Control.get_Content().GetTexture("input-box");

		private static readonly TextureRegion2D _textureArrow = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow");

		private static readonly TextureRegion2D _textureArrowActive = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow-active");

		private readonly SortedList<T, string> _items;

		private readonly SortedList<T, Color> _itemColors;

		private Color _selectedItemColor;

		private string _selectedItemText;

		private T _selectedItem;

		private string _placeholderText;

		private bool _autoSizeWidth;

		private DropdownPanel _lastPanel;

		private bool _hadPanel;

		public T SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				T previousValue = _selectedItem;
				if (((Control)this).SetProperty<T>(ref _selectedItem, value, false, "SelectedItem"))
				{
					ItemsUpdated();
					OnValueChanged(new ValueChangedEventArgs<T>(previousValue, _selectedItem));
				}
			}
		}

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
					ItemsUpdated();
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
					ItemsUpdated();
				}
			}
		}

		public bool PanelOpen => _lastPanel != null;

		public event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

		protected virtual void OnValueChanged(ValueChangedEventArgs<T> e)
		{
			this.ValueChanged?.Invoke(this, e);
		}

		public KeyValueDropdown()
			: this()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			_placeholderText = string.Empty;
			_selectedItemText = string.Empty;
			_items = new SortedList<T, string>();
			_itemColors = new SortedList<T, Color>();
			((Control)this).set_Size(((DesignStandard)(ref Dropdown.Standard)).get_Size());
		}

		public bool AddItem(T key, string value, Color color = default(Color))
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (_items.ContainsKey(key))
			{
				return false;
			}
			_items.Add(key, value);
			_itemColors.Add(key, ((Color)(ref color)).Equals(default(Color)) ? Color.FromNonPremultiplied(239, 240, 239, 255) : color);
			ItemsUpdated();
			((Control)this).Invalidate();
			return true;
		}

		public bool RemoveItem(T key)
		{
			if (!_items.Remove(key))
			{
				return false;
			}
			_itemColors.Remove(key);
			ItemsUpdated();
			((Control)this).Invalidate();
			return true;
		}

		public void Clear()
		{
			_items.Clear();
			ItemsUpdated();
			((Control)this).Invalidate();
		}

		public void HideDropdownPanel()
		{
			_hadPanel = base._mouseOver;
			DropdownPanel lastPanel = _lastPanel;
			if (lastPanel != null)
			{
				((Control)lastPanel).Dispose();
			}
		}

		private void HideDropdownPanelWithoutDebounce()
		{
			HideDropdownPanel();
			_hadPanel = false;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			if (_lastPanel == null && !_hadPanel)
			{
				_lastPanel = DropdownPanel.ShowPanel(this);
			}
			else
			{
				_hadPanel = false;
			}
		}

		private void ItemsUpdated()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			_selectedItemText = (_items.TryGetValue(SelectedItem, out var displayText) ? displayText : string.Empty);
			_selectedItemColor = (_itemColors.TryGetValue(SelectedItem, out var color) ? color : Color.get_White());
			if (AutoSizeWidth)
			{
				int width = ((Control)this).get_Width();
				if (!string.IsNullOrEmpty(_selectedItemText))
				{
					width = (int)Math.Round(Control.get_Content().get_DefaultFont14().MeasureString(_selectedItemText)
						.Width);
					}
					else if (!string.IsNullOrEmpty(_placeholderText))
					{
						width = (int)Math.Round(Control.get_Content().get_DefaultFont14().MeasureString(_placeholderText)
							.Width);
						}
						((Control)this).set_Width(width + 13 + _textureArrow.get_Width());
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
					//IL_010a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0159: Unknown result type (might be due to invalid IL or missing references)
					//IL_0166: Unknown result type (might be due to invalid IL or missing references)
					//IL_0181: Unknown result type (might be due to invalid IL or missing references)
					//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
					//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
					//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, RectangleExtension.Subtract(new Rectangle(Point.get_Zero(), base._size), new Rectangle(0, 0, 5, 0)), (Rectangle?)new Rectangle(0, 0, Math.Min(_textureInputBox.get_Width() - 5, ((Control)this).get_Width() - 5), _textureInputBox.get_Height()));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, new Rectangle(base._size.X - 5, 0, 5, base._size.Y), (Rectangle?)new Rectangle(_textureInputBox.get_Width() - 5, 0, 5, _textureInputBox.get_Height()));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (((Control)this).get_Enabled() && ((Control)this).get_MouseOver()) ? _textureArrowActive : _textureArrow, new Rectangle(base._size.X - _textureArrow.get_Width() - 5, base._size.Y / 2 - _textureArrow.get_Height() / 2, _textureArrow.get_Width(), _textureArrow.get_Height()));
					if (string.IsNullOrEmpty(_selectedItemText))
					{
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _placeholderText, Control.get_Content().get_DefaultFont14(), new Rectangle(5, 0, base._size.X - 10 - _textureArrow.get_Width(), base._size.Y), ((Control)this).get_Enabled() ? Color.FromNonPremultiplied(209, 210, 209, 255) : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
					else
					{
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _selectedItemText, Control.get_Content().get_DefaultFont14(), new Rectangle(5, 0, base._size.X - 10 - _textureArrow.get_Width(), base._size.Y), ((Control)this).get_Enabled() ? _selectedItemColor : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
					}
				}
			}
		}
