using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Nekres.Stream_Out
{
	internal static class BitmapExtensions
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

		public static Bitmap FitTo(this Bitmap source, Bitmap other)
		{
			if (other == null)
			{
				throw new ArgumentNullException();
			}
			if (source.Size.Equals(other.Size))
			{
				return source;
			}
			float scale = Math.Min(other.Width / source.Width, other.Height / source.Height);
			int newHeight = Convert.ToInt32((float)source.Width * scale);
			int newWidth = Convert.ToInt32((float)source.Height * scale);
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

		public static void Colorize(this Bitmap source, Color replacement)
		{
			for (int x = 0; x < source.Width; x++)
			{
				for (int y = 0; y < source.Height; y++)
				{
					source.SetPixel(x, y, Color.FromArgb(source.GetPixel(x, y).A, replacement.R, replacement.G, replacement.B));
				}
			}
		}

		public static Bitmap Merge(this Bitmap bmp1, Bitmap bmp2)
		{
			Bitmap result = new Bitmap(Math.Max(bmp1.Width, bmp2.Width), Math.Max(bmp1.Height, bmp2.Height));
			using (Graphics g = Graphics.FromImage(result))
			{
				g.DrawImage(bmp1, Point.Empty);
				g.DrawImage(bmp2, Point.Empty);
			}
			return result;
		}

		public static void Flip(this Bitmap source, bool xFlip, bool yFlip)
		{
			if (xFlip)
			{
				if (yFlip)
				{
					source.RotateFlip(RotateFlipType.Rotate180FlipNone);
					return;
				}
				source.RotateFlip(RotateFlipType.RotateNoneFlipX);
			}
			if (yFlip)
			{
				source.RotateFlip(RotateFlipType.Rotate180FlipX);
			}
		}
	}
}
