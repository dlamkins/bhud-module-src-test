using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration.GfxSettings;
using Gw2Sharp.Mumble.Models;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Services
{
	public class GameState : IDisposable
	{
		private bool _disposed;

		private double _lastTick;

		private double? _cutsceneStart;

		private double _cutsceneDuration;

		private readonly MaskedRegion _spinnerMask;

		private readonly MaskedRegion _logoutMask;

		private readonly List<double> _spinnerResults;

		private readonly List<GameStatus> _gameStatuses;

		private GameStatus _gameStatus;

		public ClientWindowService ClientWindowService { get; set; }

		public SharedSettings SharedSettings { get; set; }

		public int Count { get; set; }

		public Stopwatch GameTime { get; set; }

		private GameStatus NewStatus { get; set; }

		public GameStatus OldStatus { get; private set; }

		public GameStatus GameStatus
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
					case GameStatus.Ingame:
						_spinnerResults.Clear();
						_cutsceneDuration = 0.0;
						_cutsceneStart = null;
						this.ChangedToIngame?.Invoke(GameStatus, eventArgs);
						break;
					case GameStatus.CharacterSelection:
						_spinnerResults.Clear();
						_cutsceneDuration = 0.0;
						_cutsceneStart = null;
						this.ChangedToCharacterSelection?.Invoke(GameStatus, eventArgs);
						break;
					case GameStatus.LoadingScreen:
						_cutsceneDuration = 0.0;
						_cutsceneStart = null;
						this.ChangedToLoadingScreen?.Invoke(GameStatus, eventArgs);
						break;
					case GameStatus.Cutscene:
						this.ChangedToCutscene?.Invoke(GameStatus, eventArgs);
						break;
					}
					this.GameStateChanged?.Invoke(GameStatus, eventArgs);
				}
			}
		}

		public bool IsIngame => GameStatus == GameStatus.Ingame;

		public bool IsCharacterSelection => GameStatus == GameStatus.CharacterSelection;

		public event EventHandler<GameStateChangedEventArgs> GameStateChanged;

		public event EventHandler<GameStateChangedEventArgs> ChangedToIngame;

		public event EventHandler<GameStateChangedEventArgs> ChangedToCharacterSelection;

		public event EventHandler<GameStateChangedEventArgs> ChangedToLoadingScreen;

		public event EventHandler<GameStateChangedEventArgs> ChangedToCutscene;

		public GameState(ClientWindowService clientWindowService, SharedSettings sharedSettings)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			FramedMaskedRegion framedMaskedRegion = new FramedMaskedRegion();
			((Control)framedMaskedRegion).set_Visible(false);
			((Control)framedMaskedRegion).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)framedMaskedRegion).set_ZIndex(int.MaxValue);
			framedMaskedRegion.BorderColor = Color.get_Transparent();
			_spinnerMask = framedMaskedRegion;
			FramedMaskedRegion framedMaskedRegion2 = new FramedMaskedRegion();
			((Control)framedMaskedRegion2).set_Visible(false);
			((Control)framedMaskedRegion2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)framedMaskedRegion2).set_ZIndex(int.MaxValue);
			framedMaskedRegion2.BorderColor = Color.get_Transparent();
			_logoutMask = framedMaskedRegion2;
			_spinnerResults = new List<double>();
			_gameStatuses = new List<GameStatus>();
			base._002Ector();
			ClientWindowService = clientWindowService;
			SharedSettings = sharedSettings;
		}

		public void Run(GameTime gameTime)
		{
			NewStatus = GameStatus.Unknown;
			if (GameService.Gw2Mumble.get_TimeSinceTick().TotalMilliseconds <= 500.0)
			{
				NewStatus = GameStatus.Ingame;
				((Control)_logoutMask).Hide();
				((Control)_spinnerMask).Hide();
			}
			else if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				((Control)_spinnerMask).Show();
				((Control)_logoutMask).Show();
				if (gameTime.get_TotalGameTime().TotalMilliseconds - _lastTick > 250.0)
				{
					_lastTick = gameTime.get_TotalGameTime().TotalMilliseconds;
					if (IsButtonVisible())
					{
						NewStatus = GameStatus.CharacterSelection;
					}
					else
					{
						bool isLoadingSpinnerVisible = IsLoadingSpinnerVisible();
						if (GameStatus == GameStatus.CharacterSelection)
						{
							if (isLoadingSpinnerVisible)
							{
								NewStatus = GameStatus.LoadingScreen;
							}
						}
						else
						{
							if (!isLoadingSpinnerVisible)
							{
								double valueOrDefault = _cutsceneStart.GetValueOrDefault();
								if (!_cutsceneStart.HasValue)
								{
									valueOrDefault = gameTime.get_TotalGameTime().TotalMilliseconds;
									_cutsceneStart = valueOrDefault;
								}
								_cutsceneDuration += gameTime.get_TotalGameTime().TotalMilliseconds - _cutsceneStart.Value;
							}
							if (GameStatus != GameStatus.Cutscene)
							{
								if (GameStatus == GameStatus.LoadingScreen)
								{
									if (!isLoadingSpinnerVisible)
									{
										NewStatus = ((_cutsceneDuration > 1000.0) ? GameStatus.Cutscene : NewStatus);
									}
								}
								else if (GameStatus == GameStatus.Ingame)
								{
									NewStatus = (isLoadingSpinnerVisible ? GameStatus.LoadingScreen : ((_cutsceneDuration > 1000.0) ? GameStatus.Cutscene : GameStatus.Unknown));
								}
							}
						}
					}
				}
			}
			if (NewStatus != 0 && StatusConfirmed(NewStatus, _gameStatuses, (NewStatus == GameStatus.LoadingScreen) ? 6 : 3) && GameStatus != NewStatus)
			{
				GameStatus = NewStatus;
				_gameStatuses.Clear();
			}
		}

		public bool IsButtonVisible()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected I4, but got Unknown
			//IL_007d: Expected I4, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			RectangleDimensions offset = (((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? SharedSettings.WindowOffset : new RectangleDimensions(0));
			UiSize uiSize = GameService.Gw2Mumble.get_UI().get_UISize();
			Size size = new Size(50 + uiSize * 3, 40 + uiSize * 3);
			Point pos = new Point(0, -size.Height);
			double factor = GameService.Graphics.get_UIScaleMultiplier();
			((Control)_logoutMask).set_Size(new Point((int)((double)(size.Width * 2) * factor), (int)((double)(size.Height * 2) * factor)));
			((Control)_logoutMask).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Left(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() + -((Control)_logoutMask).get_Size().Y));
			using Bitmap bitmap = new Bitmap(size.Width, size.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using (new MemoryStream())
			{
				g.CopyFromScreen(new Point(wndBounds.Left + offset.Left, wndBounds.Bottom + offset.Bottom + pos.Y), Point.Empty, size);
				return bitmap.IsCutAndCheckFilled(0.4, 0.7f).Item3 > 0.35;
			}
		}

		public bool IsLoadingSpinnerVisible()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected I4, but got Unknown
			//IL_007d: Expected I4, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			RectangleDimensions offset = (((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? SharedSettings.WindowOffset : new RectangleDimensions(0));
			UiSize uiSize = GameService.Gw2Mumble.get_UI().get_UISize();
			Size size = new Size(64 + uiSize * 2, 64 + uiSize * 2);
			Point pos = new Point(-size.Width, -size.Height);
			double factor = GameService.Graphics.get_UIScaleMultiplier();
			((Control)_spinnerMask).set_Size(new Point((int)((double)(size.Width * 2) * factor), (int)((double)(size.Height * 2) * factor)));
			((Control)_spinnerMask).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Right() + -((Control)_spinnerMask).get_Size().X, ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() + -((Control)_spinnerMask).get_Size().Y));
			using Bitmap bitmap = new Bitmap(size.Width, size.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using (new MemoryStream())
			{
				g.CopyFromScreen(new Point(wndBounds.Right + offset.Right + pos.X, wndBounds.Bottom + offset.Bottom + pos.Y - 30), Point.Empty, size);
				(Bitmap, bool, double) isFilled = bitmap.IsNotBlackAndCheckFilled(0.4);
				if (isFilled.Item2)
				{
					SaveResult(isFilled.Item3, _spinnerResults);
				}
				IEnumerable<double> enumerable = _spinnerResults.Distinct();
				return enumerable != null && enumerable.Count() >= 3;
			}
		}

		private void SaveResult(double result, List<double> list)
		{
			list.Add(result);
			if (list.Count > 4)
			{
				list.RemoveAt(0);
			}
		}

		private bool StatusConfirmed(GameStatus newStatus, List<GameStatus> resultList, int threshold = 3)
		{
			resultList.Add(newStatus);
			List<GameStatus> partial = new List<GameStatus>();
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
			if (!_disposed)
			{
				_disposed = true;
				MaskedRegion spinnerMask = _spinnerMask;
				if (spinnerMask != null)
				{
					((Control)spinnerMask).Dispose();
				}
				MaskedRegion logoutMask = _logoutMask;
				if (logoutMask != null)
				{
					((Control)logoutMask).Dispose();
				}
			}
		}
	}
}
