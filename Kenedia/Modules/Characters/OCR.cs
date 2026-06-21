using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.GameIntegration.GfxSettings;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Characters.Views;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Patagames.Ocr;
using SemVer;

namespace Kenedia.Modules.Characters
{
	public class OCR : IDisposable
	{
		private readonly OcrApi _ocrApi;

		private readonly ClientWindowService _clientWindowService;

		private readonly SharedSettings _sharedSettings;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private readonly Color _spacingColor = Color.FromArgb(255, 200, 200, 200);

		private readonly Color _ignoredColor = Color.FromArgb(255, 100, 100, 100);

		private bool _isDisposed;

		public OCRView OcrView { get; set; }

		public MainWindow MainWindow
		{
			[CompilerGenerated]
			get
			{
				return _003CMainWindow_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CMainWindow_003Ek__BackingField, value, delegate(MainWindow v)
				{
					_003CMainWindow_003Ek__BackingField = v;
				}, new PropertyChangedEventHandler(MainWindowChanged), triggerOnUpdate: true, "MainWindow");
			}
		}

		public Texture2D SourceTexture { get; private set; }

		public Texture2D CleanedTexture { get; private set; }

		public Texture2D ScaledTexture { get; private set; }

		public string ReadResult { get; private set; }

		public string BestMatchResult { get; private set; }

		public bool IsLoaded { get; private set; }

		public string PathToEngine => OcrApi.PathToEngine;

		public Settings Settings { get; }

		private int CustomThreshold
		{
			get
			{
				return Settings.OCRNoPixelColumns.Value;
			}
			set
			{
				Settings.OCRNoPixelColumns.Value = value;
			}
		}

		public PathCollection Paths { get; }

		public ContentsManager ContentsManager { get; }

		public Module Module { get; }

		public OCR(ClientWindowService clientWindowService, SharedSettings sharedSettings, Settings settings, PathCollection paths, ObservableCollection<Character_Model> characterModels, ContentsManager contentsManager, Module module)
		{
			_clientWindowService = clientWindowService;
			_sharedSettings = sharedSettings;
			Settings = settings;
			Paths = paths;
			_characterModels = characterModels;
			ContentsManager = contentsManager;
			Module = module;
			EnsureTesseractExists();
			try
			{
				string path = (OcrApi.PathToEngine = paths.ModulePath + "tesseract.dll");
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Kenedia.Modules.Characters.Services.Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info($"Set Path to Tesseract Engine: {OcrApi.PathToEngine}. File exists: {File.Exists(path)}");
				_ocrApi = OcrApi.Create();
				_ocrApi.Init(paths.ModulePath, "gw2");
				IsLoaded = true;
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Kenedia.Modules.Characters.Services.Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info("OcrApi Instance created successfully. OCR is useable. " + paths.ModulePath);
			}
			catch (Exception ex)
			{
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Kenedia.Modules.Characters.Services.Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Warn("Creating the OcrApi Instance failed. OCR will not be useable. Character names can not be confirmed.");
				BaseModule<Characters, Kenedia.Modules.Characters.Views.MainWindow, Kenedia.Modules.Characters.Services.Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Warn($"{ex}");
				MainWindow?.SendTesseractFailedNotification(PathToEngine);
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_ocrApi.Dispose();
				_isDisposed = true;
				OcrView?.Dispose();
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

		private void EnsureTesseractExists()
		{
			Version module_version = Module.Version;
			if (!File.Exists(Paths.ModulePath + "\\gw2.traineddata") || Settings.Version.Value != module_version)
			{
				using Stream destination = File.Create(Paths.ModulePath + "\\gw2.traineddata");
				using Stream stream = ContentsManager.GetFileStream("data\\gw2.traineddata");
				stream.Seek(0L, SeekOrigin.Begin);
				stream.CopyTo(destination);
			}
			if (File.Exists(Paths.ModulePath + "\\tesseract.dll") && !(Settings.Version.Value != module_version))
			{
				return;
			}
			using Stream target = File.Create(Paths.ModulePath + "\\tesseract.dll");
			using Stream source = ContentsManager.GetFileStream("data\\tesseract.dll");
			source.Seek(0L, SeekOrigin.Begin);
			source.CopyTo(target);
		}

		public async Task<string?> Read(bool show = false)
		{
			string finalText = null;
			if (IsLoaded)
			{
				try
				{
					if (!show)
					{
						OcrView?.EnableMaskedRegion();
					}
					await Task.Delay(5);
					User32Dll.RECT wndBounds = _clientWindowService.WindowBounds;
					ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
					Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
					double factor = GameService.Graphics.UIScaleMultiplier;
					Point size = default(Point);
					((Point)(ref size))._002Ector((int)((double)Settings.ActiveOCRRegion.Width * factor), (int)((double)Settings.ActiveOCRRegion.Height * factor));
					using (Bitmap bitmap = new Bitmap(size.X, size.Y))
					{
						Bitmap spacingVisibleBitmap = new Bitmap(size.X, size.Y);
						using (Graphics g = Graphics.FromImage(bitmap))
						{
							int left = wndBounds.Left + p.X;
							int top = wndBounds.Top + p.Y;
							Rectangle activeOCRRegion = Settings.ActiveOCRRegion;
							int x = (int)Math.Ceiling((double)((Rectangle)(ref activeOCRRegion)).get_Left() * factor);
							activeOCRRegion = Settings.ActiveOCRRegion;
							int y = (int)Math.Ceiling((double)((Rectangle)(ref activeOCRRegion)).get_Top() * factor);
							g.CopyFromScreen(new Point(left + x, top + y), Point.Empty, new Size(size.X, size.Y));
							if (show)
							{
								using MemoryStream memoryStream = new MemoryStream();
								bitmap.Save(memoryStream, ImageFormat.Bmp);
								Texture2D sourceTexture = SourceTexture;
								if (sourceTexture != null)
								{
									((GraphicsResource)sourceTexture).Dispose();
								}
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
									int threshold = Settings.OCR_ColorThreshold.Value;
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
							using MemoryStream memoryStream2 = new MemoryStream();
							spacingVisibleBitmap.Save(memoryStream2, ImageFormat.Bmp);
							if (show)
							{
								Texture2D cleanedTexture = CleanedTexture;
								if (cleanedTexture != null)
								{
									((GraphicsResource)cleanedTexture).Dispose();
								}
								CleanedTexture = memoryStream2.CreateTexture2D();
							}
						}
						Bitmap ocr_bitmap = bitmap;
						if (bitmap.Width >= 500 || bitmap.Height >= 500)
						{
							double scale = 499.0 / (double)Math.Max(bitmap.Width, bitmap.Height);
							ocr_bitmap = new Bitmap(bitmap, (int)((double)bitmap.Width * scale), (int)((double)bitmap.Height * scale));
						}
						if (show)
						{
							using MemoryStream s = new MemoryStream();
							ocr_bitmap.Save(s, ImageFormat.Bmp);
							Texture2D scaledTexture = ScaledTexture;
							if (scaledTexture != null)
							{
								((GraphicsResource)scaledTexture).Dispose();
							}
							ScaledTexture = s.CreateTexture2D();
						}
						string[] array = (_ocrApi?.GetTextFromImage(ocr_bitmap)).Split(' ');
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
						OcrView.DisableMaskedRegion();
					}
					return finalText;
				}
				catch
				{
				}
				return "No OCR Result!";
			}
			return null;
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
			OcrView?.ToggleContainer();
		}

		private void MainWindowChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!IsLoaded)
			{
				MainWindow?.SendTesseractFailedNotification(PathToEngine);
			}
		}
	}
}
