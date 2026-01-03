using System;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;

namespace LoreBridge.Controls
{
	public class ScrollbarCustom : Control
	{
		private enum ClickFocus
		{
			None,
			UpArrow,
			DownArrow,
			AboveBar,
			BelowBar,
			Bar
		}

		private const int CONTROL_WIDTH = 12;

		private const int MIN_LENGTH = 32;

		private const int CAP_SLACK = 6;

		private const int SCROLL_ARROW = 50;

		private const int SCROLL_CONT_ARROW = 10;

		private const int SCROLL_CONT_TRACK = 15;

		private const int SCROLL_WHEEL = 30;

		private Container _associatedContainer;

		private Rectangle _barBounds;

		private int _containerLowestContent;

		private Rectangle _downArrowBounds;

		private double _lastClickTime;

		private int _scrollbarHeight = 32;

		private double _scrollbarPercent = 1.0;

		private float _scrollDistance;

		private ClickFocus _scrollFocus;

		private int _scrollingOffset;

		private float _targetScrollDistance;

		private Tween _targetScrollDistanceAnim;

		private Rectangle _trackBounds;

		private Rectangle _upArrowBounds;

		private static readonly TextureRegion2D _textureTrack = Control.TextureAtlasControl.GetRegion("scrollbar/sb-track");

		private static readonly TextureRegion2D _textureUpArrow = Control.TextureAtlasControl.GetRegion("scrollbar/sb-arrow-up");

		private static readonly TextureRegion2D _textureDownArrow = Control.TextureAtlasControl.GetRegion("scrollbar/sb-arrow-down");

		private static readonly TextureRegion2D _textureBar = Control.TextureAtlasControl.GetRegion("scrollbar/sb-bar-active");

		private static readonly TextureRegion2D _textureThumb = Control.TextureAtlasControl.GetRegion("scrollbar/sb-thumb");

		private static readonly TextureRegion2D _textureTopCap = Control.TextureAtlasControl.GetRegion("scrollbar/sb-cap-top");

		private static readonly TextureRegion2D _textureBottomCap = Control.TextureAtlasControl.GetRegion("scrollbar/sb-cap-bottom");

		private ClickFocus ScrollFocus
		{
			get
			{
				return _scrollFocus;
			}
			set
			{
				_scrollFocus = value;
				HandleClickScroll(clicked: true);
			}
		}

		private float TargetScrollDistance
		{
			get
			{
				if (_targetScrollDistanceAnim == null)
				{
					return _scrollDistance;
				}
				return _targetScrollDistance;
			}
			set
			{
				float aVal = MathHelper.Clamp(value, 0f, 1f);
				if (_associatedContainer != null && _targetScrollDistance != aVal)
				{
					_targetScrollDistance = aVal;
				}
			}
		}

		public float ScrollDistance
		{
			get
			{
				return _scrollDistance;
			}
			set
			{
				if (((Control)this).SetProperty<float>(ref _scrollDistance, MathHelper.Clamp(value, 0f, 1f), true, "ScrollDistance"))
				{
					_targetScrollDistance = _scrollDistance;
				}
				UpdateAssocContainer();
			}
		}

		private int ScrollbarHeight
		{
			get
			{
				return _scrollbarHeight;
			}
			set
			{
				if (((Control)this).SetProperty<int>(ref _scrollbarHeight, value, true, "ScrollbarHeight"))
				{
					RecalculateScrollbarSize();
					UpdateAssocContainer();
				}
			}
		}

		public Container AssociatedContainer
		{
			get
			{
				return _associatedContainer;
			}
			set
			{
				((Control)this).SetProperty<Container>(ref _associatedContainer, value, false, "AssociatedContainer");
			}
		}

		private int _containerContentDiff => _containerLowestContent - _associatedContainer.get_ContentRegion().Height;

		private int TrackLength => base._size.Y - _textureUpArrow.get_Height() - _textureDownArrow.get_Height();

		public ScrollbarCustom(Container container)
			: this()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			_associatedContainer = container;
			_upArrowBounds = Rectangle.get_Empty();
			_downArrowBounds = Rectangle.get_Empty();
			_barBounds = Rectangle.get_Empty();
			_trackBounds = Rectangle.get_Empty();
			((Control)this).set_Width(12);
			Control.get_Input().get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseOnLeftMouseButtonReleased);
			((Control)_associatedContainer).add_MouseWheelScrolled((EventHandler<MouseEventArgs>)HandleWheelScroll);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Control.get_Input().get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseOnLeftMouseButtonReleased);
			((Control)_associatedContainer).remove_MouseWheelScrolled((EventHandler<MouseEventArgs>)HandleWheelScroll);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Point position = Control.get_Input().get_Mouse().get_Position();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			ScrollFocus = GetScrollFocus(position - ((Rectangle)(ref absoluteBounds)).get_Location());
			_lastClickTime = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		private void MouseOnLeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			ScrollFocus = ClickFocus.None;
		}

		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			HandleWheelScroll(this, e);
			((Control)this).OnMouseWheelScrolled(e);
		}

		private void HandleWheelScroll(object sender, MouseEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible() || _scrollbarPercent > 0.99)
			{
				return;
			}
			Control ctrl = (Control)sender;
			while (ctrl != _associatedContainer && ctrl != null)
			{
				if (ctrl is Panel)
				{
					return;
				}
				ctrl = (Control)(object)ctrl.get_Parent();
			}
			MouseState state = GameService.Input.get_Mouse().get_State();
			if (((MouseState)(ref state)).get_ScrollWheelValue() != 0)
			{
				state = GameService.Input.get_Mouse().get_State();
				float normalScroll = Math.Sign(((MouseState)(ref state)).get_ScrollWheelValue());
				ScrollAnimated((int)normalScroll * -30 * SystemInformation.MouseWheelScrollLines);
			}
		}

		private ClickFocus GetScrollFocus(Point mousePos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			if (((Rectangle)(ref _trackBounds)).Contains(mousePos) && !((Rectangle)(ref _barBounds)).Contains(mousePos) && _barBounds.Y < mousePos.Y)
			{
				return ClickFocus.AboveBar;
			}
			if (((Rectangle)(ref _trackBounds)).Contains(mousePos) && !((Rectangle)(ref _barBounds)).Contains(mousePos) && _barBounds.Y > mousePos.Y)
			{
				return ClickFocus.BelowBar;
			}
			if (((Rectangle)(ref _barBounds)).Contains(mousePos))
			{
				return ClickFocus.Bar;
			}
			if (((Rectangle)(ref _upArrowBounds)).Contains(mousePos))
			{
				return ClickFocus.UpArrow;
			}
			if (((Rectangle)(ref _downArrowBounds)).Contains(mousePos))
			{
				return ClickFocus.DownArrow;
			}
			return ClickFocus.None;
		}

		private void HandleClickScroll(bool clicked)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			Action<int> scroll = delegate(int pixels)
			{
				ScrollDistance = ((float)_containerContentDiff * ScrollDistance + (float)pixels) / (float)_containerContentDiff;
			};
			Func<bool, Action<int>> getScrollAction = (bool c) => (!c) ? scroll : new Action<int>(ScrollAnimated);
			Point position = Control.get_Input().get_Mouse().get_Position();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			Point relMousePos = position - ((Rectangle)(ref absoluteBounds)).get_Location();
			if (ScrollFocus == ClickFocus.None)
			{
				return;
			}
			if (ScrollFocus == ClickFocus.BelowBar)
			{
				if (GetScrollFocus(relMousePos) == ClickFocus.BelowBar)
				{
					getScrollAction(clicked)(clicked ? (-ScrollbarHeight) : (-15));
				}
			}
			else if (ScrollFocus == ClickFocus.AboveBar)
			{
				if (GetScrollFocus(relMousePos) == ClickFocus.AboveBar)
				{
					getScrollAction(clicked)(clicked ? ScrollbarHeight : 15);
				}
			}
			else if (ScrollFocus == ClickFocus.UpArrow)
			{
				getScrollAction(clicked)(clicked ? (-50) : (-10));
			}
			else if (ScrollFocus == ClickFocus.DownArrow)
			{
				getScrollAction(clicked)(clicked ? 50 : 10);
			}
			else if (ScrollFocus == ClickFocus.Bar)
			{
				if (clicked)
				{
					_scrollingOffset = relMousePos.Y - _barBounds.Y;
				}
				relMousePos = relMousePos - new Point(0, _scrollingOffset) - ((Rectangle)(ref _trackBounds)).get_Location();
				ScrollDistance = (float)relMousePos.Y / (float)(TrackLength - ScrollbarHeight);
				TargetScrollDistance = ScrollDistance;
			}
		}

		private void ScrollAnimated(int pixels)
		{
			TargetScrollDistance = ((float)_containerContentDiff * ScrollDistance + (float)pixels) / (float)_containerContentDiff;
			_targetScrollDistanceAnim = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<ScrollbarCustom>(this, (object)new
			{
				ScrollDistance = TargetScrollDistance
			}, 0f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)12;
		}

		private void UpdateAssocContainer()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			AssociatedContainer.set_VerticalScrollOffset((int)Math.Floor((float)(_containerLowestContent - AssociatedContainer.get_ContentRegion().Height) * ScrollDistance));
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			double timeDiff = gameTime.get_TotalGameTime().TotalMilliseconds - _lastClickTime;
			if (ScrollFocus == ClickFocus.Bar)
			{
				HandleClickScroll(clicked: false);
			}
			else if (timeDiff > 200.0)
			{
				HandleClickScroll(clicked: false);
			}
			((Control)this).Invalidate();
		}

		public override void RecalculateLayout()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			RecalculateScrollbarSize();
			_upArrowBounds = new Rectangle(((Control)this).get_Width() / 2 - _textureUpArrow.get_Width() / 2, 0, _textureUpArrow.get_Width(), _textureUpArrow.get_Height());
			_downArrowBounds = new Rectangle(((Control)this).get_Width() / 2 - _textureDownArrow.get_Width() / 2, ((Control)this).get_Height() - _textureDownArrow.get_Height(), _textureDownArrow.get_Width(), _textureDownArrow.get_Height());
			_barBounds = new Rectangle(((Control)this).get_Width() / 2 - _textureBar.get_Width() / 2, (int)(ScrollDistance * (float)(TrackLength - ScrollbarHeight)) + _textureUpArrow.get_Height(), _textureBar.get_Width(), ScrollbarHeight);
			_trackBounds = new Rectangle(((Control)this).get_Width() / 2 - _textureTrack.get_Width() / 2, ((Rectangle)(ref _upArrowBounds)).get_Bottom(), _textureTrack.get_Width(), TrackLength);
		}

		private void RecalculateScrollbarSize()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (_associatedContainer == null)
			{
				return;
			}
			Control[] tempContainerChidlren = _associatedContainer.get_Children().ToArray();
			_containerLowestContent = 0;
			for (int i = 0; i < tempContainerChidlren.Length; i++)
			{
				ref Control child = ref tempContainerChidlren[i];
				if (child.get_Visible())
				{
					_containerLowestContent = Math.Max(_containerLowestContent, child.get_Bottom());
				}
			}
			_containerLowestContent = Math.Max(_containerLowestContent, _associatedContainer.get_ContentRegion().Height);
			_scrollbarPercent = (double)_associatedContainer.get_ContentRegion().Height / (double)_containerLowestContent;
			ScrollbarHeight = (int)Math.Max(Math.Floor((double)TrackLength * _scrollbarPercent) - 1.0, 32.0);
			UpdateAssocContainer();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			if (!(_scrollbarPercent > 0.99))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTrack, _trackBounds);
				Color drawTint = (((ScrollFocus == ClickFocus.None && ((Control)this).get_MouseOver()) || (_associatedContainer != null && ((Control)_associatedContainer).get_MouseOver())) ? Color.get_White() : Colors.Darkened(0.6f));
				drawTint = ((ScrollFocus != 0) ? Colors.Darkened(0.9f) : drawTint);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureUpArrow, _upArrowBounds, drawTint);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureDownArrow, _downArrowBounds, drawTint);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureBar, _barBounds, drawTint);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTopCap, new Rectangle(((Control)this).get_Width() / 2 - _textureTopCap.get_Width() / 2, ((Rectangle)(ref _barBounds)).get_Top() - 6, _textureTopCap.get_Width(), _textureTopCap.get_Height()));
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureBottomCap, new Rectangle(((Control)this).get_Width() / 2 - _textureBottomCap.get_Width() / 2, ((Rectangle)(ref _barBounds)).get_Bottom() - _textureBottomCap.get_Height() + 6, _textureBottomCap.get_Width(), _textureBottomCap.get_Height()));
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureThumb, new Rectangle(((Control)this).get_Width() / 2 - _textureThumb.get_Width() / 2, ((Rectangle)(ref _barBounds)).get_Top() + (ScrollbarHeight / 2 - _textureThumb.get_Height() / 2), _textureThumb.get_Width(), _textureThumb.get_Height()), drawTint);
			}
		}
	}
}
