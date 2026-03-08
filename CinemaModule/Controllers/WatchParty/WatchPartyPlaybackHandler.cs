using System;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Models.WatchParty;
using CinemaModule.Services.YouTube;
using CinemaModule.Settings;
using CinemaModule.VideoPlayer;

namespace CinemaModule.Controllers.WatchParty
{
	public sealed class WatchPartyPlaybackHandler : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<WatchPartyPlaybackHandler>();

		private const double ReportIntervalSeconds = 0.3;

		private const double LatencyMeasureIntervalSeconds = 30.0;

		private const int PostLoadSyncDelayMs = 5000;

		private readonly WatchPartyController _watchPartyController;

		private readonly PlaybackController _playbackController;

		private readonly DisplayController _displayController;

		private readonly CinemaUserSettings _userSettings;

		private readonly WatchPartyVideoLoader _videoLoader;

		private readonly WatchPartySyncManager _syncManager;

		private readonly object _videoStateLock = new object();

		private string _currentVideoId;

		private string _loadingVideoId;

		private string _pendingVideoId;

		private volatile bool _isCurrentVideoLiveStream;

		private DateTime _lastReportTime = DateTime.MinValue;

		private DateTime _lastLatencyMeasure = DateTime.MinValue;

		private volatile bool _isDisposed;

		public WatchPartyPlaybackHandler(WatchPartyController watchPartyController, PlaybackController playbackController, DisplayController displayController, YouTubeService youtubeService, CinemaUserSettings userSettings)
		{
			_watchPartyController = watchPartyController;
			_playbackController = playbackController;
			_displayController = displayController;
			_userSettings = userSettings;
			_videoLoader = new WatchPartyVideoLoader(playbackController, displayController, youtubeService, ReportMemberStateSafeAsync);
			_syncManager = new WatchPartySyncManager(playbackController, ReportMemberStateSafeAsync);
			SubscribeToEvents();
		}

		private void SubscribeToEvents()
		{
			_watchPartyController.RoomJoined += OnRoomJoined;
			_watchPartyController.StateChanged += OnStateChanged;
			_watchPartyController.RoomLeft += OnRoomLeft;
			_watchPartyController.HostStatusChanged += OnHostStatusChanged;
			_playbackController.PlaybackStateChanged += OnPlaybackStateChanged;
		}

		private void UnsubscribeFromEvents()
		{
			_watchPartyController.RoomJoined -= OnRoomJoined;
			_watchPartyController.StateChanged -= OnStateChanged;
			_watchPartyController.RoomLeft -= OnRoomLeft;
			_watchPartyController.HostStatusChanged -= OnHostStatusChanged;
			_playbackController.PlaybackStateChanged -= OnPlaybackStateChanged;
		}

		private void OnRoomJoined(object sender, EventArgs e)
		{
			Logger.Debug("Room joined - clearing selection and stopping playback");
			_userSettings.ClearStreamSelection();
			StopPlayback();
			_displayController.UpdateTwitchStreamState(isTwitchStream: false);
			_displayController.UpdateDisplayVisibility();
			ReportMemberStateSafeAsync(MemberState.Idle);
		}

		private void OnRoomLeft(object sender, EventArgs e)
		{
			Logger.Debug("Room left - stopping playback");
			StopPlayback();
		}

		private void OnHostStatusChanged(object sender, bool isHost)
		{
			UpdateViewerState();
		}

		private void OnPlaybackStateChanged(object sender, PlaybackState playbackState)
		{
			if (!_isDisposed && _watchPartyController.IsInRoom && !IsLoading())
			{
				MemberState? memberState = ConvertToMemberState(playbackState);
				if (memberState.HasValue)
				{
					ReportMemberStateSafeAsync(memberState.Value);
				}
				if (playbackState == PlaybackState.Ended && _watchPartyController.IsHost && _userSettings.WatchPartyAutoplayNext)
				{
					Logger.Debug("Video ended - triggering autoplay next");
					_watchPartyController.PlayNextInQueueAsync();
				}
			}
		}

		private void OnStateChanged(object sender, WatchPartyStateArgs e)
		{
			if (!_isDisposed && _watchPartyController.IsInRoom)
			{
				UpdateViewerState();
				if (e.ChangeType == WatchPartyStateChangeType.FullStateReceived)
				{
					ReportCurrentMemberState();
				}
				WatchPartyLocalState state = e.State;
				if (state == null || !state.HasVideo)
				{
					HandleNoVideo();
					TryAutoplayFromQueue(state, e.ChangeType);
				}
				else
				{
					ProcessStateChange(state, e.ChangeType);
				}
			}
		}

		private void TryAutoplayFromQueue(WatchPartyLocalState state, WatchPartyStateChangeType changeType)
		{
			if (_watchPartyController.IsHost && _userSettings.WatchPartyAutoplayNext && state != null && state.Queue.Count != 0 && changeType == WatchPartyStateChangeType.QueueUpdated)
			{
				Logger.Debug("Queue updated with no video playing - triggering autoplay");
				_watchPartyController.PlayNextInQueueAsync();
			}
		}

		private void ProcessStateChange(WatchPartyLocalState state, WatchPartyStateChangeType changeType)
		{
			bool isViewer = !_watchPartyController.IsHost;
			bool isLoading = IsLoading();
			bool hasVideoLoaded = HasVideoLoaded(state.CurrentVideoId);
			if (changeType == WatchPartyStateChangeType.PlayStateChanged && isViewer && !isLoading && hasVideoLoaded)
			{
				_syncManager.SyncPlayPauseImmediate(state);
			}
			else if (!ShouldWaitForVideo(state, isViewer))
			{
				if (NeedsVideoLoad(state.CurrentVideoId, isLoading))
				{
					Logger.Debug("Loading video " + state.CurrentVideoId);
					_syncManager.ResetServerTimeTracking();
					LoadAndPlayVideoAsync(state);
				}
				else if (!isLoading && isViewer && hasVideoLoaded)
				{
					SyncToHost(state, changeType);
				}
			}
		}

		public void Update()
		{
			if (!_isDisposed && _watchPartyController.IsInRoom && !IsLoading())
			{
				MeasureLatencyIfNeeded();
				if ((_playbackController.IsPlaying || _playbackController.IsPaused) && !((DateTime.UtcNow - _lastReportTime).TotalSeconds < 0.3))
				{
					_lastReportTime = DateTime.UtcNow;
					ReportPlaybackTimeSafeAsync(GetCurrentTimeSeconds());
				}
			}
		}

		public void HandleScreenEnabled()
		{
			if (!_isDisposed && _watchPartyController.IsInRoom)
			{
				Logger.Debug("Screen re-enabled - requesting state to resume playback");
				_displayController.UpdateTwitchStreamState(isTwitchStream: false);
				_displayController.UpdateDisplayVisibility();
				ClearVideoState();
				_watchPartyController.RequestStateAsync();
			}
		}

		public void ForceResync()
		{
			if (!_isDisposed && _watchPartyController.IsInRoom && !_watchPartyController.IsHost)
			{
				Logger.Debug("Force resync requested");
				ClearVideoState();
				_playbackController.Stop();
				_displayController.ClearVideoTexture();
				_displayController.UpdateOfflineState(isOffline: true);
				ReportMemberStateSafeAsync(MemberState.Idle);
				_watchPartyController.RequestStateAsync();
			}
		}

		public void HandleQualityChanged()
		{
			if (!_isDisposed && _watchPartyController.IsInRoom && !_watchPartyController.IsHost)
			{
				Logger.Debug("Quality changed - requesting state sync");
				SyncAfterQualityChangeAsync();
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_syncManager.Dispose();
				UnsubscribeFromEvents();
			}
		}

		private void ClearVideoState()
		{
			lock (_videoStateLock)
			{
				_currentVideoId = null;
				_loadingVideoId = null;
				_pendingVideoId = null;
			}
		}

		private bool HasVideoLoaded(string videoId)
		{
			lock (_videoStateLock)
			{
				return _currentVideoId == videoId;
			}
		}

		private bool IsLoading()
		{
			lock (_videoStateLock)
			{
				return _loadingVideoId != null;
			}
		}

		private bool NeedsVideoLoad(string videoId, bool isLoading)
		{
			if (isLoading)
			{
				return false;
			}
			lock (_videoStateLock)
			{
				if (videoId == _currentVideoId || videoId == _pendingVideoId)
				{
					return false;
				}
				_loadingVideoId = videoId;
				return true;
			}
		}

		private bool ShouldWaitForVideo(WatchPartyLocalState state, bool isViewer)
		{
			if (!isViewer)
			{
				return false;
			}
			string pendingId;
			lock (_videoStateLock)
			{
				pendingId = _pendingVideoId;
			}
			if (pendingId != state.CurrentVideoId)
			{
				return false;
			}
			if (state.IsPlaying || state.IsHostReady())
			{
				Logger.Debug("Ready to load pending video " + state.CurrentVideoId);
				lock (_videoStateLock)
				{
					_pendingVideoId = null;
				}
				LoadAndPlayVideoAsync(state);
			}
			return true;
		}

		private void HandleNoVideo()
		{
			bool hadVideo;
			lock (_videoStateLock)
			{
				hadVideo = _currentVideoId != null || _pendingVideoId != null;
			}
			if (hadVideo)
			{
				Logger.Debug("No current video - stopping playback");
				StopPlayback();
			}
		}

		private void StopPlayback()
		{
			ClearVideoState();
			_isCurrentVideoLiveStream = false;
			_syncManager.ResetServerTimeTracking();
			_syncManager.ResetSeekState();
			_playbackController.Stop();
			_displayController.ClearVideoTexture();
			_displayController.UpdateOfflineState(isOffline: true);
			_displayController.UpdateOfflineTexture(null);
			_displayController.UpdateStreamInfo(null, null, null);
			_displayController.UpdateWatchPartyViewerState(isViewer: false);
		}

		private async Task LoadAndPlayVideoAsync(WatchPartyLocalState state)
		{
			if (_isDisposed)
			{
				return;
			}
			string videoId = state.CurrentVideoId;
			if (!_watchPartyController.IsHost && !state.IsPlaying && !state.IsHostReady())
			{
				Logger.Debug("Waiting for host before loading " + videoId);
				lock (_videoStateLock)
				{
					_pendingVideoId = videoId;
					_loadingVideoId = null;
				}
				ReportMemberStateSafeAsync(MemberState.Idle);
				_videoLoader.LoadThumbnailAsync(videoId);
				_videoLoader.LoadVideoInfoAsync(videoId);
				return;
			}
			try
			{
				VideoLoadResult result = await _videoLoader.LoadVideoAsync(videoId, ValidateLoadingState).ConfigureAwait(continueOnCapturedContext: false);
				if (!_isDisposed && !result.IsCancelled)
				{
					if (result.IsSuccess)
					{
						await StartPlaybackAsync(videoId, result.StreamUrl, result.AudioUrl, result.IsLiveStream, state).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						HandleLoadFailure(videoId);
					}
				}
			}
			catch (Exception ex)
			{
				if (!_isDisposed)
				{
					Logger.Error(ex, "Exception during video load for " + videoId);
					HandleLoadFailure(videoId);
				}
			}
		}

		private async Task StartPlaybackAsync(string videoId, string streamUrl, string audioUrl, bool isLiveStream, WatchPartyLocalState state)
		{
			if (_isDisposed)
			{
				return;
			}
			Logger.Debug($"Playing video {videoId} (livestream: {isLiveStream})");
			_videoLoader.StartPlayback(streamUrl, audioUrl, isLiveStream);
			_isCurrentVideoLiveStream = isLiveStream;
			lock (_videoStateLock)
			{
				_currentVideoId = videoId;
				_loadingVideoId = null;
			}
			_syncManager.SetVideoLoadTime();
			if (!isLiveStream)
			{
				_videoLoader.FetchQualities(videoId);
			}
			WatchPartyLocalState currentState = _watchPartyController.CurrentState;
			if (currentState != null && !_watchPartyController.IsHost && !isLiveStream && !currentState.IsPlaying)
			{
				_playbackController.Pause();
				ReportMemberStateSafeAsync(MemberState.Paused);
			}
			else
			{
				ReportMemberStateSafeAsync(MemberState.Playing);
				if (_watchPartyController.IsHost)
				{
					ReportPlaybackTimeSafeAsync(0.0);
				}
			}
			RequestFreshStateAfterDelayAsync();
		}

		private bool ValidateLoadingState(string expectedVideoId)
		{
			lock (_videoStateLock)
			{
				if (_loadingVideoId != expectedVideoId)
				{
					Logger.Debug("Video load cancelled - different video now loading: " + _loadingVideoId);
					return false;
				}
				return true;
			}
		}

		private void HandleLoadFailure(string videoId)
		{
			Logger.Warn("Video load failed for " + videoId);
			lock (_videoStateLock)
			{
				if (_loadingVideoId == videoId)
				{
					_loadingVideoId = null;
				}
			}
			ReportMemberStateSafeAsync(MemberState.Idle);
		}

		private void SyncToHost(WatchPartyLocalState state, WatchPartyStateChangeType changeType)
		{
			if (changeType == WatchPartyStateChangeType.PlaybackUpdated || changeType == WatchPartyStateChangeType.PlayStateChanged || changeType == WatchPartyStateChangeType.FullStateReceived)
			{
				if (_syncManager.DetectHostSeek(state))
				{
					_syncManager.ForceSyncPlayback(state, _isCurrentVideoLiveStream);
				}
				else
				{
					_syncManager.SyncPlayback(state, _isCurrentVideoLiveStream);
				}
			}
		}

		private async Task SyncAfterQualityChangeAsync()
		{
			await Task.Delay(200).ConfigureAwait(continueOnCapturedContext: false);
			if (_isDisposed || !_watchPartyController.IsInRoom || _watchPartyController.IsHost)
			{
				return;
			}
			await _watchPartyController.RequestStateAsync().ConfigureAwait(continueOnCapturedContext: false);
			await Task.Delay(300).ConfigureAwait(continueOnCapturedContext: false);
			if (!_isDisposed && !IsLoading())
			{
				WatchPartyLocalState currentState = _watchPartyController.CurrentState;
				if (currentState != null)
				{
					_syncManager.ForceSyncPlayback(currentState, _isCurrentVideoLiveStream);
				}
			}
		}

		private MemberState? ConvertToMemberState(PlaybackState playbackState)
		{
			switch (playbackState)
			{
			case PlaybackState.Playing:
				return MemberState.Playing;
			case PlaybackState.Paused:
				return MemberState.Paused;
			case PlaybackState.Stopped:
			case PlaybackState.Ended:
			case PlaybackState.Error:
				return MemberState.Idle;
			default:
				return null;
			}
		}

		private void ReportCurrentMemberState()
		{
			PlaybackState playbackState = (_playbackController.IsPlaying ? PlaybackState.Playing : (_playbackController.IsPaused ? PlaybackState.Paused : PlaybackState.Stopped));
			MemberState? memberState = ConvertToMemberState(playbackState);
			if (memberState.HasValue)
			{
				ReportMemberStateSafeAsync(memberState.Value);
			}
		}

		private void UpdateViewerState()
		{
			bool isViewer = _watchPartyController.IsInRoom && !_watchPartyController.IsHost;
			_displayController.UpdateWatchPartyViewerState(isViewer);
		}

		private void MeasureLatencyIfNeeded()
		{
			if (!((DateTime.UtcNow - _lastLatencyMeasure).TotalSeconds < 30.0))
			{
				_lastLatencyMeasure = DateTime.UtcNow;
				_watchPartyController.MeasureLatencyAsync();
			}
		}

		private double GetCurrentTimeSeconds()
		{
			long durationMs = _playbackController.Duration;
			if (durationMs <= 0)
			{
				return 0.0;
			}
			return (double)_playbackController.Position * ((double)durationMs / 1000.0);
		}

		private async Task ReportPlaybackTimeSafeAsync(double currentTime)
		{
			if (_isDisposed)
			{
				return;
			}
			try
			{
				await _watchPartyController.ReportPlaybackTimeAsync(currentTime).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				if (!_isDisposed)
				{
					Logger.Warn(ex, $"Failed to report playback time: {currentTime:F1}s");
				}
			}
		}

		private async Task ReportMemberStateSafeAsync(MemberState state)
		{
			if (_isDisposed)
			{
				return;
			}
			try
			{
				await _watchPartyController.ReportMemberStateAsync(state).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				if (!_isDisposed)
				{
					Logger.Warn(ex, $"Failed to report member state: {state}");
				}
			}
		}

		private async Task RequestFreshStateAfterDelayAsync()
		{
			await Task.Delay(5000).ConfigureAwait(continueOnCapturedContext: false);
			bool shouldSkip;
			lock (_videoStateLock)
			{
				shouldSkip = _isDisposed || _loadingVideoId != null || _currentVideoId == null;
			}
			if (shouldSkip)
			{
				return;
			}
			try
			{
				await _watchPartyController.RequestStateAsync().ConfigureAwait(continueOnCapturedContext: false);
				await Task.Delay(500).ConfigureAwait(continueOnCapturedContext: false);
				if (!_isDisposed && !IsLoading())
				{
					WatchPartyLocalState currentState = _watchPartyController.CurrentState;
					if (currentState != null && !_watchPartyController.IsHost && !_isCurrentVideoLiveStream)
					{
						_syncManager.SyncPlayback(currentState, _isCurrentVideoLiveStream);
					}
				}
			}
			catch (Exception ex)
			{
				if (!_isDisposed)
				{
					Logger.Warn(ex, "Failed to request fresh state after video load");
				}
			}
		}
	}
}
