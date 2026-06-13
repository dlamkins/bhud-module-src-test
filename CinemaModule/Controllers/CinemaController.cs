using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Controllers.WatchParty;
using CinemaModule.Models;
using CinemaModule.Models.Location;
using CinemaModule.Models.Twitch;
using CinemaModule.Services;
using CinemaModule.Services.Twitch;
using CinemaModule.Services.YouTube;
using CinemaModule.Settings;
using CinemaModule.UI.VideoDisplays;
using CinemaModule.VideoPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.Controllers
{
	public class CinemaController : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<CinemaController>();

		private const float DefaultWorldScreenWidth = 10f;

		private readonly CinemaSettings _moduleSettings;

		private readonly CinemaUserSettings _userSettings;

		private readonly TwitchService _twitchService;

		private readonly YouTubeService _youtubeService;

		private readonly RadioMetadataService _radioMetadataService;

		private readonly PlaybackController _playbackController;

		private readonly DisplayController _displayController;

		private readonly TwitchIntegrationHandler _twitchHandler;

		private WatchPartyController _watchPartyController;

		private WatchPartyPlaybackHandler _watchPartyPlaybackHandler;

		private int _preMuteVolume;

		private bool _isDisposed;

		private bool IsTwitchStream => _userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel;

		public event EventHandler ShowSettingsRequested;

		public event EventHandler<string> ShowChatRequested;

		public event EventHandler<string> ToggleChatRequested;

		public event EventHandler<string> ChatChannelChangeRequested;

		public CinemaController(CinemaSettings coreSettings, CinemaUserSettings userSettings, TwitchService twitchService, YouTubeService youtubeService)
		{
			_moduleSettings = coreSettings;
			_userSettings = userSettings;
			_preMuteVolume = userSettings.Volume;
			_twitchService = twitchService;
			_youtubeService = youtubeService;
			_radioMetadataService = new RadioMetadataService();
			_playbackController = new PlaybackController(coreSettings, userSettings, twitchService, youtubeService);
			_displayController = new DisplayController(coreSettings, userSettings);
			_twitchHandler = new TwitchIntegrationHandler(userSettings, twitchService, youtubeService);
			_radioMetadataService.TrackInfoUpdated += OnRadioTrackInfoUpdated;
			SubscribeToHandlerEvents();
			SubscribeToSettingsEvents();
		}

		public void RegisterPlayer(global::CinemaModule.VideoPlayer.VideoPlayer player)
		{
			_playbackController.RegisterPlayer(player);
			_displayController.RegisterPlayer(player);
			_twitchHandler.RegisterPlayer(player);
		}

		public void RegisterWatchParty(WatchPartyController watchPartyController)
		{
			_watchPartyController = watchPartyController;
			_watchPartyPlaybackHandler = new WatchPartyPlaybackHandler(watchPartyController, _playbackController, _displayController, _youtubeService, _userSettings);
			_twitchHandler.SetWatchPartyCheck(() => _watchPartyController?.IsInRoom ?? false);
			_twitchHandler.QualityChangeCompleted += OnQualityChangeCompleted;
		}

		public void StartInitialPlaybackIfEnabled()
		{
			_playbackController.StartInitialPlaybackIfEnabled();
		}

		public void TogglePause()
		{
			_playbackController.TogglePause();
		}

		public void ToggleLockWindow()
		{
			bool newLocked = !_userSettings.WindowLocked;
			_userSettings.WindowLocked = newLocked;
			_displayController.UpdateWindowLockState(newLocked);
		}

		public void ToggleMute()
		{
			if (_userSettings.Volume > 0)
			{
				_preMuteVolume = _userSettings.Volume;
				_userSettings.Volume = 0;
			}
			else
			{
				_userSettings.Volume = _preMuteVolume;
			}
		}

		public void ToggleModuleEnabled()
		{
			_moduleSettings.EnabledSetting.set_Value(!_moduleSettings.EnabledSetting.get_Value());
		}

		public void RegisterDisplays(WindowVideoDisplay windowDisplay, WorldVideoDisplay worldDisplay)
		{
			_displayController.RegisterDisplays(windowDisplay, worldDisplay);
			_displayController.UpdateWindowZIndex(_userSettings.WindowInForeground);
			_displayController.UpdateTwitchStreamState(IsTwitchStream);
			_displayController.UpdateDisplayVisibility();
			_twitchHandler.InitializeStreamInfo();
			UpdateRadioMetadataPolling();
		}

		public void Update()
		{
			if (_moduleSettings.IsEnabled)
			{
				_playbackController.Update();
				_watchPartyPlaybackHandler?.Update();
				_displayController.SyncDisplayState();
				_displayController.UpdateActiveDisplayTexture();
				UpdateSeekState();
			}
		}

		private void UpdateSeekState()
		{
			bool isSeekable = !IsTwitchStream && _playbackController.IsSeekable;
			_displayController.UpdateSeekableState(isSeekable, _playbackController.Duration);
			_displayController.UpdateCurrentPosition(_playbackController.Position);
		}

		public void SelectSavedLocation(string id)
		{
			_userSettings.SelectedPresetLocationId = "";
			_userSettings.SelectedSavedLocationId = id;
			SavedLocation location = _userSettings.SavedLocations.Locations.Find((SavedLocation l) => l.Id == id);
			if (location?.Position != null)
			{
				_userSettings.WorldPosition = location.Position;
				_userSettings.WorldScreenWidth = location.ScreenWidth;
			}
			else
			{
				_userSettings.WorldPosition = null;
				Logger.Warn("Selected location '" + id + "' is invalid or not found");
			}
		}

		public void SelectPresetLocation(string presetId, WorldPosition3D position, float screenWidth)
		{
			_userSettings.SelectedSavedLocationId = "";
			_userSettings.SelectedPresetLocationId = presetId;
			if (position != null)
			{
				_userSettings.WorldPosition = position;
				_userSettings.WorldScreenWidth = ((screenWidth > 0f) ? screenWidth : 10f);
			}
		}

		public void SelectSavedStream(string id)
		{
			SavedStream stream = _userSettings.SavedStreams.Streams.Find((SavedStream s) => s.Id == id);
			if (stream != null)
			{
				_userSettings.SelectSavedStream(stream);
			}
			else
			{
				Logger.Warn("Saved stream '" + id + "' not found");
			}
		}

		public void PrepareForStreamChange()
		{
			_playbackController.Stop();
			_displayController.UpdateOfflineState(isOffline: true);
			LoadOfflineTextureAsync();
		}

		public void RequestShowChat(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				Logger.Warn("Cannot open Twitch chat - channel name is empty");
			}
			else
			{
				this.ShowChatRequested?.Invoke(this, channelName);
			}
		}

		public void ForceWatchPartyResync()
		{
			_watchPartyPlaybackHandler?.ForceResync();
		}

		public void ApplyLocation(SavedLocation location)
		{
			if (location?.Position != null)
			{
				_userSettings.WorldPosition = location.Position;
				_userSettings.WorldScreenWidth = ((location.ScreenWidth > 0f) ? location.ScreenWidth : 10f);
				Logger.Info("Applied shared location: " + location.Name);
			}
		}

		private void SubscribeToHandlerEvents()
		{
			_displayController.WindowPositionChanged += delegate(object s, Point pos)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_userSettings.WindowPosition = pos;
			};
			_displayController.WindowSizeChanged += delegate(object s, Point size)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_userSettings.WindowSize = size;
			};
			_displayController.WindowLockToggled += delegate(object s, bool locked)
			{
				_userSettings.WindowLocked = locked;
			};
			_displayController.WorldDisplayInRangeChanged += OnWorldDisplayInRangeChanged;
			_displayController.PlayPauseClicked += delegate
			{
				_playbackController.TogglePause();
			};
			_displayController.VolumeChangedFromUI += OnVolumeChangedFromUI;
			_displayController.SettingsClicked += delegate
			{
				this.ShowSettingsRequested?.Invoke(this, EventArgs.Empty);
			};
			_displayController.QualityChanged += delegate(object s, int index)
			{
				_twitchHandler.HandleQualityChange(index);
			};
			_displayController.TwitchChatClicked += OnTwitchChatClicked;
			_displayController.CloseClicked += delegate
			{
				_moduleSettings.EnabledSetting.set_Value(false);
			};
			_displayController.SeekRequested += OnSeekRequested;
			_twitchHandler.ChatChannelChangeRequested += delegate(object s, string channel)
			{
				this.ChatChannelChangeRequested?.Invoke(this, channel);
			};
			_twitchHandler.QualitiesUpdated += OnQualitiesUpdated;
			_twitchHandler.StreamInfoUpdated += OnStreamInfoUpdated;
			_playbackController.StreamUrlRefreshed += OnStreamUrlRefreshed;
			_playbackController.PlaybackStateChanged += OnPlaybackStateChanged;
		}

		private void SubscribeToSettingsEvents()
		{
			_moduleSettings.EnabledSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnabledChanged);
			_userSettings.StreamUrlChanged += OnStreamUrlChanged;
			_userSettings.DisplayModeChanged += OnDisplayModeChanged;
			_userSettings.WorldPositionChanged += OnWorldPositionChanged;
			_userSettings.WorldScreenWidthChanged += OnWorldScreenWidthChanged;
			_userSettings.VolumeChanged += OnVolumeChanged;
			_userSettings.CurrentStreamSourceTypeChanged += OnCurrentStreamSourceTypeChanged;
			_userSettings.CurrentStreamPresetChanged += OnCurrentStreamPresetChanged;
			_userSettings.WindowInForegroundChanged += OnWindowInForegroundChanged;
		}

		private void UnsubscribeFromSettingsEvents()
		{
			_moduleSettings.EnabledSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnabledChanged);
			_userSettings.StreamUrlChanged -= OnStreamUrlChanged;
			_userSettings.DisplayModeChanged -= OnDisplayModeChanged;
			_userSettings.WorldPositionChanged -= OnWorldPositionChanged;
			_userSettings.WorldScreenWidthChanged -= OnWorldScreenWidthChanged;
			_userSettings.VolumeChanged -= OnVolumeChanged;
			_userSettings.CurrentStreamSourceTypeChanged -= OnCurrentStreamSourceTypeChanged;
			_userSettings.CurrentStreamPresetChanged -= OnCurrentStreamPresetChanged;
			_userSettings.WindowInForegroundChanged -= OnWindowInForegroundChanged;
		}

		private void OnEnabledChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			_playbackController.HandleEnabledChanged(e.get_NewValue());
			if (e.get_NewValue())
			{
				_displayController.UpdateDisplayVisibility();
				UpdateRadioMetadataPolling();
			}
			else
			{
				_displayController.HideAllDisplays();
				_radioMetadataService.StopPolling();
			}
		}

		private void OnStreamUrlChanged(object sender, string url)
		{
			CinemaModule.Instance.TextureService.ClearCachedStreamInfo();
			if (string.IsNullOrEmpty(url))
			{
				_playbackController.HandleStreamUrlChanged(url);
				_displayController.UpdateOfflineState(isOffline: true);
				LoadOfflineTextureAsync();
			}
			else
			{
				_displayController.UpdateOfflineState(isOffline: false);
				_displayController.UpdateOfflineTexture(null);
				_playbackController.HandleStreamUrlChanged(url);
			}
			_twitchHandler.HandleStreamUrlChanged(IsTwitchStream);
			UpdateRadioMetadataPolling();
		}

		private void OnDisplayModeChanged(object sender, CinemaDisplayMode mode)
		{
			UpdateRangeBasedPlayback();
			_displayController.UpdateDisplayVisibility();
			_playbackController.RestartPlaybackIfNeeded();
		}

		private void OnWorldPositionChanged(object sender, WorldPosition3D position)
		{
			_displayController.UpdateWorldPosition(position);
			UpdateRangeBasedPlayback();
		}

		private void OnWorldScreenWidthChanged(object sender, float width)
		{
			_displayController.UpdateWorldScreenWidth(width);
		}

		private void OnVolumeChanged(object sender, int volume)
		{
			_playbackController.SetVolume(volume);
		}

		private void OnCurrentStreamSourceTypeChanged(object sender, StreamSourceType sourceType)
		{
			_displayController.UpdateTwitchStreamState(IsTwitchStream);
			_twitchHandler.HandleStreamSourceTypeChanged(sourceType);
			UpdateRadioMetadataPolling();
		}

		private void OnCurrentStreamPresetChanged(object sender, StreamPresetData preset)
		{
			UpdateRadioMetadataPolling();
			if (_playbackController.IsOffline)
			{
				LoadOfflineTextureAsync();
			}
		}

		private void OnWindowInForegroundChanged(object sender, bool inForeground)
		{
			_displayController.UpdateWindowZIndex(inForeground);
		}

		private void OnWorldDisplayInRangeChanged(object sender, bool isInRange)
		{
			UpdateRangeBasedPlayback();
		}

		private void OnVolumeChangedFromUI(object sender, int volume)
		{
			_playbackController.SetVolume(volume);
			_userSettings.Volume = volume;
		}

		private void OnTwitchChatClicked(object sender, EventArgs e)
		{
			string channelName = _twitchHandler.GetCurrentTwitchChannel();
			if (string.IsNullOrEmpty(channelName))
			{
				Logger.Warn("Cannot open Twitch chat - no valid Twitch channel detected");
			}
			else
			{
				this.ToggleChatRequested?.Invoke(this, channelName);
			}
		}

		private void OnQualitiesUpdated(object sender, TwitchQualitiesEventArgs e)
		{
			_displayController.UpdateAvailableQualities(e.QualityNames.ToList(), e.SelectedIndex);
		}

		private void OnStreamInfoUpdated(object sender, TwitchStreamInfo streamInfo)
		{
			CinemaModule.Instance.TextureService.UpdateCachedStreamInfo(streamInfo);
			if (streamInfo != null)
			{
				_displayController.UpdateStreamInfo(streamInfo.ChannelName, streamInfo.ViewerCount, streamInfo.GameName);
			}
			else
			{
				_displayController.UpdateStreamInfo(null, null, null);
			}
		}

		private void OnPlaybackStateChanged(object sender, PlaybackState state)
		{
			bool isOffline = state == PlaybackState.Stopped || state == PlaybackState.Error || state == PlaybackState.Ended;
			_displayController.UpdateOfflineState(isOffline);
			if (isOffline)
			{
				LoadOfflineTextureAsync();
			}
			else if (state == PlaybackState.Playing)
			{
				_displayController.UpdateOfflineTexture(null);
			}
		}

		private async Task LoadOfflineTextureAsync()
		{
			Texture2D offlineTexture = await CinemaModule.Instance.TextureService.LoadOfflineTextureAsync(_userSettings, _twitchService);
			if (offlineTexture != null)
			{
				_displayController.UpdateOfflineTexture(offlineTexture);
			}
		}

		private void OnQualityChangeCompleted(object sender, EventArgs e)
		{
			_watchPartyPlaybackHandler?.HandleQualityChanged();
		}

		private void OnSeekRequested(object sender, float position)
		{
			_playbackController.Seek(position);
		}

		private void OnStreamUrlRefreshed(object sender, TwitchStreamRefreshedEventArgs e)
		{
			_userSettings.StreamUrl = e.StreamUrl;
			this.ChatChannelChangeRequested?.Invoke(this, e.ChannelName);
		}

		private void UpdateRangeBasedPlayback()
		{
			_playbackController.UpdateRangeBasedPlayback(_displayController.IsWorldDisplayInRange, _userSettings.DisplayMode);
		}

		private void OnRadioTrackInfoUpdated(object sender, RadioTrackInfo trackInfo)
		{
			string trackName = trackInfo?.TrackName;
			_displayController.UpdateRadioTrackInfo(trackName);
		}

		private void UpdateRadioMetadataPolling()
		{
			StreamPresetData preset = _userSettings.CurrentStreamPreset;
			if (preset == null || !preset.IsRadio || preset == null || !preset.AsylumInfo || IsTwitchStream)
			{
				_radioMetadataService.StopPolling();
				_displayController.UpdateRadioTrackInfo(null);
			}
			else
			{
				_radioMetadataService.StartPolling(_userSettings.StreamUrl, preset.InfoUrl);
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				UnsubscribeFromSettingsEvents();
				_radioMetadataService.TrackInfoUpdated -= OnRadioTrackInfoUpdated;
				_radioMetadataService.Dispose();
				_playbackController?.Dispose();
				_displayController?.Dispose();
				_twitchHandler?.Dispose();
				_watchPartyPlaybackHandler?.Dispose();
				_isDisposed = true;
			}
		}
	}
}
