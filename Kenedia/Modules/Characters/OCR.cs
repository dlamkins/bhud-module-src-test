using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration.GfxSettings;
using Characters.Views;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Patagames.Ocr;

namespace Kenedia.Modules.Characters
{
	public class OCR : IDisposable
	{
		private readonly OCRView _view;

		private readonly OcrApi _ocrApi;

		private readonly ClientWindowService _clientWindowService;

		private readonly SharedSettings _sharedSettings;

		private readonly SettingsModel _settings;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private readonly Color _spacingColor = Color.FromArgb(255, 200, 200, 200);

		private readonly Color _ignoredColor = Color.FromArgb(255, 100, 100, 100);

		private bool _disposed;

		public Texture2D SourceTexture { get; private set; }

		public Texture2D CleanedTexture { get; private set; }

		public string ReadResult { get; private set; }

		public string BestMatchResult { get; private set; }

		private int CustomThreshold
		{
			get
			{
				return _settings.OCRNoPixelColumns.get_Value();
			}
			set
			{
				_settings.OCRNoPixelColumns.set_Value(value);
			}
		}

		public OCR(ClientWindowService clientWindowService, SharedSettings sharedSettings, SettingsModel settings, string basePath, ObservableCollection<Character_Model> characterModels)
		{
			_clientWindowService = clientWindowService;
			_sharedSettings = sharedSettings;
			_settings = settings;
			_characterModels = characterModels;
			OcrApi.PathToEngine = basePath + "\\tesseract.dll";
			_ocrApi = OcrApi.Create();
			_ocrApi.Init(basePath + "\\", "gw2");
			OCRView oCRView = new OCRView(_settings, this);
			((Control)oCRView).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)oCRView).set_ZIndex(2147483646);
			((Control)oCRView).set_Visible(false);
			_view = oCRView;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				OCRView view = _view;
				if (view != null)
				{
					((Control)view).Dispose();
				}
				Texture2D cleanedTexture = CleanedTexture;
				if (cleanedTexture != null)
				{
					((GraphicsResource)cleanedTexture).Dispose();
				}
				Texture2D sourceTexture = SourceTexture;
				if (sourceTexture != null)
				{
					((GraphicsResource)sourceTexture).Dispose();
				}
			}
		}

		public async Task<string?> Read(bool show = false)
		{
			string finalText = null;
			if (!show)
			{
				_view.EnableMaskedRegion();
			}
			await Task.Delay(50);
			Texture2D cleanedTexture = CleanedTexture;
			if (cleanedTexture != null)
			{
				((GraphicsResource)cleanedTexture).Dispose();
			}
			Texture2D sourceTexture = SourceTexture;
			if (sourceTexture != null)
			{
				((GraphicsResource)sourceTexture).Dispose();
			}
			User32Dll.RECT wndBounds = _clientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
			double factor = GameService.Graphics.get_UIScaleMultiplier();
			Point size = default(Point);
			((Point)(ref size))._002Ector((int)((double)_settings.ActiveOCRRegion.Width * factor), (int)((double)_settings.ActiveOCRRegion.Height * factor));
			using (Bitmap bitmap = new Bitmap(size.X, size.Y))
			{
				Bitmap spacingVisibleBitmap = new Bitmap(size.X, size.Y);
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					int left = wndBounds.Left + p.X;
					int top = wndBounds.Top + p.Y;
					Rectangle activeOCRRegion = _settings.ActiveOCRRegion;
					int x = (int)Math.Ceiling((double)((Rectangle)(ref activeOCRRegion)).get_Left() * factor);
					activeOCRRegion = _settings.ActiveOCRRegion;
					int y = (int)Math.Ceiling((double)((Rectangle)(ref activeOCRRegion)).get_Top() * factor);
					g.CopyFromScreen(new Point(left + x, top + y), Point.Empty, new Size(size.X, size.Y));
					if (show)
					{
						using MemoryStream memoryStream = new MemoryStream();
						bitmap.Save(memoryStream, ImageFormat.Bmp);
						SourceTexture = memoryStream.CreateTexture2D();
					}
					int emptyPixelRow = 0;
					bool stringStarted = false;
					for (int i = 0; i < bitmap.Width; i++)
					{
						bool containsPixel = false;
						for (int k = 0; k < bitmap.Height; k++)
						{
							Color oc = bitmap.GetPixel(i, k);
							int threshold = _settings.OCR_ColorThreshold.get_Value();
							if (oc.R >= threshold && oc.G >= threshold && oc.B >= threshold && emptyPixelRow < CustomThreshold)
							{
								bitmap.SetPixel(i, k, Color.Black);
								if (show)
								{
									spacingVisibleBitmap.SetPixel(i, k, Color.Black);
								}
								containsPixel = true;
								stringStarted = true;
							}
							else if (emptyPixelRow >= CustomThreshold)
							{
								if (show)
								{
									spacingVisibleBitmap.SetPixel(i, k, _ignoredColor);
								}
								bitmap.SetPixel(i, k, Color.White);
							}
							else
							{
								if (show)
								{
									spacingVisibleBitmap.SetPixel(i, k, Color.White);
								}
								bitmap.SetPixel(i, k, Color.White);
							}
						}
						if (emptyPixelRow >= CustomThreshold)
						{
							continue;
						}
						if (!containsPixel)
						{
							if (show)
							{
								for (int j = 0; j < bitmap.Height; j++)
								{
									spacingVisibleBitmap.SetPixel(i, j, _spacingColor);
								}
							}
							if (stringStarted)
							{
								emptyPixelRow++;
							}
						}
						else
						{
							emptyPixelRow = 0;
						}
					}
					using MemoryStream s = new MemoryStream();
					spacingVisibleBitmap.Save(s, ImageFormat.Bmp);
					if (show)
					{
						CleanedTexture = s.CreateTexture2D();
					}
				}
				string[] array = _ocrApi.GetTextFromImage(bitmap).Split(' ');
				for (int l = 0; l < array.Length; l++)
				{
					string wordText = array[l].Trim();
					if (wordText.StartsWith("l"))
					{
						wordText = "I" + wordText.Remove(0, 1);
					}
					finalText = ((finalText == null) ? wordText : (finalText + " " + wordText));
				}
				finalText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(finalText?.ToLower());
				BestMatchResult = GetBestMatch(finalText).Item1;
				ReadResult = finalText;
			}
			if (!show)
			{
				_view.DisableMaskedRegion();
			}
			return finalText;
		}

		private (string, int, int, int, bool) GetBestMatch(string name)
		{
			List<(string, int, int, int, bool)> distances = new List<(string, int, int, int, bool)>();
			foreach (Character_Model c in _characterModels)
			{
				int distance = name.LevenshteinDistance(c.Name);
				distances.Add((c.Name, distance, 0, 0, true));
			}
			distances.Sort(((string, int, int, int, bool) a, (string, int, int, int, bool) b) => a.Item2.CompareTo(b.Item2));
			return (distances?.FirstOrDefault() ?? new(string, int, int, int, bool)?((string.Empty, 0, 0, 0, false))).Value;
		}

		public void ToggleContainer()
		{
			_view?.ToggleContainer();
		}
	}
}
