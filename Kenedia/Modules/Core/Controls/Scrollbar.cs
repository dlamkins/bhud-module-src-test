using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Blish_HUD.Input;
using Glide;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

		private Tween _targetScrollDistanceAnim;

		private float _targetScrollDistance;

		private double _scrollbarPercent;

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
			[CompilerGenerated]
			get
			{
				return _003CScrollFocus_003Ek__BackingField;
			}
			set
			{
				_003CScrollFocus_003Ek__BackingField = value;
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
				return ScrollDistance;
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
			[CompilerGenerated]
			get
			{
				return _003CScrollDistance_003Ek__BackingField;
			}
			set
			{
				if (SetProperty(ref _003CScrollDistance_003Ek__BackingField, MathHelper.Clamp(value, 0f, 1f), invalidateLayout: true, "ScrollDistance"))
				{
					_targetScrollDistance = _003CScrollDistance_003Ek__BackingField;
				}
				UpdateAssocContainer();
			}
		}

		private int ScrollbarHeight
		{
			[CompilerGenerated]
			get
			{
				return _003CScrollbarHeight_003Ek__BackingField;
			}
			set
			{
				if (Common.SetProperty(_003CScrollbarHeight_003Ek__BackingField, value, delegate(int v)
				{
					_003CScrollbarHeight_003Ek__BackingField = v;
				}))
				{
					Invalidate();
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

		private int TrackLength => _size.Y - s_textureUpArrow.Height - s_textureDownArrow.Height;

		public Scrollbar(Container container)
		{
			_003CScrollbarHeight_003Ek__BackingField = 32;
			_scrollbarPercent = 1.0;
			base._002Ector();
			_associatedContainer = container;
			_upArrowBounds = Rectangle.Empty;
			_downArrowBounds = Rectangle.Empty;
			_barBounds = Rectangle.Empty;
			_trackBounds = Rectangle.Empty;
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
			base.OnLeftMouseButtonPressed(e);
			ScrollFocus = GetScrollFocus(Blish_HUD.Controls.Control.Input.Mouse.Position - base.AbsoluteBounds.Location);
			_lastClickTime = GameService.Overlay.CurrentGameTime.TotalGameTime.TotalMilliseconds;
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
			if (GameService.Input.Mouse.State.ScrollWheelValue != 0)
			{
				float normalScroll = Math.Sign(GameService.Input.Mouse.State.ScrollWheelValue);
				ScrollAnimated((int)normalScroll * -30 * SystemInformation.MouseWheelScrollLines);
			}
		}

		private ClickFocus GetScrollFocus(Point mousePos)
		{
			Point point = mousePos;
			if (_trackBounds.Contains(point) && !_barBounds.Contains(point) && _barBounds.Y < point.Y)
			{
				return ClickFocus.AboveBar;
			}
			Point point2 = mousePos;
			if (_trackBounds.Contains(point2) && !_barBounds.Contains(point2) && _barBounds.Y > point2.Y)
			{
				return ClickFocus.BelowBar;
			}
			if (_barBounds.Contains(mousePos))
			{
				return ClickFocus.Bar;
			}
			if (_upArrowBounds.Contains(mousePos))
			{
				return ClickFocus.UpArrow;
			}
			if (_downArrowBounds.Contains(mousePos))
			{
				return ClickFocus.DownArrow;
			}
			return ClickFocus.None;
		}

		private void HandleClickScroll(bool clicked)
		{
			Point relMousePos = Blish_HUD.Controls.Control.Input.Mouse.Position - base.AbsoluteBounds.Location;
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
				ScrollDistance = (float)(relMousePos - new Point(0, _scrollingOffset) - _trackBounds.Location).Y / (float)(TrackLength - ScrollbarHeight);
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
			AssociatedContainer.VerticalScrollOffset = (int)Math.Floor((float)(_containerLowestContent - AssociatedContainer.ContentRegion.Height) * ScrollDistance);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			base.DoUpdate(gameTime);
			double timeDiff = gameTime.TotalGameTime.TotalMilliseconds - _lastClickTime;
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
			double scrollbarPercent = _scrollbarPercent;
			int previousVerticalOffset = _associatedContainer?.VerticalScrollOffset ?? 0;
			RecalculateScrollbarSize();
			if (scrollbarPercent != _scrollbarPercent && _associatedContainer != null)
			{
				int maxOffset = Math.Max(_containerLowestContent - _associatedContainer.ContentRegion.Height, 0);
				if (maxOffset != 0)
				{
					float preservedDistance = (TargetScrollDistance = (ScrollDistance = (float)Math.Min(previousVerticalOffset, maxOffset) / (float)maxOffset));
				}
				else
				{
					ScrollDistance = 0f;
					TargetScrollDistance = 0f;
				}
			}
			_upArrowBounds = new Rectangle(base.Width / 2 - s_textureUpArrow.Width / 2, 0, s_textureUpArrow.Width, s_textureUpArrow.Height);
			_downArrowBounds = new Rectangle(base.Width / 2 - s_textureDownArrow.Width / 2, base.Height - s_textureDownArrow.Height, s_textureDownArrow.Width, s_textureDownArrow.Height);
			_barBounds = new Rectangle(base.Width / 2 - s_textureBar.Width / 2, (int)(ScrollDistance * (float)(TrackLength - ScrollbarHeight)) + s_textureUpArrow.Height, s_textureBar.Width, ScrollbarHeight);
			_trackBounds = new Rectangle(base.Width / 2 - s_textureTrack.Width / 2, _upArrowBounds.Bottom, s_textureTrack.Width, TrackLength);
		}

		private void RecalculateScrollbarSize()
		{
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
			if (!(_scrollbarPercent > 0.99))
			{
				Color drawTint = (((ScrollFocus == ClickFocus.None && base.MouseOver) || (_associatedContainer != null && _associatedContainer.MouseOver)) ? Color.White : ContentService.Colors.Darkened(0.6f));
				drawTint = ((ScrollFocus != 0) ? ContentService.Colors.Darkened(0.9f) : drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureTrack, _trackBounds);
				spriteBatch.DrawOnCtrl(this, s_textureUpArrow, _upArrowBounds, drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureDownArrow, _downArrowBounds, drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureBar, _barBounds, drawTint);
				spriteBatch.DrawOnCtrl(this, s_textureTopCap, new Rectangle(base.Width / 2 - s_textureTopCap.Width / 2, _barBounds.Top - 6, s_textureTopCap.Width, s_textureTopCap.Height));
				spriteBatch.DrawOnCtrl(this, s_textureBottomCap, new Rectangle(base.Width / 2 - s_textureBottomCap.Width / 2, _barBounds.Bottom - s_textureBottomCap.Height + 6, s_textureBottomCap.Width, s_textureBottomCap.Height));
				spriteBatch.DrawOnCtrl(this, s_textureThumb, new Rectangle(base.Width / 2 - s_textureThumb.Width / 2, _barBounds.Top + (ScrollbarHeight / 2 - s_textureThumb.Height / 2), s_textureThumb.Width, s_textureThumb.Height), drawTint);
			}
		}
	}
}
