using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public static class BitmapExtensions
	{
		public static void Copy(this Bitmap source, Texture2D destination)
		{
			BitmapData bitmapData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
			int dataSize = bitmapData.Stride * bitmapData.Height;
			byte[] data = new byte[dataSize];
			Marshal.Copy(bitmapData.Scan0, data, 0, dataSize);
			source.UnlockBits(bitmapData);
			destination.SetData<byte>(data);
		}
	}
}
