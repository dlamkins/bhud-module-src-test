using System;
using System.Runtime.InteropServices;

namespace Nekres.Mistwar
{
	internal static class PInvoke
	{
		private const uint KEY_PRESSED = 32768u;

		private const uint VK_LCONTROL = 162u;

		private const uint VK_LSHIFT = 160u;

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

		public static bool IsLShiftPressed()
		{
			return Convert.ToBoolean((long)GetKeyState(160u) & 0x8000L);
		}
	}
}
