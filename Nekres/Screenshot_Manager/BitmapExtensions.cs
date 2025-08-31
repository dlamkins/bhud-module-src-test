using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nekres.Screenshot_Manager
{
	internal static class BitmapExtensions
	{
		public static Bitmap Fit(this Bitmap source, Size size)
		{
			if (source.Size.Equals(size))
			{
				return source;
			}
			float scale = Math.Min(size.Width / source.Width, size.Height / source.Height);
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

		public static async Task SaveOnNetworkShare(this Image image, string fileName, ImageFormat imageFormat)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			try
			{
				using MemoryStream lMemoryStream = new MemoryStream();
				image.Save(lMemoryStream, imageFormat);
				using FileStream lFileStream = new FileStream(fileName, FileMode.Create);
				lMemoryStream.Position = 0L;
				await lMemoryStream.CopyToAsync(lFileStream);
			}
			catch (Exception ex)
			{
				ScreenshotManagerModule.Logger.Warn(ex, ex.Message);
			}
		}

		public static void SaveToClipboard(this Image image, ImageFormat imageFormat)
		{
			try
			{
				DataObject dataObject = new DataObject();
				dataObject.SetData(DataFormats.Bitmap, autoConvert: true, image);
				using (MemoryStream stream = new MemoryStream())
				{
					image.Save(stream, imageFormat);
					stream.Position = 0L;
					dataObject.SetData(imageFormat.ToString(), autoConvert: false, stream);
				}
				Clipboard.SetDataObject(dataObject, copy: true);
			}
			catch (Exception ex)
			{
				ScreenshotManagerModule.Logger.Warn(ex, ex.Message);
			}
		}

		public static Bitmap CompressToTargetSize(this Bitmap bitmap, long maxBytes)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			if (maxBytes <= 0)
			{
				throw new ArgumentOutOfRangeException("maxBytes");
			}
			ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageEncoders().First((ImageCodecInfo e) => e.FormatID == ImageFormat.Jpeg.Guid);
			int minQ = 1;
			int maxQ = 100;
			byte[] bestData = null;
			while (minQ <= maxQ)
			{
				int q = (minQ + maxQ) / 2;
				using MemoryStream ms = new MemoryStream();
				EncoderParameters encoderParams = new EncoderParameters(1);
				encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, q);
				bitmap.Save(ms, jpegEncoder, encoderParams);
				if (ms.Length > maxBytes)
				{
					maxQ = q - 1;
					continue;
				}
				bestData = ms.ToArray();
				minQ = q + 1;
			}
			if (bestData == null)
			{
				throw new Exception("Cannot compress bitmap below target size");
			}
			using MemoryStream resultStream = new MemoryStream(bestData);
			return new Bitmap(resultStream);
		}
	}
}
