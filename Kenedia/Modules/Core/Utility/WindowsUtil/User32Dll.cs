using System;
using System.Runtime.InteropServices;

namespace Kenedia.Modules.Core.Utility.WindowsUtil
{
	public class User32Dll
	{
		public struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;

			public bool Matches(RECT rect)
			{
				if (rect.Left == Left && rect.Top == Top && rect.Right == Right)
				{
					return rect.Bottom == Bottom;
				}
				return false;
			}
		}

		public const uint WM_COMMAND = 273u;

		public const uint WM_PASTE = 770u;

		[DllImport("user32")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32")]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
	}
}
