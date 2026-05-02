using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public abstract class BaseDropdown<T> : Control
	{
		protected sealed class DropdownMenu : FlowPanel
		{
			public sealed class DropdownItem : Control
			{
				private readonly KeyValuePair<T, Func<string>> _item;

				public readonly DropdownMenu _menu;

				public T Item => _item.Key;

				public DropdownItem(DropdownMenu assocMenu, KeyValuePair<T, Func<string>> item)
					: this()
				{
					_menu = assocMenu;
					_item = item;
				}

				protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					_menu._dropdown.PaintDropdownItem(this, spriteBatch, bounds);
				}

				protected override CaptureType CapturesInput()
				{
					return (CaptureType)1;
				}

				public override void DoUpdate(GameTime gameTime)
				{
					if (((Control)this).get_MouseOver())
					{
						_menu.HoveredItem = _item.Key;
					}
				}
			}

			private const int TOOLTIP_HOVER_DELAY = 800;

			private const int SCROLL_CLOSE_THRESHOLD = 20;

			private BaseDropdown<T> _dropdown;

			private T _hoveredItem;

			private double _hoverTime;

			private readonly int _startTop;

			private T HoveredItem
			{
				get
				{
					return _hoveredItem;
				}
				set
				{
					if (((Control)this).SetProperty<T>(ref _hoveredItem, value, false, "HoveredItem"))
					{
						_hoverTime = 0.0;
					}
				}
			}

			private DropdownMenu(BaseDropdown<T> assocDropdown)
				: this()
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				_dropdown = assocDropdown;
				((Control)this)._size = _dropdown.GetDropdownSize();
				((Control)this)._location = GetPanelLocation();
				((Control)this)._zIndex = 2147483615;
				_startTop = ((Control)this)._location.Y;
				((Panel)this)._canScroll = true;
				base._outerControlPadding = new Vector2((float)_dropdown.Margin, (float)_dropdown.Margin);
				base._controlPadding = new Vector2((float)_dropdown.Spacing, (float)_dropdown.Spacing);
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
				Rectangle absoluteBounds = ((Control)_dropdown).get_AbsoluteBounds();
				Point dropdownLocation = ((Rectangle)(ref absoluteBounds)).get_Location();
				int yUnderDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Bottom() - (dropdownLocation.Y + ((Control)_dropdown).get_Height() + ((Control)this)._size.Y);
				int yAboveDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Top() + (dropdownLocation.Y - ((Control)this)._size.Y);
				if (yUnderDef <= 0 && yUnderDef <= yAboveDef)
				{
					return dropdownLocation - new Point(0, ((Control)this)._size.Y + 1);
				}
				return dropdownLocation + new Point(0, ((Control)_dropdown).get_Height() - 1);
			}

			public static DropdownMenu ShowPanel(BaseDropdown<T> assocDropdown)
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				DropdownMenu menu = new DropdownMenu(assocDropdown);
				foreach (KeyValuePair<T, Func<string>> item in assocDropdown._items)
				{
					DropdownItem dropdownItem = new DropdownItem(menu, item);
					((Control)dropdownItem).set_Parent((Container)(object)menu);
					((Control)dropdownItem).set_Size(assocDropdown.GetDropdownItemSize());
				}
				return menu;
			}

			private void InputOnMousedOffDropdownPanel(object sender, MouseEventArgs e)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Invalid comparison between Unknown and I4
				if (!((Control)this).get_MouseOver())
				{
					if ((int)e.get_EventType() == 516)
					{
						_dropdown.HideDropdownPanelWithoutDebounce();
					}
					else
					{
						_dropdown.HideDropdownPanel();
					}
				}
			}

			private void UpdateHoverTimer(double elapsedMilliseconds)
			{
				if (((Control)this)._mouseOver)
				{
					_hoverTime += elapsedMilliseconds;
				}
				else
				{
					_hoverTime = 0.0;
				}
				if (_hoverTime > 800.0 && _dropdown._items.TryGetValue(_hoveredItem, out var func))
				{
					((Control)this).set_BasicTooltipText(func?.Invoke());
				}
				else
				{
					((Control)this).set_BasicTooltipText(string.Empty);
				}
			}

			private void UpdateDropdownLocation()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this)._location = GetPanelLocation();
				if (Math.Abs(((Control)this)._location.Y - _startTop) > 20)
				{
					((Control)this).Dispose();
				}
			}

			public override void UpdateContainer(GameTime gameTime)
			{
				UpdateHoverTimer(gameTime.get_ElapsedGameTime().TotalMilliseconds);
				UpdateDropdownLocation();
			}

			protected override void OnClick(MouseEventArgs e)
			{
				_dropdown.SelectedItem = _hoveredItem;
				((Panel)this).OnClick(e);
				((Control)this).Dispose();
			}

			public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
			{
				_dropdown.PaintDropdown(this, spriteBatch);
			}

			protected override void DisposeControl()
			{
				if (_dropdown != null)
				{
					_dropdown._menu = null;
					_dropdown = null;
				}
				Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				Control.get_Input().get_Mouse().remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				((FlowPanel)this).DisposeControl();
			}
		}

		protected const int BORDER_WIDTH = 2;

		private readonly SortedList<T, Func<string>> _items;

		private T _selectedItem;

		private Point _menuSize = Point.get_Zero();

		private int _spacing;

		private int _margin = 6;

		private DropdownMenu _menu;

		private bool _hadPanel;

		public T SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				T prev = _selectedItem;
				if (((Control)this).SetProperty<T>(ref _selectedItem, value, false, "SelectedItem"))
				{
					HasSelected = true;
					this.SelectedItemChanged?.Invoke(this, new ValueChangedEventArgs<T>(prev, _selectedItem));
					OnSelectedItemChanged(prev, _selectedItem);
				}
			}
		}

		public Point MenuSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _menuSize;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				if (!(_menuSize == value) && value.X >= 0 && value.Y >= 0)
				{
					Point menuSize = _menuSize;
					_menuSize = value;
					((Control)this).OnPropertyChanged("MenuSize");
					if (menuSize.Y != _menuSize.Y)
					{
						((Control)this).OnPropertyChanged("MenuHeight", false);
					}
					if (menuSize.X != _menuSize.X)
					{
						((Control)this).OnPropertyChanged("MenuWidth", false);
					}
				}
			}
		}

		public int MenuHeight
		{
			get
			{
				return _menuSize.Y;
			}
			set
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				if (_menuSize.Y != value)
				{
					MenuSize = new Point(_menuSize.X, value);
				}
			}
		}

		public int MenuWidth
		{
			get
			{
				return _menuSize.X;
			}
			set
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				if (_menuSize.X != value)
				{
					MenuSize = new Point(value, _menuSize.Y);
				}
			}
		}

		public int Spacing
		{
			get
			{
				return _spacing;
			}
			set
			{
				if (((Control)this).SetProperty<int>(ref _spacing, value, false, "Spacing"))
				{
					((Control)this).Invalidate();
				}
			}
		}

		public int Margin
		{
			get
			{
				return _margin;
			}
			set
			{
				if (((Control)this).SetProperty<int>(ref _margin, value, false, "Margin"))
				{
					((Control)this).Invalidate();
				}
			}
		}

		protected bool HasSelected { get; private set; }

		public event EventHandler<ValueChangedEventArgs<T>> SelectedItemChanged;

		protected virtual void OnSelectedItemChanged(T previous, T current)
		{
		}

		protected BaseDropdown()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			_items = new SortedList<T, Func<string>>();
			((Control)this).set_Size(((DesignStandard)(ref Dropdown.Standard)).get_Size());
		}

		protected bool AddItem(T item, Func<string> tooltip)
		{
			if (_items.ContainsKey(item))
			{
				return false;
			}
			_items.Add(item, tooltip);
			OnItemAdded(item);
			OnItemsUpdated();
			((Control)this).Invalidate();
			return true;
		}

		public bool RemoveItem(T key)
		{
			if (!_items.Remove(key))
			{
				return false;
			}
			OnItemRemoved(key);
			OnItemsUpdated();
			((Control)this).Invalidate();
			return true;
		}

		public void Clear()
		{
			Deselect();
			_items.Clear();
			OnItemsCleared();
			OnItemsUpdated();
			((Control)this).Invalidate();
		}

		public void Deselect()
		{
			HasSelected = false;
		}

		public void HideDropdownPanel()
		{
			_hadPanel = base._mouseOver;
			OnDropdownMenuClosed(_menu);
			DropdownMenu menu = _menu;
			if (menu != null)
			{
				((Control)menu).Dispose();
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
			if (_menu == null && !_hadPanel)
			{
				_menu = DropdownMenu.ShowPanel(this);
				OnDropdownMenuShown(_menu);
			}
			else
			{
				_hadPanel = false;
			}
		}

		protected virtual void OnItemAdded(T item)
		{
		}

		protected virtual void OnItemRemoved(T item)
		{
		}

		protected virtual void OnItemsCleared()
		{
		}

		protected virtual void OnItemsUpdated()
		{
		}

		protected virtual void PaintDropdown(DropdownMenu menu, SpriteBatch spriteBatch)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)menu, Textures.get_Pixel(), new Rectangle(Point.get_Zero(), ((Control)menu).get_Size()), Color.get_Black());
			spriteBatch.DrawBorderOnCtrl((Control)(object)menu, new Rectangle(Point.get_Zero(), ((Control)menu).get_Size()), Color.get_White() * 0.5f, 2);
		}

		protected abstract void PaintDropdownItem(DropdownMenu.DropdownItem ctrl, SpriteBatch spriteBatch, Rectangle bounds);

		protected virtual Point GetDropdownSize()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return MenuSize;
		}

		protected virtual Point GetDropdownItemSize()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Point(((Control)this).get_Width() - 2, ((Control)this).get_Height());
		}

		protected virtual void OnDropdownMenuShown(DropdownMenu menu)
		{
		}

		protected virtual void OnDropdownMenuClosed(DropdownMenu menu)
		{
		}
	}
}
