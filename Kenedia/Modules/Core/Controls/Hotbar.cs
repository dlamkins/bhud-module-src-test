using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
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

		private readonly FlowPanel _itemsPanel;

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

		public ExpandType ExpandType
		{
			get
			{
				return _expandType;
			}
			set
			{
				Common.SetProperty(ref _expandType, value, OnExpandTypeChanged);
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
				Common.SetProperty(ref _expandBar, value, OnExpandChanged);
			}
		}

		public ModifierKeys MoveModifier { get; set; } = (ModifierKeys)2;


		public int MinButtonSize { get; set; } = 24;


		public Action<Point> OnMoveAction { get; set; }

		public Hotbar()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(155960);
			base.BackgroundImageColor = Color.get_DarkGray() * 0.8f;
			Dummy dummy = new Dummy();
			((Control)dummy).set_Parent((Container)(object)this);
			((Control)dummy).set_Size(new Point(16, 32));
			_expandDummy = dummy;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)2);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			_itemsPanel = flowPanel;
			OnExpandTypeChanged(this, new ValueChangedEventArgs<ExpandType>(ExpandType?.LeftToRight, ExpandType?.LeftToRight));
			ExpandType = ExpandType.BottomToTop;
		}

		private void OnExpandTypeChanged(object sender, ValueChangedEventArgs<ExpandType> e)
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
			if (_itemsPanel != null)
			{
				_resizeBarPending = true;
				((Control)_itemsPanel).set_Size(Point.get_Zero());
				((Control)this).set_Size(Point.get_Zero());
				switch (e.NewValue)
				{
				case ExpandType?.LeftToRight:
					_expander.Texture = AsyncTexture2D.FromAssetId(155909);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155910);
					_expander.TextureRegion = new Rectangle(new Point(0, 0), new Point(16, 32));
					((Control)_expandDummy).set_Size(new Point(16, 32));
					((FlowPanel)_itemsPanel).set_FlowDirection((ControlFlowDirection)2);
					((Container)_itemsPanel).set_WidthSizingMode((SizingMode)0);
					((Container)_itemsPanel).set_HeightSizingMode((SizingMode)1);
					_itemsPanel.ContentPadding = new RectangleDimensions(5, 4, 0, 4);
					((FlowPanel)_itemsPanel).set_ControlPadding(new Vector2(5f));
					base.ContentPadding = new RectangleDimensions(0);
					break;
				case ExpandType?.RightToLeft:
					_expander.Texture = AsyncTexture2D.FromAssetId(155906);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155907);
					_expander.TextureRegion = new Rectangle(new Point(16, 0), new Point(16, 32));
					((Control)_expandDummy).set_Size(new Point(16, 32));
					((FlowPanel)_itemsPanel).set_FlowDirection((ControlFlowDirection)5);
					((Container)_itemsPanel).set_WidthSizingMode((SizingMode)0);
					((Container)_itemsPanel).set_HeightSizingMode((SizingMode)1);
					_itemsPanel.ContentPadding = new RectangleDimensions(-5, 4, 5, 4);
					((FlowPanel)_itemsPanel).set_ControlPadding(new Vector2(5f));
					base.ContentPadding = new RectangleDimensions(0);
					break;
				case ExpandType?.TopToBottom:
					_expander.Texture = AsyncTexture2D.FromAssetId(155929);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155929);
					_expander.TextureRegion = new Rectangle(new Point(0, 8), new Point(32, 16));
					((Control)_expandDummy).set_Size(new Point(32, 16));
					((Container)_itemsPanel).set_WidthSizingMode((SizingMode)1);
					((Container)_itemsPanel).set_HeightSizingMode((SizingMode)0);
					((FlowPanel)_itemsPanel).set_FlowDirection((ControlFlowDirection)3);
					_itemsPanel.ContentPadding = new RectangleDimensions(5, 4, 5, 4);
					((FlowPanel)_itemsPanel).set_ControlPadding(new Vector2(5f));
					base.ContentPadding = new RectangleDimensions(0, 2);
					break;
				case ExpandType?.BottomToTop:
					_expander.Texture = AsyncTexture2D.FromAssetId(155929);
					_expander.HoveredTexture = AsyncTexture2D.FromAssetId(155929);
					_expander.TextureRegion = new Rectangle(new Point(0, 8), new Point(32, 16));
					((Control)_expandDummy).set_Size(new Point(32, 16));
					((Container)_itemsPanel).set_WidthSizingMode((SizingMode)1);
					((Container)_itemsPanel).set_HeightSizingMode((SizingMode)0);
					((FlowPanel)_itemsPanel).set_FlowDirection((ControlFlowDirection)7);
					_itemsPanel.ContentPadding = new RectangleDimensions(5, 0, 5, 2);
					((FlowPanel)_itemsPanel).set_ControlPadding(new Vector2(5f));
					base.ContentPadding = new RectangleDimensions(0, 2);
					break;
				}
				((Control)this).RecalculateLayout();
			}
		}

		private void OnExpandChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.NewValue)
			{
				_ = 1;
			}
			else
				_ = ((IEnumerable<Control>)((Container)_itemsPanel).get_Children()).FirstOrDefault((Control e) => e.get_Visible()) != null;
			_resizeBarPending = true;
			foreach (ICheckable c in ((IEnumerable)((Container)_itemsPanel).get_Children()).OfType<ICheckable>())
			{
				((Control)((c is Control) ? c : null)).set_Visible(e.NewValue || c.get_Checked());
			}
			((Control)this).RecalculateLayout();
		}

		public void AddItem(ICheckable item)
		{
			Control control = (Control)(object)((item is Control) ? item : null);
			if (control != null)
			{
				control.set_Parent((Container)(object)_itemsPanel);
				item.add_CheckedChanged((EventHandler<CheckChangedEvent>)Item_CheckedChanged);
			}
			((Control)this).RecalculateLayout();
		}

		private void Item_CheckedChanged(object sender, CheckChangedEvent e)
		{
			Control control = (Control)((sender is Control) ? sender : null);
			if (control != null)
			{
				control.set_Visible(ExpandBar || e.get_Checked());
			}
			((Control)this).RecalculateLayout();
		}

		public void RemoveItem(ICheckable item)
		{
			Control control = (Control)(object)((item is Control) ? item : null);
			if (control != null)
			{
				item.remove_CheckedChanged((EventHandler<CheckChangedEvent>)Item_CheckedChanged);
				control.Dispose();
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			_dragging = Control.get_Input().get_Keyboard().get_ActiveModifiers() == MoveModifier;
			_dragStart = (_dragging ? ((Control)this).get_RelativeMousePosition() : Point.get_Zero());
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			_dragging = false;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			ExpandBar = ((Control)this).get_MouseOver() || ((Control)_expandDummy).get_MouseOver();
			_dragging = _dragging && ((Control)this).get_MouseOver() && Control.get_Input().get_Keyboard().get_ActiveModifiers() == MoveModifier;
			if (_dragging)
			{
				MoveBar();
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
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_itemsPanel != null)
			{
				if (base.BackgroundImage != null)
				{
					Rectangle bounds = base.BackgroundImage.get_Bounds();
					int num = Math.Min(((Rectangle)(ref bounds)).get_Size().X, ((Control)this).get_Width());
					int height = ((Control)this).get_Height();
					bounds = base.BackgroundImage.get_Bounds();
					base.TextureRectangle = new Rectangle(50, 50, num, Math.Min(height, ((Rectangle)(ref bounds)).get_Size().Y));
				}
				switch (ExpandType)
				{
				case ExpandType.LeftToRight:
					_expander.Bounds = ((Control)_expandDummy).get_LocalBounds();
					CalculateLeftToRight();
					break;
				case ExpandType.RightToLeft:
					_expander.Bounds = ((Control)_expandDummy).get_LocalBounds();
					CalculateRightToLeft();
					break;
				case ExpandType.TopToBottom:
					_expander.Bounds = ((Control)_expandDummy).get_LocalBounds();
					CalculateTopToBottom();
					break;
				case ExpandType.BottomToTop:
					_expander.Bounds = new Rectangle(((Control)_expandDummy).get_Location().Add(new Point(0, ((Control)_expandDummy).get_Height() - 5)), ((Control)_expandDummy).get_Size());
					CalculateBottomToTop();
					break;
				}
			}
		}

		public int GetItemPanelSize(bool any = false, bool isChecked = false, bool vertical = false)
		{
			return (int)(isChecked ? (from e in ((IEnumerable)((Container)_itemsPanel).get_Children()).OfType<ICheckable>()
				where any || e.get_Checked()
				select e).Cast<Control>() : ((IEnumerable<Control>)((Container)_itemsPanel).get_Children()).Where((Control e) => any || e.get_Visible())).Sum((Control e) => (float)(vertical ? e.get_Height() : e.get_Width()) + (vertical ? ((FlowPanel)_itemsPanel).get_ControlPadding().Y : ((FlowPanel)_itemsPanel).get_ControlPadding().X));
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
				Rectangle bounds = base.BackgroundImage.get_Bounds();
				int num = Math.Min(((Rectangle)(ref bounds)).get_Size().X, ((Control)this).get_Width());
				int height = ((Control)this).get_Height();
				bounds = base.BackgroundImage.get_Bounds();
				base.TextureRectangle = new Rectangle(50, 50, num, Math.Min(height, ((Rectangle)(ref bounds)).get_Size().Y));
			}
			if (_itemsPanel != null)
			{
				IEnumerable<Control> visibleItems = ((IEnumerable<Control>)((Container)_itemsPanel).get_Children()).Where((Control e) => e.get_Visible());
				((Control)_itemsPanel).set_Size(new Point((int)visibleItems.Sum((Control e) => (float)e.get_Width() + ((FlowPanel)_itemsPanel).get_ControlPadding().X) + ((visibleItems != null && visibleItems.Count() > 0) ? _itemsPanel.ContentPadding.Horizontal : 0), ((Control)this).get_Height() - ((Container)this).get_AutoSizePadding().Y));
				((Control)_itemsPanel).set_Location(new Point(0, 0));
			}
			if (_expandDummy != null)
			{
				Dummy expandDummy = _expandDummy;
				FlowPanel itemsPanel = _itemsPanel;
				int num2 = Math.Max((itemsPanel != null) ? ((Control)itemsPanel).get_Right() : 0, 5);
				FlowPanel itemsPanel2 = _itemsPanel;
				((Control)expandDummy).set_Location(new Point(num2, (((itemsPanel2 != null) ? ((Control)itemsPanel2).get_Height() : ((Control)this).get_Height()) - ((Control)_expandDummy).get_Height()) / 2));
				_expanderBackgroundBounds = new Rectangle(((Control)_expandDummy).get_Left() - 2, base.BorderWidth.Top, ((Control)_expandDummy).get_Width() + 2, ((Control)this).get_Height() - base.BorderWidth.Vertical);
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
			bool isAnyVisible = ((IEnumerable<Control>)((Container)_itemsPanel).get_Children()).Any((Control e) => e.get_Visible());
			int expandedItemsWidth = GetItemPanelSize(any: true);
			int checkedItemsWidth = GetItemPanelSize(any: false, isChecked: true);
			int padding = (isAnyVisible ? _itemsPanel.ContentPadding.Horizontal : 0);
			if (_resizeBarPending)
			{
				if (ExpandBar)
				{
					_start = ((Control)this).get_Location();
					_start_ItemWidth = new Point(checkedItemsWidth, 0);
					((Control)this).set_Location(_start.Add(new Point(-(expandedItemsWidth - checkedItemsWidth), 0)));
					((Control)_itemsPanel).set_Width(expandedItemsWidth + padding);
				}
				else
				{
					_delta = new Point(_start_ItemWidth.X - checkedItemsWidth, 0);
					((Control)this).set_Location(_start.Add(_delta));
					((Control)_itemsPanel).set_Width(isAnyVisible ? (checkedItemsWidth + padding) : 0);
				}
				_resizeBarPending = false;
			}
			Dummy expandDummy = _expandDummy;
			FlowPanel itemsPanel = _itemsPanel;
			((Control)expandDummy).set_Location(new Point(0, (((itemsPanel != null) ? ((Control)itemsPanel).get_Height() : ((Control)this).get_Height()) - ((Control)_expandDummy).get_Height()) / 2));
			_expanderBackgroundBounds = new Rectangle(base.BorderWidth.Left, base.BorderWidth.Top, ((Control)_expandDummy).get_Width() + 2, ((Control)this).get_Height() - base.BorderWidth.Vertical);
			((Control)_itemsPanel).set_Location(new Point(((Control)_expandDummy).get_Right() + base.BorderWidth.Horizontal, 0));
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
			Math.Max(MinButtonSize, Math.Min(((Control)this).get_Width(), ((Control)this).get_Height()) - _itemPadding - 10);
			if (base.BackgroundImage != null)
			{
				Rectangle bounds = base.BackgroundImage.get_Bounds();
				int num = Math.Min(((Rectangle)(ref bounds)).get_Size().X, ((Control)this).get_Width());
				int height = ((Control)this).get_Height();
				bounds = base.BackgroundImage.get_Bounds();
				base.TextureRectangle = new Rectangle(50, 50, num, Math.Min(height, ((Rectangle)(ref bounds)).get_Size().Y));
			}
			if (_itemsPanel != null)
			{
				IEnumerable<Control> visibleItems = ((IEnumerable<Control>)((Container)_itemsPanel).get_Children()).Where((Control e) => e.get_Visible());
				((Control)_itemsPanel).set_Size(new Point(((Control)this).get_Width() - ((Container)this).get_AutoSizePadding().X, (int)visibleItems.Sum((Control e) => (float)e.get_Width() + ((FlowPanel)_itemsPanel).get_ControlPadding().X) + ((visibleItems != null && visibleItems.Count() > 0) ? _itemsPanel.ContentPadding.Horizontal : 0)));
				((Control)_itemsPanel).set_Location(new Point(0, 0));
			}
			if (_expandDummy != null)
			{
				Dummy expandDummy = _expandDummy;
				FlowPanel itemsPanel = _itemsPanel;
				int num2 = (((itemsPanel != null) ? ((Control)itemsPanel).get_Width() : ((Control)this).get_Width()) - ((Control)_expandDummy).get_Width()) / 2;
				FlowPanel itemsPanel2 = _itemsPanel;
				((Control)expandDummy).set_Location(new Point(num2, Math.Max((itemsPanel2 != null) ? ((Control)itemsPanel2).get_Bottom() : 0, 5) - 5));
				_expanderBackgroundBounds = new Rectangle(base.BorderWidth.Left, ((Control)this).get_Height() - ((Control)_expandDummy).get_Height() - base.BorderWidth.Bottom, ((Control)this).get_Width() - base.BorderWidth.Horizontal, ((Control)_expandDummy).get_Height());
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
			bool isAnyVisible = ((IEnumerable<Control>)((Container)_itemsPanel).get_Children()).Any((Control e) => e.get_Visible());
			int expandedItemsWidth = GetItemPanelSize(any: true, isChecked: false, vertical: true);
			int checkedItemsWidth = GetItemPanelSize(any: false, isChecked: true, vertical: true);
			int padding = (isAnyVisible ? _itemsPanel.ContentPadding.Vertical : 0);
			if (_resizeBarPending)
			{
				if (ExpandBar)
				{
					_start = ((Control)this).get_Location();
					_start_ItemWidth = new Point(checkedItemsWidth, 0);
					((Control)this).set_Location(_start.Add(new Point(0, -(expandedItemsWidth - checkedItemsWidth))));
					((Control)_itemsPanel).set_Height(expandedItemsWidth + padding);
				}
				else
				{
					_delta = new Point(0, _start_ItemWidth.X - checkedItemsWidth);
					((Control)this).set_Location(_start.Add(_delta));
					((Control)_itemsPanel).set_Height(isAnyVisible ? (checkedItemsWidth + padding) : 0);
				}
				_resizeBarPending = false;
			}
			((Control)_expandDummy).set_Location(new Point((((Control)this).get_Width() - ((Container)this).get_AutoSizePadding().X - ((Control)_expandDummy).get_Width()) / 2, 0));
			_expanderBackgroundBounds = new Rectangle(base.BorderWidth.Left, base.BorderWidth.Top, ((Control)this).get_Width() - base.BorderWidth.Horizontal, ((Control)_expandDummy).get_Height());
			((Control)_itemsPanel).set_Location(new Point(0, ((Control)_expandDummy).get_Bottom()));
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
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
			((Control)this).set_ClipsBounds(false);
			if (ExpandType == ExpandType.BottomToTop)
			{
				spriteBatch.Draw(Textures.get_Pixel(), RectangleExtension.Add(_expanderBackgroundBounds, new Rectangle(((Control)this).get_Location(), Point.get_Zero())), Color.get_Black() * 0.5f);
				spriteBatch.DrawCenteredRotationOnCtrl((Control)(object)this, AsyncTexture2D.op_Implicit(_expander.Texture), _expander.Bounds, _expander.TextureRegion, Color.get_White(), 0f, flipVertically: true, flipHorizontally: false);
			}
			else
			{
				spriteBatch.Draw(Textures.get_Pixel(), RectangleExtension.Add(_expanderBackgroundBounds, new Rectangle(((Control)this).get_Location(), Point.get_Zero())), Color.get_Black() * 0.5f);
				_expander?.Draw((Control)(object)this, spriteBatch, ((Control)this).get_RelativeMousePosition());
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
			((Control)this).RecalculateLayout();
			switch (ExpandType)
			{
			case ExpandType.LeftToRight:
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(-_dragStart.X, -_dragStart.Y)));
				break;
			case ExpandType.RightToLeft:
			{
				int expandedItemsWidth = GetItemPanelSize(any: true);
				int checkedItemsWidth = GetItemPanelSize(any: false, isChecked: true);
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(-_dragStart.X, -_dragStart.Y)));
				_start = ((Control)this).get_Location().Add(new Point(expandedItemsWidth - checkedItemsWidth, 0));
				break;
			}
			case ExpandType.TopToBottom:
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(-_dragStart.X, -_dragStart.Y)));
				break;
			case ExpandType.BottomToTop:
			{
				int expandedItemsWidth2 = GetItemPanelSize(any: true, isChecked: false, vertical: true);
				int checkedItemsWidth2 = GetItemPanelSize(any: false, isChecked: true, vertical: true);
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(-_dragStart.X, -_dragStart.Y)));
				_start = ((Control)this).get_Location().Add(new Point(0, expandedItemsWidth2 - checkedItemsWidth2));
				break;
			}
			}
			ForceOnScreen();
			OnMoveAction?.Invoke(((Control)this).get_Location());
		}

		private void ForceOnScreen()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			Rectangle screen = ((Control)Control.get_Graphics().get_SpriteScreen()).get_LocalBounds();
			if (((Control)this).get_Location().X < ((Rectangle)(ref screen)).get_Left())
			{
				((Control)this).set_Location(new Point(((Rectangle)(ref screen)).get_Left(), ((Control)this).get_Location().Y));
			}
			else if (((Control)this).get_Location().X + ((Control)this).get_Width() > ((Rectangle)(ref screen)).get_Right())
			{
				((Control)this).set_Location(new Point(((Rectangle)(ref screen)).get_Right() - ((Control)this).get_Width(), ((Control)this).get_Location().Y));
			}
			else if (((Control)this).get_Location().Y < ((Rectangle)(ref screen)).get_Top())
			{
				((Control)this).set_Location(new Point(((Control)this).get_Location().X, ((Rectangle)(ref screen)).get_Top()));
			}
			else if (((Control)this).get_Location().Y + ((Control)this).get_Height() > ((Rectangle)(ref screen)).get_Bottom())
			{
				((Control)this).set_Location(new Point(((Control)this).get_Location().X, ((Rectangle)(ref screen)).get_Bottom() - ((Control)this).get_Height()));
			}
		}
	}
}
