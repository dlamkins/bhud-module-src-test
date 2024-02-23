using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Ideka.CustomCombatText
{
	public class DirectBitmap : IDisposable
	{
		public Bitmap Bitmap { get; private set; }

		public int[] Bits { get; private set; }

		public bool Disposed { get; private set; }

		public int Height { get; private set; }

		public int Width { get; private set; }

		protected GCHandle BitsHandle { get; private set; }

		public DirectBitmap(int width, int height)
		{
			Width = width;
			Height = height;
			Bits = new int[width * height];
			BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
			Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
		}

		public DirectBitmap(Bitmap bitmap)
			: this(bitmap.Width, bitmap.Height)
		{
			using Graphics g = Graphics.FromImage(Bitmap);
			g.DrawImage(bitmap, 0, 0);
		}

		public void SetPixel(int x, int y, Color colour)
		{
			int index = x + y * Width;
			int col = colour.ToArgb();
			Bits[index] = col;
		}

		public Color GetPixel(int x, int y)
		{
			int index = x + y * Width;
			return Color.FromArgb(Bits[index]);
		}

		public void Dispose()
		{
			if (!Disposed)
			{
				Disposed = true;
				Bitmap.Dispose();
				BitsHandle.Free();
			}
		}
	}
}
