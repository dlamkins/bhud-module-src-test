using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD.Content;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Utility
{
	public static class ScreenCapture
	{
		public static AsyncTexture2D CaptureRegion(User32Dll.RECT wndBounds, Microsoft.Xna.Framework.Point p, Microsoft.Xna.Framework.Rectangle bounds, double factor, Microsoft.Xna.Framework.Point size)
		{
			using Bitmap bitmap = new Bitmap((int)((double)bounds.Width * factor), (int)((double)bounds.Height * factor));
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			int x = (int)((double)bounds.X * factor);
			int y = (int)((double)bounds.Y * factor);
			g.CopyFromScreen(new System.Drawing.Point(wndBounds.Left + p.X + x, wndBounds.Top + p.Y + y), System.Drawing.Point.Empty, new Size(size.X, size.Y));
			bitmap.Save(s, ImageFormat.Bmp);
			return (AsyncTexture2D)s.CreateTexture2D();
		}

		public static AsyncTexture2D CaptureRegion(Microsoft.Xna.Framework.Rectangle window, Microsoft.Xna.Framework.Rectangle region)
		{
			using Bitmap bitmap = new Bitmap(region.Width, region.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new System.Drawing.Point(window.Left + region.Left, window.Top + region.Top), System.Drawing.Point.Empty, new Size(region.Width, region.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			return (AsyncTexture2D)s.CreateTexture2D();
		}

		public static AsyncTexture2D CaptureRegion(User32Dll.RECT window, Microsoft.Xna.Framework.Rectangle region)
		{
			using Bitmap bitmap = new Bitmap(region.Width, region.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new System.Drawing.Point(window.Left + region.Left, window.Top + region.Top), System.Drawing.Point.Empty, new Size(region.Width, region.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			return (AsyncTexture2D)s.CreateTexture2D();
		}
	}
}
