using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Extensions
{
	public static class Texture2DExtension
	{
		public static Texture2D CreateTexture2D(this MemoryStream s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0L)
			{
				throw new ArgumentException("The image stream is empty.", "s");
			}
			s.Position = 0L;
			using GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext();
			return Texture2D.FromStream(device.GraphicsDevice, s);
		}

		public static Texture2D CreateTexture2D(this Bitmap bitmap)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			using MemoryStream stream = new MemoryStream();
			bitmap.Save(stream, ImageFormat.Png);
			return stream.CreateTexture2D();
		}

		public static Texture2D ToGrayScaledPalettable(this Texture2D original)
		{
			if (original == null || original.IsDisposed)
			{
				return null;
			}
			Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[original.Width * original.Height];
			original.GetData(colors);
			Microsoft.Xna.Framework.Color[] destColors = new Microsoft.Xna.Framework.Color[original.Width * original.Height];
			Texture2D newTexture;
			using (GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext())
			{
				newTexture = new Texture2D(device.GraphicsDevice, original.Width, original.Height);
			}
			for (int i = 0; i < original.Width; i++)
			{
				for (int j = 0; j < original.Height; j++)
				{
					int index = i + j * original.Width;
					Microsoft.Xna.Framework.Color originalColor = colors[index];
					float maxval = 1.79f;
					float grayScale = (float)(int)originalColor.R / 255f * 0.3f + (float)(int)originalColor.G / 255f * 0.59f + (float)(int)originalColor.B / 255f * 0.11f + (float)(int)originalColor.A / 255f * 0.79f;
					grayScale /= maxval;
					destColors[index] = new Microsoft.Xna.Framework.Color(grayScale, grayScale, grayScale, (int)originalColor.A);
				}
			}
			newTexture.SetData(destColors);
			return newTexture;
		}
	}
}
