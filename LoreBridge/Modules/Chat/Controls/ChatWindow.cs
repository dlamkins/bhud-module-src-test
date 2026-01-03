using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoreBridge.Modules.Chat.Controls
{
	public abstract class ChatWindow : Container, IWindow
	{
		private const int TitlebarHeight = 26;

		private const int TitlebarVerticalOffset = 23;

		private const int CornerOffset = 3;

		private const int TitleOffset = 44;

		private const int ContentTopOffset = 2;

		private const int Margin = 1;

		private const int ResizeHandleSize = 16;

		private const int MinWindowWidth = 300;

		private const int MinWindowHeight = 210;

		private const int MaxWindowWidth = 610;

		private const int MaxWindowHeight = 532;

		private readonly Tween _animFade;

		private bool _canClose = true;

		private bool _canCloseWithEscape = true;

		private bool _canResize;

		private bool _dragging;

		private bool _resizing;

		private string _title = "No Title";

		private bool _topMost;

		private bool _transparent;

		private static readonly Texture2D TextureExitButton = Control.get_Content().GetTexture("button-exit");

		private static readonly Texture2D TextureExitButtonActive = Control.get_Content().GetTexture("button-exit-active");

		private readonly AsyncTexture2D _textureContentBackground = AsyncTexture2D.FromAssetId(155139);

		private readonly AsyncTexture2D _textureTitleBarLeft = AsyncTexture2D.FromAssetId(155147);

		private readonly AsyncTexture2D _textureTitleBarRight = AsyncTexture2D.FromAssetId(156009);

		private readonly AsyncTexture2D _textureTitleBarDivider = AsyncTexture2D.FromAssetId(156052);

		private readonly AsyncTexture2D _textureWindowCorner = AsyncTexture2D.FromAssetId(156008);

		private readonly AsyncTexture2D _textureWindowResizableCorner = AsyncTexture2D.FromAssetId(156009);

		private readonly AsyncTexture2D _textureWindowResizableCornerActive = AsyncTexture2D.FromAssetId(156010);

		private double _lastWindowInteract;

		private Rectangle _leftTitleBarDrawBounds = Rectangle.get_Empty();

		private Rectangle _rightTitleBarDrawBounds = Rectangle.get_Empty();

		private Point _dragStart = Point.get_Zero();

		private Point _resizeStart = Point.get_Zero();

		public override int ZIndex
		{
			get
			{
				return ((Control)this)._zIndex + GetZIndex((IWindow)(object)this);
			}
			set
			{
				((Control)this).SetProperty<int>(ref ((Control)this)._zIndex, value, false, "ZIndex");
			}
		}

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

		public bool CanResize
		{
			get
			{
				return _canResize;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canResize, value, false, "CanResize");
			}
		}

		public bool Dragging
		{
			get
			{
				return _dragging;
			}
			private set
			{
				((Control)this).SetProperty<bool>(ref _dragging, value, false, "Dragging");
			}
		}

		public bool Resizing
		{
			get
			{
				return _resizing;
			}
			private set
			{
				((Control)this).SetProperty<bool>(ref _resizing, value, false, "Resizing");
			}
		}

		public bool Transparent
		{
			get
			{
				return _transparent;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _transparent, value, false, "Transparent");
			}
		}

		public bool CanClose
		{
			get
			{
				return _canClose;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canClose, value, false, "CanClose");
			}
		}

		public bool CanCloseWithEscape
		{
			get
			{
				return _canCloseWithEscape;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canCloseWithEscape, value, false, "CanCloseWithEscape");
			}
		}

		public bool TopMost
		{
			get
			{
				return _topMost;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _topMost, value, false, "TopMost");
			}
		}

		public static IWindow ActiveWindow
		{
			get
			{
				return (from w in GetWindows()
					where w.get_Visible()
					select w).OrderByDescending(GetZIndex).FirstOrDefault();
			}
			set
			{
				value.BringWindowToFront();
			}
		}

		double LastInteraction => _lastWindowInteract;

		protected Rectangle TitleBarBounds { get; private set; } = Rectangle.get_Empty();


		protected Rectangle ExitButtonBounds { get; private set; } = Rectangle.get_Empty();


		protected Rectangle ResizeHandleBounds { get; private set; } = Rectangle.get_Empty();


		protected Rectangle BackgroundDestinationBounds { get; private set; } = Rectangle.get_Empty();


		protected bool MouseOverTitleBar { get; private set; }

		protected bool MouseOverExitButton { get; private set; }

		protected bool MouseOverResizeHandle { get; private set; }

		protected ChatWindow()
			: this()
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Opacity(0f);
			((Control)this).set_Visible(false);
			((Control)this)._zIndex = 41;
			((Control)this).set_ClipsBounds(false);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
			_animFade = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<ChatWindow>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(-1).Reflect();
			_animFade.Pause();
			_animFade.OnComplete((Action)delegate
			{
				_animFade.Pause();
				if (((Control)this)._opacity <= 0f)
				{
					((Control)this).set_Visible(false);
				}
			});
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
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			if (Dragging)
			{
				Point nOffset2 = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset2);
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			else if (Resizing)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Size(HandleWindowResize(_resizeStart + nOffset));
			}
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
			((Container)this).DisposeControl();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (!_transparent)
			{
				PaintWindowBackground(spriteBatch);
				PaintTitleBar(spriteBatch);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (!_transparent)
			{
				PaintTitleText(spriteBatch);
				PaintExitButton(spriteBatch);
				PaintCorner(spriteBatch);
			}
		}

		private void PaintCorner(SpriteBatch spriteBatch)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (CanResize)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((MouseOverResizeHandle || Resizing) ? _textureWindowResizableCornerActive : _textureWindowResizableCorner), ResizeHandleBounds);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureWindowCorner), ResizeHandleBounds);
			}
		}

		private void PaintWindowBackground(SpriteBatch spriteBatch)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			int scaledWidth = (int)((double)BackgroundDestinationBounds.Width * 0.9);
			int scaledHeight = (int)((double)BackgroundDestinationBounds.Height * 0.9);
			int height = _textureContentBackground.get_Height() - scaledHeight;
			int bottomPoint = ((height >= 0) ? height : 0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureContentBackground), BackgroundDestinationBounds, (Rectangle?)new Rectangle(0, bottomPoint, scaledWidth, scaledHeight));
		}

		private Rectangle CalculateIntersection(Rectangle rect1, Rectangle rect2)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			int x1 = Math.Max(rect1.X, rect2.X);
			int y1 = Math.Max(rect1.Y, rect2.Y);
			int x2 = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width);
			int y2 = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height);
			if (x2 > x1 && y2 > y1)
			{
				return new Rectangle(x1, y1, x2 - x1, y2 - y1);
			}
			return Rectangle.get_Empty();
		}

		private void PaintTitleBar(SpriteBatch spriteBatch)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver())
			{
				_ = MouseOverTitleBar;
			}
			int maxOffset = 52;
			int leftTextureOffset = Math.Max(0, maxOffset - (610 - TitleBarBounds.Width));
			if (leftTextureOffset > 0)
			{
				Rectangle destinationRectFirst = default(Rectangle);
				((Rectangle)(ref destinationRectFirst))._002Ector(_leftTitleBarDrawBounds.X, _leftTitleBarDrawBounds.Y, leftTextureOffset, _textureTitleBarLeft.get_Height());
				Rectangle sourceRectFirst = default(Rectangle);
				((Rectangle)(ref sourceRectFirst))._002Ector(0, 0, leftTextureOffset, _textureTitleBarLeft.get_Height());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureTitleBarLeft), destinationRectFirst, (Rectangle?)sourceRectFirst);
			}
			int rightTextureOffset = Math.Max(0, maxOffset - (TitleBarBounds.Width - _textureTitleBarLeft.get_Width()));
			Rectangle destinationRect = default(Rectangle);
			((Rectangle)(ref destinationRect))._002Ector(TitleBarBounds.X + leftTextureOffset, _leftTitleBarDrawBounds.Y, Math.Min(_textureTitleBarLeft.get_Width() - rightTextureOffset, TitleBarBounds.Width), _textureTitleBarLeft.get_Height());
			Rectangle sourceRect = default(Rectangle);
			((Rectangle)(ref sourceRect))._002Ector(rightTextureOffset, 0, destinationRect.Width, _textureTitleBarLeft.get_Height());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureTitleBarLeft), destinationRect, (Rectangle?)sourceRect);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureTitleBarRight), _rightTitleBarDrawBounds, (Rectangle?)null, Color.get_Black(), MathHelper.ToRadians(270f), new Vector2((float)_textureTitleBarRight.get_Height(), 0f), (SpriteEffects)0);
			int dividerScaledWidth = (int)((double)_textureTitleBarDivider.get_Width() * 1.11);
			int dividerScaledHeight = (int)((double)_textureTitleBarDivider.get_Height() * 1.3);
			Rectangle titleBarBounds = TitleBarBounds;
			Rectangle targetRect = default(Rectangle);
			((Rectangle)(ref targetRect))._002Ector(0, ((Rectangle)(ref titleBarBounds)).get_Bottom() - dividerScaledHeight + 8, dividerScaledWidth, dividerScaledHeight);
			Rectangle clipRect = default(Rectangle);
			((Rectangle)(ref clipRect))._002Ector(0, 0, TitleBarBounds.Width, TitleBarBounds.Height);
			Rectangle visibleRect = CalculateIntersection(targetRect, clipRect);
			float scaleX = (float)_textureTitleBarDivider.get_Width() / (float)targetRect.Width;
			float scaleY = (float)_textureTitleBarDivider.get_Height() / (float)targetRect.Height;
			Rectangle dividerSourceRect = default(Rectangle);
			((Rectangle)(ref dividerSourceRect))._002Ector((int)((float)(visibleRect.X - targetRect.X) * scaleX), (int)((float)(visibleRect.Y - targetRect.Y) * scaleY), (int)((float)visibleRect.Width * scaleX), (int)((float)visibleRect.Height * scaleY));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureTitleBarDivider), visibleRect, (Rectangle?)dividerSourceRect, Color.get_White() * 0.6f);
		}

		private void PaintTitleText(SpriteBatch spriteBatch)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrWhiteSpace(Title))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Title, Control.get_Content().get_DefaultFont16(), RectangleExtension.OffsetBy(_leftTitleBarDrawBounds, 44, 2), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		private void PaintExitButton(SpriteBatch spriteBatch)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (CanClose)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MouseOverExitButton ? TextureExitButtonActive : TextureExitButton, ExitButtonBounds);
			}
		}

		public static IEnumerable<IWindow> GetWindows()
		{
			return ((Container)GameService.Graphics.get_SpriteScreen()).GetChildrenOfType<IWindow>();
		}

		public static int GetZIndex(IWindow thisWindow)
		{
			IWindow[] windows = GetWindows().ToArray();
			if (!windows.Contains(thisWindow))
			{
				throw new InvalidOperationException("thisWindow must be a direct child of GameService.Graphics.SpriteScreen before ZIndex can automatically be calculated.");
			}
			return 41 + (from window in windows
				orderby window.get_TopMost(), window.get_LastInteraction()
				select window).TakeWhile((IWindow window) => window != thisWindow).Count();
		}

		public void ToggleWindow()
		{
			if (((Control)this).get_Visible())
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
			BringWindowToFront();
			if (!((Control)this).get_Visible())
			{
				((Control)this).set_Opacity(0f);
				((Control)this).set_Visible(true);
				_animFade.Resume();
			}
		}

		public override void Hide()
		{
			if (((Control)this).get_Visible())
			{
				Dragging = false;
				_animFade.Resume();
				Control.get_Content().PlaySoundEffectByName("window-close");
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			_rightTitleBarDrawBounds = new Rectangle(TitleBarBounds.Width - _textureTitleBarRight.get_Width() + 6 - 1, TitleBarBounds.Y - 3, _textureTitleBarRight.get_Width(), _textureTitleBarRight.get_Height());
			Rectangle titleBarBounds = TitleBarBounds;
			int x = ((Rectangle)(ref titleBarBounds)).get_Location().X;
			titleBarBounds = TitleBarBounds;
			_leftTitleBarDrawBounds = new Rectangle(x, ((Rectangle)(ref titleBarBounds)).get_Location().Y - 23, _textureTitleBarLeft.get_Width(), _textureTitleBarLeft.get_Height());
			ExitButtonBounds = new Rectangle(((Rectangle)(ref _rightTitleBarDrawBounds)).get_Right() - 2 - TextureExitButton.get_Width(), _rightTitleBarDrawBounds.Y, TextureExitButton.get_Width(), TextureExitButton.get_Height());
			ResizeHandleBounds = new Rectangle(((Control)this).get_Width() - _textureWindowCorner.get_Width() + 3, ((Control)this).get_Height() - _textureWindowCorner.get_Height() + 6, _textureWindowCorner.get_Width(), _textureWindowCorner.get_Height());
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			ResetMouseRegionStates();
			int y = ((Control)this).get_RelativeMousePosition().Y;
			Rectangle val = TitleBarBounds;
			if (y < ((Rectangle)(ref val)).get_Bottom())
			{
				val = ExitButtonBounds;
				if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					MouseOverExitButton = true;
				}
				else
				{
					MouseOverTitleBar = true;
				}
			}
			else if (_canResize)
			{
				val = ResizeHandleBounds;
				if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					int x = ((Control)this).get_RelativeMousePosition().X;
					val = ResizeHandleBounds;
					if (x > ((Rectangle)(ref val)).get_Right() - 16)
					{
						int y2 = ((Control)this).get_RelativeMousePosition().Y;
						val = ResizeHandleBounds;
						if (y2 > ((Rectangle)(ref val)).get_Bottom() - 16)
						{
							MouseOverResizeHandle = true;
						}
					}
				}
			}
			((Control)this).OnMouseMoved(e);
		}

		private void OnGlobalMouseRelease(object sender, MouseEventArgs e)
		{
			if (((Control)this).get_Visible())
			{
				Dragging = false;
				Resizing = false;
			}
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			ResetMouseRegionStates();
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			BringWindowToFront();
			if (MouseOverTitleBar)
			{
				Dragging = true;
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			else if (MouseOverResizeHandle)
			{
				Resizing = true;
				_resizeStart = ((Control)this).get_Size();
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			else if (MouseOverExitButton && CanClose)
			{
				((Control)this).Hide();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		private void ResetMouseRegionStates()
		{
			MouseOverTitleBar = false;
			MouseOverExitButton = false;
			MouseOverResizeHandle = false;
		}

		protected virtual Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, 300, 610), MathHelper.Clamp(newSize.Y, 210, 532));
		}

		public void BringWindowToFront()
		{
			_lastWindowInteract = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			CalculateWindow();
			((Container)this).OnResized(e);
		}

		private void CalculateWindow()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).set_ContentRegion(new Rectangle(0, 28, ((Control)this).get_Width(), ((Control)this).get_Height() - 26 - 2));
			TitleBarBounds = new Rectangle(0, 0, ((Control)this).get_Size().X, 28);
			int drawWidth = ((Container)this).get_ContentRegion().Width + ((Container)this).get_ContentRegion().X;
			int drawHeight = ((Container)this).get_ContentRegion().Height + ((Container)this).get_ContentRegion().Y - 26;
			BackgroundDestinationBounds = new Rectangle(0, 26, drawWidth, drawHeight);
		}
	}
}
