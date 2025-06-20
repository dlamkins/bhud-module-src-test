using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class ScreenCaptureService
	{
		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		private static Rectangle GetGw2Bounds()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			IntPtr hWnd = GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().MainWindowHandle;
			if (hWnd != IntPtr.Zero && GetWindowRect(hWnd, out var rect))
			{
				int width = rect.Right - rect.Left;
				int height = rect.Bottom - rect.Top;
				return new Rectangle(rect.Left, rect.Top, width, height);
			}
			return new Rectangle(0, 0, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y);
		}

		public static Bitmap CaptureBitmap(int resizeWidth = 1000)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Rectangle screenBounds = GetGw2Bounds();
			Bitmap _screenCaptureImage = new Bitmap(screenBounds.Width, screenBounds.Height);
			CaptureRegionOnScreen(screenBounds, _screenCaptureImage);
			return ResizeImage(_screenCaptureImage, resizeWidth);
		}

		private static Bitmap ResizeImage(Bitmap originalImage, int maxWidth)
		{
			int newHeight = (int)((double)originalImage.Height * ((double)maxWidth / (double)originalImage.Width));
			Bitmap resizedImage = new Bitmap(maxWidth, newHeight);
			using Graphics graphics = Graphics.FromImage(resizedImage);
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.DrawImage(originalImage, 0, 0, maxWidth, newHeight);
			return resizedImage;
		}

		private static void CaptureRegionOnScreen(Rectangle region, Bitmap dest)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using Graphics g = Graphics.FromImage(dest);
				g.CopyFromScreen(new Point(region.X, region.Y), Point.Empty, new Size(region.Width, region.Height));
			}
			catch
			{
			}
		}
	}
}
