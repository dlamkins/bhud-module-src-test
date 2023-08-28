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
		public static AsyncTexture2D CaptureRegion(User32Dll.RECT wndBounds, Point p, Rectangle bounds, double factor, Point size)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			using Bitmap bitmap = new Bitmap((int)((double)bounds.Width * factor), (int)((double)bounds.Height * factor));
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			int x = (int)((double)bounds.X * factor);
			int y = (int)((double)bounds.Y * factor);
			g.CopyFromScreen(new Point(wndBounds.Left + p.X + x, wndBounds.Top + p.Y + y), Point.Empty, new Size(size.X, size.Y));
			bitmap.Save(s, ImageFormat.Bmp);
			return AsyncTexture2D.op_Implicit(s.CreateTexture2D());
		}

		public static AsyncTexture2D CaptureRegion(Rectangle window, Rectangle region)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			using Bitmap bitmap = new Bitmap(region.Width, region.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(((Rectangle)(ref window)).get_Left() + ((Rectangle)(ref region)).get_Left(), ((Rectangle)(ref window)).get_Top() + ((Rectangle)(ref region)).get_Top()), Point.Empty, new Size(region.Width, region.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			return AsyncTexture2D.op_Implicit(s.CreateTexture2D());
		}

		public static AsyncTexture2D CaptureRegion(User32Dll.RECT window, Rectangle region)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			using Bitmap bitmap = new Bitmap(region.Width, region.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(window.Left + ((Rectangle)(ref region)).get_Left(), window.Top + ((Rectangle)(ref region)).get_Top()), Point.Empty, new Size(region.Width, region.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			return AsyncTexture2D.op_Implicit(s.CreateTexture2D());
		}
	}
}
