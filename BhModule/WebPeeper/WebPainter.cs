using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using CefSharp;
using CefSharp.Enums;
using CefSharp.OffScreen;
using CefSharp.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BhModule.WebPeeper
{
	public class WebPainter : Control
	{
		public static WebPainter Instance;

		private static Texture2D _webTexture;

		private byte[] _webTextureBufferBytes = Array.Empty<byte>();

		private Rectangle _webTextureRect = Rectangle.get_Empty();

		private bool _isLeftMouseButtonPressed;

		private bool _isRightMouseButtonPressed;

		private static bool _isError = false;

		private static readonly Texture2D _errorTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("error.png");

		private static readonly Texture2D _quesTexture = WebPeeperModule.Instance.ContentsManager.GetTexture("what.png");

		private static readonly Color _errorTextureColor = new Color(2155905152u);

		private Rectangle _errorTextureRect = Rectangle.get_Empty();

		private Rectangle _questionTextureRect = Rectangle.get_Empty();

		private Process _cefPaintProcess;

		private Color _webBackgroundColor;

		public bool Disabled;

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

		private CefService CefService => WebPeeperModule.Instance.CefService;

		private ModuleSettings Settings => WebPeeperModule.Instance.Settings;

		private ChromiumWebBrowser WebBrowser => CefService.WebBrowser;

		public WebPainter()
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Instance = this;
			if (WebBrowser != null)
			{
				WebBrowser.Paint += CefOnPaint;
			}
			GameService.Input.get_Keyboard().add_KeyStateChanged((EventHandler<KeyboardEventArgs>)KeyboardHandler);
			ApplyBgTexture();
			if (_webTexture != null)
			{
				return;
			}
			CefService.GetScreenshot().ContinueWith(delegate(Task<Texture2D> t)
			{
				if (_webTexture == null)
				{
					_webTexture = t.Result;
				}
			});
		}

		public override Control TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Invalid comparison between Unknown and I4
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Invalid comparison between Unknown and I4
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Invalid comparison between Unknown and I4
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Invalid comparison between Unknown and I4
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Invalid comparison between Unknown and I4
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Invalid comparison between Unknown and I4
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			if (WebBrowser != null && WebBrowser.IsBrowserInitialized && !Disabled)
			{
				bool value = Settings.IsUseTouch.get_Value();
				IBrowserHost browserHost = WebBrowser.GetBrowserHost();
				Point position = ((MouseState)(ref ms)).get_Position();
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				Point val = position - ((Rectangle)(ref absoluteBounds)).get_Location();
				CefEventFlags cefEventFlags = GetCurrentKeyboardModifiers();
				if (_isLeftMouseButtonPressed)
				{
					cefEventFlags |= CefEventFlags.LeftMouseButton;
				}
				else if (_isRightMouseButtonPressed)
				{
					cefEventFlags |= CefEventFlags.RightMouseButton;
				}
				if ((int)mouseEventType == 512)
				{
					if (value && _isLeftMouseButtonPressed)
					{
						TouchEvent touchEvent = default(TouchEvent);
						touchEvent.Id = 1;
						touchEvent.X = val.X;
						touchEvent.Y = val.Y;
						touchEvent.PointerType = PointerType.Touch;
						touchEvent.Pressure = 1f;
						touchEvent.Type = TouchEventType.Moved;
						touchEvent.Modifiers = cefEventFlags;
						TouchEvent evt = touchEvent;
						browserHost.SendTouchEvent(evt);
					}
					else
					{
						MouseEvent mouseEvent = new MouseEvent(val.X, val.Y, cefEventFlags);
						browserHost.SendMouseMoveEvent(mouseEvent, mouseLeave: false);
					}
				}
				else if ((int)mouseEventType == 522)
				{
					MouseEvent mouseEvent2 = new MouseEvent(val.X, val.Y, cefEventFlags);
					int num = ((((MouseState)(ref ms)).get_ScrollWheelValue() < -1000) ? 240 : ((MouseState)(ref ms)).get_ScrollWheelValue());
					if (cefEventFlags.HasFlag(CefEventFlags.ControlDown) && !cefEventFlags.HasFlag(CefEventFlags.AltDown) && !cefEventFlags.HasFlag(CefEventFlags.ShiftDown))
					{
						Zoom((num > 0) ? 1 : (-1));
					}
					else
					{
						browserHost.SendMouseWheelEvent(mouseEvent2, 0, num);
					}
				}
				else if ((int)mouseEventType == 513)
				{
					browserHost.SendFocusEvent(setFocus: true);
					if (value)
					{
						TouchEvent touchEvent = default(TouchEvent);
						touchEvent.Id = 1;
						touchEvent.X = val.X;
						touchEvent.Y = val.Y;
						touchEvent.PointerType = PointerType.Touch;
						touchEvent.Pressure = 1f;
						touchEvent.Type = TouchEventType.Pressed;
						touchEvent.Modifiers = cefEventFlags;
						TouchEvent evt2 = touchEvent;
						browserHost.SendTouchEvent(evt2);
					}
					else
					{
						MouseEvent mouseEvent3 = new MouseEvent(val.X, val.Y, cefEventFlags);
						browserHost.SendMouseClickEvent(mouseEvent3, MouseButtonType.Left, mouseUp: false, 0);
					}
					_isLeftMouseButtonPressed = true;
				}
				else if ((int)mouseEventType == 516)
				{
					browserHost.SendFocusEvent(setFocus: true);
					MouseEvent mouseEvent4 = new MouseEvent(val.X, val.Y, cefEventFlags);
					browserHost.SendMouseClickEvent(mouseEvent4, MouseButtonType.Right, mouseUp: false, 0);
					_isRightMouseButtonPressed = true;
				}
				else if ((int)mouseEventType == 514)
				{
					if (value)
					{
						TouchEvent touchEvent = default(TouchEvent);
						touchEvent.Id = 1;
						touchEvent.X = val.X;
						touchEvent.Y = val.Y;
						touchEvent.PointerType = PointerType.Touch;
						touchEvent.Pressure = 1f;
						touchEvent.Type = TouchEventType.Released;
						touchEvent.Modifiers = cefEventFlags;
						TouchEvent evt3 = touchEvent;
						browserHost.SendTouchEvent(evt3);
					}
					else
					{
						MouseEvent mouseEvent5 = new MouseEvent(val.X, val.Y, cefEventFlags);
						browserHost.SendMouseClickEvent(mouseEvent5, MouseButtonType.Left, _isLeftMouseButtonPressed, 0);
					}
					_isLeftMouseButtonPressed = false;
				}
				else if ((int)mouseEventType == 517)
				{
					MouseEvent mouseEvent6 = new MouseEvent(val.X, val.Y, cefEventFlags);
					browserHost.SendMouseClickEvent(mouseEvent6, MouseButtonType.Right, _isRightMouseButtonPressed, 0);
					_isRightMouseButtonPressed = false;
				}
			}
			return ((Control)this).TriggerMouseInput(mouseEventType, ms);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((Rectangle)(ref _webTextureRect)).set_Size(((Control)this).get_Size());
			CefService.SetBrowserSize(((Control)this).get_Width(), ((Control)this).get_Height());
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
			}
			else if (_isError)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _errorTexture, _errorTextureRect, _errorTextureColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _quesTexture, _questionTextureRect, (Rectangle?)_quesTexture.get_Bounds(), _errorTextureColor, MathHelper.ToRadians(30f), Vector2.get_Zero(), (SpriteEffects)0);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _webTextureRect, _webBackgroundColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _webTexture, _webTextureRect);
			}
		}

		private void CefOnPaint(object sender, OnPaintEventArgs e)
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Invalid comparison between Unknown and I4
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			e.Handled = true;
			if (!((Control)WebPeeperModule.Instance.UIService.BrowserWindow).get_Visible())
			{
				return;
			}
			int num = e.Width * e.Height * 4;
			if (num != _webTextureBufferBytes.Length)
			{
				_webTextureBufferBytes = new byte[num];
			}
			if (_cefPaintProcess == null)
			{
				_cefPaintProcess = Process.GetCurrentProcess();
			}
			Utils.ReadProcessMemory(_cefPaintProcess.Handle, e.BufferHandle, _webTextureBufferBytes, num, out var _);
			if (_webTexture == null || (int)((Texture)_webTexture).get_Format() != 21 || _webTexture.get_Width() != e.Width || _webTexture.get_Height() != e.Height)
			{
				Texture2D webTexture = _webTexture;
				if (webTexture != null)
				{
					((GraphicsResource)webTexture).Dispose();
				}
				GraphicsDeviceContext val = Control.get_Graphics().LendGraphicsDeviceContext();
				try
				{
					_webTexture = new Texture2D(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), e.Width, e.Height, false, (SurfaceFormat)21);
				}
				finally
				{
					((GraphicsDeviceContext)(ref val)).Dispose();
				}
			}
			_webTexture.SetData<byte>(_webTextureBufferBytes);
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

		private void KeyboardHandler(object sender, KeyboardEventArgs e)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Invalid comparison between Unknown and I4
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected I4, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected I4, but got Unknown
			if (WebBrowser != null && WebBrowser.IsBrowserInitialized && base._mouseOver && !WebPeeperModule.BlishHudInstance.get_Form().Focused)
			{
				KeyEvent keyEvent = default(KeyEvent);
				keyEvent.Modifiers = GetCurrentKeyboardModifiers();
				keyEvent.Type = (((int)e.get_EventType() == 256) ? KeyEventType.KeyDown : KeyEventType.KeyUp);
				Keys key = e.get_Key();
				int windowsKeyCode;
				switch (key - 160)
				{
				case 4:
				case 5:
					windowsKeyCode = 18;
					break;
				case 2:
				case 3:
					windowsKeyCode = 17;
					break;
				case 0:
				case 1:
					windowsKeyCode = 16;
					break;
				default:
					windowsKeyCode = (int)e.get_Key();
					break;
				}
				keyEvent.WindowsKeyCode = windowsKeyCode;
				KeyEvent keyEvent2 = keyEvent;
				WebBrowser.GetBrowserHost().SendKeyEvent(keyEvent2);
			}
		}

		public void Zoom(float rate)
		{
			if (WebBrowser != null)
			{
				Task<double> zoomLevelAsync = WebBrowser.GetZoomLevelAsync();
				zoomLevelAsync.Wait(TimeSpan.FromSeconds(1.0));
				if (!zoomLevelAsync.IsCanceled)
				{
					WebBrowser.SetZoomLevel(zoomLevelAsync.Result + (double)(0.2f * rate));
				}
			}
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Keyboard().remove_KeyStateChanged((EventHandler<KeyboardEventArgs>)KeyboardHandler);
			if (WebBrowser != null)
			{
				WebBrowser.Paint -= CefOnPaint;
			}
			_cefPaintProcess?.Dispose();
		}

		public static void DisposeWebTexture()
		{
			Texture2D webTexture = _webTexture;
			if (webTexture != null)
			{
				((GraphicsResource)webTexture).Dispose();
			}
		}

		private static CefEventFlags GetCurrentKeyboardModifiers()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			CefEventFlags cefEventFlags = CefEventFlags.None;
			ModifierKeys activeModifiers = GameService.Input.get_Keyboard().get_ActiveModifiers();
			if (((Enum)activeModifiers).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				cefEventFlags |= CefEventFlags.ControlDown;
			}
			if (((Enum)activeModifiers).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				cefEventFlags |= CefEventFlags.AltDown;
			}
			if (((Enum)activeModifiers).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				cefEventFlags |= CefEventFlags.ShiftDown;
			}
			return cefEventFlags;
		}
	}
}
