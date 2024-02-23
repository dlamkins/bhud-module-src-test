using System;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	internal static class BitmapUtils
	{
		public static Bitmap FromTexture(Texture2D texture)
		{
			using MemoryStream stream = new MemoryStream();
			texture.SaveAsPng((Stream)stream, texture.get_Width(), texture.get_Height());
			stream.Seek(0L, SeekOrigin.Begin);
			return (Bitmap)Image.FromStream(stream);
		}

		public static double ColorGrayscale(Color color)
		{
			return 0.11 * (double)(int)color.B + 0.59 * (double)(int)color.G + 0.3 * (double)(int)color.R;
		}

		public static Rectangle GetBoundingBox(DirectBitmap bitmap, Color bg, double tolerance)
		{
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			int w = bitmap.Width;
			int h = bitmap.Height;
			int xMin = w - 1;
			int yMin = h - 1;
			int xMax = 0;
			int yMax = 0;
			double bgGrayscale = ColorGrayscale(bg);
			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					Color color = bitmap.GetPixel(x, y);
					if (!((double)(int)color.A / 255.0 <= tolerance) && !(Math.Abs(ColorGrayscale(color) - bgGrayscale) / 255.0 <= tolerance))
					{
						if (x < xMin)
						{
							xMin = x;
						}
						if (y < yMin)
						{
							yMin = y;
						}
						if (x > xMax)
						{
							xMax = x;
						}
						if (y > yMax)
						{
							yMax = y;
						}
					}
				}
			}
			return new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
		}
	}
}
