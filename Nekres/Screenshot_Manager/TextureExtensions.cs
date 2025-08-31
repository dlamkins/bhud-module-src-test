using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Screenshot_Manager
{
	public static class TextureExtensions
	{
		public unsafe static Bitmap ToBitmap(this Texture2D texture)
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			if (texture == null)
			{
				throw new ArgumentNullException("texture");
			}
			int width = texture.get_Width();
			int height = texture.get_Height();
			Color[] pixelData = (Color[])(object)new Color[width * height];
			texture.GetData<Color>(pixelData);
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			byte* dstPtr = (byte*)(void*)bmpData.Scan0;
			for (int y = 0; y < height; y++)
			{
				byte* row = dstPtr + y * bmpData.Stride;
				for (int x = 0; x < width; x++)
				{
					Color color = pixelData[y * width + x];
					row[x * 4] = ((Color)(ref color)).get_B();
					row[x * 4 + 1] = ((Color)(ref color)).get_G();
					row[x * 4 + 2] = ((Color)(ref color)).get_R();
					row[x * 4 + 3] = ((Color)(ref color)).get_A();
				}
			}
			bitmap.UnlockBits(bmpData);
			return bitmap;
		}
	}
}
