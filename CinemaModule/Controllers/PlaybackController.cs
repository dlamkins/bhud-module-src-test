using System;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Models;
using CinemaModule.Services.Twitch;
using CinemaModule.Services.YouTube;
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

		private readonly YouTubeService _youtubeService;

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

		public bool IsPlaying => _videoPlayer?.IsPlaying ?? false;

		public bool IsPaused => _videoPlayer?.IsPaused ?? false;

		public bool IsBuffering => _videoPlayer?.IsBuffering ?? false;

		public bool IsSeekable => _videoPlayer?.IsSeekable ?? false;

		public long Duration => _videoPlayer?.Length ?? 0;

		public float Position => _videoPlayer?.Position ?? 0f;

		public event EventHandler<TwitchStreamRefreshedEventArgs> StreamUrlRefreshed;

		public event EventHandler<YouTubeStreamRefreshedEventArgs> YouTubeStreamUrlRefreshed;

		public event EventHandler<PlaybackState> PlaybackStateChanged;

		public PlaybackController(CinemaSettings moduleSettings, CinemaUserSettings userSettings, TwitchService twitchService, YouTubeService youtubeService)
		{
			_moduleSettings = moduleSettings;
			_userSettings = userSettings;
			_twitchService = twitchService;
			_youtubeService = youtubeService;
		}

		public void RegisterPlayer(global::CinemaModule.VideoPlayer.VideoPlayer player)
		{
			_videoPlayer = player;
			_videoPlayer.PlaybackStateChanged += OnPlaybackStateChanged;
		}

		public void StartInitialPlaybackIfEnabled()
		{
			if (!_moduleSettings.IsEnabled)
			{
				return;
			}
			bool autoplay = _userSettings.AutoplayOnStartup;
			if (_userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel)
			{
				if (!string.IsNullOrEmpty(_userSettings.CurrentTwitchChannel))
				{
					RefreshTwitchStreamAsync(autoplay);
					return;
				}
			}
			else if (_userSettings.CurrentStreamSourceType == StreamSourceType.YouTubeVideo && !string.IsNullOrEmpty(_userSettings.CurrentYouTubeVideo))
			{
				RefreshYouTubeStreamAsync(autoplay);
				return;
			}
			if (!string.IsNullOrEmpty(_userSettings.StreamUrl))
			{
				_videoPlayer.Play(_userSettings.StreamUrl);
				if (!autoplay)
				{
					_videoPlayer.Pause();
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

		public void Pause()
		{
			_videoPlayer?.Pause();
		}

		public void Resume()
		{
			_videoPlayer?.Resume();
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
			if (_videoPlayer != null && !float.IsNaN(position) && !float.IsInfinity(position))
			{
				_videoPlayer.Position = Math.Max(0f, Math.Min(1f, position));
			}
		}

		public void HandleStreamUrlChanged(string url)
		{
			if (_videoPlayer != null && _moduleSettings.IsEnabled)
			{
				if (string.IsNullOrEmpty(url))
				{
					_videoPlayer.Stop();
				}
				else if (!(_videoPlayer.CurrentUrl == url))
				{
					_twitchService.ClearCachedQualities();
					_youtubeService.ClearCachedQualities();
					_videoPlayer.Stop();
					_videoPlayer.Play(url);
					FetchQualitiesForCurrentSource();
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
				else
				{
					StartPlaybackForCurrentSource();
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
			await RefreshTwitchStreamAsync(autoplay: true);
		}

		public async Task RefreshYouTubeStreamAndPlayAsync(YouTubeVideoInfo videoInfo = null)
		{
			await RefreshYouTubeStreamAsync(autoplay: true, videoInfo);
		}

		public async Task RefreshTwitchStreamAsync(bool autoplay)
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
				PlayTwitchStream(freshUrl, channelName, autoplay);
				this.StreamUrlRefreshed?.Invoke(this, new TwitchStreamRefreshedEventArgs(channelName, freshUrl));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to refresh Twitch stream for channel: " + channelName);
			}
		}

		public async Task RefreshYouTubeStreamAsync(bool autoplay, YouTubeVideoInfo videoInfo = null)
		{
			string videoId = _userSettings.CurrentYouTubeVideo;
			if (string.IsNullOrEmpty(videoId))
			{
				Logger.Warn("Cannot refresh YouTube stream - no video ID");
				return;
			}
			try
			{
				YouTubeVideoInfo youTubeVideoInfo = videoInfo;
				if (youTubeVideoInfo == null)
				{
					youTubeVideoInfo = await _youtubeService.GetVideoInfoAsync(videoId);
				}
				videoInfo = youTubeVideoInfo;
				string text = ((!(videoInfo?.IsLiveStream ?? false)) ? (await _youtubeService.GetPlayableStreamUrlAsync(videoId)) : (await _youtubeService.GetLiveStreamUrlAsync(videoId)));
				string freshUrl = text;
				if (string.IsNullOrEmpty(freshUrl))
				{
					Logger.Warn("Failed to get fresh stream URL for video: " + videoId);
					return;
				}
				PlayYouTubeStream(freshUrl, videoId, autoplay);
				this.YouTubeStreamUrlRefreshed?.Invoke(this, new YouTubeStreamRefreshedEventArgs(videoId, freshUrl));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to refresh YouTube stream for video: " + videoId);
			}
		}

		private void PlayTwitchStream(string streamUrl, string channelName, bool autoplay = true)
		{
			PlayAndPauseIfNeeded(streamUrl, autoplay);
			_twitchService.FetchAndCacheQualitiesAsync(channelName);
		}

		private void PlayYouTubeStream(string streamUrl, string videoId, bool autoplay = true)
		{
			PlayAndPauseIfNeeded(streamUrl, autoplay);
			_youtubeService.FetchAndCacheQualitiesAsync(videoId);
		}

		private void PlayAndPauseIfNeeded(string url, bool autoplay)
		{
			_videoPlayer.Play(url);
			if (!autoplay)
			{
				_videoPlayer.Pause();
			}
		}

		private void FetchQualitiesForCurrentSource()
		{
			switch (_userSettings.CurrentStreamSourceType)
			{
			case StreamSourceType.TwitchChannel:
			{
				string channelName = _userSettings.CurrentTwitchChannel;
				if (!string.IsNullOrEmpty(channelName))
				{
					_twitchService.FetchAndCacheQualitiesAsync(channelName);
				}
				break;
			}
			case StreamSourceType.YouTubeVideo:
			{
				string videoId = _userSettings.CurrentYouTubeVideo;
				if (!string.IsNullOrEmpty(videoId))
				{
					_youtubeService.FetchAndCacheQualitiesAsync(videoId);
				}
				break;
			}
			}
		}

		private void StartPlaybackForCurrentSource()
		{
			switch (_userSettings.CurrentStreamSourceType)
			{
			case StreamSourceType.TwitchChannel:
				RefreshTwitchStreamAndPlayAsync();
				return;
			case StreamSourceType.YouTubeVideo:
				RefreshYouTubeStreamAndPlayAsync();
				return;
			}
			if (!string.IsNullOrEmpty(_userSettings.StreamUrl))
			{
				_videoPlayer.Play(_userSettings.StreamUrl);
			}
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
