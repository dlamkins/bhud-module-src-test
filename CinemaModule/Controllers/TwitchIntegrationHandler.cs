using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using CinemaModule.VideoPlayer;

namespace CinemaModule.Controllers
{
	public class TwitchIntegrationHandler : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<TwitchIntegrationHandler>();

		private readonly CinemaUserSettings _userSettings;

		private readonly TwitchService _twitchService;

		private global::CinemaModule.VideoPlayer.VideoPlayer _videoPlayer;

		private bool _isDisposed;

		public event EventHandler<string> ChatChannelChangeRequested;

		public event EventHandler<TwitchQualitiesEventArgs> QualitiesUpdated;

		public event EventHandler<TwitchStreamInfo> StreamInfoUpdated;

		public TwitchIntegrationHandler(CinemaUserSettings userSettings, TwitchService twitchService)
		{
			_userSettings = userSettings;
			_twitchService = twitchService;
			_twitchService.QualitiesChanged += OnTwitchQualitiesChanged;
		}

		public void RegisterPlayer(global::CinemaModule.VideoPlayer.VideoPlayer player)
		{
			_videoPlayer = player;
			_videoPlayer.QualitiesChanged += OnVideoQualitiesChanged;
		}

		public void InitializeStreamInfo()
		{
			if (_userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel)
			{
				string channelName = _userSettings.CurrentTwitchChannel;
				if (!string.IsNullOrEmpty(channelName))
				{
					FetchStreamInfoAsync(channelName);
				}
			}
		}

		public bool IsTwitchStreamWithQualities()
		{
			if (_userSettings.CurrentStreamSourceType == StreamSourceType.TwitchChannel)
			{
				return _twitchService.CachedQualities.Count > 0;
			}
			return false;
		}

		public void HandleQualityChange(int qualityIndex)
		{
			if (_videoPlayer != null)
			{
				if (IsTwitchStreamWithQualities())
				{
					HandleTwitchQualityChange(qualityIndex);
				}
				else
				{
					HandleVideoPlayerQualityChange(qualityIndex);
				}
			}
		}

		public void HandleStreamSourceTypeChanged(StreamSourceType sourceType)
		{
			if (sourceType == StreamSourceType.TwitchChannel && (_videoPlayer?.IsPlaying ?? false))
			{
				string channelName = _userSettings.CurrentTwitchChannel;
				if (!string.IsNullOrEmpty(channelName))
				{
					_twitchService.FetchAndCacheQualitiesAsync(channelName);
					FetchStreamInfoAsync(channelName);
					this.ChatChannelChangeRequested?.Invoke(this, channelName);
				}
			}
			else if (sourceType != StreamSourceType.TwitchChannel)
			{
				_twitchService.ClearCachedQualities();
				this.ChatChannelChangeRequested?.Invoke(this, null);
				this.StreamInfoUpdated?.Invoke(this, null);
			}
		}

		public void HandleStreamUrlChanged(bool isTwitchStream)
		{
			if (isTwitchStream)
			{
				string channelName = _userSettings.CurrentTwitchChannel;
				if (!string.IsNullOrEmpty(channelName))
				{
					this.ChatChannelChangeRequested?.Invoke(this, channelName);
					FetchStreamInfoAsync(channelName);
				}
			}
			else
			{
				this.ChatChannelChangeRequested?.Invoke(this, null);
				this.StreamInfoUpdated?.Invoke(this, null);
			}
		}

		public async Task FetchStreamInfoAsync(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				this.StreamInfoUpdated?.Invoke(this, null);
				return;
			}
			TwitchStreamInfo streamInfo = await _twitchService.GetStreamInfoAsync(channelName);
			this.StreamInfoUpdated?.Invoke(this, streamInfo);
		}

		public string GetCurrentTwitchChannel()
		{
			string channel = _userSettings.CurrentTwitchChannel;
			if (!string.IsNullOrEmpty(channel))
			{
				return channel;
			}
			return null;
		}

		public void FetchQualitiesForChannel(string channelName)
		{
			_twitchService.FetchAndCacheQualitiesAsync(channelName);
		}

		public void ClearCachedQualities()
		{
			_twitchService.ClearCachedQualities();
		}

		private void OnTwitchQualitiesChanged(object sender, TwitchQualitiesEventArgs e)
		{
			this.QualitiesUpdated?.Invoke(this, e);
		}

		private void OnVideoQualitiesChanged(object sender, EventArgs e)
		{
			if (_videoPlayer != null && (_userSettings.CurrentStreamSourceType != StreamSourceType.TwitchChannel || _twitchService.CachedQualities.Count <= 0))
			{
				TwitchQualitiesEventArgs eventArgs = new TwitchQualitiesEventArgs(_videoPlayer.AvailableQualities.Select((VideoQuality q) => q.Name).ToList(), _videoPlayer.SelectedQualityIndex);
				this.QualitiesUpdated?.Invoke(this, eventArgs);
			}
		}

		private void HandleTwitchQualityChange(int qualityIndex)
		{
			string streamUrl = _twitchService.SelectQuality(qualityIndex);
			if (!string.IsNullOrEmpty(streamUrl))
			{
				_videoPlayer.Stop();
				_videoPlayer.Play(streamUrl);
			}
		}

		private void HandleVideoPlayerQualityChange(int qualityIndex)
		{
			_videoPlayer.SetQuality(qualityIndex);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_twitchService.QualitiesChanged -= OnTwitchQualitiesChanged;
				if (_videoPlayer != null)
				{
					_videoPlayer.QualitiesChanged -= OnVideoQualitiesChanged;
				}
				_isDisposed = true;
			}
		}
	}
}
