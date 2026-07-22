using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Controls
{
	public class Hotbar : Panel
	{
		private readonly DetailedTexture _expander = new DetailedTexture(155909, 155910);

		private readonly Dummy _expandDummy;

		private readonly int _itemPadding = 4;

		private bool _resizeBarPending;

		private Point _dragStart;

		private bool _dragging;

		private Point _start;

		private Point _start_ItemWidth;

		private Point _delta;

		private bool _hasCollapseAnchor;

		private Rectangle _expanderBackgroundBounds;

		protected readonly FlowPanel ItemsPanel;

		public ExpandType ExpandType
		{
			[CompilerGenerated]
			get
			{
				return _003CExpandType_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CExpandType_003Ek__BackingField, value, delegate(ExpandType v)
				{
					_003CExpandType_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<ExpandType>(OnExpandTypeChanged));
			}
		}

		public SortType SortType
		{
			[CompilerGenerated]
			get
			{
				return _003CSortType_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSortType_003Ek__BackingField, value, delegate(SortType v)
				{
					_003CSortType_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<SortType>(OnSortTypeCanged));
			}
		}

		public bool ExpandBar
		{
			[CompilerGenerated]
			get
			{
				return _003CExpandBar_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CExpandBar_003Ek__BackingField, value, delegate(bool v)
				{
					_003CExpandBar_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<bool>(OnExpandChanged));
			}
		}

		public ModifierKeys MoveModifier { get; set; } = ModifierKeys.Alt;


		public int MinButtonSize { get; set; } = 24;


		public Action<Point> OnMoveAction { get; set; }

		public Action OpenSettingsAction { get; set; }

		public Hotbar()
		{
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.BorderColor = Color.Black;
			base.BorderWidth = new RectangleDimensions(2);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(155960);
			base.BackgroundImageColor = Color.DarkGray * 0.8f;
			_expandDummy = new Dummy
			{
				Parent = this,
				Size = new Point(16, 32)
			};
			ItemsPanel = new FlowPanel
			{
				Parent = this,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				HeightSizingMode = SizingMode.AutoSize
			};
			OnExpandTypeChanged(this, new Kenedia.Modules.Core.Models.ValueChangedEventArgs<ExpandType>(ExpandType?.LeftToRight, ExpandType?.LeftToRight));
			ExpandType = ExpandType.BottomToTop;
			base.BasicTooltipText = $"Press {MoveModifier} and drag the hotbar to the desired position";
			_expandDummy.BasicTooltipText = $"Press {MoveModifier} and drag the hotbar to the desired position";
			base.Menu = new ContextMenuStrip();
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings_common.OpenSettings, delegate
			{
				OpenSettingsAction?.Invoke();
			}));
		}

		private void OnSortTypeCanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<SortType> e)
		{
			if (ItemsPanel != null)
			{
				SetButtonsExpanded();
				ForceOnScreen();
				RecalculateLayout();
			}
		}

		private void OnExpandTypeChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ExpandType> e)
		{
			if (ItemsPanel != null)
			{
				_resizeBarPending = true;
				_hasCollapseAnchor = false;
				ItemsPanel.Size = Point.Zero;
				base.Size = Point.Zero;
				switch (e.NewValue)
				{
				case ExpandType?.LeftToRight:
					_expander.Texture = AsyncTexture2D.FromAssetId(155909);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155910);
					_expander.TextureRegion = new Rectangle(new Point(0, 0), new Point(16, 32));
					_expandDummy.Size = new Point(16, 32);
					ItemsPanel.FlowDirection = ControlFlowDirection.SingleLeftToRight;
					ItemsPanel.WidthSizingMode = SizingMode.Standard;
					ItemsPanel.HeightSizingMode = SizingMode.AutoSize;
					ItemsPanel.ContentPadding = new RectangleDimensions(5, 4, 0, 4);
					ItemsPanel.ControlPadding = new Vector2(5f);
					base.ContentPadding = new RectangleDimensions(0);
					break;
				case ExpandType?.RightToLeft:
					_expander.Texture = AsyncTexture2D.FromAssetId(155906);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155907);
					_expander.TextureRegion = new Rectangle(new Point(16, 0), new Point(16, 32));
					_expandDummy.Size = new Point(16, 32);
					ItemsPanel.FlowDirection = ControlFlowDirection.SingleRightToLeft;
					ItemsPanel.WidthSizingMode = SizingMode.Standard;
					ItemsPanel.HeightSizingMode = SizingMode.AutoSize;
					ItemsPanel.ContentPadding = new RectangleDimensions(-5, 4, 5, 4);
					ItemsPanel.ControlPadding = new Vector2(5f);
					base.ContentPadding = new RectangleDimensions(0);
					break;
				case ExpandType?.TopToBottom:
					_expander.Texture = AsyncTexture2D.FromAssetId(155929);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155929);
					_expander.TextureRegion = new Rectangle(new Point(0, 8), new Point(32, 16));
					_expandDummy.Size = new Point(32, 16);
					ItemsPanel.WidthSizingMode = SizingMode.AutoSize;
					ItemsPanel.HeightSizingMode = SizingMode.Standard;
					ItemsPanel.FlowDirection = ControlFlowDirection.SingleTopToBottom;
					ItemsPanel.ContentPadding = new RectangleDimensions(5, 4, 5, 4);
					ItemsPanel.ControlPadding = new Vector2(5f);
					base.ContentPadding = new RectangleDimensions(0, 2);
					break;
				case ExpandType?.BottomToTop:
					_expander.Texture = AsyncTexture2D.FromAssetId(155929);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155929);
					_expander.TextureRegion = new Rectangle(new Point(0, 8), new Point(32, 16));
					_expandDummy.Size = new Point(32, 16);
					ItemsPanel.WidthSizingMode = SizingMode.AutoSize;
					ItemsPanel.HeightSizingMode = SizingMode.Standard;
					ItemsPanel.FlowDirection = ControlFlowDirection.SingleBottomToTop;
					ItemsPanel.ContentPadding = new RectangleDimensions(5, 0, 5, 2);
					ItemsPanel.ControlPadding = new Vector2(5f);
					base.ContentPadding = new RectangleDimensions(0, 2);
					break;
				}
				ForceOnScreen();
				RecalculateLayout();
			}
		}

		private void OnExpandChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<bool> e)
		{
			if (e.NewValue)
			{
				_ = 1;
			}
			else
				_ = ItemsPanel.Children.FirstOrDefault((Control e) => e.Visible) != null;
			_resizeBarPending = true;
			SetButtonsExpanded();
			RecalculateLayout();
		}

		public virtual void SetButtonsExpanded()
		{
			foreach (ICheckable c in ItemsPanel.Children.OfType<ICheckable>())
			{
				(c as Control).Visible = ExpandBar || c.Checked;
			}
		}

		public void AddItem(ICheckable item)
		{
			Control control = item as Control;
			if (control != null)
			{
				control.Parent = ItemsPanel;
				item.CheckedChanged += Item_CheckedChanged;
			}
			RecalculateLayout();
			SetButtonsExpanded();
		}

		private void Item_CheckedChanged(object sender, CheckChangedEvent e)
		{
			Control control = sender as Control;
			if (control != null)
			{
				control.Visible = ExpandBar || e.Checked;
			}
			RecalculateLayout();
		}

		public void RemoveItem(ICheckable item)
		{
			Control control = item as Control;
			if (control != null)
			{
				item.CheckedChanged -= Item_CheckedChanged;
				control.Dispose();
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			_dragging = Control.Input.Keyboard.ActiveModifiers == MoveModifier;
			_dragStart = (_dragging ? base.RelativeMousePosition : Point.Zero);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			_dragging = false;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			ExpandBar = base.MouseOver || _expandDummy.MouseOver;
			_dragging = _dragging && base.MouseOver && Control.Input.Keyboard.ActiveModifiers == MoveModifier;
			if (_dragging)
			{
				MoveBar();
			}
		}

		protected virtual void SortButtons()
		{
			switch (SortType)
			{
			case SortType.ActivesFirst:
				ItemsPanel.SortChildren((HotbarButton a, HotbarButton b) => b.Checked.CompareTo(a.Checked));
				break;
			case SortType.ByModuleName:
				ItemsPanel.SortChildren((HotbarButton a, HotbarButton b) => a.BasicTooltipText.CompareTo(b.BasicTooltipText));
				break;
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (ItemsPanel != null)
			{
				if (base.BackgroundImage != null)
				{
					base.TextureRectangle = new Rectangle(50, 50, Math.Min(base.BackgroundImage.Bounds.Size.X, base.Width), Math.Min(base.Height, base.BackgroundImage.Bounds.Size.Y));
				}
				SortButtons();
				switch (ExpandType)
				{
				case ExpandType.LeftToRight:
					_expander.Bounds = _expandDummy.LocalBounds;
					CalculateLeftToRight();
					break;
				case ExpandType.RightToLeft:
					_expander.Bounds = _expandDummy.LocalBounds;
					CalculateRightToLeft();
					break;
				case ExpandType.TopToBottom:
					_expander.Bounds = _expandDummy.LocalBounds;
					CalculateTopToBottom();
					break;
				case ExpandType.BottomToTop:
					_expander.Bounds = new Rectangle(_expandDummy.Location.Add(new Point(0, _expandDummy.Height - 5)), _expandDummy.Size);
					CalculateBottomToTop();
					break;
				}
			}
		}

		public int GetItemPanelSize(bool any = false, bool isChecked = false, bool vertical = false)
		{
			IEnumerable<Control> source;
			if (!isChecked)
			{
				source = ItemsPanel.Children.Where((Control e) => any || e.Visible);
			}
			else
			{
				IEnumerable<Control> enumerable = (from e in ItemsPanel.Children.OfType<ICheckable>()
					where any || e.Checked
					select e).Cast<Control>();
				source = enumerable;
			}
			return (int)source.Sum((Control e) => (float)(vertical ? e.Height : e.Width) + (vertical ? ItemsPanel.ControlPadding.Y : ItemsPanel.ControlPadding.X));
		}

		private void CalculateLeftToRight()
		{
			if (base.BackgroundImage != null)
			{
				base.TextureRectangle = new Rectangle(50, 50, Math.Min(base.BackgroundImage.Bounds.Size.X, base.Width), Math.Min(base.Height, base.BackgroundImage.Bounds.Size.Y));
			}
			if (ItemsPanel != null)
			{
				IEnumerable<Control> visibleItems = ItemsPanel.Children.Where((Control e) => e.Visible);
				ItemsPanel.Size = new Point((int)visibleItems.Sum((Control e) => (float)e.Width + ItemsPanel.ControlPadding.X) + ((visibleItems != null && visibleItems.Count() > 0) ? ItemsPanel.ContentPadding.Horizontal : 0), base.Height - base.AutoSizePadding.Y);
				ItemsPanel.Location = new Point(0, 0);
			}
			if (_expandDummy != null)
			{
				_expandDummy.Location = new Point(Math.Max(ItemsPanel?.Right ?? 0, 5), ((ItemsPanel?.Height ?? base.Height) - _expandDummy.Height) / 2);
				_expanderBackgroundBounds = new Rectangle(_expandDummy.Left - 2, base.BorderWidth.Top, _expandDummy.Width + 2, base.Height - base.BorderWidth.Vertical);
			}
		}

		private void CalculateRightToLeft()
		{
			bool isAnyVisible = ItemsPanel.Children.Any((Control e) => e.Visible);
			int expandedItemsWidth = GetItemPanelSize(any: true);
			int checkedItemsWidth = GetItemPanelSize(any: false, isChecked: true);
			int padding = (isAnyVisible ? ItemsPanel.ContentPadding.Horizontal : 0);
			if (_resizeBarPending)
			{
				if (ExpandBar)
				{
					_start = base.Location;
					_start_ItemWidth = new Point(checkedItemsWidth, 0);
					_hasCollapseAnchor = true;
					base.Location = _start.Add(new Point(-(expandedItemsWidth - checkedItemsWidth), 0));
					ItemsPanel.Width = expandedItemsWidth + padding;
				}
				else
				{
					if (_hasCollapseAnchor)
					{
						_delta = new Point(_start_ItemWidth.X - checkedItemsWidth, 0);
						base.Location = _start.Add(_delta);
					}
					ItemsPanel.Width = (isAnyVisible ? (checkedItemsWidth + padding) : 0);
				}
				_resizeBarPending = false;
			}
			_expandDummy.Location = new Point(0, ((ItemsPanel?.Height ?? base.Height) - _expandDummy.Height) / 2);
			_expanderBackgroundBounds = new Rectangle(base.BorderWidth.Left, base.BorderWidth.Top, _expandDummy.Width + 2, base.Height - base.BorderWidth.Vertical);
			ItemsPanel.Location = new Point(_expandDummy.Right + base.BorderWidth.Horizontal, 0);
		}

		private void CalculateTopToBottom()
		{
			Math.Max(MinButtonSize, Math.Min(base.Width, base.Height) - _itemPadding - 10);
			if (base.BackgroundImage != null)
			{
				base.TextureRectangle = new Rectangle(50, 50, Math.Min(base.BackgroundImage.Bounds.Size.X, base.Width), Math.Min(base.Height, base.BackgroundImage.Bounds.Size.Y));
			}
			if (ItemsPanel != null)
			{
				IEnumerable<Control> visibleItems = ItemsPanel.Children.Where((Control e) => e.Visible);
				ItemsPanel.Size = new Point(base.Width - base.AutoSizePadding.X, (int)visibleItems.Sum((Control e) => (float)e.Width + ItemsPanel.ControlPadding.X) + ((visibleItems != null && visibleItems.Count() > 0) ? ItemsPanel.ContentPadding.Horizontal : 0));
				ItemsPanel.Location = new Point(0, 0);
			}
			if (_expandDummy != null)
			{
				_expandDummy.Location = new Point(((ItemsPanel?.Width ?? base.Width) - _expandDummy.Width) / 2, Math.Max(ItemsPanel?.Bottom ?? 0, 5) - 5);
				_expanderBackgroundBounds = new Rectangle(base.BorderWidth.Left, base.Height - _expandDummy.Height - base.BorderWidth.Bottom, base.Width - base.BorderWidth.Horizontal, _expandDummy.Height);
			}
		}

		private void CalculateBottomToTop()
		{
			bool isAnyVisible = ItemsPanel.Children.Any((Control e) => e.Visible);
			int expandedItemsWidth = GetItemPanelSize(any: true, isChecked: false, vertical: true);
			int checkedItemsWidth = GetItemPanelSize(any: false, isChecked: true, vertical: true);
			int padding = (isAnyVisible ? ItemsPanel.ContentPadding.Vertical : 0);
			if (_resizeBarPending)
			{
				if (ExpandBar)
				{
					_start = base.Location;
					_start_ItemWidth = new Point(checkedItemsWidth, 0);
					_hasCollapseAnchor = true;
					base.Location = _start.Add(new Point(0, -(expandedItemsWidth - checkedItemsWidth)));
					ItemsPanel.Height = expandedItemsWidth + padding;
				}
				else
				{
					if (_hasCollapseAnchor)
					{
						_delta = new Point(0, _start_ItemWidth.X - checkedItemsWidth);
						base.Location = _start.Add(_delta);
					}
					ItemsPanel.Height = (isAnyVisible ? (checkedItemsWidth + padding) : 0);
				}
				_resizeBarPending = false;
			}
			_expandDummy.Location = new Point((base.Width - base.AutoSizePadding.X - _expandDummy.Width) / 2, 0);
			_expanderBackgroundBounds = new Rectangle(base.BorderWidth.Left, base.BorderWidth.Top, base.Width - base.BorderWidth.Horizontal, _expandDummy.Height);
			ItemsPanel.Location = new Point(0, _expandDummy.Bottom);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			base.ClipsBounds = false;
			if (ExpandType == ExpandType.BottomToTop)
			{
				spriteBatch.Draw(ContentService.Textures.Pixel, _expanderBackgroundBounds.Add(new Rectangle(base.Location, Point.Zero)), Color.Black * 0.5f);
				spriteBatch.DrawCenteredRotationOnCtrl(this, (Texture2D)_expander.Texture, _expander.Bounds, _expander.TextureRegion, Color.White, 0f, flipVertically: true, flipHorizontally: false);
			}
			else
			{
				spriteBatch.Draw(ContentService.Textures.Pixel, _expanderBackgroundBounds.Add(new Rectangle(base.Location, Point.Zero)), Color.Black * 0.5f);
				_expander?.Draw(this, spriteBatch, base.RelativeMousePosition);
			}
		}

		private void MoveBar()
		{
			RecalculateLayout();
			switch (ExpandType)
			{
			case ExpandType.LeftToRight:
				base.Location = Control.Input.Mouse.Position.Add(new Point(-_dragStart.X, -_dragStart.Y));
				break;
			case ExpandType.RightToLeft:
			{
				int expandedItemsWidth = GetItemPanelSize(any: true);
				int checkedItemsWidth = GetItemPanelSize(any: false, isChecked: true);
				base.Location = Control.Input.Mouse.Position.Add(new Point(-_dragStart.X, -_dragStart.Y));
				_start = base.Location.Add(new Point(expandedItemsWidth - checkedItemsWidth, 0));
				_hasCollapseAnchor = true;
				break;
			}
			case ExpandType.TopToBottom:
				base.Location = Control.Input.Mouse.Position.Add(new Point(-_dragStart.X, -_dragStart.Y));
				break;
			case ExpandType.BottomToTop:
			{
				int expandedItemsWidth2 = GetItemPanelSize(any: true, isChecked: false, vertical: true);
				int checkedItemsWidth2 = GetItemPanelSize(any: false, isChecked: true, vertical: true);
				base.Location = Control.Input.Mouse.Position.Add(new Point(-_dragStart.X, -_dragStart.Y));
				_start = base.Location.Add(new Point(0, expandedItemsWidth2 - checkedItemsWidth2));
				_hasCollapseAnchor = true;
				break;
			}
			}
			ForceOnScreen();
			OnMoveAction?.Invoke(GetPersistedLocation());
		}

		private Point GetPersistedLocation()
		{
			ExpandType expandType = ExpandType;
			if ((expandType == ExpandType.RightToLeft || expandType == ExpandType.BottomToTop) && _hasCollapseAnchor)
			{
				return _start;
			}
			return base.Location;
		}

		private void ForceOnScreen()
		{
			Rectangle screen = Control.Graphics.SpriteScreen.LocalBounds;
			if (base.Location.X < screen.Left)
			{
				base.Location = new Point(screen.Left, base.Location.Y);
			}
			if (base.Location.X + base.Width > screen.Right)
			{
				base.Location = new Point(screen.Right - base.Width, base.Location.Y);
			}
			if (base.Location.Y < screen.Top)
			{
				base.Location = new Point(base.Location.X, screen.Top);
			}
			if (base.Location.Y + base.Height > screen.Bottom)
			{
				base.Location = new Point(base.Location.X, screen.Bottom - base.Height);
			}
		}
	}
}
