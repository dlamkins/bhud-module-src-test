using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Blish_HUD;

namespace WvWPipTally.Services
{
	public static class GameWindowCapture
	{
		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		public static Bitmap CaptureGw2Client()
		{
			IntPtr hwnd = GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle();
			if (hwnd == IntPtr.Zero)
			{
				throw new InvalidOperationException("Guild Wars 2 window not available from Blish HUD. Is the game running?");
			}
			if (!GetClientRect(hwnd, out var client))
			{
				throw new InvalidOperationException("Could not read GW2 client size.");
			}
			int num = client.Right - client.Left;
			int height = client.Bottom - client.Top;
			if (num <= 0 || height <= 0)
			{
				throw new InvalidOperationException("GW2 window has invalid size.");
			}
			Bitmap full = new Bitmap(num, height, PixelFormat.Format32bppArgb);
			using Graphics g = Graphics.FromImage(full);
			IntPtr hdc = g.GetHdc();
			try
			{
				if (!PrintWindow(hwnd, hdc, 2u))
				{
					full.Dispose();
					throw new InvalidOperationException("PrintWindow failed. Try windowed or borderless windowed mode.");
				}
				return full;
			}
			finally
			{
				g.ReleaseHdc(hdc);
			}
		}

		[DllImport("user32.dll")]
		private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
	}
}
