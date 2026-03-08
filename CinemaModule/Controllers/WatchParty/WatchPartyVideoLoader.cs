using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using CinemaModule.Models.WatchParty;
using CinemaModule.Services;
using CinemaModule.Services.YouTube;

namespace CinemaModule.Controllers.WatchParty
{
	public sealed class WatchPartyVideoLoader
	{
		private static readonly Logger Logger = Logger.GetLogger<WatchPartyVideoLoader>();

		private readonly PlaybackController _playbackController;

		private readonly DisplayController _displayController;

		private readonly YouTubeService _youtubeService;

		private readonly Func<MemberState, Task> _reportMemberState;

		public WatchPartyVideoLoader(PlaybackController playbackController, DisplayController displayController, YouTubeService youtubeService, Func<MemberState, Task> reportMemberState)
		{
			_playbackController = playbackController;
			_displayController = displayController;
			_youtubeService = youtubeService;
			_reportMemberState = reportMemberState;
		}

		public async Task<VideoLoadResult> LoadVideoAsync(string videoId, Func<string, bool> validateLoadingState)
		{
			Logger.Debug("Starting video load " + videoId);
			PrepareForVideoLoad();
			_reportMemberState(MemberState.Loading);
			LoadThumbnailAsync(videoId);
			var (streamUrl, audioUrl, isLiveStream) = await ResolveStreamUrlAsync(videoId, validateLoadingState).ConfigureAwait(continueOnCapturedContext: false);
			if (string.IsNullOrEmpty(streamUrl))
			{
				Logger.Warn("Failed to resolve stream URL for " + videoId + " - video may be unavailable or age-restricted");
				return VideoLoadResult.Failed();
			}
			if (!validateLoadingState(videoId))
			{
				return VideoLoadResult.Cancelled();
			}
			return VideoLoadResult.Success(streamUrl, audioUrl, isLiveStream);
		}

		public void StartPlayback(string streamUrl, string audioUrl, bool isLiveStream)
		{
			_playbackController.Play(streamUrl, audioUrl);
			_displayController.UpdateOfflineState(isOffline: false);
			_displayController.UpdateSeekableState(!isLiveStream, 0L);
		}

		public async Task LoadThumbnailAsync(string videoId)
		{
			try
			{
				TextureService textureService = CinemaModule.Instance?.TextureService;
				if (textureService != null)
				{
					AsyncTexture2D texture = await textureService.GetYouTubeThumbnailAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
					if (((texture != null) ? texture.get_Texture() : null) != null)
					{
						_displayController.UpdateOfflineTexture(texture.get_Texture());
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load thumbnail for video: " + videoId);
			}
		}

		public async Task LoadVideoInfoAsync(string videoId)
		{
			try
			{
				YouTubeVideoInfo videoInfo = await _youtubeService.GetVideoInfoAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
				if (videoInfo != null)
				{
					_displayController.UpdateStreamInfo(videoInfo.Title, null, videoInfo.Author);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load video info: " + videoId);
			}
		}

		public void FetchQualities(string videoId)
		{
			_youtubeService.FetchAndCacheQualitiesAsync(videoId);
		}

		private void PrepareForVideoLoad()
		{
			_playbackController.Stop();
			_displayController.ClearVideoTexture();
			_displayController.UpdateOfflineTexture(null);
			_displayController.UpdateOfflineState(isOffline: true);
			_youtubeService.ClearVideoInfoCache();
		}

		private async Task<(string Url, string AudioUrl, bool IsLiveStream)> ResolveStreamUrlAsync(string videoId, Func<string, bool> validateLoadingState)
		{
			YouTubeVideoInfo videoInfo = await _youtubeService.GetVideoInfoAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
			if (!validateLoadingState(videoId))
			{
				return (null, null, false);
			}
			bool isLiveStream = videoInfo?.IsLiveStream ?? false;
			var (streamUrl, audioUrl, actualIsLiveStream) = await TryResolveStreamUrlAsync(videoId, isLiveStream).ConfigureAwait(continueOnCapturedContext: false);
			if (videoInfo != null)
			{
				_displayController.UpdateStreamInfo(videoInfo.Title, null, videoInfo.Author);
			}
			return (streamUrl, audioUrl, actualIsLiveStream);
		}

		private async Task<(string Url, string AudioUrl, bool IsLiveStream)> TryResolveStreamUrlAsync(string videoId, bool isLiveStream)
		{
			if (isLiveStream)
			{
				Logger.Debug("Video " + videoId + " is livestream, fetching HLS URL");
				string hlsUrl = await _youtubeService.GetLiveStreamUrlAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
				if (!string.IsNullOrEmpty(hlsUrl))
				{
					return (hlsUrl, null, true);
				}
				Logger.Warn("Failed to get HLS URL for " + videoId + ", trying regular stream URL");
				YouTubeStreamUrls regularUrls = await _youtubeService.GetBestQualityStreamUrlsAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
				return (regularUrls.VideoUrl, regularUrls.AudioUrl, false);
			}
			YouTubeStreamUrls streamUrls = await _youtubeService.GetBestQualityStreamUrlsAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
			if (!string.IsNullOrEmpty(streamUrls.VideoUrl))
			{
				return (streamUrls.VideoUrl, streamUrls.AudioUrl, false);
			}
			Logger.Debug("Regular stream URL failed for " + videoId + ", trying livestream URL");
			string fallbackUrl = await _youtubeService.GetLiveStreamUrlAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
			return (fallbackUrl, null, !string.IsNullOrEmpty(fallbackUrl));
		}
	}
}
