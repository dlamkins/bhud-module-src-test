using System;
using System.Runtime.InteropServices;

namespace Nekres.Inquest_Module
{
	internal static class KeyStateUtil
	{
		private const uint KEY_PRESSED = 32768u;

		private const uint VK_LSHIFT = 160u;

		private const uint VK_RSHIFT = 161u;

		private const uint VK_LCONTROL = 162u;

		private const uint VK_RCONTROL = 163u;

		private const uint VK_CONTROL = 17u;

		private const uint VK_SHIFT = 16u;

		[DllImport("USER32.dll")]
		private static extern short GetKeyState(uint vk);

		private static bool IsPressed(uint key)
		{
			return Convert.ToBoolean((long)GetKeyState(key) & 0x8000L);
		}

		public static bool IsLControlPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(162u) & 0x8000L);
		}

		public static bool IsRControlPressed()
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

		public static bool IsAnyControlPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(17u) & 0x8000L);
		}

		public static bool IsAnyShiftPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(16u) & 0x8000L);
		}
	}
}
