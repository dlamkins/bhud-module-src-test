using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

namespace Frtal.LorebookReader
{
	public static class OcrService
	{
		public static async Task<string> RecognizeAsync(Bitmap source, string languageTag, bool invert = false)
		{
			using Bitmap prepared = Preprocess(source, invert);
			using MemoryStream ms = new MemoryStream();
			prepared.Save(ms, ImageFormat.Bmp);
			ms.Position = 0L;
			TaskAwaiter<BitmapDecoder> taskAwaiter = WindowsRuntimeSystemExtensions.GetAwaiter<BitmapDecoder>(BitmapDecoder.CreateAsync(WindowsRuntimeStreamExtensions.AsRandomAccessStream((Stream)ms)));
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
			SoftwareBitmap result2 = taskAwaiter3.GetResult();
			using SoftwareBitmap sb = result2;
			OcrEngine obj = OcrEngine.TryCreateFromLanguage(new Language(languageTag)) ?? OcrEngine.TryCreateFromUserProfileLanguages();
			if (obj == null)
			{
				throw new InvalidOperationException("Windows OCR language pack for '" + languageTag + "' is not installed.");
			}
			TaskAwaiter<OcrResult> taskAwaiter5 = WindowsRuntimeSystemExtensions.GetAwaiter<OcrResult>(obj.RecognizeAsync(sb));
			if (!taskAwaiter5.IsCompleted)
			{
				await taskAwaiter5;
				TaskAwaiter<OcrResult> taskAwaiter6 = default(TaskAwaiter<OcrResult>);
				taskAwaiter5 = taskAwaiter6;
			}
			OcrResult result = taskAwaiter5.GetResult();
			return string.Join("\n", result.Lines.Select((OcrLine l) => l.Text));
		}

		public static async Task<string> RecognizeLineAsync(Bitmap source, string languageTag, bool invert = false)
		{
			return Regex.Replace(((await RecognizeAsync(source, languageTag, invert).ConfigureAwait(continueOnCapturedContext: false)) ?? "").Replace("\r", " ").Replace("\n", " "), "\\s+", " ").Trim();
		}

		private static Bitmap Preprocess(Bitmap src, bool invert = false)
		{
			Bitmap dst = new Bitmap(src.Width * 2, src.Height * 2, PixelFormat.Format24bppRgb);
			using Graphics g = Graphics.FromImage(dst);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			ColorMatrix matrix = ((!invert) ? new ColorMatrix(new float[5][]
			{
				new float[5] { 0.299f, 0.299f, 0.299f, 0f, 0f },
				new float[5] { 0.587f, 0.587f, 0.587f, 0f, 0f },
				new float[5] { 0.114f, 0.114f, 0.114f, 0f, 0f },
				new float[5] { 0f, 0f, 0f, 1f, 0f },
				new float[5] { 0f, 0f, 0f, 0f, 1f }
			}) : new ColorMatrix(new float[5][]
			{
				new float[5] { -0.299f, -0.299f, -0.299f, 0f, 0f },
				new float[5] { -0.587f, -0.587f, -0.587f, 0f, 0f },
				new float[5] { -0.114f, -0.114f, -0.114f, 0f, 0f },
				new float[5] { 0f, 0f, 0f, 1f, 0f },
				new float[5] { 1f, 1f, 1f, 0f, 1f }
			}));
			using ImageAttributes attrs = new ImageAttributes();
			attrs.SetColorMatrix(matrix);
			g.DrawImage(src, new Rectangle(0, 0, dst.Width, dst.Height), 0, 0, src.Width, src.Height, GraphicsUnit.Pixel, attrs);
			return dst;
		}
	}
}
