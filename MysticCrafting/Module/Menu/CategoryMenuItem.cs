using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Menu
{
	public class CategoryMenuItem : Container, ICategoryMenuItem, IAccordion
	{
		private const int DefaultItemHeight = 32;

		private const int IconPadding = 10;

		private const int IconSize = 32;

		private const int ArrowSize = 16;

		private readonly AsyncTexture2D _textureArrow = AsyncTexture2D.FromAssetId(156057);

		protected int _menuItemHeight = 32;

		protected bool _shouldShift;

		protected CategoryMenuItem _selectedMenuItem;

		protected int _menuDepth;

		protected string _text;

		protected AsyncTexture2D _icon;

		protected bool _canCheck;

		protected bool _collapsed = true;

		protected Color _textColor = Color.White;

		protected bool _overSection;

		private bool _mouseIsOverChildItem;

		private Tween _slideAnim;

		private ScrollingHighlightEffect _scrollEffect;

		public int MenuItemHeight
		{
			get
			{
				return _menuItemHeight;
			}
			set
			{
				if (!SetProperty(ref _menuItemHeight, value, invalidateLayout: true, "MenuItemHeight"))
				{
					return;
				}
				base.Height = _menuItemHeight;
				foreach (ICategoryMenuItem item in _children.Cast<ICategoryMenuItem>().ToList())
				{
					item.MenuItemHeight = _menuItemHeight;
				}
			}
		}

		public bool ShouldShift
		{
			get
			{
				return _shouldShift;
			}
			set
			{
				SetProperty(ref _shouldShift, value, invalidateLayout: true, "ShouldShift");
			}
		}

		public bool Selected => _selectedMenuItem == this;

		public CategoryMenuItem SelectedMenuItem => _selectedMenuItem;

		protected int MenuDepth
		{
			get
			{
				return _menuDepth;
			}
			set
			{
				SetProperty(ref _menuDepth, value, invalidateLayout: false, "MenuDepth");
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				SetProperty(ref _text, value, invalidateLayout: false, "Text");
			}
		}

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				SetProperty(ref _icon, value, invalidateLayout: false, "Icon");
			}
		}

		public bool CanCheck
		{
			get
			{
				return _canCheck;
			}
			set
			{
				SetProperty(ref _canCheck, value, invalidateLayout: false, "CanCheck");
			}
		}

		public bool Collapsed
		{
			get
			{
				return _collapsed;
			}
			set
			{
				if (value)
				{
					Collapse();
				}
				else
				{
					Expand();
				}
			}
		}

		public MysticItemFilter ItemFilter { get; set; }

		public Color TextColor
		{
			get
			{
				return _textColor;
			}
			set
			{
				SetProperty(ref _textColor, value, invalidateLayout: false, "TextColor");
			}
		}

		private bool OverSection
		{
			get
			{
				return _overSection;
			}
			set
			{
				if (_overSection != value)
				{
					_overSection = value;
					OnPropertyChanged("OverSection");
				}
			}
		}

		public float ArrowRotation { get; set; } = -(float)Math.PI / 2f;


		private bool MouseOverIconBox { get; set; }

		private int LeftSidePadding
		{
			get
			{
				int leftSideBuilder = 10;
				if (!_children.IsEmpty)
				{
					leftSideBuilder += 16;
				}
				return leftSideBuilder + MenuDepth * 40;
			}
		}

		private Rectangle FirstItemBoxRegion => new Rectangle(0, MenuItemHeight / 2 - 16, 32, 32);

		public event EventHandler<ControlActivatedEventArgs> ItemSelected;

		public event EventHandler<ControlActivatedEventArgs> ItemClicked;

		protected virtual void OnItemSelected(ControlActivatedEventArgs e)
		{
			this.ItemSelected?.Invoke(this, e);
		}

		protected virtual void OnItemClicked(ControlActivatedEventArgs e)
		{
			this.ItemClicked?.Invoke(this, e);
		}

		public CategoryMenuItem(string text, MysticItemFilter filter)
			: this(text, (AsyncTexture2D)null)
		{
			ItemFilter = filter;
		}

		public CategoryMenuItem(string text, AsyncTexture2D icon)
		{
			_text = text;
			_icon = icon;
			Initialize();
		}

		private void Initialize()
		{
			_scrollEffect = new ScrollingHighlightEffect(this);
			base.EffectBehind = _scrollEffect;
			base.Height = MenuItemHeight;
			base.ContentRegion = new Rectangle(0, MenuItemHeight, base.Width, 0);
		}

		public void Select()
		{
			if (!Selected)
			{
				_selectedMenuItem = this;
				_scrollEffect.ForceActive = true;
				Select(this);
				OnPropertyChanged("Selected");
			}
		}

		public void Select(CategoryMenuItem menuItem)
		{
			((ICategoryMenuItem)this).Select(menuItem, new List<CategoryMenuItem> { this });
		}

		public void Select(CategoryMenuItem menuItem, List<CategoryMenuItem> itemPath)
		{
			itemPath.Add(this);
			OnItemSelected(new ControlActivatedEventArgs(menuItem));
			if (!_children.IsEmpty)
			{
				Expand();
			}
			(base.Parent as ICategoryMenuItem)?.Select(menuItem, itemPath);
			_selectedMenuItem = this;
			_scrollEffect.ForceActive = true;
		}

		public void Deselect()
		{
			bool selected = Selected;
			_selectedMenuItem = null;
			_scrollEffect.ForceActive = false;
			if (selected)
			{
				OnPropertyChanged("Selected");
			}
		}

		public override void RecalculateLayout()
		{
			_scrollEffect.Size = new Vector2(_size.X, _menuItemHeight);
			UpdateContentRegion();
		}

		private void UpdateContentRegion()
		{
			Control[] children = _children.ToArray();
			int bottomChild = ReflowChildLayout(children);
			base.ContentRegion = (children.Any() ? new Rectangle(0, MenuItemHeight, _size.X, bottomChild) : new Rectangle(0, MenuItemHeight, _size.X, 0));
			base.Height = ((!_collapsed) ? base.ContentRegion.Bottom : MenuItemHeight);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (_overSection)
			{
				if (!_children.IsEmpty)
				{
					ToggleAccordionState();
					ServiceContainer.AudioService.PlayMenuClick();
				}
				ServiceContainer.AudioService.PlayMenuItemClick();
				OnItemClicked(new ControlActivatedEventArgs(this));
			}
			if (!Selected || !_overSection)
			{
				Select();
			}
			base.OnClick(e);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			OverSection = base.RelativeMousePosition.Y <= _menuItemHeight;
			if (OverSection)
			{
				_scrollEffect.Enable();
			}
			else if (!_scrollEffect.ForceActive)
			{
				_scrollEffect.Disable();
			}
			MouseOverIconBox = _canCheck && _overSection && FirstItemBoxRegion.OffsetBy(LeftSidePadding, 0).Contains(base.RelativeMousePosition);
			base.OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			OverSection = false;
			base.OnMouseLeft(e);
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			CategoryMenuItem newChild = e.ChangedChild as CategoryMenuItem;
			if (newChild == null)
			{
				e.Cancel = true;
				return;
			}
			newChild.MenuItemHeight = MenuItemHeight;
			newChild.MenuDepth = MenuDepth + 1;
			ReflowChildLayout(_children.ToArray());
		}

		private int ReflowChildLayout(IEnumerable<Control> allChildren)
		{
			int lastBottom = 0;
			foreach (Control item in allChildren.Where((Control c) => c.Visible))
			{
				item.Location = new Point(0, lastBottom);
				item.Width = base.Width;
				lastBottom = item.Bottom;
			}
			return lastBottom;
		}

		public bool ToggleAccordionState()
		{
			Collapsed = !_collapsed;
			return _collapsed;
		}

		public void Expand()
		{
			if (_collapsed)
			{
				_slideAnim?.CancelAndComplete();
				SetProperty(ref _collapsed, newValue: false, invalidateLayout: false, "Expand");
				_slideAnim = Control.Animation.Tweener.Tween(this, new
				{
					ArrowRotation = 0f
				}, 0.3f).Ease(Ease.QuadOut);
				base.Height = base.ContentRegion.Bottom;
			}
		}

		public void Collapse()
		{
			if (!_collapsed)
			{
				_slideAnim?.CancelAndComplete();
				SetProperty(ref _collapsed, newValue: true, invalidateLayout: false, "Collapse");
				_slideAnim = Control.Animation.Tweener.Tween(this, new
				{
					ArrowRotation = -(float)Math.PI / 2f
				}, 0.3f).Ease(Ease.QuadOut);
				base.Height = MenuItemHeight;
			}
		}

		private void DrawDropdownArrow(SpriteBatch spriteBatch)
		{
			SpriteBatchExtensions.DrawOnCtrl(origin: new Vector2(8f, 8f), destinationRectangle: new Rectangle(13, MenuItemHeight / 2, 16, 16), spriteBatch: spriteBatch, ctrl: this, texture: _textureArrow, sourceRectangle: null, color: Color.White, rotation: ArrowRotation);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			int currentLeftSidePadding = LeftSidePadding;
			if (!_children.IsEmpty)
			{
				DrawDropdownArrow(spriteBatch);
			}
			TextureRegion2D firstItemSprite = null;
			if (Icon != null && _children.IsEmpty)
			{
				firstItemSprite = new TextureRegion2D(Icon);
			}
			if (firstItemSprite != null)
			{
				spriteBatch.DrawOnCtrl(this, firstItemSprite, FirstItemBoxRegion.OffsetBy(currentLeftSidePadding - 10, 0));
			}
			if (_canCheck)
			{
				currentLeftSidePadding += 42;
			}
			else if (!_children.IsEmpty)
			{
				currentLeftSidePadding += 10;
			}
			else if (_icon != null)
			{
				currentLeftSidePadding += 25;
			}
			spriteBatch.DrawStringOnCtrl(this, _text, Control.Content.DefaultFont18, new Rectangle(currentLeftSidePadding, 0, base.Width - (currentLeftSidePadding - 10), MenuItemHeight), _textColor, wrap: true, stroke: true);
		}
	}
}
