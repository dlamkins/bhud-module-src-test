using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CefHelper;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	internal class ImeService : NativeWindow
	{
		private readonly IntPtr _winHandle;

		private IntPtr _himc;

		private Message _m;

		private object _keyStateChangedCloned;

		private object _keyReleasedCloned;

		private object _keyPressedCloned;

		private bool _mouseLeftPressed;

		private Action _mouseLeftReleaseCallback;

		private readonly Dictionary<string, (Action<object>, Func<object>)> _keybindsBackupMap = new Dictionary<string, (Action<object>, Func<object>)>();

		public ImeService()
		{
			_winHandle = WebPeeperModule.BlishHudInstance.get_FormHandle();
			AssignHandle(_winHandle);
			ActiveAutoBlur();
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

		public void Unload()
		{
			ReleaseHandle();
			WebPeeperModule.BlishHudInstance.get_Form().LostFocus -= OnHudLostFocus;
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
		}

		protected override void WndProc(ref Message m)
		{
			UiService uiService = WebPeeperModule.Instance.UiService;
			int num;
			if (uiService == null)
			{
				num = 0;
			}
			else
			{
				BrowserWindow browserWindow = uiService.BrowserWindow;
				num = (((browserWindow != null) ? new bool?(((Control)browserWindow).get_Visible()) : null).GetValueOrDefault() ? 1 : 0);
			}
			if (num != 0 && CefService.LibLoadStarted)
			{
				_m = m;
				if (HandleMsg())
				{
					return;
				}
			}
			base.WndProc(ref m);
		}

		private bool HandleMsg()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Invalid comparison between Unknown and I4
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			bool result = false;
			WM val = (WM)_m.Msg;
			if (val - 256 > 2)
			{
				switch (val - 269)
				{
				case 2:
					SetCefComposition();
					result = true;
					break;
				case 1:
					Browser.ImeSetComposition("", 0, Array.Empty<(int, int, uint, uint, bool)>());
					Browser.ImeFinishComposingText();
					result = true;
					break;
				case 0:
					result = true;
					break;
				}
			}
			else
			{
				Browser.SendKeyEvent(_m.Msg, _m.WParam, _m.LParam);
				result = true;
			}
			_m = default(Message);
			return result;
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
			WebPeeperModule.Logger.Debug("ImeService.OnHudLostFocus: Blish.HUD lost focus, do BlurInput");
			Browser.BlurInput();
		}

		private void SetCefComposition()
		{
			if (GetCompositionText(GCS.RESULTSTR, out var text))
			{
				Browser.ImeCommitText(text);
				Browser.ImeSetComposition(text, 0, Array.Empty<(int, int, uint, uint, bool)>());
				Browser.ImeFinishComposingText();
			}
			else if (GetCompositionText(GCS.COMPSTR, out text))
			{
				GetCompositionSelection(text, out var underlines, out var caretPosition);
				Browser.ImeSetComposition(text, caretPosition, underlines.ToArray());
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

		private void SetCompositionPostion()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (WebPainter.Instance == null)
			{
				return;
			}
			int x = WebPainter.Instance.LocationAtForm.X;
			int y = WebPainter.Instance.LocationAtForm.Y;
			Browser.GetInputPosition().ContinueWith(delegate(Task<(int, int)> t)
			{
				(int, int) result = t.Result;
				int item = result.Item1;
				int item2 = result.Item2;
				int offsetX = (int)((float)item * GameService.Graphics.get_UIScaleMultiplier());
				int offsetY = (int)((float)item2 * GameService.Graphics.get_UIScaleMultiplier());
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

		private void GetCompositionSelection(string text, out List<(int, int, uint, uint, bool)> underlines, out int caretPosition)
		{
			int i = text.Length;
			int j = text.Length;
			caretPosition = 0;
			underlines = new List<(int, int, uint, uint, bool)>();
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
					int num4 = BitConverter.ToInt32(array2, k * 4);
					int num5 = BitConverter.ToInt32(array2, (k + 1) * 4);
					bool item = num4 >= i && num5 <= j;
					underlines.Add((num4, num5, 4278190080u, 0u, item));
				}
			}
			if (underlines.Count == 0)
			{
				int item2 = 0;
				int item3 = 0;
				bool item4 = false;
				if (i > 0)
				{
					item3 = i;
				}
				if (j > i)
				{
					item2 = i;
					item3 = j;
					item4 = true;
				}
				if (j < text.Length)
				{
					item2 = j;
					item3 = text.Length;
				}
				underlines.Add((item2, item3, 4278190080u, 0u, item4));
			}
		}

		private void DisableAllKeybinds()
		{
			WebPeeperModule.Logger.Debug("ImeService.DisableAllKeybinds");
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

		private void ActiveAutoBlur()
		{
			if (CefService.LibLoadStarted)
			{
				active();
				return;
			}
			CefService.LibLoadStart += delegate
			{
				active();
			};
			void active()
			{
				WebPeeperModule.BlishHudInstance.get_Form().LostFocus -= OnHudLostFocus;
				WebPeeperModule.BlishHudInstance.get_Form().LostFocus += OnHudLostFocus;
			}
		}

		private void RestoreAllKeybinds()
		{
			WebPeeperModule.Logger.Debug("ImeService.RestoreAllKeybinds");
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
			WebPeeperModule.Logger.Debug("ImeService.Enable: enable typing");
			WebPeeperModule.Logger.Debug("ImeService.Enable: bring Blish.HUD to the foreground");
			Utils.SetForegroundWindow(_winHandle);
			if (!((Game)WebPeeperModule.BlishHudInstance).get_Window().IsForeground())
			{
				WebPeeperModule.Logger.Debug("ImeService.Enable: retry bring Blish.HUD to the foreground");
				GameService.GameIntegration.get_Gw2Instance().FocusGw2();
				Utils.SetForegroundWindow(_winHandle);
			}
			Utils.ImmAssociateContextEx(_winHandle, 0, 16);
			_himc = Utils.ImmGetContext(_winHandle);
			Utils.ImmSetOpenStatus(_himc, isOpen: true);
			SetCompositionPostion();
		}

		public void Disable(bool tryFocusGame = true)
		{
			if (!(_himc == IntPtr.Zero))
			{
				RestoreAllKeybinds();
				WebPeeperModule.Logger.Debug("ImeService.Disable: disable typing");
				Utils.ImmSetOpenStatus(_himc, isOpen: false);
				Utils.ImmReleaseContext(_winHandle, _himc);
				_himc = IntPtr.Zero;
				if (tryFocusGame && ((Game)WebPeeperModule.BlishHudInstance).get_Window().IsForeground())
				{
					GameService.GameIntegration.get_Gw2Instance().FocusGw2();
				}
			}
		}
	}
}
