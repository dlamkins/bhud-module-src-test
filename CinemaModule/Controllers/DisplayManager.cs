using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using CinemaModule.Models;
using CinemaModule.Settings;
using CinemaModule.UI.VideoDisplays;
using CinemaModule.VideoPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.Controllers
{
	public class DisplayManager : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<DisplayManager>();

		private readonly CinemaSettings _moduleSettings;

		private readonly CinemaUserSettings _userSettings;

		private WindowVideoDisplay _windowDisplay;

		private WorldVideoDisplay _worldDisplay;

		private global::CinemaModule.VideoPlayer.VideoPlayer _videoPlayer;

		private bool _isDisposed;

		public bool IsWorldDisplayInRange => _worldDisplay?.IsInRange ?? false;

		public event EventHandler<Point> WindowPositionChanged;

		public event EventHandler<Point> WindowSizeChanged;

		public event EventHandler<bool> WorldDisplayInRangeChanged;

		public event EventHandler<bool> WindowLockToggled;

		public event EventHandler PlayPauseClicked;

		public event EventHandler<int> VolumeChangedFromUI;

		public event EventHandler SettingsClicked;

		public event EventHandler<int> QualityChanged;

		public event EventHandler TwitchChatClicked;

		public event EventHandler CloseClicked;

		public event EventHandler<float> SeekRequested;

		public DisplayManager(CinemaSettings moduleSettings, CinemaUserSettings userSettings)
		{
			_moduleSettings = moduleSettings;
			_userSettings = userSettings;
		}

		public void RegisterPlayer(global::CinemaModule.VideoPlayer.VideoPlayer player)
		{
			_videoPlayer = player;
		}

		public void RegisterDisplays(WindowVideoDisplay windowDisplay, WorldVideoDisplay worldDisplay)
		{
			_windowDisplay = windowDisplay;
			_worldDisplay = worldDisplay;
			SubscribeToDisplayEvents(_windowDisplay);
			SubscribeToDisplayEvents(_worldDisplay);
			if (_windowDisplay != null)
			{
				_windowDisplay.PositionChanged += OnWindowPositionChanged;
				_windowDisplay.SizeChanged += OnWindowSizeChanged;
				_windowDisplay.LockToggled += OnWindowLockToggled;
			}
			if (_worldDisplay != null)
			{
				_worldDisplay.InRangeChanged += OnWorldDisplayInRangeChanged;
			}
		}

		public void SyncDisplayState()
		{
			SyncDisplayState(_windowDisplay);
			SyncDisplayState(_worldDisplay);
		}

		public void UpdateActiveDisplayTexture()
		{
			IVideoDisplay activeDisplay = GetActiveDisplay();
			if (activeDisplay != null && (!activeDisplay.IsOffline || activeDisplay.OfflineTexture == null))
			{
				StreamPresetData preset = _userSettings.CurrentStreamPreset;
				if (preset != null && preset.IsRadio && preset.StaticImageTexture != null)
				{
					activeDisplay.UpdateTexture(AsyncTexture2D.op_Implicit(preset.StaticImageTexture));
				}
				else if (_videoPlayer != null)
				{
					activeDisplay.UpdateTexture(_videoPlayer.VideoTexture);
				}
			}
		}

		public void UpdateOfflineState(bool isOffline)
		{
			ForEachDisplay(delegate(IVideoDisplay d)
			{
				d.IsOffline = isOffline;
			});
		}

		public void UpdateOfflineTexture(Texture2D texture)
		{
			ForEachDisplay(delegate(IVideoDisplay d)
			{
				d.OfflineTexture = texture;
			});
		}

		public void UpdateDisplayVisibility()
		{
			bool isOnScreen = _userSettings.DisplayMode == CinemaDisplayMode.OnScreen;
			bool isEnabled = _moduleSettings.IsEnabled;
			if (_windowDisplay != null)
			{
				((Control)_windowDisplay).set_Visible(isEnabled && isOnScreen);
			}
			if (_worldDisplay != null)
			{
				((Control)_worldDisplay).set_Visible(isEnabled && !isOnScreen);
			}
		}

		public void HideAllDisplays()
		{
			if (_windowDisplay != null)
			{
				((Control)_windowDisplay).set_Visible(false);
			}
			if (_worldDisplay != null)
			{
				((Control)_worldDisplay).set_Visible(false);
			}
		}

		public void UpdateTwitchStreamState(bool isTwitchStream)
		{
			ForEachDisplay(delegate(IVideoDisplay d)
			{
				d.IsTwitchStream = isTwitchStream;
			});
		}

		public void UpdateAvailableQualities(IReadOnlyList<string> qualityNames, int selectedIndex)
		{
			ForEachDisplay(delegate(IVideoDisplay d)
			{
				d.UpdateAvailableQualities(qualityNames, selectedIndex);
			});
		}

		public void UpdateWorldPosition(WorldPosition3D position)
		{
			if (_worldDisplay != null)
			{
				_worldDisplay.WorldPosition = position;
			}
		}

		public void UpdateWorldScreenWidth(float width)
		{
			if (_worldDisplay != null)
			{
				_worldDisplay.WorldWidth = width;
			}
		}

		private void SubscribeToDisplayEvents(IVideoDisplay display)
		{
			if (display != null)
			{
				display.PlayPauseClicked += OnPlayPauseClicked;
				display.VolumeChanged += OnVolumeChangedFromUI;
				display.SettingsClicked += OnDisplaySettingsClicked;
				display.QualityChanged += OnQualityChanged;
				display.TwitchChatClicked += OnTwitchChatClicked;
				display.CloseClicked += OnCloseClicked;
				display.SeekRequested += OnSeekRequested;
			}
		}

		private void UnsubscribeFromDisplayEvents()
		{
			UnsubscribeFromCommonDisplayEvents(_windowDisplay);
			UnsubscribeFromCommonDisplayEvents(_worldDisplay);
			if (_windowDisplay != null)
			{
				_windowDisplay.PositionChanged -= OnWindowPositionChanged;
				_windowDisplay.SizeChanged -= OnWindowSizeChanged;
				_windowDisplay.LockToggled -= OnWindowLockToggled;
			}
			if (_worldDisplay != null)
			{
				_worldDisplay.InRangeChanged -= OnWorldDisplayInRangeChanged;
			}
		}

		private void UnsubscribeFromCommonDisplayEvents(IVideoDisplay display)
		{
			if (display != null)
			{
				display.PlayPauseClicked -= OnPlayPauseClicked;
				display.VolumeChanged -= OnVolumeChangedFromUI;
				display.SettingsClicked -= OnDisplaySettingsClicked;
				display.QualityChanged -= OnQualityChanged;
				display.TwitchChatClicked -= OnTwitchChatClicked;
				display.CloseClicked -= OnCloseClicked;
				display.SeekRequested -= OnSeekRequested;
			}
		}

		private void SyncDisplayState(IVideoDisplay display)
		{
			if (display != null && _videoPlayer != null)
			{
				display.IsPaused = _videoPlayer.IsPaused || _videoPlayer.IsEnded;
				display.Volume = _videoPlayer.Volume;
			}
		}

		private IVideoDisplay GetActiveDisplay()
		{
			if (_userSettings.DisplayMode != 0)
			{
				return _worldDisplay;
			}
			return _windowDisplay;
		}

		private void OnWindowPositionChanged(object sender, Point position)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			this.WindowPositionChanged?.Invoke(this, position);
		}

		private void OnWindowSizeChanged(object sender, Point size)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			this.WindowSizeChanged?.Invoke(this, size);
		}

		private void OnWindowLockToggled(object sender, bool isLocked)
		{
			this.WindowLockToggled?.Invoke(this, isLocked);
		}

		private void OnWorldDisplayInRangeChanged(object sender, bool isInRange)
		{
			this.WorldDisplayInRangeChanged?.Invoke(this, isInRange);
		}

		private void OnPlayPauseClicked(object sender, EventArgs e)
		{
			this.PlayPauseClicked?.Invoke(this, e);
		}

		private void OnVolumeChangedFromUI(object sender, int volume)
		{
			this.VolumeChangedFromUI?.Invoke(this, volume);
		}

		private void OnDisplaySettingsClicked(object sender, EventArgs e)
		{
			this.SettingsClicked?.Invoke(this, e);
		}

		private void OnQualityChanged(object sender, int qualityIndex)
		{
			this.QualityChanged?.Invoke(this, qualityIndex);
		}

		private void OnTwitchChatClicked(object sender, EventArgs e)
		{
			this.TwitchChatClicked?.Invoke(this, e);
		}

		private void OnCloseClicked(object sender, EventArgs e)
		{
			this.CloseClicked?.Invoke(this, e);
		}

		private void OnSeekRequested(object sender, float position)
		{
			this.SeekRequested?.Invoke(this, position);
		}

		private void ForEachDisplay(Action<IVideoDisplay> action)
		{
			if (_windowDisplay != null)
			{
				action(_windowDisplay);
			}
			if (_worldDisplay != null)
			{
				action(_worldDisplay);
			}
		}

		public void UpdateSeekableState(bool isSeekable, long duration)
		{
			ForEachDisplay(delegate(IVideoDisplay d)
			{
				d.IsSeekable = isSeekable;
				d.Duration = duration;
			});
		}

		public void UpdateCurrentPosition(float position)
		{
			ForEachDisplay(delegate(IVideoDisplay d)
			{
				d.CurrentPosition = position;
			});
		}

		public void UpdateStreamInfo(string title, int? viewerCount, string gameName)
		{
			if (_windowDisplay != null)
			{
				_windowDisplay.StreamTitle = title;
				_windowDisplay.ViewerCount = viewerCount;
				_windowDisplay.GameName = gameName;
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				UnsubscribeFromDisplayEvents();
				_isDisposed = true;
			}
		}
	}
}
