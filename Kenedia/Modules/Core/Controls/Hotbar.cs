using System;
using System.Collections.Generic;
using System.Linq;
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

		private bool _expandBar;

		private Point _dragStart;

		private bool _dragging;

		private Point _start;

		private Point _start_ItemWidth;

		private Point _delta;

		private Rectangle _expanderBackgroundBounds;

		private ExpandType _expandType;

		private SortType _sortType;

		protected readonly FlowPanel ItemsPanel;

		public ExpandType ExpandType
		{
			get
			{
				return _expandType;
			}
			set
			{
				Common.SetProperty(ref _expandType, value, new ValueChangedEventHandler<ExpandType>(OnExpandTypeChanged));
			}
		}

		public SortType SortType
		{
			get
			{
				return _sortType;
			}
			set
			{
				Common.SetProperty(ref _sortType, value, new ValueChangedEventHandler<SortType>(OnSortTypeCanged));
			}
		}

		public bool ExpandBar
		{
			get
			{
				return _expandBar;
			}
			set
			{
				Common.SetProperty(ref _expandBar, value, new ValueChangedEventHandler<bool>(OnExpandChanged));
			}
		}

		public ModifierKeys MoveModifier { get; set; } = ModifierKeys.Alt;


		public int MinButtonSize { get; set; } = 24;


		public Action<Point> OnMoveAction { get; set; }

		public Action OpenSettingsAction { get; set; }

		public Hotbar()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(155960);
			base.BackgroundImageColor = Color.get_DarkGray() * 0.8f;
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
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			if (ItemsPanel != null)
			{
				_resizeBarPending = true;
				ItemsPanel.Size = Point.get_Zero();
				base.Size = Point.get_Zero();
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
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			base.OnLeftMouseButtonPressed(e);
			_dragging = Control.Input.Keyboard.ActiveModifiers == MoveModifier;
			_dragStart = (_dragging ? base.RelativeMousePosition : Point.get_Zero());
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
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (ItemsPanel != null)
			{
				if (base.BackgroundImage != null)
				{
					Rectangle bounds = base.BackgroundImage.Bounds;
					int num = Math.Min(((Rectangle)(ref bounds)).get_Size().X, base.Width);
					int height = base.Height;
					bounds = base.BackgroundImage.Bounds;
					base.TextureRectangle = new Rectangle(50, 50, num, Math.Min(height, ((Rectangle)(ref bounds)).get_Size().Y));
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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			if (base.BackgroundImage != null)
			{
				Rectangle bounds = base.BackgroundImage.Bounds;
				int num = Math.Min(((Rectangle)(ref bounds)).get_Size().X, base.Width);
				int height = base.Height;
				bounds = base.BackgroundImage.Bounds;
				base.TextureRectangle = new Rectangle(50, 50, num, Math.Min(height, ((Rectangle)(ref bounds)).get_Size().Y));
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
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
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
					base.Location = _start.Add(new Point(-(expandedItemsWidth - checkedItemsWidth), 0));
					ItemsPanel.Width = expandedItemsWidth + padding;
				}
				else
				{
					_delta = new Point(_start_ItemWidth.X - checkedItemsWidth, 0);
					base.Location = _start.Add(_delta);
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
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			Math.Max(MinButtonSize, Math.Min(base.Width, base.Height) - _itemPadding - 10);
			if (base.BackgroundImage != null)
			{
				Rectangle bounds = base.BackgroundImage.Bounds;
				int num = Math.Min(((Rectangle)(ref bounds)).get_Size().X, base.Width);
				int height = base.Height;
				bounds = base.BackgroundImage.Bounds;
				base.TextureRectangle = new Rectangle(50, 50, num, Math.Min(height, ((Rectangle)(ref bounds)).get_Size().Y));
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
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
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
					base.Location = _start.Add(new Point(0, -(expandedItemsWidth - checkedItemsWidth)));
					ItemsPanel.Height = expandedItemsWidth + padding;
				}
				else
				{
					_delta = new Point(0, _start_ItemWidth.X - checkedItemsWidth);
					base.Location = _start.Add(_delta);
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			base.ClipsBounds = false;
			if (ExpandType == ExpandType.BottomToTop)
			{
				spriteBatch.Draw(ContentService.Textures.Pixel, _expanderBackgroundBounds.Add(new Rectangle(base.Location, Point.get_Zero())), Color.get_Black() * 0.5f);
				spriteBatch.DrawCenteredRotationOnCtrl(this, (Texture2D)_expander.Texture, _expander.Bounds, _expander.TextureRegion, Color.get_White(), 0f, flipVertically: true, flipHorizontally: false);
			}
			else
			{
				spriteBatch.Draw(ContentService.Textures.Pixel, _expanderBackgroundBounds.Add(new Rectangle(base.Location, Point.get_Zero())), Color.get_Black() * 0.5f);
				_expander?.Draw(this, spriteBatch, base.RelativeMousePosition);
			}
		}

		private void MoveBar()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
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
				break;
			}
			}
			ForceOnScreen();
			OnMoveAction?.Invoke(base.Location);
		}

		private void ForceOnScreen()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			Rectangle screen = Control.Graphics.SpriteScreen.LocalBounds;
			if (base.Location.X < ((Rectangle)(ref screen)).get_Left())
			{
				base.Location = new Point(((Rectangle)(ref screen)).get_Left(), base.Location.Y);
			}
			if (base.Location.X + base.Width > ((Rectangle)(ref screen)).get_Right())
			{
				base.Location = new Point(((Rectangle)(ref screen)).get_Right() - base.Width, base.Location.Y);
			}
			if (base.Location.Y < ((Rectangle)(ref screen)).get_Top())
			{
				base.Location = new Point(base.Location.X, ((Rectangle)(ref screen)).get_Top());
			}
			if (base.Location.Y + base.Height > ((Rectangle)(ref screen)).get_Bottom())
			{
				base.Location = new Point(base.Location.X, ((Rectangle)(ref screen)).get_Bottom() - base.Height);
			}
		}
	}
}
