using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Controls
{
	internal class AlertWindow : Container
	{
		private const int TITLEBAR_HEIGHT = 32;

		private const int TITLE_OFFSET = 80;

		private const int MAX_EMBLEM_WIDTH = 80;

		private const int COMMON_MARGIN = 16;

		private const int SUBTITLE_OFFSET = 20;

		protected string _title = "No Title";

		protected string _subtitle = "";

		protected Texture2D _emblem;

		protected bool _onlyShowChildren;

		protected bool Dragging;

		protected Point DragStart = Point.get_Zero();

		protected Rectangle _windowBackgroundBounds;

		protected Rectangle _titleBarBounds;

		protected Rectangle _emblemBounds;

		private Rectangle _layoutLeftTitleBarBounds;

		private Rectangle _layoutRightTitleBarBounds;

		private Rectangle _layoutSubtitleBounds;

		private Rectangle _layoutWindowCornerBounds;

		protected bool MouseOverTitleBar;

		public Texture2D WindowTitleBarLeft;

		public Texture2D WindowTitleBarRight;

		public Texture2D WindowTitleBarLeftActive;

		public Texture2D WindowTitleBarRightActive;

		public Texture2D WindowCorner;

		public Texture2D WindowBackground;

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _title, value, true, "Title");
			}
		}

		public string Subtitle
		{
			get
			{
				return _subtitle;
			}
			set
			{
				if (((Control)this).SetProperty<string>(ref _subtitle, value, false, "Subtitle"))
				{
					((Control)this).RecalculateLayout();
				}
			}
		}

		public Texture2D Emblem
		{
			get
			{
				return _emblem;
			}
			set
			{
				if (((Control)this).SetProperty<Texture2D>(ref _emblem, value, false, "Emblem"))
				{
					((Control)this).RecalculateLayout();
				}
			}
		}

		public bool OnlyShowChildren
		{
			get
			{
				return _onlyShowChildren;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _onlyShowChildren, value, false, "OnlyShowChildren");
			}
		}

		public AlertWindow()
			: this()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			WindowTitleBarLeft = TimersModule.ModuleInstance.Resources.WindowTitleBarLeft;
			WindowTitleBarRight = TimersModule.ModuleInstance.Resources.WindowTitleBarRight;
			WindowTitleBarLeftActive = TimersModule.ModuleInstance.Resources.WindowTitleBarLeftActive;
			WindowTitleBarRightActive = TimersModule.ModuleInstance.Resources.WindowTitleBarRightActive;
			WindowCorner = TimersModule.ModuleInstance.Resources.WindowCorner;
			WindowBackground = TimersModule.ModuleInstance.Resources.WindowBackground;
			((Control)this).set_ZIndex(41);
			Control.get_Input().get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				Dragging = false;
			});
			((Control)this).set_Padding(Thickness.Zero);
			_titleBarBounds = new Rectangle(0, 0, ((Control)this).get_Width(), 32);
			_windowBackgroundBounds = new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height() - 32);
		}

		public Rectangle ValidChildRegion()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			int num = Math.Max(((Rectangle)(ref _layoutLeftTitleBarBounds)).get_Bottom(), ((Rectangle)(ref _emblemBounds)).get_Bottom());
			int width = ((Container)this).get_ContentRegion().Width;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			return new Rectangle(0, num, width, ((Rectangle)(ref contentRegion)).get_Bottom() - Math.Max(((Rectangle)(ref _layoutLeftTitleBarBounds)).get_Bottom(), ((Rectangle)(ref _emblemBounds)).get_Bottom()));
		}

		public override void RecalculateLayout()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			if (_emblem != null)
			{
				float emblemHWRatio = _emblem.get_Height() / _emblem.get_Width();
				float emblemWidth = Math.Max(_emblem.get_Width(), 80);
				_emblemBounds = new Rectangle(0, 0, (int)emblemWidth, (int)(emblemWidth * emblemHWRatio));
			}
			else
			{
				_emblemBounds = Rectangle.get_Empty();
			}
			_titleBarBounds = new Rectangle(0, 0, ((Control)this).get_Width(), 32);
			int titleBarDrawOffset = _titleBarBounds.Y - (WindowTitleBarLeft.get_Height() / 2 - _titleBarBounds.Height / 2);
			titleBarDrawOffset += ((_emblem != null) ? (_emblemBounds.Height / 2 - WindowTitleBarLeft.get_Height() / 2) : 0);
			int titleBarRightWidth = WindowTitleBarRight.get_Width() - 16;
			_layoutLeftTitleBarBounds = new Rectangle(_titleBarBounds.X, titleBarDrawOffset, ((Rectangle)(ref _titleBarBounds)).get_Right() - titleBarRightWidth, WindowTitleBarLeft.get_Height());
			_layoutRightTitleBarBounds = new Rectangle(((Rectangle)(ref _titleBarBounds)).get_Right() - titleBarRightWidth, titleBarDrawOffset, WindowTitleBarRight.get_Width(), WindowTitleBarRight.get_Height());
			if (!string.IsNullOrEmpty(_title) && !string.IsNullOrEmpty(_subtitle))
			{
				int titleTextWidth = (int)Control.get_Content().get_DefaultFont32().MeasureString(_title)
					.Width;
				int titleOffset = ((_emblem == null) ? 80 : (_emblemBounds.Width + 5));
				_layoutSubtitleBounds = RectangleExtension.OffsetBy(_layoutLeftTitleBarBounds, titleOffset + titleTextWidth + 20, 0);
			}
			int num = ((Rectangle)(ref _layoutRightTitleBarBounds)).get_Right() - WindowCorner.get_Width() - 16;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			_layoutWindowCornerBounds = new Rectangle(num, ((Rectangle)(ref contentRegion)).get_Bottom() - WindowCorner.get_Height() + 16, WindowCorner.get_Width(), WindowCorner.get_Height());
			int num2 = -(WindowBackground.get_Width() / 4);
			int num3 = ((Rectangle)(ref _layoutLeftTitleBarBounds)).get_Bottom() / 2;
			int num4 = _titleBarBounds.Width + _layoutRightTitleBarBounds.Width + WindowBackground.get_Width() / 4;
			contentRegion = ((Container)this).get_ContentRegion();
			_windowBackgroundBounds = new Rectangle(num2, num3, num4, ((Rectangle)(ref contentRegion)).get_Bottom());
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			MouseOverTitleBar = false;
			if (((Control)this).get_RelativeMousePosition().Y < ((Rectangle)(ref _layoutLeftTitleBarBounds)).get_Bottom())
			{
				MouseOverTitleBar = true;
			}
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			MouseOverTitleBar = false;
			((Control)this).OnMouseLeft(e);
		}

		protected override CaptureType CapturesInput()
		{
			if (!OnlyShowChildren)
			{
				return (CaptureType)13;
			}
			return (CaptureType)0;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (MouseOverTitleBar)
			{
				Dragging = true;
				DragStart = Control.get_Input().get_Mouse().get_Position();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			Dragging = false;
			((Control)this).OnLeftMouseButtonReleased(e);
		}

		public void ToggleWindow()
		{
			if (((Control)this)._visible)
			{
				((Control)this).Hide();
			}
			else
			{
				((Control)this).Show();
			}
		}

		public override void Show()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this)._visible)
			{
				((Control)this).set_Location(new Point(Math.Max(0, ((Control)this)._location.X), Math.Max(0, ((Control)this)._location.Y)));
				((Control)this).set_Opacity(1f);
				((Control)this).set_Visible(true);
			}
		}

		public override void Hide()
		{
			if (((Control)this).get_Visible())
			{
				((Control)this).set_Opacity(0f);
				((Control)this).set_Visible(false);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (Dragging)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - DragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				DragStart = Control.get_Input().get_Mouse().get_Position();
			}
		}

		protected void PaintWindowBackground(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowBackground, bounds, (Rectangle?)null);
		}

		protected void PaintTitleBar(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this)._mouseOver && MouseOverTitleBar)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarLeftActive, _layoutLeftTitleBarBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarLeftActive, _layoutLeftTitleBarBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarRightActive, _layoutRightTitleBarBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarRightActive, _layoutRightTitleBarBounds);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarLeft, _layoutLeftTitleBarBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarLeft, _layoutLeftTitleBarBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarRight, _layoutRightTitleBarBounds);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowTitleBarRight, _layoutRightTitleBarBounds);
			}
			if (!string.IsNullOrEmpty(_title))
			{
				int titleOffset = ((_emblem == null) ? 80 : (_emblemBounds.Width + 5));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _title, Control.get_Content().get_DefaultFont32(), RectangleExtension.OffsetBy(_layoutLeftTitleBarBounds, titleOffset, 0), Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				if (!string.IsNullOrEmpty(_subtitle))
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _subtitle, Control.get_Content().get_DefaultFont16(), _layoutSubtitleBounds, Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		protected void PaintEmblem(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (_emblem != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _emblem, _emblemBounds);
			}
		}

		protected void PaintCorner(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, WindowCorner, _layoutWindowCornerBounds);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (!_onlyShowChildren)
			{
				PaintWindowBackground(spriteBatch, _windowBackgroundBounds);
				PaintTitleBar(spriteBatch, bounds);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (!_onlyShowChildren)
			{
				PaintEmblem(spriteBatch, bounds);
				PaintCorner(spriteBatch, bounds);
			}
		}
	}
}
