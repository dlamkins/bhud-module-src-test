using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Frtal.LorebookReader
{
	public static class ScreenCapture
	{
		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		private struct POINT
		{
			public int X;

			public int Y;
		}

		private const int SM_CXSCREEN = 0;

		private const int SM_CYSCREEN = 1;

		[DllImport("user32.dll")]
		private static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

		[DllImport("user32.dll")]
		private static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

		[DllImport("user32.dll")]
		private static extern int GetSystemMetrics(int index);

		public static Bitmap Grab(IntPtr gw2WindowHandle, out Rectangle screenRect)
		{
			screenRect = GetGw2ClientRect(gw2WindowHandle) ?? new Rectangle(0, 0, GetSystemMetrics(0), GetSystemMetrics(1));
			Bitmap bmp = new Bitmap(screenRect.Width, screenRect.Height, PixelFormat.Format24bppRgb);
			using Graphics g = Graphics.FromImage(bmp);
			g.CopyFromScreen(screenRect.Left, screenRect.Top, 0, 0, new Size(screenRect.Width, screenRect.Height));
			return bmp;
		}

		private static Rectangle? GetGw2ClientRect(IntPtr hwnd)
		{
			if (hwnd == IntPtr.Zero)
			{
				return null;
			}
			if (!GetClientRect(hwnd, out var rc))
			{
				return null;
			}
			int w = rc.Right - rc.Left;
			int h = rc.Bottom - rc.Top;
			if (w < 200 || h < 200)
			{
				return null;
			}
			POINT pOINT = default(POINT);
			pOINT.X = 0;
			pOINT.Y = 0;
			POINT origin = pOINT;
			if (!ClientToScreen(hwnd, ref origin))
			{
				return null;
			}
			return new Rectangle(origin.X, origin.Y, w, h);
		}
	}
}
