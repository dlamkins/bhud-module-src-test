using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Displays
{
	public class WindowVideoDisplay : Panel, IVideoDisplay, IDisposable
	{
		private enum ResizeDirection
		{
			None,
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight,
			Top,
			Bottom,
			Left,
			Right
		}

		private static readonly Logger Logger = Logger.GetLogger<WindowVideoDisplay>();

		private const int BorderSize = 6;

		private const int HandleSize = 12;

		private const int MinWidth = 320;

		private const int MaxWidth = 2300;

		private const float AspectRatio = 1.7777778f;

		private const int ResizeCornerSize = 128;

		private const int TopMargin = 35;

		private Texture2D _currentTexture;

		private WindowVideoControls _controlsOverlay;

		private readonly AsyncTexture2D _textureResizeCorner = CinemaModule.Instance.TextureService.GetResizeCorner();

		private readonly AsyncTexture2D _textureResizeCornerActive = CinemaModule.Instance.TextureService.GetResizeCornerActive();

		private bool _isDragging;

		private bool _isResizing;

		private bool _isHoveringResizeCorner;

		private Point _dragStartMouse;

		private Point _dragStartLocation;

		private Point _dragStartSize;

		private ResizeDirection _resizeDirection;

		public bool IsPaused
		{
			get
			{
				return _controlsOverlay?.IsPaused ?? false;
			}
			set
			{
				if (_controlsOverlay != null)
				{
					_controlsOverlay.IsPaused = value;
				}
			}
		}

		public int Volume
		{
			get
			{
				return _controlsOverlay?.Volume ?? 100;
			}
			set
			{
				if (_controlsOverlay != null)
				{
					_controlsOverlay.Volume = value;
				}
			}
		}

		public bool IsTwitchStream
		{
			get
			{
				return _controlsOverlay?.IsTwitchStream ?? false;
			}
			set
			{
				if (_controlsOverlay != null)
				{
					_controlsOverlay.IsTwitchStream = value;
				}
			}
		}

		public Point Size
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)this).get_Size();
			}
			set
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				int clampedWidth = Math.Max(320, Math.Min(value.X, 2300));
				int calculatedHeight = (int)((float)clampedWidth / 1.7777778f);
				((Control)this).set_Size(new Point(clampedWidth, calculatedHeight));
			}
		}

		public Point Location
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)this).get_Location();
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				Point clampedLocation = ClampToScreenBounds(value, Size);
				((Control)this).set_Location(clampedLocation);
			}
		}

		public event EventHandler<Point> PositionChanged;

		public event EventHandler<Point> SizeChanged;

		public event EventHandler PlayPauseClicked;

		public event EventHandler<int> VolumeChanged;

		public event EventHandler SettingsClicked;

		public event EventHandler TwitchChatClicked;

		public event EventHandler CloseClicked;

		public event EventHandler<int> QualityChanged;

		public WindowVideoDisplay()
			: this()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BackgroundColor(new Color(40, 40, 40));
			((Panel)this).set_ShowBorder(false);
			Size = new Point(800, 450);
			Location = new Point(100, 50);
			_controlsOverlay = new WindowVideoControls((Container)(object)this);
			_controlsOverlay.PlayPauseClicked += delegate
			{
				this.PlayPauseClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlsOverlay.VolumeChanged += delegate(object s, int vol)
			{
				this.VolumeChanged?.Invoke(this, vol);
			};
			_controlsOverlay.SettingsClicked += delegate
			{
				this.SettingsClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlsOverlay.TwitchChatClicked += delegate
			{
				this.TwitchChatClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlsOverlay.CloseClicked += delegate
			{
				this.CloseClicked?.Invoke(this, EventArgs.Empty);
			};
			_controlsOverlay.QualityChanged += delegate(object s, int index)
			{
				this.QualityChanged?.Invoke(this, index);
			};
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
		}

		public void UpdateTexture(Texture2D texture)
		{
			if (_currentTexture == null || ((GraphicsResource)_currentTexture).get_IsDisposed())
			{
				_currentTexture = texture;
			}
			else if (texture != _currentTexture)
			{
				_currentTexture = texture;
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			((Control)this).Invalidate();
			this.SizeChanged?.Invoke(this, Size);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMoved(e);
			((Control)this).Invalidate();
			this.PositionChanged?.Invoke(this, Location);
		}

		public void UpdateAvailableQualities(IReadOnlyList<string> qualityNames, int selectedIndex)
		{
			_controlsOverlay?.UpdateAvailableQualities(qualityNames, selectedIndex);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			Rectangle panelRect = default(Rectangle);
			((Rectangle)(ref panelRect))._002Ector(Location.X, Location.Y, Size.X, Size.Y);
			Point mousePos = GameService.Input.get_Mouse().get_Position();
			Rectangle resizeCornerRect = default(Rectangle);
			((Rectangle)(ref resizeCornerRect))._002Ector(panelRect.X + panelRect.Width - 128, panelRect.Y + panelRect.Height - 128, 128, 128);
			_isHoveringResizeCorner = ((Rectangle)(ref resizeCornerRect)).Contains(mousePos);
			spriteBatch.Draw(Textures.get_Pixel(), panelRect, new Color(30, 30, 30, 230));
			if (_currentTexture != null && !((GraphicsResource)_currentTexture).get_IsDisposed())
			{
				Rectangle videoRect = default(Rectangle);
				((Rectangle)(ref videoRect))._002Ector(Location.X + 6, Location.Y + 6, Math.Max(1, Size.X - 12), Math.Max(1, Size.Y - 12));
				spriteBatch.Draw(_currentTexture, videoRect, Color.get_White());
			}
			DrawBorder(spriteBatch, panelRect, new Color(80, 80, 80, 210));
			DrawCornerHandles(spriteBatch, panelRect);
			_controlsOverlay?.Update(panelRect);
			_controlsOverlay?.Draw(spriteBatch);
		}

		private void DrawBorder(SpriteBatch spriteBatch, Rectangle panelRect, Color baseColor)
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			Color layerColor = default(Color);
			for (int i = 0; i < 6; i++)
			{
				float gradientFactor = (float)i / 5f;
				byte r = (byte)MathHelper.Lerp((float)(int)((Color)(ref baseColor)).get_R() * 0.4f, (float)(int)((Color)(ref baseColor)).get_R(), gradientFactor);
				byte g = (byte)MathHelper.Lerp((float)(int)((Color)(ref baseColor)).get_G() * 0.4f, (float)(int)((Color)(ref baseColor)).get_G(), gradientFactor);
				byte b = (byte)MathHelper.Lerp((float)(int)((Color)(ref baseColor)).get_B() * 0.4f, (float)(int)((Color)(ref baseColor)).get_B(), gradientFactor);
				((Color)(ref layerColor))._002Ector(r, g, b, ((Color)(ref baseColor)).get_A());
				int x = panelRect.X + i;
				int y = panelRect.Y + i;
				int width = panelRect.Width - i * 2;
				int height = panelRect.Height - i * 2;
				spriteBatch.Draw(pixel, new Rectangle(x, y, width, 1), layerColor);
				spriteBatch.Draw(pixel, new Rectangle(x, y + height - 1, width, 1), layerColor);
				spriteBatch.Draw(pixel, new Rectangle(x, y, 1, height), layerColor);
				spriteBatch.Draw(pixel, new Rectangle(x + width - 1, y, 1, height), layerColor);
			}
		}

		private void DrawCornerHandles(SpriteBatch spriteBatch, Rectangle panelRect)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D resizeTexture = ((_isHoveringResizeCorner || _isResizing) ? _textureResizeCornerActive : _textureResizeCorner);
			if (resizeTexture != null)
			{
				Rectangle resizeRect = default(Rectangle);
				((Rectangle)(ref resizeRect))._002Ector(panelRect.X + panelRect.Width - 128, panelRect.Y + panelRect.Height - 128, 128, 128);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(resizeTexture), resizeRect, Color.get_White());
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Point mousePos = GameService.Input.get_Mouse().get_Position();
			if (_controlsOverlay == null || !_controlsOverlay.HandleMouseDown(mousePos))
			{
				Point localPos = default(Point);
				((Point)(ref localPos))._002Ector(mousePos.X - ((Control)this).get_AbsoluteBounds().X, mousePos.Y - ((Control)this).get_AbsoluteBounds().Y);
				_resizeDirection = GetResizeDirection(localPos);
				_dragStartMouse = mousePos;
				_dragStartLocation = Location;
				_dragStartSize = Size;
				if (_resizeDirection != 0)
				{
					_isResizing = true;
				}
				else
				{
					_isDragging = true;
				}
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			StopDragAndResize();
		}

		private void OnGlobalMouseRelease(object sender, MouseEventArgs e)
		{
			StopDragAndResize();
		}

		private void StopDragAndResize()
		{
			_isDragging = false;
			_isResizing = false;
			_resizeDirection = ResizeDirection.None;
		}

		private ResizeDirection GetResizeDirection(Point localPos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			bool isLeft = localPos.X < 12;
			bool isRight = localPos.X > Size.X - 12;
			bool isTop = localPos.Y < 12;
			bool isBottom = localPos.Y > Size.Y - 12;
			if (isTop && isLeft)
			{
				return ResizeDirection.TopLeft;
			}
			if (isTop && isRight)
			{
				return ResizeDirection.TopRight;
			}
			if (isBottom && isLeft)
			{
				return ResizeDirection.BottomLeft;
			}
			if (isBottom && isRight)
			{
				return ResizeDirection.BottomRight;
			}
			if (isTop)
			{
				return ResizeDirection.Top;
			}
			if (isBottom)
			{
				return ResizeDirection.Bottom;
			}
			if (isLeft)
			{
				return ResizeDirection.Left;
			}
			if (isRight)
			{
				return ResizeDirection.Right;
			}
			return ResizeDirection.None;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			if (_isDragging)
			{
				Point nOffset = GameService.Input.get_Mouse().get_Position() - _dragStartMouse;
				Point newLocation = ClampToScreenBounds(_dragStartLocation + nOffset, Size);
				if (newLocation != Location)
				{
					Location = newLocation;
					((Control)this).Invalidate();
				}
			}
			else if (_isResizing)
			{
				HandleResize(GameService.Input.get_Mouse().get_Position());
			}
		}

		private void HandleResize(Point mousePos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			Point delta = mousePos - _dragStartMouse;
			int newWidth = _dragStartSize.X;
			int newX = _dragStartLocation.X;
			int newY = _dragStartLocation.Y;
			switch (_resizeDirection)
			{
			case ResizeDirection.BottomRight:
				newWidth = _dragStartSize.X + delta.X;
				break;
			case ResizeDirection.BottomLeft:
				newWidth = _dragStartSize.X - delta.X;
				newX = _dragStartLocation.X + delta.X;
				break;
			case ResizeDirection.TopRight:
				newWidth = _dragStartSize.X + delta.X;
				newY = _dragStartLocation.Y + (_dragStartSize.Y - (int)((float)newWidth / 1.7777778f));
				break;
			case ResizeDirection.TopLeft:
				newWidth = _dragStartSize.X - delta.X;
				newX = _dragStartLocation.X + delta.X;
				newY = _dragStartLocation.Y + (_dragStartSize.Y - (int)((float)newWidth / 1.7777778f));
				break;
			case ResizeDirection.Right:
				newWidth = _dragStartSize.X + delta.X;
				break;
			case ResizeDirection.Left:
				newWidth = _dragStartSize.X - delta.X;
				newX = _dragStartLocation.X + (_dragStartSize.X - newWidth);
				break;
			case ResizeDirection.Bottom:
				newWidth = (int)((float)(_dragStartSize.Y + delta.Y) * 1.7777778f);
				break;
			case ResizeDirection.Top:
				newWidth = (int)((float)(_dragStartSize.Y - delta.Y) * 1.7777778f);
				newY = _dragStartLocation.Y + (_dragStartSize.Y - (int)((float)newWidth / 1.7777778f));
				break;
			}
			newWidth = Math.Max(320, Math.Min(newWidth, 2300));
			int newHeight = (int)((float)newWidth / 1.7777778f);
			Point newSize = default(Point);
			((Point)(ref newSize))._002Ector(newWidth, newHeight);
			Point newLoc = ClampToScreenBounds(new Point(newX, newY), newSize);
			if (newSize != Size || newLoc != Location)
			{
				Size = newSize;
				Location = newLoc;
				((Control)this).Invalidate();
			}
		}

		private Point ClampToScreenBounds(Point location, Point size)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			int screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
			int screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			if (screenWidth <= 0 || screenHeight <= 0)
			{
				return location;
			}
			int num = Math.Max(0, Math.Min(location.X, screenWidth - size.X));
			int clampedY = Math.Max(35, Math.Min(location.Y, screenHeight - size.Y));
			return new Point(num, clampedY);
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
			_controlsOverlay?.Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
