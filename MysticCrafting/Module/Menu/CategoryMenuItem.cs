using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

		protected Color _textColor = Color.get_White();

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
				if (!((Control)this).SetProperty<int>(ref _menuItemHeight, value, true, "MenuItemHeight"))
				{
					return;
				}
				((Control)this).set_Height(_menuItemHeight);
				foreach (ICategoryMenuItem item in ((IEnumerable)base._children).Cast<ICategoryMenuItem>().ToList())
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
				((Control)this).SetProperty<bool>(ref _shouldShift, value, true, "ShouldShift");
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
				((Control)this).SetProperty<int>(ref _menuDepth, value, false, "MenuDepth");
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
				((Control)this).SetProperty<string>(ref _text, value, false, "Text");
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
				((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, false, "Icon");
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
				((Control)this).SetProperty<bool>(ref _canCheck, value, false, "CanCheck");
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
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _textColor, value, false, "TextColor");
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
					((Control)this).OnPropertyChanged("OverSection");
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
				if (!base._children.get_IsEmpty())
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
			: this()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_text = text;
			_icon = icon;
			Initialize();
		}

		private void Initialize()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			_scrollEffect = new ScrollingHighlightEffect((Control)(object)this);
			((Control)this).set_EffectBehind((ControlEffect)(object)_scrollEffect);
			((Control)this).set_Height(MenuItemHeight);
			((Container)this).set_ContentRegion(new Rectangle(0, MenuItemHeight, ((Control)this).get_Width(), 0));
		}

		public void Select()
		{
			if (!Selected)
			{
				_selectedMenuItem = this;
				_scrollEffect.set_ForceActive(true);
				Select(this);
				((Control)this).OnPropertyChanged("Selected");
			}
		}

		public void Select(CategoryMenuItem menuItem)
		{
			((ICategoryMenuItem)this).Select(menuItem, new List<CategoryMenuItem> { this });
		}

		public void Select(CategoryMenuItem menuItem, List<CategoryMenuItem> itemPath)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			itemPath.Add(this);
			OnItemSelected(new ControlActivatedEventArgs((Control)(object)menuItem));
			if (!base._children.get_IsEmpty())
			{
				Expand();
			}
			(((Control)this).get_Parent() as ICategoryMenuItem)?.Select(menuItem, itemPath);
			_selectedMenuItem = this;
			_scrollEffect.set_ForceActive(true);
		}

		public void Deselect()
		{
			bool selected = Selected;
			_selectedMenuItem = null;
			_scrollEffect.set_ForceActive(false);
			if (selected)
			{
				((Control)this).OnPropertyChanged("Selected");
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			((ControlEffect)_scrollEffect).set_Size(new Vector2((float)((Control)this)._size.X, (float)_menuItemHeight));
			UpdateContentRegion();
		}

		private void UpdateContentRegion()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			Control[] children = base._children.ToArray();
			int bottomChild = ReflowChildLayout(children);
			((Container)this).set_ContentRegion(children.Any() ? new Rectangle(0, MenuItemHeight, ((Control)this)._size.X, bottomChild) : new Rectangle(0, MenuItemHeight, ((Control)this)._size.X, 0));
			int height;
			if (_collapsed)
			{
				height = MenuItemHeight;
			}
			else
			{
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				height = ((Rectangle)(ref contentRegion)).get_Bottom();
			}
			((Control)this).set_Height(height);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			if (_overSection)
			{
				if (!base._children.get_IsEmpty())
				{
					ToggleAccordionState();
					ServiceContainer.AudioService.PlayMenuClick();
				}
				ServiceContainer.AudioService.PlayMenuItemClick();
				OnItemClicked(new ControlActivatedEventArgs((Control)(object)this));
			}
			if (!Selected || !_overSection)
			{
				Select();
			}
			((Control)this).OnClick(e);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			OverSection = ((Control)this).get_RelativeMousePosition().Y <= _menuItemHeight;
			if (OverSection)
			{
				((ControlEffect)_scrollEffect).Enable();
			}
			else if (!_scrollEffect.get_ForceActive())
			{
				((ControlEffect)_scrollEffect).Disable();
			}
			int mouseOverIconBox;
			if (_canCheck && _overSection)
			{
				Rectangle val = RectangleExtension.OffsetBy(FirstItemBoxRegion, LeftSidePadding, 0);
				mouseOverIconBox = (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()) ? 1 : 0);
			}
			else
			{
				mouseOverIconBox = 0;
			}
			MouseOverIconBox = (byte)mouseOverIconBox != 0;
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			OverSection = false;
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			CategoryMenuItem newChild = e.get_ChangedChild() as CategoryMenuItem;
			if (newChild == null)
			{
				((CancelEventArgs)(object)e).Cancel = true;
				return;
			}
			newChild.MenuItemHeight = MenuItemHeight;
			newChild.MenuDepth = MenuDepth + 1;
			ReflowChildLayout(base._children.ToArray());
		}

		private int ReflowChildLayout(IEnumerable<Control> allChildren)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			int lastBottom = 0;
			foreach (Control item in allChildren.Where((Control c) => c.get_Visible()))
			{
				item.set_Location(new Point(0, lastBottom));
				item.set_Width(((Control)this).get_Width());
				lastBottom = item.get_Bottom();
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
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (_collapsed)
			{
				Tween slideAnim = _slideAnim;
				if (slideAnim != null)
				{
					slideAnim.CancelAndComplete();
				}
				((Control)this).SetProperty<bool>(ref _collapsed, false, false, "Expand");
				_slideAnim = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<CategoryMenuItem>(this, (object)new
				{
					ArrowRotation = 0f
				}, 0.3f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)this).set_Height(((Rectangle)(ref contentRegion)).get_Bottom());
			}
		}

		public void Collapse()
		{
			if (!_collapsed)
			{
				Tween slideAnim = _slideAnim;
				if (slideAnim != null)
				{
					slideAnim.CancelAndComplete();
				}
				((Control)this).SetProperty<bool>(ref _collapsed, true, false, "Collapse");
				_slideAnim = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<CategoryMenuItem>(this, (object)new
				{
					ArrowRotation = -(float)Math.PI / 2f
				}, 0.3f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
				((Control)this).set_Height(MenuItemHeight);
			}
		}

		private void DrawDropdownArrow(SpriteBatch spriteBatch)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Vector2 arrowOrigin = default(Vector2);
			((Vector2)(ref arrowOrigin))._002Ector(8f, 8f);
			Rectangle arrowDest = default(Rectangle);
			((Rectangle)(ref arrowDest))._002Ector(13, MenuItemHeight / 2, 16, 16);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureArrow), arrowDest, (Rectangle?)null, Color.get_White(), ArrowRotation, arrowOrigin, (SpriteEffects)0);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			int currentLeftSidePadding = LeftSidePadding;
			if (!base._children.get_IsEmpty())
			{
				DrawDropdownArrow(spriteBatch);
			}
			TextureRegion2D firstItemSprite = null;
			if (Icon != null && base._children.get_IsEmpty())
			{
				firstItemSprite = new TextureRegion2D(AsyncTexture2D.op_Implicit(Icon));
			}
			if (firstItemSprite != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, firstItemSprite, RectangleExtension.OffsetBy(FirstItemBoxRegion, currentLeftSidePadding - 10, 0));
			}
			if (_canCheck)
			{
				currentLeftSidePadding += 42;
			}
			else if (!base._children.get_IsEmpty())
			{
				currentLeftSidePadding += 10;
			}
			else if (_icon != null)
			{
				currentLeftSidePadding += 25;
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, Control.get_Content().get_DefaultFont18(), new Rectangle(currentLeftSidePadding, 0, ((Control)this).get_Width() - (currentLeftSidePadding - 10), MenuItemHeight), _textColor, true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
