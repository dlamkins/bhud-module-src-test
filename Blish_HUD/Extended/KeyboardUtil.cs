using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Blish_HUD.Extended.WinApi;

namespace Blish_HUD.Extended
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
			internal short wVk;

			internal short wScan;

			internal KeyEventF dwFlags;

			internal int time;

			internal UIntPtr dwExtraInfo;
		}

		private class ExtraKeyInfo
		{
			public ushort repeatCount;

			public char scanCode;

			public ushort extendedKey;

			public ushort prevKeyState;

			public ushort transitionState;

			public int GetInt()
			{
				return (int)(repeatCount | ((uint)scanCode << 16)) | (extendedKey << 24) | (prevKeyState << 30) | (transitionState << 31);
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

		private static List<int> _extendedKeys = new List<int>
		{
			45, 36, 34, 46, 35, 33, 165, 161, 163, 38,
			40, 37, 39, 144, 42
		};

		[DllImport("USER32.dll")]
		private static extern short GetKeyState(uint vk);

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SendInput(uint nInputs, [In][MarshalAs(UnmanagedType.LPArray)] Input[] pInputs, int cbSize);

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
				scanCode = (char)MapVirtualKey((uint)keyCode, 0u)
			};
			if (_extendedKeys.Contains(keyCode))
			{
				lParam.extendedKey = 1;
			}
			uint msg = 256u;
			if (!pressed)
			{
				msg = 257u;
				lParam.repeatCount = 1;
				lParam.prevKeyState = 1;
				lParam.transitionState = 1;
			}
			return PostMessage(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), msg, (uint)keyCode, lParam.GetInt());
		}

		private static bool SendInput(int keyCode, bool pressed)
		{
			KeyEventF dwFlags = ((!pressed) ? KeyEventF.KEYUP : ((KeyEventF)0u));
			Input input;
			InputUnion u;
			KeybdInput ki;
			Input[] nInputs;
			if (_extendedKeys.Contains(keyCode))
			{
				Input[] array = new Input[2];
				input = new Input
				{
					type = InputType.KEYBOARD
				};
				u = default(InputUnion);
				ki = new KeybdInput
				{
					wScan = 224,
					wVk = 0,
					dwFlags = (KeyEventF)0u
				};
				u.ki = ki;
				input.U = u;
				array[0] = input;
				input = new Input
				{
					type = InputType.KEYBOARD
				};
				u = default(InputUnion);
				ki = new KeybdInput
				{
					wScan = (short)MapVirtualKey((uint)keyCode, 0u),
					wVk = (short)keyCode,
					dwFlags = (KeyEventF.EXTENDEDKEY | dwFlags)
				};
				u.ki = ki;
				input.U = u;
				array[1] = input;
				nInputs = array;
			}
			else
			{
				Input[] array2 = new Input[1];
				input = new Input
				{
					type = InputType.KEYBOARD
				};
				u = default(InputUnion);
				ki = new KeybdInput
				{
					wScan = (short)MapVirtualKey((uint)keyCode, 0u),
					wVk = (short)keyCode,
					dwFlags = dwFlags
				};
				u.ki = ki;
				input.U = u;
				array2[0] = input;
				nInputs = array2;
			}
			return SendInput((uint)nInputs.Length, nInputs, Input.Size) != 0;
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
