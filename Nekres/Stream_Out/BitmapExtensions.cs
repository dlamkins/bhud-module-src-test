using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Nekres.Stream_Out
{
	public static class BitmapExtensions
	{
		public static Bitmap FitToHeight(this Bitmap source, int destHeight)
		{
			if (destHeight < 1)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (destHeight == source.Height)
			{
				return source;
			}
			double ratio = (double)destHeight / (double)source.Height;
			int newHeight = Convert.ToInt32((double)source.Width * ratio);
			int newWidth = Convert.ToInt32((double)source.Height * ratio);
			Bitmap newBitmap = new Bitmap(newWidth, newHeight);
			using (Graphics gfx = Graphics.FromImage(newBitmap))
			{
				gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gfx.SmoothingMode = SmoothingMode.HighQuality;
				gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
				gfx.CompositingQuality = CompositingQuality.HighQuality;
				gfx.Clear(Color.Transparent);
				gfx.DrawImage(source, 0, 0, newWidth, newHeight);
				gfx.Flush();
				gfx.Save();
			}
			source.Dispose();
			return newBitmap;
		}
	}
}
