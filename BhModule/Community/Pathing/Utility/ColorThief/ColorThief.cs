using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Utility.ColorThief
{
	public static class ColorThief
	{
		private const int DEFAULT_COLOR_COUNT = 5;

		private const int DEFAULT_QUALITY = 10;

		private const bool DEFAULT_IGNORE_WHITE = true;

		public static List<QuantizedColor> GetPalette(Texture2D sourceImage, int colorCount = 5, int quality = 10, bool ignoreWhite = true)
		{
			CMap cmap = GetColorMap(GetPixelsFast(sourceImage, quality, ignoreWhite), colorCount);
			if (cmap != null)
			{
				return cmap.GeneratePalette();
			}
			return new List<QuantizedColor>();
		}

		private static byte[][] GetPixelsFast(Texture2D sourceImage, int quality, bool ignoreWhite)
		{
			if (quality < 1)
			{
				quality = 10;
			}
			Color[] intFromPixel = GetIntFromPixel(sourceImage);
			int pixelCount = sourceImage.get_Width() * sourceImage.get_Height();
			return ConvertPixels(intFromPixel, pixelCount, quality, ignoreWhite);
		}

		private static Color[] GetIntFromPixel(Texture2D texture)
		{
			Color[] colors1D = (Color[])(object)new Color[texture.get_Width() * texture.get_Height()];
			texture.GetData<Color>(colors1D);
			return colors1D;
		}

		private static CMap GetColorMap(byte[][] pixelArray, int colorCount)
		{
			if (colorCount > 0)
			{
				colorCount--;
			}
			return Mmcq.Quantize(pixelArray, colorCount);
		}

		private static byte[][] ConvertPixels(Color[] pixels, int pixelCount, int quality, bool ignoreWhite)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if (pixelCount != pixels.Length)
			{
				throw new ArgumentException($"(expectedDataLength = {pixelCount}) != (pixels.length = {pixels.Length})");
			}
			int num = (pixelCount + quality - 1) / quality;
			int numUsedPixels = 0;
			byte[][] pixelArray = new byte[num][];
			for (int i = 0; i < pixelCount; i += quality)
			{
				Color color = pixels[i];
				byte b = ((Color)(ref color)).get_B();
				byte g = ((Color)(ref color)).get_G();
				byte r = ((Color)(ref color)).get_R();
				if (((Color)(ref color)).get_A() >= 125 && (!ignoreWhite || r <= 250 || g <= 250 || b <= 250))
				{
					pixelArray[numUsedPixels] = new byte[3] { r, g, b };
					numUsedPixels++;
				}
			}
			byte[][] copy = new byte[numUsedPixels][];
			Array.Copy(pixelArray, copy, numUsedPixels);
			return copy;
		}
	}
}
