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

namespace Kenedia.Modules.Core.Controls
{
	public class Scrollbar : Blish_HUD.Controls.Control
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

		private const int s_control_width = 12;

		private const int s_min_length = 32;

		private const int s_cap_slack = 6;

		private const int s_scroll_arrow = 50;

		private const int s_scroll_cont_arrow = 10;

		private const int s_scroll_cont_track = 15;

		private const int s_scroll_wheel = 30;

		private static readonly TextureRegion2D s_textureTrack = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("scrollbar/sb-track");

		private static readonly TextureRegion2D s_textureUpArrow = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("scrollbar/sb-arrow-up");

		private static readonly TextureRegion2D s_textureDownArrow = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("scrollbar/sb-arrow-down");

		private static readonly TextureRegion2D s_textureBar = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("scrollbar/sb-bar-active");

		private static readonly TextureRegion2D s_textureThumb = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("scrollbar/sb-thumb");

		private static readonly TextureRegion2D s_textureTopCap = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("scrollbar/sb-cap-top");

		private static readonly TextureRegion2D s_textureBottomCap = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("scrollbar/sb-cap-bottom");

		private ClickFocus _scrollFocus;

		private Tween _targetScrollDistanceAnim;

		private float _targetScrollDistance;

		private float _scrollDistance;

		private int _scrollbarHeight = 32;

		private double _scrollbarPercent = 1.0;

		private Container _associatedContainer;

		private int _scrollingOffset;

		private Rectangle _upArrowBounds;

		private Rectangle _downArrowBounds;

		private Rectangle _barBounds;

		private Rectangle _trackBounds;

		private double _lastClickTime;

		private int _containerLowestContent;

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
				if (_targetScrollDistanceAnim != null)
				{
					return _targetScrollDistance;
				}
				return _scrollDistance;
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
				if (SetProperty(ref _scrollDistance, MathHelper.Clamp(value, 0f, 1f), invalidateLayout: true, "ScrollDistance"))
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
				if (SetProperty(ref _scrollbarHeight, value, invalidateLayout: true, "ScrollbarHeight"))
				{
					RecalculateScrollbarSize();
					UpdateAssocContainer();
				}
			}
		}

		public bool Drawn
		{
			get
			{
				if (base.Visible)
				{
					return _scrollbarPercent < 0.99;
				}
				return false;
			}
		}

		public int ScrollbarWidth => _barBounds.Width;

		public Container AssociatedContainer
		{
			get
			{
				return _associatedContainer;
			}
			set
			{
				SetProperty(ref _associatedContainer, value, invalidateLayout: false, "AssociatedContainer");
			}
		}

		private int ContainerContentDiff => _containerLowestContent - _associatedContainer.ContentRegion.Height;

		private int TrackLength => _size.Y - s_textureUpArrow.get_Height() - s_textureDownArrow.get_Height();

		public Scrollbar(Container container)
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
			base.Width = 12;
			Blish_HUD.Controls.Control.Input.Mouse.LeftMouseButtonReleased += MouseOnLeftMouseButtonReleased;
			_associatedContainer.MouseWheelScrolled += HandleWheelScroll;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Blish_HUD.Controls.Control.Input.Mouse.LeftMouseButtonReleased -= MouseOnLeftMouseButtonReleased;
			_associatedContainer.MouseWheelScrolled -= HandleWheelScroll;
		}

		protected override void OnLeftMouseButtonPressed(Blish_HUD.Input.MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			base.OnLeftMouseButtonPressed(e);
			Point position = Blish_HUD.Controls.Control.Input.Mouse.Position;
			Rectangle absoluteBounds = base.AbsoluteBounds;
			ScrollFocus = GetScrollFocus(position - ((Rectangle)(ref absoluteBounds)).get_Location());
			_lastClickTime = GameService.Overlay.CurrentGameTime.get_TotalGameTime().TotalMilliseconds;
		}

		private void MouseOnLeftMouseButtonReleased(object sender, Blish_HUD.Input.MouseEventArgs e)
		{
			ScrollFocus = ClickFocus.None;
		}

		protected override void OnMouseWheelScrolled(Blish_HUD.Input.MouseEventArgs e)
		{
			HandleWheelScroll(this, e);
			base.OnMouseWheelScrolled(e);
		}

		private void HandleWheelScroll(object sender, Blish_HUD.Input.MouseEventArgs e)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			if (!base.Visible || _scrollbarPercent > 0.99)
			{
				return;
			}
			Blish_HUD.Controls.Control ctrl = (Blish_HUD.Controls.Control)sender;
			while (ctrl != _associatedContainer && ctrl != null)
			{
				if (ctrl is Panel)
				{
					return;
				}
				ctrl = ctrl.Parent;
			}
			MouseState state = GameService.Input.Mouse.State;
			if (((MouseState)(ref state)).get_ScrollWheelValue() != 0)
			{
				state = GameService.Input.Mouse.State;
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
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			Point position = Blish_HUD.Controls.Control.Input.Mouse.Position;
			Rectangle absoluteBounds = base.AbsoluteBounds;
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
			Action<int> getScrollAction(bool c)
			{
				if (!c)
				{
					return new Action<int>(scroll);
				}
				return new Action<int>(ScrollAnimated);
			}
			void scroll(int pixels)
			{
				ScrollDistance = ((float)ContainerContentDiff * ScrollDistance + (float)pixels) / (float)ContainerContentDiff;
			}
		}

		private void ScrollAnimated(int pixels)
		{
			TargetScrollDistance = ((float)ContainerContentDiff * ScrollDistance + (float)pixels) / (float)ContainerContentDiff;
			_targetScrollDistanceAnim = Blish_HUD.Controls.Control.Animation.Tweener.Tween(this, new
			{
				ScrollDistance = TargetScrollDistance
			}, 0f).Ease(Ease.QuadOut);
		}

		protected override CaptureType CapturesInput()
		{
			return CaptureType.Mouse | CaptureType.MouseWheel;
		}

		private void UpdateAssocContainer()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			AssociatedContainer.VerticalScrollOffset = (int)Math.Floor((float)(_containerLowestContent - AssociatedContainer.ContentRegion.Height) * ScrollDistance);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			base.DoUpdate(gameTime);
			double timeDiff = gameTime.get_TotalGameTime().TotalMilliseconds - _lastClickTime;
			if (ScrollFocus == ClickFocus.Bar)
			{
				HandleClickScroll(clicked: false);
			}
			else if (timeDiff > 200.0)
			{
				HandleClickScroll(clicked: false);
			}
			Invalidate();
		}

		public override void RecalculateLayout()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			double scrollbarPercent = _scrollbarPercent;
			RecalculateScrollbarSize();
			if (scrollbarPercent != _scrollbarPercent && _associatedContainer != null)
			{
				ScrollDistance = 0f;
				TargetScrollDistance = 0f;
			}
			_upArrowBounds = new Rectangle(base.Width / 2 - s_textureUpArrow.get_Width() / 2, 0, s_textureUpArrow.get_Width(), s_textureUpArrow.get_Height());
			_downArrowBounds = new Rectangle(base.Width / 2 - s_textureDownArrow.get_Width() / 2, base.Height - s_textureDownArrow.get_Height(), s_textureDownArrow.get_Width(), s_textureDownArrow.get_Height());
			_barBounds = new Rectangle(base.Width / 2 - s_textureBar.get_Width() / 2, (int)(ScrollDistance * (float)(TrackLength - ScrollbarHeight)) + s_textureUpArrow.get_Height(), s_textureBar.get_Width(), ScrollbarHeight);
			_trackBounds = new Rectangle(base.Width / 2 - s_textureTrack.get_Width() / 2, ((Rectangle)(ref _upArrowBounds)).get_Bottom(), s_textureTrack.get_Width(), TrackLength);
		}

		private void RecalculateScrollbarSize()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (_associatedContainer == null)
			{
				return;
			}
			Blish_HUD.Controls.Control[] tempContainerChidlren = _associatedContainer.Children.ToArray();
			_containerLowestContent = 0;
			for (int i = 0; i < tempContainerChidlren.Length; i++)
			{
				ref Blish_HUD.Controls.Control child = ref tempContainerChidlren[i];
				if (child.Visible)
				{
					_containerLowestContent = Math.Max(_containerLowestContent, child.Bottom);
				}
			}
			_containerLowestContent = Math.Max(_containerLowestContent, _associatedContainer.ContentRegion.Height);
			_scrollbarPercent = (double)_associatedContainer.ContentRegion.Height / (double)_containerLowestContent;
			ScrollbarHeight = (int)Math.Max(Math.Floor((double)TrackLength * _scrollbarPercent) - 1.0, 32.0);
			UpdateAssocContainer();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
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
				Color drawTint = (((ScrollFocus == ClickFocus.None && base.MouseOver) || (_associatedContainer != null && _associatedContainer.MouseOver)) ? Color.get_White() : ContentService.Colors.Darkened(0.6f));
				drawTint = ((ScrollFocus != 0) ? ContentService.Colors.Darkened(0.9f) : drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureTrack, _trackBounds);
				spriteBatch.DrawOnCtrl(this, s_textureUpArrow, _upArrowBounds, drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureDownArrow, _downArrowBounds, drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureBar, _barBounds, drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureTopCap, new Rectangle(base.Width / 2 - s_textureTopCap.get_Width() / 2, ((Rectangle)(ref _barBounds)).get_Top() - 6, s_textureTopCap.get_Width(), s_textureTopCap.get_Height()));
				spriteBatch.DrawOnCtrl(this, s_textureBottomCap, new Rectangle(base.Width / 2 - s_textureBottomCap.get_Width() / 2, ((Rectangle)(ref _barBounds)).get_Bottom() - s_textureBottomCap.get_Height() + 6, s_textureBottomCap.get_Width(), s_textureBottomCap.get_Height()));
				spriteBatch.DrawOnCtrl(this, s_textureThumb, new Rectangle(base.Width / 2 - s_textureThumb.get_Width() / 2, ((Rectangle)(ref _barBounds)).get_Top() + (ScrollbarHeight / 2 - s_textureThumb.get_Height() / 2), s_textureThumb.get_Width(), s_textureThumb.get_Height()), drawTint);
			}
		}
	}
}
