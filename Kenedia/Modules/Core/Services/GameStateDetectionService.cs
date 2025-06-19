using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.GameIntegration.GfxSettings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Services
{
	public class GameStateDetectionService : IDisposable
	{
		private class ScreenChanging
		{
			public bool IsTopLeftChanging { get; set; }

			public bool IsTopRightChanging { get; set; }

			public bool IsBottomLeftChanging { get; set; }

			public bool IsBottomRightChanging { get; set; }

			public bool IsCenterChanging { get; set; }

			public bool IsSpinnerChanging { get; set; }

			public bool AreCornersChanging
			{
				get
				{
					if (!IsTopLeftChanging && !IsTopRightChanging && !IsBottomLeftChanging)
					{
						return IsBottomRightChanging;
					}
					return true;
				}
			}

			public bool AreAllChanging
			{
				get
				{
					if (AreCornersChanging && IsCenterChanging)
					{
						return IsSpinnerChanging;
					}
					return false;
				}
			}

			public bool NoneChanging
			{
				get
				{
					if (!IsTopLeftChanging && !IsTopRightChanging && !IsBottomRightChanging && !IsBottomLeftChanging && !IsCenterChanging)
					{
						return !IsSpinnerChanging;
					}
					return false;
				}
			}
		}

		private readonly double _startingMumbleTick = GameService.Gw2Mumble.Tick;

		private bool _isDisposed;

		private double _lastTick;

		private readonly FramedMaskedRegion _spinnerMask = new FramedMaskedRegion
		{
			Visible = false,
			Parent = GameService.Graphics.SpriteScreen,
			ZIndex = int.MaxValue,
			BorderColor = Color.get_Transparent()
		};

		private readonly FramedMaskedRegion _topRightMask = new FramedMaskedRegion
		{
			Visible = false,
			Parent = GameService.Graphics.SpriteScreen,
			ZIndex = int.MaxValue,
			BorderColor = Color.get_Red(),
			BorderWidth = new RectangleDimensions(2)
		};

		private readonly FramedMaskedRegion _topLeftMask = new FramedMaskedRegion
		{
			Visible = false,
			Parent = GameService.Graphics.SpriteScreen,
			ZIndex = int.MaxValue,
			BorderColor = Color.get_Red(),
			BorderWidth = new RectangleDimensions(2)
		};

		private readonly FramedMaskedRegion _bottomLeftMask = new FramedMaskedRegion
		{
			Visible = false,
			Parent = GameService.Graphics.SpriteScreen,
			ZIndex = int.MaxValue,
			BorderColor = Color.get_Red(),
			BorderWidth = new RectangleDimensions(2)
		};

		private readonly FramedMaskedRegion _bottomRightMask = new FramedMaskedRegion
		{
			Visible = false,
			Parent = GameService.Graphics.SpriteScreen,
			ZIndex = int.MaxValue,
			BorderColor = Color.get_Red(),
			BorderWidth = new RectangleDimensions(2)
		};

		private readonly FramedMaskedRegion _centerMask = new FramedMaskedRegion
		{
			Visible = false,
			Parent = GameService.Graphics.SpriteScreen,
			ZIndex = int.MaxValue,
			BorderColor = Color.get_Red(),
			BorderWidth = new RectangleDimensions(2)
		};

		private readonly List<GameStatusType> _gameStatuses = new List<GameStatusType>();

		private GameStatusType _gameStatus = GameStatusType.None;

		private (Bitmap lastImage, Bitmap newImage) _bottomLeftImages = (null, null);

		private (Bitmap lastImage, Bitmap newImage) _bottomRightImages = (null, null);

		private (Bitmap lastImage, Bitmap newImage) _topRightImages = (null, null);

		private (Bitmap lastImage, Bitmap newImage) _topLeftImages = (null, null);

		private (Bitmap lastImage, Bitmap newImage) _centerImages = (null, null);

		private (Bitmap lastImage, Bitmap newImage) _spinnerImages = (null, null);

		public bool Enabled { get; set; } = true;


		public ClientWindowService ClientWindowService { get; set; }

		public SharedSettings SharedSettings { get; set; }

		private GameStatusType NewStatus { get; set; }

		public GameStatusType OldStatus { get; private set; }

		public GameStatusType GameStatus
		{
			get
			{
				return _gameStatus;
			}
			private set
			{
				if (_gameStatus != value)
				{
					OldStatus = _gameStatus;
					_gameStatus = value;
					GameStateChangedEventArgs eventArgs = new GameStateChangedEventArgs
					{
						OldStatus = OldStatus,
						Status = _gameStatus
					};
					switch (_gameStatus)
					{
					case GameStatusType.Ingame:
						this.ChangedToIngame?.Invoke(GameStatus, eventArgs);
						break;
					case GameStatusType.CharacterSelection:
						this.ChangedToCharacterSelection?.Invoke(GameStatus, eventArgs);
						break;
					case GameStatusType.LoadingScreen:
						this.ChangedToLoadingScreen?.Invoke(GameStatus, eventArgs);
						break;
					case GameStatusType.Cutscene:
						this.ChangedToCutscene?.Invoke(GameStatus, eventArgs);
						break;
					case GameStatusType.Vista:
						this.ChangedToVista?.Invoke(GameStatus, eventArgs);
						break;
					}
					this.GameStateChanged?.Invoke(GameStatus, eventArgs);
				}
			}
		}

		public bool IsIngame => GameStatus == GameStatusType.Ingame;

		public bool IsCharacterSelection => GameStatus == GameStatusType.CharacterSelection;

		public event EventHandler<GameStateChangedEventArgs> GameStateChanged;

		public event EventHandler<GameStateChangedEventArgs> ChangedToIngame;

		public event EventHandler<GameStateChangedEventArgs> ChangedToCharacterSelection;

		public event EventHandler<GameStateChangedEventArgs> ChangedToLoadingScreen;

		public event EventHandler<GameStateChangedEventArgs> ChangedToCutscene;

		public event EventHandler<GameStateChangedEventArgs> ChangedToVista;

		public GameStateDetectionService(ClientWindowService clientWindowService, SharedSettings sharedSettings)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			ClientWindowService = clientWindowService;
			SharedSettings = sharedSettings;
		}

		public void Run(GameTime gameTime)
		{
			if (!Enabled)
			{
				return;
			}
			NewStatus = GameStatusType.Unknown;
			if (GameService.Gw2Mumble.TimeSinceTick.TotalMilliseconds <= 500.0 && _startingMumbleTick != (double)GameService.Gw2Mumble.Tick)
			{
				NewStatus = GameStatusType.Ingame;
				_topLeftMask.Hide();
				_topRightMask.Hide();
				_bottomLeftMask.Hide();
				_bottomRightMask.Hide();
				_spinnerMask.Hide();
			}
			else
			{
				if (GameStatus == GameStatusType.None)
				{
					return;
				}
				if (GameService.GameIntegration.Gw2Instance.Gw2HasFocus && gameTime.get_TotalGameTime().TotalMilliseconds - _lastTick > 250.0)
				{
					ScreenChanging sC = IsScreenChanging();
					_topLeftMask.Show();
					_topRightMask.Show();
					_bottomLeftMask.Show();
					_bottomRightMask.Show();
					_spinnerMask.Show();
					_lastTick = gameTime.get_TotalGameTime().TotalMilliseconds;
					bool vista = sC.AreAllChanging;
					bool cutscene = !sC.AreCornersChanging && sC.IsCenterChanging && !sC.IsSpinnerChanging;
					bool characterSelection = sC.NoneChanging || (!sC.AreCornersChanging && sC.IsCenterChanging && sC.IsSpinnerChanging);
					bool loadingScreen = !sC.AreCornersChanging && !sC.IsCenterChanging && sC.IsSpinnerChanging;
					bool characterCreation = false;
					bool creationCutscene = !sC.AreCornersChanging && sC.IsCenterChanging && sC.IsSpinnerChanging;
					if ((GameStatus == GameStatusType.Ingame && vista) || GameStatus == GameStatusType.Vista)
					{
						NewStatus = GameStatusType.Vista;
					}
					else
					{
						GameStatusType gameStatus = GameStatus;
						bool flag = ((gameStatus == GameStatusType.Ingame || gameStatus == GameStatusType.LoadingScreen) ? true : false);
						if (flag && (cutscene || creationCutscene))
						{
							NewStatus = GameStatusType.Cutscene;
						}
						else
						{
							gameStatus = GameStatus;
							flag = (((uint)(gameStatus - 1) <= 1u || gameStatus == GameStatusType.Cutscene) ? true : false);
							if (flag && loadingScreen)
							{
								NewStatus = GameStatusType.LoadingScreen;
							}
							else if (GameStatus == GameStatusType.CharacterSelection && characterCreation)
							{
								NewStatus = GameStatusType.CharacterCreation;
							}
							else if (GameStatus == GameStatusType.Ingame && characterSelection)
							{
								NewStatus = GameStatusType.CharacterSelection;
							}
						}
					}
				}
			}
			if (NewStatus != 0 && StatusConfirmed(NewStatus, _gameStatuses, (NewStatus == GameStatusType.LoadingScreen) ? 2 : 4) && GameStatus != NewStatus)
			{
				GameStatus = NewStatus;
				_gameStatuses.Clear();
			}
		}

		private (Bitmap lastImage, Bitmap newImage) CaptureRegion((Bitmap lastImage, Bitmap newImage) images, FramedMaskedRegion region, ScreenRegionType t)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			images.lastImage?.Dispose();
			images.lastImage = images.newImage;
			Rectangle b = GameService.Graphics.SpriteScreen.LocalBounds;
			Point maskSize = (Point)(t switch
			{
				ScreenRegionType.TopLeft => new Point(100, 25), 
				ScreenRegionType.TopRight => new Point(100, 25), 
				ScreenRegionType.BottomLeft => new Point(100, 25), 
				ScreenRegionType.BottomRight => new Point(100, 25), 
				ScreenRegionType.Center => new Point(100, 100), 
				ScreenRegionType.LoadingSpinner => new Point(100, 100), 
				_ => Point.get_Zero(), 
			});
			Point maskPos = (Point)(t switch
			{
				ScreenRegionType.TopLeft => new Point(0, 0), 
				ScreenRegionType.TopRight => new Point(b.Width - maskSize.X, 0), 
				ScreenRegionType.BottomLeft => new Point(0, b.Height - maskSize.Y), 
				ScreenRegionType.BottomRight => new Point(b.Width - maskSize.X, b.Height - maskSize.Y), 
				ScreenRegionType.Center => new Point(((Rectangle)(ref b)).get_Center().X - maskSize.X / 2, ((Rectangle)(ref b)).get_Center().Y - maskSize.Y / 2), 
				ScreenRegionType.LoadingSpinner => new Point(b.Width - maskSize.X, b.Height - maskSize.Y - 50), 
				_ => Point.get_Zero(), 
			});
			region.BorderColor = Color.get_Transparent();
			region.Location = maskPos;
			region.Size = maskSize;
			try
			{
				User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
				ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
				RectangleDimensions offset = (((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? SharedSettings.WindowOffset : new RectangleDimensions(0));
				Bitmap bitmap = new Bitmap(maskSize.X, maskSize.Y);
				using Graphics g = Graphics.FromImage(bitmap);
				using (new MemoryStream())
				{
					double factor = GameService.Graphics.UIScaleMultiplier;
					g.CopyFromScreen(new Point(wndBounds.Left + offset.Left + (int)((double)maskPos.X * factor), wndBounds.Top + offset.Top + (int)((double)maskPos.Y * factor)), Point.Empty, new Size((int)((double)maskSize.X * factor), (int)((double)maskSize.Y * factor)));
					images.newImage = bitmap;
					return images;
				}
			}
			catch
			{
				return images;
			}
		}

		private ScreenChanging IsScreenChanging()
		{
			double threshold = 0.999;
			double spinner_threshold = 1.0;
			double topLeft;
			double topRight;
			double bottomLeft;
			double bottomRight;
			double loadingspinner;
			double center;
			return new ScreenChanging
			{
				IsTopLeftChanging = ((topLeft = CompareImagesMSE(_topLeftImages = CaptureRegion(_topLeftImages, _topLeftMask, ScreenRegionType.TopLeft))) < threshold),
				IsTopRightChanging = ((topRight = CompareImagesMSE(_topRightImages = CaptureRegion(_topRightImages, _topRightMask, ScreenRegionType.TopRight))) < threshold),
				IsBottomLeftChanging = ((bottomLeft = CompareImagesMSE(_bottomLeftImages = CaptureRegion(_bottomLeftImages, _bottomLeftMask, ScreenRegionType.BottomLeft))) < threshold),
				IsBottomRightChanging = ((bottomRight = CompareImagesMSE(_bottomRightImages = CaptureRegion(_bottomRightImages, _bottomRightMask, ScreenRegionType.BottomRight))) < threshold),
				IsSpinnerChanging = ((loadingspinner = CompareImagesMSE(_spinnerImages = CaptureRegion(_spinnerImages, _spinnerMask, ScreenRegionType.LoadingSpinner))) < spinner_threshold),
				IsCenterChanging = ((center = CompareImagesMSE(_centerImages = CaptureRegion(_centerImages, _centerMask, ScreenRegionType.Center))) < threshold)
			};
		}

		private static double CompareImagesMSE((Bitmap lastImage, Bitmap newImage) images)
		{
			var (image1, image2) = images;
			if (image1 == null || image2 == null || image1?.Size != image2?.Size)
			{
				return 0.0;
			}
			double sumSquaredDiff = 0.0;
			int pixelCount = image1.Width * image1.Height;
			for (int y = 0; y < image1.Height; y++)
			{
				for (int x = 0; x < image1.Width; x++)
				{
					Color color1 = image1.GetPixel(x, y);
					Color color2 = image2.GetPixel(x, y);
					int diffR = color1.R - color2.R;
					int diffG = color1.G - color2.G;
					int diffB = color1.B - color2.B;
					sumSquaredDiff += (double)(diffR * diffR + diffG * diffG + diffB * diffB);
				}
			}
			double mse = sumSquaredDiff / (double)pixelCount;
			return 1.0 - mse / 195075.0;
		}

		private bool StatusConfirmed(GameStatusType newStatus, List<GameStatusType> resultList, int threshold = 3)
		{
			resultList.Add(newStatus);
			List<GameStatusType> partial = new List<GameStatusType>();
			for (int i = resultList.Count - 1; i >= 0; i--)
			{
				partial.Add(resultList[i]);
				if (partial.Count >= threshold)
				{
					break;
				}
			}
			if (partial.Count >= threshold)
			{
				return partial.Distinct().Count() == 1;
			}
			return false;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_spinnerMask?.Dispose();
				_spinnerImages.lastImage?.Dispose();
				_spinnerImages.newImage?.Dispose();
				_centerMask?.Dispose();
				_centerImages.lastImage?.Dispose();
				_centerImages.newImage?.Dispose();
				_topLeftMask?.Dispose();
				_topLeftImages.lastImage?.Dispose();
				_topLeftImages.newImage?.Dispose();
				_topRightMask?.Dispose();
				_topRightImages.lastImage?.Dispose();
				_topRightImages.newImage?.Dispose();
				_bottomLeftMask?.Dispose();
				_bottomLeftImages.lastImage?.Dispose();
				_bottomLeftImages.newImage?.Dispose();
				_bottomRightMask?.Dispose();
				_bottomRightImages.lastImage?.Dispose();
				_bottomRightImages.newImage?.Dispose();
			}
		}
	}
}
