using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
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
			SoftwareBitmap result = taskAwaiter3.GetResult();
			using SoftwareBitmap sb = result;
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
			return AssembleWithParagraphs(taskAwaiter5.GetResult());
		}

		public static async Task<string> RecognizeLineAsync(Bitmap source, string languageTag, bool invert = false)
		{
			return Regex.Replace(((await RecognizeAsync(source, languageTag, invert).ConfigureAwait(continueOnCapturedContext: false)) ?? "").Replace("\r", " ").Replace("\n", " "), "\\s+", " ").Trim();
		}

		private static string AssembleWithParagraphs(OcrResult result)
		{
			IReadOnlyList<OcrLine> lines = result?.Lines;
			if (lines == null || lines.Count == 0)
			{
				return "";
			}
			int m = lines.Count;
			List<double> tops = new List<double>(m);
			List<double> widths = new List<double>(m);
			foreach (OcrLine item in lines)
			{
				double top = double.MaxValue;
				double l = double.MaxValue;
				double r = 0.0;
				foreach (OcrWord word in item.Words)
				{
					Windows.Foundation.Rect b = word.BoundingRect;
					if (b.get_Y() < top)
					{
						top = b.get_Y();
					}
					if (b.get_X() < l)
					{
						l = b.get_X();
					}
					if (b.get_X() + b.get_Width() > r)
					{
						r = b.get_X() + b.get_Width();
					}
				}
				tops.Add((top == double.MaxValue) ? 0.0 : top);
				widths.Add((r > l) ? (r - l) : 0.0);
			}
			List<double> deltas = new List<double>(Math.Max(0, m - 1));
			for (int k = 1; k < m; k++)
			{
				deltas.Add(tops[k] - tops[k - 1]);
			}
			double pitch = Median(deltas);
			double medWidth = Median(widths);
			StringBuilder sb = new StringBuilder();
			for (int j = 0; j < m; j++)
			{
				if (j > 0)
				{
					double delta = tops[j] - tops[j - 1];
					bool brk = (pitch > 0.0 && delta > pitch * 1.5) || IsHeading(j - 1) || IsHeading(j);
					sb.Append(brk ? "\n\n" : "\n");
				}
				sb.Append(lines[j].Text);
			}
			return sb.ToString();
			bool IsHeading(int i)
			{
				if (pitch > 0.0 && medWidth > 0.0 && m >= 3 && widths[i] < medWidth * 0.62 && i + 1 < m)
				{
					return tops[i + 1] - tops[i] <= pitch * 1.5;
				}
				return false;
			}
		}

		private static double Median(List<double> xs)
		{
			if (xs == null || xs.Count == 0)
			{
				return 0.0;
			}
			List<double> s = xs.OrderBy((double x) => x).ToList();
			int i = s.Count;
			if (i % 2 != 1)
			{
				return (s[i / 2 - 1] + s[i / 2]) / 2.0;
			}
			return s[i / 2];
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
