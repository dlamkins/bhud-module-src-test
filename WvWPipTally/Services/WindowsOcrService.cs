using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace WvWPipTally.Services
{
	public static class WindowsOcrService
	{
		public static async Task<string> RecognizeAsync(Bitmap bitmap)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			OcrEngine engine = OcrEngine.TryCreateFromUserProfileLanguages() ?? OcrEngine.TryCreateFromLanguage(new Language("en"));
			if (engine == null)
			{
				throw new InvalidOperationException("Windows OCR is unavailable. Install an English language pack (Settings → Time & Language → Language).");
			}
			float scale = ((bitmap.Width >= 2560) ? 1.25f : ((bitmap.Width >= 1600) ? 1.5f : 2f));
			using Bitmap prepared = Upscale(bitmap, scale);
			using InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
			await SaveBitmapAsPngAsync(prepared, stream).ConfigureAwait(continueOnCapturedContext: false);
			stream.Seek(0uL);
			TaskAwaiter<BitmapDecoder> taskAwaiter = WindowsRuntimeSystemExtensions.GetAwaiter<BitmapDecoder>(BitmapDecoder.CreateAsync(stream));
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<BitmapDecoder> taskAwaiter2 = default(TaskAwaiter<BitmapDecoder>);
				taskAwaiter = taskAwaiter2;
			}
			TaskAwaiter<SoftwareBitmap> taskAwaiter3 = WindowsRuntimeSystemExtensions.GetAwaiter<SoftwareBitmap>(taskAwaiter.GetResult().GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied));
			if (!taskAwaiter3.IsCompleted)
			{
				await taskAwaiter3;
				TaskAwaiter<SoftwareBitmap> taskAwaiter4 = default(TaskAwaiter<SoftwareBitmap>);
				taskAwaiter3 = taskAwaiter4;
			}
			SoftwareBitmap softwareBitmap = taskAwaiter3.GetResult();
			TaskAwaiter<OcrResult> taskAwaiter5 = WindowsRuntimeSystemExtensions.GetAwaiter<OcrResult>(engine.RecognizeAsync(softwareBitmap));
			if (!taskAwaiter5.IsCompleted)
			{
				await taskAwaiter5;
				TaskAwaiter<OcrResult> taskAwaiter6 = default(TaskAwaiter<OcrResult>);
				taskAwaiter5 = taskAwaiter6;
			}
			return taskAwaiter5.GetResult()?.Text ?? string.Empty;
		}

		private static Bitmap Upscale(Bitmap source, float scale)
		{
			if (Math.Abs(scale - 1f) < 0.01f)
			{
				return (Bitmap)source.Clone();
			}
			int w = Math.Max(1, (int)((float)source.Width * scale));
			int h = Math.Max(1, (int)((float)source.Height * scale));
			Bitmap scaled = new Bitmap(w, h, PixelFormat.Format32bppArgb);
			using Graphics g = Graphics.FromImage(scaled);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.DrawImage(source, 0, 0, w, h);
			return scaled;
		}

		private static async Task SaveBitmapAsPngAsync(Bitmap bitmap, IRandomAccessStream stream)
		{
			using MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Png);
			ms.Position = 0L;
			byte[] bytes = ms.ToArray();
			TaskAwaiter<uint> taskAwaiter = WindowsRuntimeSystemExtensions.GetAwaiter<uint, uint>(stream.WriteAsync(WindowsRuntimeBufferExtensions.AsBuffer(bytes)));
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<uint> taskAwaiter2 = default(TaskAwaiter<uint>);
				taskAwaiter = taskAwaiter2;
			}
			taskAwaiter.GetResult();
		}
	}
}
