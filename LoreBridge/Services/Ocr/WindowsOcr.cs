using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace LoreBridge.Services.Ocr
{
	public class WindowsOcr
	{
		private readonly OcrEngine? _engine = OcrEngine.TryCreateFromLanguage(new Language("en-US"));

		public string[] GetTextLines(Bitmap bitmap)
		{
			if (_engine == null)
			{
				throw new Exception("The English (USA) Language Pack must be installed for Windows OCR to work properly.");
			}
			SoftwareBitmap softwareBitmap;
			using (InMemoryRandomAccessStream randStream = new InMemoryRandomAccessStream())
			{
				bitmap.Save(WindowsRuntimeStreamExtensions.AsStream((IRandomAccessStream)randStream), ImageFormat.Tiff);
				softwareBitmap = WindowsRuntimeSystemExtensions.AsTask<SoftwareBitmap>(WindowsRuntimeSystemExtensions.AsTask<BitmapDecoder>(BitmapDecoder.CreateAsync(randStream)).Result.GetSoftwareBitmapAsync()).Result;
			}
			return WindowsRuntimeSystemExtensions.AsTask<OcrResult>(_engine!.RecognizeAsync(softwareBitmap)).Result.Lines.Select((OcrLine line) => line.Text).ToArray();
		}
	}
}
