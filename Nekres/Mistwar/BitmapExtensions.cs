using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Nekres.Mistwar
{
	internal static class BitmapExtensions
	{
		public static void MakeGrayscale(this Bitmap original)
		{
			Graphics g = Graphics.FromImage(original);
			g.CompositingMode = CompositingMode.SourceCopy;
			ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
			{
				new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
				new float[5] { 0.59f, 0.59f, 0.59f, 0f, 0f },
				new float[5] { 0.11f, 0.11f, 0.11f, 0f, 0f },
				new float[5] { 0f, 0f, 0f, 1f, 0f },
				new float[5] { 0f, 0f, 0f, 0f, 1f }
			});
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(colorMatrix);
			g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
			g.Dispose();
		}
	}
}
