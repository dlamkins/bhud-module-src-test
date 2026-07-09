using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rp.spark.UI
{
	internal sealed class SparkCompactWindow : Panel
	{
		private const int HeaderHeight = 46;

		private const int TitleBarVerticalOffset = 11;

		private const int LeftTitleBarHorizontalOffset = 2;

		private const int RightTitleBarHorizontalOffset = 16;

		private const int ContentPadding = 10;

		private const int TitleTextY = 1;

		private const int CloseButtonSize = 26;

		private const int CloseButtonTop = 3;

		private const int CloseButtonRightPadding = 6;

		private static readonly Texture2D TextureTitleBarLeft = Control.get_Content().GetTexture("titlebar-inactive");

		private static readonly Texture2D TextureTitleBarRight = Control.get_Content().GetTexture("window-topright");

		private static readonly Texture2D TextureTitleBarLeftActive = Control.get_Content().GetTexture("titlebar-active");

		private static readonly Texture2D TextureTitleBarRightActive = Control.get_Content().GetTexture("window-topright-active");

		private readonly AsyncTexture2D _background;

		private readonly Rectangle _backgroundSource;

		private readonly ViewContainer _viewContainer;

		private readonly Label _titleLabel;

		private readonly StandardButton _closeButton;

		private Texture2D _windowBackgroundTexture;

		private bool _dragging;

		private Point _dragStart;

		private Rectangle _leftTitleBarDrawBounds = Rectangle.get_Empty();

		private Rectangle _rightTitleBarDrawBounds = Rectangle.get_Empty();

		private bool _mouseOverTitleBar;

		private bool _draggingLocked;

		private bool _movedDuringDrag;

		public bool DraggingLocked
		{
			get
			{
				return _draggingLocked;
			}
			set
			{
				_draggingLocked = value;
				if (_draggingLocked)
				{
					_dragging = false;
					_movedDuringDrag = false;
					_mouseOverTitleBar = false;
				}
			}
		}

		public event Action<Point> LocationSaved;

		public SparkCompactWindow(string title, AsyncTexture2D background, Rectangle backgroundSource, Point size)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			_background = background;
			_backgroundSource = backgroundSource;
			((Control)this).set_Size(size);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((Control)this).set_ClipsBounds(true);
			((Panel)this).set_ShowBorder(false);
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			((Control)this).set_ZIndex(1000);
			if (_background != null)
			{
				_windowBackgroundTexture = _background.get_Texture();
				if (!_background.get_HasSwapped())
				{
					_background.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)HandleTextureSwapped);
				}
			}
			Label val = new Label();
			val.set_Text(title);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			((Control)val).set_Location(new Point(10, 1));
			((Control)val).set_Parent((Container)(object)this);
			_titleLabel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("X");
			((Control)val2).set_Size(new Point(26, 26));
			((Control)val2).set_Parent((Container)(object)this);
			_closeButton = val2;
			((Control)_closeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).set_Visible(false);
			});
			ViewContainer val3 = new ViewContainer();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_FadeView(false);
			_viewContainer = val3;
			ApplyLayout();
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)HandleGlobalMouseReleased);
		}

		public void Show(IView view)
		{
			_viewContainer.Show(view);
			((Control)this).set_Visible(true);
			((Control)this).set_ZIndex(1000);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				Point delta = GameService.Input.get_Mouse().get_Position() - _dragStart;
				if (delta.X != 0 || delta.Y != 0)
				{
					((Control)this).set_Location(((Control)this).get_Location() + delta);
					_movedDuringDrag = true;
				}
				_dragStart = GameService.Input.get_Mouse().get_Position();
			}
			((Container)this).UpdateContainer(gameTime);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)12;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_ZIndex(1000);
			_mouseOverTitleBar = IsInTitleDragRegion(((Control)this).get_RelativeMousePosition());
			if (_mouseOverTitleBar)
			{
				_dragging = true;
				_movedDuringDrag = false;
				_dragStart = GameService.Input.get_Mouse().get_Position();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			_mouseOverTitleBar = IsInTitleDragRegion(((Control)this).get_RelativeMousePosition());
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_mouseOverTitleBar = false;
			((Control)this).OnMouseLeft(e);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			if (_windowBackgroundTexture != null)
			{
				Rectangle destination = default(Rectangle);
				((Rectangle)(ref destination))._002Ector(((Control)this).get_AbsoluteBounds().X, ((Control)this).get_AbsoluteBounds().Y, Math.Max(0, ((Control)this).get_Width()), Math.Max(0, ((Control)this).get_Height()));
				spriteBatch.Draw(_windowBackgroundTexture, destination, (Rectangle?)_backgroundSource, Color.get_White());
			}
			DrawTitleBar(spriteBatch);
		}

		private void DrawTitleBar(SpriteBatch spriteBatch)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			bool active = !DraggingLocked && (_dragging || _mouseOverTitleBar);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, active ? TextureTitleBarLeftActive : TextureTitleBarLeft, _leftTitleBarDrawBounds);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, active ? TextureTitleBarRightActive : TextureTitleBarRight, _rightTitleBarDrawBounds);
		}

		private bool IsInTitleDragRegion(Point position)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (DraggingLocked)
			{
				return false;
			}
			if (position.Y >= 0 && position.Y < 46 && position.X >= 0)
			{
				return position.X < ((Control)this).get_Width() - 26 - 12;
			}
			return false;
		}

		private void ApplyTitleBarLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			Rectangle titleBarBounds = default(Rectangle);
			((Rectangle)(ref titleBarBounds))._002Ector(0, 0, ((Control)this).get_Width(), 46);
			_rightTitleBarDrawBounds = new Rectangle(titleBarBounds.Width - TextureTitleBarRight.get_Width() + 16, titleBarBounds.Y - 11, TextureTitleBarRight.get_Width(), TextureTitleBarRight.get_Height());
			_leftTitleBarDrawBounds = new Rectangle(titleBarBounds.X - 2, titleBarBounds.Y - 11, Math.Max(0, Math.Min(TextureTitleBarLeft.get_Width(), ((Rectangle)(ref _rightTitleBarDrawBounds)).get_Left() - 2)), TextureTitleBarLeft.get_Height());
		}

		private void ApplyLayout()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			ApplyTitleBarLayout();
			((Control)_titleLabel).set_Size(new Point(Math.Max(0, ((Control)this).get_Width() - 62), 45));
			((Control)_closeButton).set_Size(new Point(26, 26));
			((Control)_closeButton).set_Location(new Point(((Control)this).get_Width() - 26 - 6, 3));
			((Control)_viewContainer).set_Location(new Point(10, 46));
			((Control)_viewContainer).set_Size(new Point(Math.Max(0, ((Control)this).get_Width() - 20), Math.Max(0, ((Control)this).get_Height() - 46 - 10)));
		}

		private void HandleGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging && _movedDuringDrag)
			{
				this.LocationSaved?.Invoke(((Control)this).get_Location());
			}
			_dragging = false;
			_movedDuringDrag = false;
		}

		private void HandleTextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_windowBackgroundTexture = e.get_NewValue();
		}

		protected override void DisposeControl()
		{
			if (_background != null)
			{
				_background.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)HandleTextureSwapped);
			}
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)HandleGlobalMouseReleased);
			ViewContainer viewContainer = _viewContainer;
			if (viewContainer != null)
			{
				viewContainer.Clear();
			}
			((Panel)this).DisposeControl();
		}
	}
}
