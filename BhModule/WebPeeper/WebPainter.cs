using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using CefHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BhModule.WebPeeper
{
	internal class WebPainter : Control
	{
		private enum CefEvtModifiresFlags : uint
		{
			None = 0u,
			CapsLockOn = 1u,
			ShiftDown = 2u,
			ControlDown = 4u,
			AltDown = 8u,
			LeftMouseButton = 0x10u,
			MiddleMouseButton = 0x20u,
			RightMouseButton = 0x40u
		}

		private static Texture2D _webTexture;

		private Rectangle _webTextureRect = Rectangle.get_Empty();

		private readonly ConcurrentQueue<(byte[], int, int, int)> _queuedWebTextureBuffer = new ConcurrentQueue<(byte[], int, int, int)>();

		private readonly Stopwatch _updateWebTextureTimer = Stopwatch.StartNew();

		private static readonly long _updateMaxTick = Stopwatch.Frequency * 15 / 1000;

		private bool _isTriggerMouseLeftButtonPressed;

		private bool _isTriggerMouseRightButtonPressed;

		private bool _isMouseStateLeftButtonPressed;

		private bool _isMouseStateRightButtonPressed;

		private static bool _isError = false;

		private static readonly Texture2D _errorTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("error.png");

		private static readonly Texture2D _quesTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("what.png");

		private static readonly Color _errorTextureColor = new Color(2155905152u);

		private Rectangle _errorTextureRect = Rectangle.get_Empty();

		private Rectangle _questionTextureRect = Rectangle.get_Empty();

		private Process _cefPaintProcess;

		private Color _webBackgroundColor;

		public bool Disabled;

		public static WebPainter Instance { get; private set; }

		public Point LocationAtForm
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				if (Instance != null)
				{
					Rectangle absoluteBounds = ((Control)Instance).get_AbsoluteBounds();
					int num = (int)((float)((Rectangle)(ref absoluteBounds)).get_Location().X * GameService.Graphics.get_UIScaleMultiplier());
					absoluteBounds = ((Control)Instance).get_AbsoluteBounds();
					return new Point(num, (int)((float)((Rectangle)(ref absoluteBounds)).get_Location().Y * GameService.Graphics.get_UIScaleMultiplier()));
				}
				return Point.get_Zero();
			}
		}

		private ModuleSettings Settings => WebPeeperModule.Instance.Settings;

		public WebPainter()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Instance = this;
			Browser.add_Paint((Func<IntPtr, int, int, bool>)CefOnPaint);
			Browser.add_FrameLoadStart((Action)OnFrameLoadStart);
			Browser.add_UrlLoadError((Action<string>)OnUrlLoadError);
			GameService.Input.get_Keyboard().add_KeyStateChanged((EventHandler<KeyboardEventArgs>)KeyboardHandler);
			ApplyBgTexture();
			Browser.Repaint();
		}

		public override Control TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (!Disabled)
			{
				MouseHandler(mouseEventType, ms);
			}
			return ((Control)this).TriggerMouseInput(mouseEventType, ms);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (_isTriggerMouseLeftButtonPressed)
			{
				MouseHandler((MouseEventType)514, GameService.Input.get_Mouse().get_State());
			}
			if (_isTriggerMouseRightButtonPressed)
			{
				MouseHandler((MouseEventType)517, GameService.Input.get_Mouse().get_State());
			}
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((Rectangle)(ref _webTextureRect)).set_Size(((Control)this).get_Size());
			Browser.SetSize(((Control)this).get_Width(), ((Control)this).get_Height());
			SetErrorTextureRect();
			((Control)this).OnResized(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			if (_webTexture == null)
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, new Rectangle(((Control)this).get_Size().X / 2 - 50, ((Control)this).get_Size().Y / 2 - 50, 100, 100));
				return;
			}
			if (_isError)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _errorTexture, _errorTextureRect, _errorTextureColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _quesTexture, _questionTextureRect, (Rectangle?)_quesTexture.get_Bounds(), _errorTextureColor, MathHelper.ToRadians(30f), Vector2.get_Zero(), (SpriteEffects)0);
				return;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _webTextureRect, _webBackgroundColor);
			try
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _webTexture, _webTextureRect);
			}
			catch
			{
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			UpdateWebTexture();
			TriggerMissedMouseEvent();
		}

		private void OnFrameLoadStart()
		{
			SetErrorState(state: false);
		}

		public void OnUrlLoadError(string url)
		{
			SetErrorState(state: true);
		}

		private bool CefOnPaint(IntPtr bufferHandle, int width, int height)
		{
			UiService uiService = WebPeeperModule.Instance.UiService;
			int num;
			if (uiService == null)
			{
				num = 1;
			}
			else
			{
				BrowserWindow browserWindow = uiService.BrowserWindow;
				num = ((!((browserWindow != null) ? new bool?(((Control)browserWindow).get_Visible()) : null).GetValueOrDefault()) ? 1 : 0);
			}
			if (num != 0)
			{
				return true;
			}
			int num2 = width * height * 4;
			byte[] array = ArrayPool<byte>.get_Shared().Rent(num2);
			if (_cefPaintProcess == null)
			{
				_cefPaintProcess = Process.GetCurrentProcess();
			}
			Utils.ReadProcessMemory(_cefPaintProcess.Handle, bufferHandle, array, num2, out var _);
			_queuedWebTextureBuffer.Enqueue((array, num2, width, height));
			return true;
		}

		private void ClearWebTextureBufferQueued()
		{
			(byte[], int, int, int) result;
			while (_queuedWebTextureBuffer.TryDequeue(out result))
			{
				ArrayPool<byte>.get_Shared().Return(result.Item1, false);
			}
		}

		private void UpdateWebTexture()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			_updateWebTextureTimer.Restart();
			(byte[], int, int, int) result;
			while (_queuedWebTextureBuffer.TryDequeue(out result))
			{
				var (array, num, num2, num3) = result;
				if (_webTexture == null || _webTexture.get_Width() != num2 || _webTexture.get_Height() != num3)
				{
					DisposeWebTexture();
					GraphicsDeviceContext val = Control.get_Graphics().LendGraphicsDeviceContext();
					try
					{
						_webTexture = new Texture2D(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), num2, num3, false, (SurfaceFormat)21);
					}
					finally
					{
						((GraphicsDeviceContext)(ref val)).Dispose();
					}
				}
				_webTexture.SetData<byte>(array, 0, num);
				ArrayPool<byte>.get_Shared().Return(array, false);
				if (_updateWebTextureTimer.ElapsedTicks > _updateMaxTick)
				{
					break;
				}
			}
		}

		public void SetErrorState(bool state)
		{
			_isError = state;
		}

		public void ApplyBgTexture()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_webBackgroundColor = ColorHelper.FromHex(Settings.WebBgColor.get_Value());
			}
			catch
			{
			}
		}

		private void SetErrorTextureRect()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			int num = MathHelper.Min((int)((double)((Control)this).get_Size().X * 0.6), (int)((double)((Control)this).get_Size().Y * 0.6));
			_errorTextureRect = new Rectangle(0, ((Control)this).get_Size().Y - num, num, num);
			int num2 = (int)((double)num * 0.2);
			_questionTextureRect = new Rectangle((int)((double)num * 0.7), ((Control)this).get_Size().Y - (int)((double)num * 1.05), num2, num2);
		}

		private void MouseHandler(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Invalid comparison between Unknown and I4
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Invalid comparison between Unknown and I4
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Invalid comparison between Unknown and I4
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Invalid comparison between Unknown and I4
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			Point position = ((MouseState)(ref ms)).get_Position();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			Point val = position - ((Rectangle)(ref absoluteBounds)).get_Location();
			CefEvtModifiresFlags cefEvtModifiresFlags = GetCurrentKeyboardModifiers();
			if (_isTriggerMouseLeftButtonPressed)
			{
				cefEvtModifiresFlags |= CefEvtModifiresFlags.LeftMouseButton;
			}
			else if (_isTriggerMouseRightButtonPressed)
			{
				cefEvtModifiresFlags |= CefEvtModifiresFlags.RightMouseButton;
			}
			if ((int)mouseEventType == 513)
			{
				_isTriggerMouseLeftButtonPressed = true;
			}
			else if ((int)mouseEventType == 516)
			{
				_isTriggerMouseRightButtonPressed = true;
			}
			else if ((int)mouseEventType == 514)
			{
				_isTriggerMouseLeftButtonPressed = false;
			}
			else if ((int)mouseEventType == 517)
			{
				_isTriggerMouseRightButtonPressed = false;
			}
			Browser.SendCursorEvent(val.X, val.Y, ((MouseState)(ref ms)).get_ScrollWheelValue(), mouseEventType, (int)cefEvtModifiresFlags, Settings.IsUseTouch.get_Value());
		}

		private void TriggerMissedMouseEvent()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Invalid comparison between Unknown and I4
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			if (!base._mouseOver)
			{
				return;
			}
			MouseState state = GameService.Input.get_Mouse().get_State();
			bool flag = (int)((MouseState)(ref state)).get_LeftButton() == 1;
			if (flag != _isMouseStateLeftButtonPressed)
			{
				_isMouseStateLeftButtonPressed = flag;
				if (flag != _isTriggerMouseLeftButtonPressed)
				{
					MouseHandler((MouseEventType)(flag ? 513 : 514), state);
				}
			}
			bool flag2 = (int)((MouseState)(ref state)).get_RightButton() == 1;
			if (flag2 != _isMouseStateRightButtonPressed)
			{
				_isMouseStateRightButtonPressed = flag2;
				if (flag2 != _isTriggerMouseRightButtonPressed)
				{
					MouseHandler((MouseEventType)(flag2 ? 516 : 517), state);
				}
			}
		}

		private void KeyboardHandler(object sender, KeyboardEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Invalid comparison between Unknown and I4
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected I4, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected I4, but got Unknown
			if (base._mouseOver && !((Game)WebPeeperModule.BlishHudInstance).get_Window().IsForeground())
			{
				bool flag = (int)e.get_EventType() == 256;
				Keys key = e.get_Key();
				int num;
				switch (key - 160)
				{
				case 4:
				case 5:
					num = 18;
					break;
				case 2:
				case 3:
					num = 17;
					break;
				case 0:
				case 1:
					num = 16;
					break;
				default:
					num = (int)e.get_Key();
					break;
				}
				int num2 = num;
				Browser.SendKeyEvent((int)GetCurrentKeyboardModifiers(), flag, num2);
			}
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Keyboard().remove_KeyStateChanged((EventHandler<KeyboardEventArgs>)KeyboardHandler);
			Browser.remove_Paint((Func<IntPtr, int, int, bool>)CefOnPaint);
			Browser.remove_FrameLoadStart((Action)OnFrameLoadStart);
			Browser.remove_UrlLoadError((Action<string>)OnUrlLoadError);
			_cefPaintProcess?.Dispose();
			ClearWebTextureBufferQueued();
		}

		public static void DisposeWebTexture()
		{
			Texture2D webTexture = _webTexture;
			if (webTexture != null)
			{
				((GraphicsResource)webTexture).Dispose();
			}
			_webTexture = null;
		}

		private static CefEvtModifiresFlags GetCurrentKeyboardModifiers()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			CefEvtModifiresFlags cefEvtModifiresFlags = CefEvtModifiresFlags.None;
			ModifierKeys activeModifiers = GameService.Input.get_Keyboard().get_ActiveModifiers();
			if (((Enum)activeModifiers).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				cefEvtModifiresFlags |= CefEvtModifiresFlags.ControlDown;
			}
			if (((Enum)activeModifiers).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				cefEvtModifiresFlags |= CefEvtModifiresFlags.AltDown;
			}
			if (((Enum)activeModifiers).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				cefEvtModifiresFlags |= CefEvtModifiresFlags.ShiftDown;
			}
			return cefEvtModifiresFlags;
		}
	}
}
