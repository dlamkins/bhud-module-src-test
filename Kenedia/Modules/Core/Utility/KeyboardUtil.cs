using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Blish_HUD;
using Kenedia.Modules.Core.Utility.WinApi;

namespace Kenedia.Modules.Core.Utility
{
	public static class KeyboardUtil
	{
		[Flags]
		internal enum KeyEventF : uint
		{
			EXTENDEDKEY = 0x1u,
			KEYUP = 0x2u,
			SCANCODE = 0x8u,
			UNICODE = 0x4u
		}

		internal struct KeybdInput
		{
			internal short _wVk;

			internal short _wScan;

			internal KeyEventF _dwFlags;

			internal int _time;

			internal UIntPtr _dwExtraInfo;
		}

		private class ExtraKeyInfo
		{
			public ushort RepeatCount;

			public char ScanCode;

			public ushort ExtendedKey;

			public ushort PrevKeyState;

			public ushort TransitionState;

			public int GetInt()
			{
				return (int)(RepeatCount | ((uint)ScanCode << 16)) | (ExtendedKey << 24) | (PrevKeyState << 30) | (TransitionState << 31);
			}
		}

		private const uint WM_KEYDOWN = 256u;

		private const uint WM_KEYUP = 257u;

		private const uint WM_CHAR = 258u;

		private const uint WM_PASTE = 770u;

		private const uint MAPVK_VK_TO_VSC = 0u;

		private const uint MAPVK_VSC_TO_VK = 1u;

		private const uint MAPVK_VK_TO_CHAR = 2u;

		private const uint MAPVK_VSC_TO_VK_EX = 3u;

		private const uint MAPVK_VK_TO_VSC_EX = 4u;

		private const uint KEY_PRESSED = 32768u;

		private const uint VK_LSHIFT = 160u;

		private const uint VK_RSHIFT = 161u;

		private const uint VK_LCONTROL = 162u;

		private const uint VK_RCONTROL = 163u;

		private const uint VK_CONTROL = 17u;

		private const uint VK_SHIFT = 16u;

		private static readonly List<int> s_extendedKeys = new List<int>
		{
			45, 36, 34, 46, 35, 33, 165, 161, 163, 38,
			40, 37, 39, 144, 42
		};

		[DllImport("USER32.dll")]
		private static extern short GetKeyState(uint vk);

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SendInput(uint nInputs, [In][MarshalAs(UnmanagedType.LPArray)] Kenedia.Modules.Core.Utility.WinApi.Input[] pInputs, int cbSize);

		[DllImport("kernel32.dll")]
		private static extern uint GetLastError();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SendMessage(IntPtr hWnd, uint msg, uint wParam, int lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, int lParam);

		public static bool Press(int keyCode, bool sendToSystem = true)
		{
			return EmulateInput(keyCode, pressed: true, sendToSystem);
		}

		public static bool Release(int keyCode, bool sendToSystem = true)
		{
			return EmulateInput(keyCode, pressed: false, sendToSystem);
		}

		private static bool EmulateInput(int keyCode, bool pressed, bool sendToSystem = false)
		{
			DateTime waitTil = DateTime.UtcNow.AddMilliseconds(500.0);
			while (DateTime.UtcNow < waitTil)
			{
				if (sendToSystem)
				{
					if (SendInput(keyCode, pressed))
					{
						return true;
					}
				}
				else if (PostMessage(keyCode, pressed))
				{
					return true;
				}
			}
			return false;
		}

		private static bool PostMessage(int keyCode, bool pressed)
		{
			ExtraKeyInfo lParam = new ExtraKeyInfo
			{
				ScanCode = (char)MapVirtualKey((uint)keyCode, 0u)
			};
			if (s_extendedKeys.Contains(keyCode))
			{
				lParam.ExtendedKey = 1;
			}
			uint msg = 256u;
			if (!pressed)
			{
				msg = 257u;
				lParam.RepeatCount = 1;
				lParam.PrevKeyState = 1;
				lParam.TransitionState = 1;
			}
			return PostMessage(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), msg, (uint)keyCode, lParam.GetInt());
		}

		private static bool SendInput(int keyCode, bool pressed)
		{
			KeyEventF dwFlags = ((!pressed) ? KeyEventF.KEYUP : ((KeyEventF)0u));
			Kenedia.Modules.Core.Utility.WinApi.Input[] array;
			Kenedia.Modules.Core.Utility.WinApi.Input input;
			InputUnion u;
			KeybdInput ki;
			if (!s_extendedKeys.Contains(keyCode))
			{
				array = new Kenedia.Modules.Core.Utility.WinApi.Input[1];
				Kenedia.Modules.Core.Utility.WinApi.Input[] obj = array;
				input = new Kenedia.Modules.Core.Utility.WinApi.Input
				{
					_type = InputType.KEYBOARD
				};
				u = default(InputUnion);
				ki = new KeybdInput
				{
					_wScan = (short)MapVirtualKey((uint)keyCode, 0u),
					_wVk = (short)keyCode,
					_dwFlags = dwFlags
				};
				u._ki = ki;
				input._u = u;
				obj[0] = input;
			}
			else
			{
				array = new Kenedia.Modules.Core.Utility.WinApi.Input[2];
				Kenedia.Modules.Core.Utility.WinApi.Input[] obj2 = array;
				input = new Kenedia.Modules.Core.Utility.WinApi.Input
				{
					_type = InputType.KEYBOARD
				};
				u = default(InputUnion);
				ki = new KeybdInput
				{
					_wScan = 224,
					_wVk = 0,
					_dwFlags = (KeyEventF)0u
				};
				u._ki = ki;
				input._u = u;
				obj2[0] = input;
				Kenedia.Modules.Core.Utility.WinApi.Input[] obj3 = array;
				input = new Kenedia.Modules.Core.Utility.WinApi.Input
				{
					_type = InputType.KEYBOARD
				};
				u = default(InputUnion);
				ki = new KeybdInput
				{
					_wScan = (short)MapVirtualKey((uint)keyCode, 0u),
					_wVk = (short)keyCode,
					_dwFlags = (KeyEventF.EXTENDEDKEY | dwFlags)
				};
				u._ki = ki;
				input._u = u;
				obj3[1] = input;
			}
			Kenedia.Modules.Core.Utility.WinApi.Input[] nInputs = array;
			return SendInput((uint)nInputs.Length, nInputs, Kenedia.Modules.Core.Utility.WinApi.Input.Size) != 0;
		}

		public static bool Paste(bool sendToSystem = true)
		{
			if (!Press(162, sendToSystem))
			{
				return false;
			}
			if (!Stroke(86, sendToSystem))
			{
				return false;
			}
			Thread.Sleep(50);
			return Release(162, sendToSystem);
		}

		public static bool Clear(bool sendToSystem = true)
		{
			if (!SelectAll(sendToSystem))
			{
				return false;
			}
			return Stroke(46, sendToSystem);
		}

		public static bool SelectAll(bool sendToSystem = true)
		{
			if (!Press(162, sendToSystem))
			{
				return false;
			}
			if (!Stroke(65, sendToSystem))
			{
				return false;
			}
			Thread.Sleep(50);
			return Release(162, sendToSystem);
		}

		public static bool Stroke(int keyCode, bool sendToSystem = true)
		{
			if (Press(keyCode, sendToSystem))
			{
				return Release(keyCode, sendToSystem);
			}
			return false;
		}

		public static bool IsPressed(uint keyCode)
		{
			return Convert.ToBoolean((long)GetKeyState(keyCode) & 0x8000L);
		}

		public static bool IsLCtrlPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(162u) & 0x8000L);
		}

		public static bool IsRCtrlPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(163u) & 0x8000L);
		}

		public static bool IsLShiftPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(160u) & 0x8000L);
		}

		public static bool IsRShiftPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(161u) & 0x8000L);
		}

		public static bool IsCtrlPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(17u) & 0x8000L);
		}

		public static bool IsShiftPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(16u) & 0x8000L);
		}
	}
}
