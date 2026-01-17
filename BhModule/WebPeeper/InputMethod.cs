using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Input;
using CefSharp;
using CefSharp.Internals;
using CefSharp.OffScreen;
using CefSharp.Structs;

namespace BhModule.WebPeeper
{
	public class InputMethod : NativeWindow
	{
		private IntPtr _himc;

		private Message _m;

		private object _keyStateChangedCloned;

		private object _keyReleasedCloned;

		private object _keyPressedCloned;

		private bool _mouseLeftPressed;

		private Action _mouseLeftReleaseCallback;

		private readonly Dictionary<string, (Action<object>, Func<object>)> _keybindsBackupMap = new Dictionary<string, (Action<object>, Func<object>)>();

		private IntPtr WinHandle => WebPeeperModule.BlishHudInstance.get_FormHandle();

		private ChromiumWebBrowser Browser => WebPeeperModule.Instance.CefService.WebBrowser;

		public InputMethod()
		{
			AssignHandle(WinHandle);
			WebPeeperModule.BlishHudInstance.get_Form().LostFocus += OnHudLostFocus;
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
			_keybindsBackupMap.Add("KeyStateChanged", (delegate(object v)
			{
				_keyStateChangedCloned = v;
			}, () => _keyStateChangedCloned));
			_keybindsBackupMap.Add("KeyReleased", (delegate(object v)
			{
				_keyReleasedCloned = v;
			}, () => _keyReleasedCloned));
			_keybindsBackupMap.Add("KeyPressed", (delegate(object v)
			{
				_keyPressedCloned = v;
			}, () => _keyPressedCloned));
		}

		public void Dispose()
		{
			ReleaseHandle();
			WebPeeperModule.BlishHudInstance.get_Form().LostFocus -= OnHudLostFocus;
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
		}

		protected override void WndProc(ref Message m)
		{
			_m = m;
			switch (m.Msg)
			{
			case 256:
			case 257:
			case 258:
				SendKey();
				break;
			case 271:
				SetCefComposition();
				return;
			case 270:
				Browser?.GetBrowserHost().ImeSetComposition("", Array.Empty<CompositionUnderline>(), new Range(int.MaxValue, int.MaxValue), new Range(0, 0));
				Browser?.GetBrowserHost().ImeFinishComposingText(keepSelection: false);
				return;
			case 269:
				return;
			}
			_m = default(Message);
			base.WndProc(ref m);
		}

		private bool LParmHasFlag(object flag)
		{
			int num = (int)flag;
			return (_m.LParam.ToInt64() & num) == num;
		}

		private void OnLeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			_mouseLeftPressed = true;
		}

		private void OnLeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			_mouseLeftPressed = false;
			_mouseLeftReleaseCallback?.Invoke();
			_mouseLeftReleaseCallback = null;
		}

		private void OnHudLostFocus(object sender, EventArgs e)
		{
			if (Browser != null && Browser.CanExecuteJavascriptInMainFrame)
			{
				Browser.ExecuteScriptAsync("webPeeper_blur()");
			}
		}

		private void SendKey()
		{
			int num = _m.WParam.CastToInt32();
			int num2 = _m.LParam.CastToInt32();
			KeyEvent keyEvent = default(KeyEvent);
			keyEvent.Modifiers = GetCefKeyboardModifiers(num, num2);
			keyEvent.WindowsKeyCode = num;
			keyEvent.NativeKeyCode = num2;
			keyEvent.IsSystemKey = false;
			keyEvent.Type = _m.Msg switch
			{
				256 => KeyEventType.KeyDown, 
				257 => KeyEventType.KeyUp, 
				258 => KeyEventType.Char, 
				_ => (KeyEventType)(-1), 
			};
			KeyEvent keyEvent2 = keyEvent;
			Browser.GetBrowserHost().SendKeyEvent(keyEvent2);
		}

		private static bool IsKeyDown(VK wparam)
		{
			return (Utils.GetKeyState(wparam) & 0x8000) != 0;
		}

		private static CefEventFlags GetCefKeyboardModifiers(int wParam, int lParam)
		{
			CefEventFlags cefEventFlags = CefEventFlags.None;
			if (IsKeyDown(VK.SHIFT))
			{
				cefEventFlags |= CefEventFlags.ShiftDown;
			}
			if (IsKeyDown(VK.CONTROL))
			{
				cefEventFlags |= CefEventFlags.ControlDown;
			}
			if (IsKeyDown(VK.MENU))
			{
				cefEventFlags |= CefEventFlags.AltDown;
			}
			if (((uint)Utils.GetKeyState(VK.NUMLOCK) & (true ? 1u : 0u)) != 0)
			{
				cefEventFlags |= CefEventFlags.NumLockOn;
			}
			if (((uint)Utils.GetKeyState(VK.CAPITAL) & (true ? 1u : 0u)) != 0)
			{
				cefEventFlags |= CefEventFlags.CapsLockOn;
			}
			switch (wParam)
			{
			case 13:
				if (((uint)(lParam >> 16) & 0x100u) != 0)
				{
					cefEventFlags |= CefEventFlags.IsKeyPad;
				}
				break;
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
			case 40:
			case 45:
			case 46:
				if (((lParam >> 16) & 0x100) == 0)
				{
					cefEventFlags |= CefEventFlags.IsKeyPad;
				}
				break;
			case 12:
			case 96:
			case 97:
			case 98:
			case 99:
			case 100:
			case 101:
			case 102:
			case 103:
			case 104:
			case 105:
			case 106:
			case 107:
			case 109:
			case 110:
			case 111:
			case 144:
				cefEventFlags |= CefEventFlags.IsKeyPad;
				break;
			case 16:
				if (IsKeyDown(VK.LSHIFT))
				{
					cefEventFlags |= CefEventFlags.IsLeft;
				}
				else if (IsKeyDown(VK.RSHIFT))
				{
					cefEventFlags |= CefEventFlags.IsRight;
				}
				break;
			case 17:
				if (IsKeyDown(VK.LCONTROL))
				{
					cefEventFlags |= CefEventFlags.IsLeft;
				}
				else if (IsKeyDown(VK.RCONTROL))
				{
					cefEventFlags |= CefEventFlags.IsRight;
				}
				break;
			case 18:
				if (IsKeyDown(VK.LMENU))
				{
					cefEventFlags |= CefEventFlags.IsLeft;
				}
				else if (IsKeyDown(VK.RMENU))
				{
					cefEventFlags |= CefEventFlags.IsRight;
				}
				break;
			case 91:
				cefEventFlags |= CefEventFlags.IsLeft;
				break;
			case 92:
				cefEventFlags |= CefEventFlags.IsRight;
				break;
			}
			return cefEventFlags;
		}

		private void SetCefComposition()
		{
			IBrowserHost browserHost = Browser?.GetBrowserHost();
			if (browserHost != null)
			{
				if (GetCompositionText(GCS.RESULTSTR, out var text))
				{
					browserHost.ImeCommitText(text, new Range(int.MaxValue, int.MaxValue), 0);
					browserHost.ImeSetComposition(text, Array.Empty<CompositionUnderline>(), new Range(int.MaxValue, int.MaxValue), new Range(0, 0));
					browserHost.ImeFinishComposingText(keepSelection: false);
				}
				else if (GetCompositionText(GCS.COMPSTR, out text))
				{
					GetCompositionSelection(text, out var underlines, out var caretPosition);
					browserHost.ImeSetComposition(text, underlines.ToArray(), new Range(int.MaxValue, int.MaxValue), new Range(caretPosition, caretPosition));
				}
			}
		}

		private bool GetCompositionText(GCS gcs, out string text)
		{
			int num = Utils.ImmGetCompositionStringW(_himc, (int)gcs, null, 0);
			if (num <= 0)
			{
				text = string.Empty;
				return false;
			}
			byte[] array = new byte[num];
			Utils.ImmGetCompositionStringW(_himc, (int)gcs, array, num);
			text = Encoding.Unicode.GetString(array);
			return true;
		}

		public void SetCompositionPostion()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (Browser == null || WebPainter.Instance == null)
			{
				return;
			}
			int x = WebPainter.Instance.LocationAtForm.X;
			int y = WebPainter.Instance.LocationAtForm.Y;
			Browser.EvaluateScriptAsync("webPeeper_getFocusLocation()").ContinueWith(delegate(Task<JavascriptResponse> t)
			{
				List<object> obj = (List<object>)t.Result.Result;
				int num = (int)obj[0] + 2;
				int num2 = (int)obj[1];
				int offsetX = (int)((float)num * GameService.Graphics.get_UIScaleMultiplier());
				int offsetY = (int)((float)num2 * GameService.Graphics.get_UIScaleMultiplier());
				WebPeeperModule.BlishHudInstance.get_Form().SafeInvoke(delegate
				{
					Utils.ImmSetCompositionWindow(_himc, new CompositionForm
					{
						Style = 2u,
						X = x + offsetX,
						Y = y + offsetY
					});
				});
			});
		}

		private void GetCompositionSelection(string text, out List<CompositionUnderline> underlines, out int caretPosition)
		{
			int i = text.Length;
			int j = text.Length;
			caretPosition = 0;
			underlines = new List<CompositionUnderline>();
			if (LParmHasFlag(GCS.COMPATTR))
			{
				int num = Utils.ImmGetCompositionStringW(_himc, 16, null, 0);
				if (num > 0)
				{
					byte[] array = new byte[num];
					Utils.ImmGetCompositionStringW(_himc, 16, array, num);
					for (i = 0; i < num && array[i] != 1 && array[i] != 3; i++)
					{
					}
					for (j = i; j < num; j++)
					{
						if (array[i] != 1 && array[i] != 3)
						{
							break;
						}
					}
				}
			}
			if (!LParmHasFlag(16384) && LParmHasFlag(GCS.CURSORPOS))
			{
				caretPosition = Utils.ImmGetCompositionStringW(_himc, 128, null, 0);
			}
			if (LParmHasFlag(GCS.COMPCLAUSE))
			{
				int num2 = Utils.ImmGetCompositionStringW(_himc, 32, null, 0);
				int num3 = num2 / 4;
				byte[] array2 = new byte[num2];
				Utils.ImmGetCompositionStringW(_himc, 32, array2, num2);
				for (int k = 0; k < num3 - 1; k++)
				{
					int from = BitConverter.ToInt32(array2, k * 4);
					int to = BitConverter.ToInt32(array2, (k + 1) * 4);
					Range range = new Range(from, to);
					bool thick = range.From >= i && range.To <= j;
					underlines.Add(new CompositionUnderline(range, 4278190080u, 0u, thick));
				}
			}
			if (underlines.Count == 0)
			{
				Range range2 = default(Range);
				bool thick2 = false;
				if (i > 0)
				{
					range2 = new Range(0, i);
				}
				if (j > i)
				{
					range2 = new Range(i, j);
					thick2 = true;
				}
				if (j < text.Length)
				{
					range2 = new Range(j, text.Length);
				}
				underlines.Add(new CompositionUnderline(range2, 4278190080u, 0u, thick2));
			}
		}

		private void DisableAllKeybinds()
		{
			foreach (KeyValuePair<string, (Action<object>, Func<object>)> item in _keybindsBackupMap)
			{
				string key = item.Key;
				(Action<object>, Func<object>) value = item.Value;
				var (action, _) = value;
				if (value.Item2() == null)
				{
					FieldInfo field = typeof(KeyboardHandler).GetField(key, BindingFlags.Instance | BindingFlags.NonPublic);
					EventHandler<KeyboardEventArgs> eventHandler = field.GetValue(GameService.Input.get_Keyboard()) as EventHandler<KeyboardEventArgs>;
					if (eventHandler != null)
					{
						action(eventHandler?.Clone());
						field.SetValue(GameService.Input.get_Keyboard(), null);
					}
				}
			}
		}

		private void RestoreAllKeybinds()
		{
			foreach (KeyValuePair<string, (Action<object>, Func<object>)> item2 in _keybindsBackupMap)
			{
				string key = item2.Key;
				(Action<object>, Func<object>) value = item2.Value;
				Action<object> item = value.Item1;
				object obj = value.Item2();
				if (obj == null)
				{
					break;
				}
				typeof(KeyboardHandler).GetField(key, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(GameService.Input.get_Keyboard(), obj);
				item(null);
			}
		}

		public void Enable()
		{
			if (_mouseLeftPressed)
			{
				_mouseLeftReleaseCallback = Enable;
				return;
			}
			if (_himc != IntPtr.Zero)
			{
				Disable(tryFocusGame: false);
			}
			if (WebPeeperModule.Instance.Settings.IsBlockKeybinds.get_Value())
			{
				DisableAllKeybinds();
			}
			Utils.SetForegroundWindow(WinHandle);
			if (!WebPeeperModule.BlishHudInstance.get_Form().Focused)
			{
				GameService.GameIntegration.get_Gw2Instance().FocusGw2();
				Utils.SetForegroundWindow(WinHandle);
			}
			Utils.ImmAssociateContextEx(WinHandle, 0, 16);
			_himc = Utils.ImmGetContext(WinHandle);
			Utils.ImmSetOpenStatus(_himc, isOpen: true);
			SetCompositionPostion();
		}

		public void Disable(bool tryFocusGame = true)
		{
			if (!(_himc == IntPtr.Zero))
			{
				RestoreAllKeybinds();
				Utils.ImmSetOpenStatus(_himc, isOpen: false);
				Utils.ImmReleaseContext(WinHandle, _himc);
				_himc = IntPtr.Zero;
				if (tryFocusGame && WebPeeperModule.BlishHudInstance.get_Form().Focused)
				{
					GameService.GameIntegration.get_Gw2Instance().FocusGw2();
				}
			}
		}
	}
}
