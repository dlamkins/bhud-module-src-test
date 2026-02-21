using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using CinemaModule.Controllers;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using CinemaModule.UI.VideoDisplays;
using CinemaModule.VideoPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule
{
	public class CinemaController : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<CinemaController>();

		private const float DefaultWorldScreenWidth = 10f;

		private readonly CinemaSettings _moduleSettings;

		private readonly CinemaUserSettings _userSettings;

		private readonly TwitchService _twitchService;

		private readonly RadioMetadataService _radioMetadataService;

		private readonly PlaybackController _playbackController;

		private readonly DisplayManager _displayManager;

		private readonly TwitchIntegrationHandler _twitchHandler;

		private TwitchStreamInfo _currentTwitchStreamInfo;

		private bool _isDisposed;

		private bool IsTwitchStream => _userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel;

		public event EventHandler ShowSettingsRequested;

		public event EventHandler<string> ShowChatRequested;

		public event EventHandler<string> ToggleChatRequested;

		public event EventHandler<string> ChatChannelChangeRequested;

		public CinemaController(CinemaSettings coreSettings, CinemaUserSettings userSettings, TwitchService twitchService)
		{
			_moduleSettings = coreSettings;
			_userSettings = userSettings;
			_twitchService = twitchService;
			_radioMetadataService = new RadioMetadataService();
			_playbackController = new PlaybackController(coreSettings, userSettings, twitchService);
			_displayManager = new DisplayManager(coreSettings, userSettings);
			_twitchHandler = new TwitchIntegrationHandler(userSettings, twitchService);
			_radioMetadataService.TrackInfoUpdated += OnRadioTrackInfoUpdated;
			SubscribeToHandlerEvents();
			SubscribeToSettingsEvents();
		}

		public void RegisterPlayer(global::CinemaModule.VideoPlayer.VideoPlayer player)
		{
			_playbackController.RegisterPlayer(player);
			_displayManager.RegisterPlayer(player);
			_twitchHandler.RegisterPlayer(player);
		}

		public void StartInitialPlaybackIfEnabled()
		{
			_playbackController.StartInitialPlaybackIfEnabled();
		}

		public void RegisterDisplays(WindowVideoDisplay windowDisplay, WorldVideoDisplay worldDisplay)
		{
			_displayManager.RegisterDisplays(windowDisplay, worldDisplay);
			_displayManager.UpdateTwitchStreamState(IsTwitchStream);
			_displayManager.UpdateDisplayVisibility();
			_twitchHandler.InitializeStreamInfo();
			UpdateRadioMetadataPolling();
		}

		public void Update()
		{
			if (_moduleSettings.IsEnabled)
			{
				_playbackController.Update();
				_displayManager.SyncDisplayState();
				_displayManager.UpdateActiveDisplayTexture();
				UpdateSeekState();
			}
		}

		private void UpdateSeekState()
		{
			bool isSeekable = !IsTwitchStream && _playbackController.IsSeekable;
			_displayManager.UpdateSeekableState(isSeekable, _playbackController.Duration);
			_displayManager.UpdateCurrentPosition(_playbackController.Position);
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

		private void SubscribeToHandlerEvents()
		{
			_displayManager.WindowPositionChanged += delegate(object s, Point pos)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_userSettings.WindowPosition = pos;
			};
			_displayManager.WindowSizeChanged += delegate(object s, Point size)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_userSettings.WindowSize = size;
			};
			_displayManager.WindowLockToggled += delegate(object s, bool locked)
			{
				_userSettings.WindowLocked = locked;
			};
			_displayManager.WorldDisplayInRangeChanged += OnWorldDisplayInRangeChanged;
			_displayManager.PlayPauseClicked += delegate
			{
				_playbackController.TogglePause();
			};
			_displayManager.VolumeChangedFromUI += OnVolumeChangedFromUI;
			_displayManager.SettingsClicked += delegate
			{
				this.ShowSettingsRequested?.Invoke(this, EventArgs.Empty);
			};
			_displayManager.QualityChanged += delegate(object s, int index)
			{
				_twitchHandler.HandleQualityChange(index);
			};
			_displayManager.TwitchChatClicked += OnTwitchChatClicked;
			_displayManager.CloseClicked += delegate
			{
				_moduleSettings.EnabledSetting.set_Value(false);
			};
			_displayManager.SeekRequested += OnSeekRequested;
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
		}

		private void OnEnabledChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			_playbackController.HandleEnabledChanged(e.get_NewValue());
			if (e.get_NewValue())
			{
				_displayManager.UpdateDisplayVisibility();
				UpdateRadioMetadataPolling();
			}
			else
			{
				_displayManager.HideAllDisplays();
				_radioMetadataService.StopPolling();
			}
		}

		private void OnStreamUrlChanged(object sender, string url)
		{
			_displayManager.UpdateOfflineState(isOffline: false);
			_displayManager.UpdateOfflineTexture(null);
			_currentTwitchStreamInfo = null;
			_playbackController.HandleStreamUrlChanged(url);
			_twitchHandler.HandleStreamUrlChanged(IsTwitchStream);
			UpdateRadioMetadataPolling();
		}

		private void OnDisplayModeChanged(object sender, CinemaDisplayMode mode)
		{
			UpdateRangeBasedPlayback();
			_displayManager.UpdateDisplayVisibility();
			_playbackController.RestartPlaybackIfNeeded();
		}

		private void OnWorldPositionChanged(object sender, WorldPosition3D position)
		{
			_displayManager.UpdateWorldPosition(position);
			UpdateRangeBasedPlayback();
		}

		private void OnWorldScreenWidthChanged(object sender, float width)
		{
			_displayManager.UpdateWorldScreenWidth(width);
		}

		private void OnVolumeChanged(object sender, int volume)
		{
			_playbackController.SetVolume(volume);
		}

		private void OnCurrentStreamSourceTypeChanged(object sender, StreamSourceType sourceType)
		{
			_displayManager.UpdateTwitchStreamState(IsTwitchStream);
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
			_displayManager.UpdateAvailableQualities(e.QualityNames.ToList(), e.SelectedIndex);
		}

		private void OnStreamInfoUpdated(object sender, TwitchStreamInfo streamInfo)
		{
			_currentTwitchStreamInfo = streamInfo;
			if (streamInfo != null)
			{
				_displayManager.UpdateStreamInfo(streamInfo.ChannelName, streamInfo.ViewerCount, streamInfo.GameName);
			}
			else
			{
				_displayManager.UpdateStreamInfo(null, null, null);
			}
		}

		private void OnPlaybackStateChanged(object sender, PlaybackState state)
		{
			bool isOffline = state == PlaybackState.Stopped || state == PlaybackState.Error || state == PlaybackState.Ended;
			_displayManager.UpdateOfflineState(isOffline);
			if (isOffline)
			{
				LoadOfflineTextureAsync();
			}
			else if (state == PlaybackState.Playing)
			{
				_displayManager.UpdateOfflineTexture(null);
			}
		}

		private async Task LoadOfflineTextureAsync()
		{
			Texture2D val = ((!IsTwitchStream) ? (await LoadUrlStaticImageTextureAsync()) : (await LoadTwitchAvatarTextureAsync()));
			Texture2D offlineTexture = val;
			if (offlineTexture != null)
			{
				_displayManager.UpdateOfflineTexture(offlineTexture);
			}
		}

		private async Task<Texture2D> LoadTwitchAvatarTextureAsync()
		{
			string channelName = _userSettings.CurrentTwitchChannel;
			if (string.IsNullOrEmpty(channelName))
			{
				return null;
			}
			TwitchStreamInfo twitchStreamInfo = _currentTwitchStreamInfo;
			if (twitchStreamInfo == null)
			{
				twitchStreamInfo = await _twitchService.GetStreamInfoAsync(channelName);
			}
			TwitchStreamInfo streamInfo = twitchStreamInfo;
			if (streamInfo == null || string.IsNullOrEmpty(streamInfo.AvatarUrl))
			{
				return null;
			}
			AsyncTexture2D obj = await _twitchService.GetAvatarTextureAsync("offline_" + channelName, streamInfo.AvatarUrl);
			return (obj != null) ? obj.get_Texture() : null;
		}

		private async Task<Texture2D> LoadUrlStaticImageTextureAsync()
		{
			StreamPresetData preset = _userSettings.CurrentStreamPreset;
			if (preset == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(preset.StaticImage))
			{
				return null;
			}
			AsyncTexture2D asyncTexture = await CinemaModule.Instance.TextureService.GetImageFromUrlAsync("offline_static_" + preset.Id, preset.StaticImage);
			if (asyncTexture != null)
			{
				preset.StaticImageTexture = asyncTexture;
			}
			return (asyncTexture != null) ? asyncTexture.get_Texture() : null;
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
			_playbackController.UpdateRangeBasedPlayback(_displayManager.IsWorldDisplayInRange, _userSettings.DisplayMode);
		}

		private void OnRadioTrackInfoUpdated(object sender, RadioTrackInfo trackInfo)
		{
			string trackName = trackInfo?.TrackName;
			_displayManager.UpdateRadioTrackInfo(trackName);
		}

		private void UpdateRadioMetadataPolling()
		{
			StreamPresetData preset = _userSettings.CurrentStreamPreset;
			bool isRadio = preset?.IsRadio ?? false;
			bool showMetadata = preset?.AsylumInfo ?? false;
			if (!isRadio || IsTwitchStream || !showMetadata)
			{
				_radioMetadataService.StopPolling();
				_displayManager.UpdateRadioTrackInfo(null);
			}
			else
			{
				string streamUrl = _userSettings.StreamUrl;
				string infoUrl = preset?.InfoUrl;
				_radioMetadataService.StartPolling(streamUrl, infoUrl);
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				UnsubscribeFromSettingsEvents();
				_radioMetadataService.TrackInfoUpdated -= OnRadioTrackInfoUpdated;
				_radioMetadataService?.Dispose();
				_playbackController?.Dispose();
				_displayManager?.Dispose();
				_twitchHandler?.Dispose();
				_isDisposed = true;
			}
		}
	}
}
