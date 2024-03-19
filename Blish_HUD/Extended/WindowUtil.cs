using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Blish_HUD.Extended
{
	public static class WindowUtil
	{
		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		public static bool GetInnerBounds(IntPtr hWnd, out Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			bounds = Rectangle.get_Empty();
			if (!GetWindowRect(hWnd, out var wndBounds) || !GetClientRect(hWnd, out var clientBounds))
			{
				return false;
			}
			int widthOffset = wndBounds.Right - wndBounds.Left - (clientBounds.Right - clientBounds.Left);
			int heightOffset = wndBounds.Bottom - wndBounds.Top - (clientBounds.Bottom - clientBounds.Top);
			int width = Math.Abs(wndBounds.Left - wndBounds.Right) - widthOffset * 2;
			int height = Math.Abs(wndBounds.Top - wndBounds.Bottom) - heightOffset * 2;
			bounds = new Rectangle(wndBounds.Left + widthOffset, wndBounds.Top + heightOffset, width, height);
			return true;
		}
	}
}
