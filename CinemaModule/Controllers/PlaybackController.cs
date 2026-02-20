using System;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using CinemaModule.VideoPlayer;

namespace CinemaModule.Controllers
{
	public class PlaybackController : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<PlaybackController>();

		private readonly CinemaSettings _moduleSettings;

		private readonly CinemaUserSettings _userSettings;

		private readonly TwitchService _twitchService;

		private global::CinemaModule.VideoPlayer.VideoPlayer _videoPlayer;

		private bool _isPausedDueToRange;

		private bool _isDisposed;

		public bool IsPausedDueToRange => _isPausedDueToRange;

		public bool IsOffline
		{
			get
			{
				if (_videoPlayer != null && !_videoPlayer.IsEnded)
				{
					if (!_videoPlayer.IsPlaying)
					{
						return !_videoPlayer.IsPaused;
					}
					return false;
				}
				return true;
			}
		}

		public bool IsSeekable => _videoPlayer?.IsSeekable ?? false;

		public long Duration => _videoPlayer?.Length ?? 0;

		public float Position => _videoPlayer?.Position ?? 0f;

		public event EventHandler<TwitchStreamRefreshedEventArgs> StreamUrlRefreshed;

		public event EventHandler<PlaybackState> PlaybackStateChanged;

		public PlaybackController(CinemaSettings moduleSettings, CinemaUserSettings userSettings, TwitchService twitchService)
		{
			_moduleSettings = moduleSettings;
			_userSettings = userSettings;
			_twitchService = twitchService;
		}

		public void RegisterPlayer(global::CinemaModule.VideoPlayer.VideoPlayer player)
		{
			_videoPlayer = player;
			_videoPlayer.PlaybackStateChanged += OnPlaybackStateChanged;
		}

		public void StartInitialPlaybackIfEnabled()
		{
			if (!_moduleSettings.IsEnabled || string.IsNullOrEmpty(_userSettings.StreamUrl))
			{
				return;
			}
			_videoPlayer.Play(_userSettings.StreamUrl);
			if (_userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel)
			{
				string channelName = _userSettings.CurrentTwitchChannel;
				if (!string.IsNullOrEmpty(channelName))
				{
					_twitchService.FetchAndCacheQualitiesAsync(channelName);
				}
			}
		}

		private void OnPlaybackStateChanged(object sender, PlaybackStateEventArgs e)
		{
			this.PlaybackStateChanged?.Invoke(this, e.State);
		}

		public void Update()
		{
			_videoPlayer?.Update();
		}

		public void Play(string url)
		{
			_videoPlayer?.Play(url);
		}

		public void Stop()
		{
			_videoPlayer?.Stop();
		}

		public void TogglePause()
		{
			_videoPlayer?.TogglePause();
		}

		public void SetVolume(int volume)
		{
			if (_videoPlayer != null)
			{
				_videoPlayer.Volume = volume;
			}
		}

		public void SetQuality(int qualityIndex)
		{
			_videoPlayer?.SetQuality(qualityIndex);
		}

		public void Seek(float position)
		{
			if (_videoPlayer != null)
			{
				_videoPlayer.Position = position;
			}
		}

		public void HandleStreamUrlChanged(string url)
		{
			if (_videoPlayer == null)
			{
				return;
			}
			_twitchService.ClearCachedQualities();
			if (!_moduleSettings.IsEnabled || string.IsNullOrEmpty(url) || _videoPlayer.CurrentUrl == url)
			{
				return;
			}
			_videoPlayer.Stop();
			_videoPlayer.Play(url);
			if (_userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel)
			{
				string channelName = _userSettings.CurrentTwitchChannel;
				if (!string.IsNullOrEmpty(channelName))
				{
					_twitchService.FetchAndCacheQualitiesAsync(channelName);
				}
			}
		}

		public void HandleEnabledChanged(bool isEnabled)
		{
			if (_videoPlayer != null)
			{
				if (!isEnabled)
				{
					_videoPlayer.Stop();
				}
				else if (_userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel)
				{
					RefreshTwitchStreamAndPlayAsync();
				}
				else if (!string.IsNullOrEmpty(_userSettings.StreamUrl))
				{
					_videoPlayer.Play(_userSettings.StreamUrl);
				}
			}
		}

		public void RestartPlaybackIfNeeded()
		{
			if (_moduleSettings.IsEnabled && !string.IsNullOrEmpty(_userSettings.StreamUrl) && _videoPlayer != null && !_videoPlayer.IsPlaying)
			{
				_videoPlayer.Play(_userSettings.StreamUrl);
			}
		}

		public void UpdateRangeBasedPlayback(bool isInRange, CinemaDisplayMode displayMode)
		{
			if (_videoPlayer != null)
			{
				bool shouldPause = displayMode == CinemaDisplayMode.InGame && !isInRange;
				if (shouldPause && !_isPausedDueToRange && !_videoPlayer.IsPaused)
				{
					_videoPlayer.Pause();
					_isPausedDueToRange = true;
				}
				else if (!shouldPause && _isPausedDueToRange)
				{
					_videoPlayer.Resume();
					_isPausedDueToRange = false;
				}
			}
		}

		public async Task RefreshTwitchStreamAndPlayAsync()
		{
			string channelName = _userSettings.CurrentTwitchChannel;
			if (string.IsNullOrEmpty(channelName))
			{
				Logger.Warn("Cannot refresh Twitch stream - no channel name");
				return;
			}
			try
			{
				string freshUrl = await _twitchService.GetPlayableStreamUrlAsync(channelName);
				if (string.IsNullOrEmpty(freshUrl))
				{
					Logger.Warn("Failed to get fresh stream URL for channel: " + channelName);
					return;
				}
				PlayTwitchStream(freshUrl, channelName);
				this.StreamUrlRefreshed?.Invoke(this, new TwitchStreamRefreshedEventArgs(channelName, freshUrl));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to refresh Twitch stream for channel: " + channelName);
			}
		}

		private void PlayTwitchStream(string streamUrl, string channelName)
		{
			_videoPlayer.Play(streamUrl);
			_twitchService.FetchAndCacheQualitiesAsync(channelName);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				if (_videoPlayer != null)
				{
					_videoPlayer.PlaybackStateChanged -= OnPlaybackStateChanged;
				}
				_isPausedDueToRange = false;
				_isDisposed = true;
			}
		}
	}
}
