using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace CinemaModule.Services.YouTube
{
	public class YouTubeService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<YouTubeService>();

		private const int MaxCachedVideoInfos = 50;

		private readonly YoutubeClient _youtubeClient;

		private readonly SemaphoreSlim _apiSemaphore = new SemaphoreSlim(1, 1);

		private readonly object _qualitiesLock = new object();

		private readonly Dictionary<string, YouTubeVideoInfo> _videoInfoCache = new Dictionary<string, YouTubeVideoInfo>();

		private readonly List<string> _videoInfoCacheOrder = new List<string>();

		private List<YouTubeStreamQuality> _cachedQualities = new List<YouTubeStreamQuality>();

		private int _selectedQualityIndex;

		private int _isFetchingQualities;

		private bool _isDisposed;

		public IReadOnlyList<YouTubeStreamQuality> CachedQualities
		{
			get
			{
				lock (_qualitiesLock)
				{
					return _cachedQualities.ToList();
				}
			}
		}

		public event EventHandler<YouTubeQualitiesEventArgs> QualitiesChanged;

		public YouTubeService()
		{
			_youtubeClient = new YoutubeClient();
		}

		public static bool IsYouTubeUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return false;
			}
			if (!url.Contains("youtube.com"))
			{
				return url.Contains("youtu.be");
			}
			return true;
		}

		public static string ExtractVideoId(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return null;
			}
			try
			{
				return VideoId.TryParse(url)?.Value;
			}
			catch
			{
				return null;
			}
		}

		public async Task<YouTubeVideoInfo> GetVideoInfoAsync(string videoIdOrUrl)
		{
			if (_isDisposed || string.IsNullOrWhiteSpace(videoIdOrUrl))
			{
				return null;
			}
			string videoId = ExtractVideoId(videoIdOrUrl) ?? videoIdOrUrl;
			if (_videoInfoCache.TryGetValue(videoId, out var cachedInfo))
			{
				return cachedInfo;
			}
			if (string.IsNullOrWhiteSpace(videoId) || videoId.Length < 5)
			{
				return null;
			}
			await _apiSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (_isDisposed)
				{
					return null;
				}
				if (_videoInfoCache.TryGetValue(videoId, out cachedInfo))
				{
					return cachedInfo;
				}
				Video video = await _youtubeClient.Videos.GetAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
				if (video == null)
				{
					return null;
				}
				YouTubeVideoInfo info = new YouTubeVideoInfo
				{
					VideoId = video.Id.Value,
					Title = (video.Title ?? "Unknown"),
					Author = (video.Author?.ChannelTitle ?? "Unknown"),
					ThumbnailUrl = video.Thumbnails?.OrderByDescending((Thumbnail t) => t.Resolution.Area).FirstOrDefault()?.Url,
					Duration = (video.Duration ?? TimeSpan.Zero),
					IsLiveStream = !video.Duration.HasValue
				};
				CacheVideoInfo(videoId, info);
				return info;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get video info for: " + videoIdOrUrl);
				return null;
			}
			finally
			{
				_apiSemaphore.Release();
			}
		}

		private void CacheVideoInfo(string videoId, YouTubeVideoInfo info)
		{
			if (!_videoInfoCache.ContainsKey(videoId))
			{
				if (_videoInfoCacheOrder.Count >= 50)
				{
					string oldestId = _videoInfoCacheOrder[0];
					_videoInfoCacheOrder.RemoveAt(0);
					_videoInfoCache.Remove(oldestId);
				}
				_videoInfoCache[videoId] = info;
				_videoInfoCacheOrder.Add(videoId);
			}
		}

		public async Task<string> GetPlayableStreamUrlAsync(string videoIdOrUrl)
		{
			return (await GetBestQualityStreamUrlsAsync(videoIdOrUrl).ConfigureAwait(continueOnCapturedContext: false)).VideoUrl;
		}

		public async Task<YouTubeStreamUrls> GetBestQualityStreamUrlsAsync(string videoIdOrUrl)
		{
			if (_isDisposed || string.IsNullOrWhiteSpace(videoIdOrUrl))
			{
				return default(YouTubeStreamUrls);
			}
			try
			{
				string videoId = ExtractVideoId(videoIdOrUrl) ?? videoIdOrUrl;
				StreamManifest streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
				List<VideoOnlyStreamInfo> videoOnlyStreams = (from s in streamManifest.GetVideoOnlyStreams()
					orderby s.VideoQuality.MaxHeight descending, s.Bitrate.BitsPerSecond descending
					select s).ToList();
				VideoOnlyStreamInfo preferredVideo = videoOnlyStreams.FirstOrDefault((VideoOnlyStreamInfo s) => Is1080p(s.VideoQuality.MaxHeight, s.VideoQuality.Label)) ?? videoOnlyStreams.FirstOrDefault();
				if (preferredVideo != null)
				{
					AudioOnlyStreamInfo bestAudio = (from a in streamManifest.GetAudioOnlyStreams()
						orderby a.Bitrate.BitsPerSecond descending
						select a).FirstOrDefault();
					return new YouTubeStreamUrls(preferredVideo.Url, bestAudio?.Url);
				}
				List<MuxedStreamInfo> muxedStreams = (from s in streamManifest.GetMuxedStreams()
					orderby s.VideoQuality.MaxHeight descending
					select s).ToList();
				return new YouTubeStreamUrls((muxedStreams.FirstOrDefault((MuxedStreamInfo s) => Is1080p(s.VideoQuality.MaxHeight, s.VideoQuality.Label)) ?? muxedStreams.FirstOrDefault())?.Url);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get playable stream URL for: " + videoIdOrUrl);
				return default(YouTubeStreamUrls);
			}
		}

		public async Task<string> GetLiveStreamUrlAsync(string videoIdOrUrl)
		{
			if (_isDisposed || string.IsNullOrWhiteSpace(videoIdOrUrl))
			{
				return null;
			}
			try
			{
				string videoId = ExtractVideoId(videoIdOrUrl) ?? videoIdOrUrl;
				return await _youtubeClient.Videos.Streams.GetHttpLiveStreamUrlAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get live stream URL for: " + videoIdOrUrl);
				return null;
			}
		}

		public async Task<YouTubeStreamUrls> GetBestStreamUrlsAsync(string videoIdOrUrl)
		{
			if (_isDisposed || string.IsNullOrWhiteSpace(videoIdOrUrl))
			{
				return default(YouTubeStreamUrls);
			}
			if ((await GetVideoInfoAsync(videoIdOrUrl).ConfigureAwait(continueOnCapturedContext: false))?.IsLiveStream ?? false)
			{
				return new YouTubeStreamUrls(await GetLiveStreamUrlAsync(videoIdOrUrl).ConfigureAwait(continueOnCapturedContext: false));
			}
			YouTubeStreamUrls streamUrls = await GetBestQualityStreamUrlsAsync(videoIdOrUrl).ConfigureAwait(continueOnCapturedContext: false);
			if (!string.IsNullOrEmpty(streamUrls.VideoUrl))
			{
				return streamUrls;
			}
			return new YouTubeStreamUrls(await GetLiveStreamUrlAsync(videoIdOrUrl).ConfigureAwait(continueOnCapturedContext: false));
		}

		public async Task<string> GetBestStreamUrlAsync(string videoIdOrUrl)
		{
			return (await GetBestStreamUrlsAsync(videoIdOrUrl).ConfigureAwait(continueOnCapturedContext: false)).VideoUrl;
		}

		public async Task<List<YouTubeStreamQuality>> GetStreamQualitiesAsync(string videoIdOrUrl)
		{
			List<YouTubeStreamQuality> qualities = new List<YouTubeStreamQuality>();
			if (_isDisposed || string.IsNullOrWhiteSpace(videoIdOrUrl))
			{
				return qualities;
			}
			try
			{
				string videoId = ExtractVideoId(videoIdOrUrl) ?? videoIdOrUrl;
				StreamManifest streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId).ConfigureAwait(continueOnCapturedContext: false);
				string bestAudioUrl = (from a in streamManifest.GetAudioOnlyStreams()
					orderby a.Bitrate.BitsPerSecond descending
					select a).FirstOrDefault()?.Url;
				foreach (MuxedStreamInfo stream in streamManifest.GetMuxedStreams())
				{
					qualities.Add(new YouTubeStreamQuality
					{
						DisplayName = stream.VideoQuality.Label,
						StreamUrl = stream.Url,
						Width = stream.VideoResolution.Width,
						Height = stream.VideoResolution.Height,
						Bitrate = stream.Bitrate.BitsPerSecond
					});
				}
				foreach (VideoOnlyStreamInfo stream2 in streamManifest.GetVideoOnlyStreams())
				{
					qualities.Add(new YouTubeStreamQuality
					{
						DisplayName = stream2.VideoQuality.Label,
						StreamUrl = stream2.Url,
						AudioUrl = bestAudioUrl,
						Width = stream2.VideoResolution.Width,
						Height = stream2.VideoResolution.Height,
						Bitrate = stream2.Bitrate.BitsPerSecond
					});
				}
				return (from q in qualities
					orderby q.Height descending, q.Bitrate descending
					group q by q.Height into g
					select g.First()).ToList();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get stream qualities for: " + videoIdOrUrl);
				return qualities;
			}
		}

		public async Task FetchAndCacheQualitiesAsync(string videoIdOrUrl)
		{
			if (_isDisposed || string.IsNullOrWhiteSpace(videoIdOrUrl) || Interlocked.CompareExchange(ref _isFetchingQualities, 1, 0) != 0)
			{
				return;
			}
			try
			{
				List<YouTubeStreamQuality> qualities = await GetStreamQualitiesAsync(videoIdOrUrl).ConfigureAwait(continueOnCapturedContext: false);
				if (qualities.Count == 0)
				{
					return;
				}
				IReadOnlyList<string> qualityNames;
				int selectedIndex;
				lock (_qualitiesLock)
				{
					_cachedQualities = qualities;
					_selectedQualityIndex = FindPreferredQualityIndex(qualities);
					qualityNames = _cachedQualities.Select((YouTubeStreamQuality q) => q.DisplayName).ToList();
					selectedIndex = _selectedQualityIndex;
				}
				this.QualitiesChanged?.Invoke(this, new YouTubeQualitiesEventArgs(qualityNames, selectedIndex));
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to fetch YouTube qualities for: " + videoIdOrUrl);
			}
			finally
			{
				Interlocked.Exchange(ref _isFetchingQualities, 0);
			}
		}

		private static int FindPreferredQualityIndex(List<YouTubeStreamQuality> qualities)
		{
			int index1080 = qualities.FindIndex((YouTubeStreamQuality q) => Is1080p(q.Height, q.DisplayName));
			if (index1080 >= 0)
			{
				return index1080;
			}
			return 0;
		}

		private static bool Is1080p(int height, string displayName)
		{
			if (height >= 1070 && height <= 1090)
			{
				return true;
			}
			if (!string.IsNullOrEmpty(displayName) && displayName.Contains("1080"))
			{
				return true;
			}
			return false;
		}

		public YouTubeStreamQuality SelectQuality(int qualityIndex)
		{
			lock (_qualitiesLock)
			{
				if (qualityIndex < 0 || qualityIndex >= _cachedQualities.Count)
				{
					return null;
				}
				if (_selectedQualityIndex == qualityIndex)
				{
					return _cachedQualities[qualityIndex];
				}
				_selectedQualityIndex = qualityIndex;
				List<string> qualityNames = _cachedQualities.Select((YouTubeStreamQuality q) => q.DisplayName).ToList();
				int selectedIndex = _selectedQualityIndex;
				YouTubeStreamQuality result = _cachedQualities[qualityIndex];
				this.QualitiesChanged?.Invoke(this, new YouTubeQualitiesEventArgs(qualityNames, selectedIndex));
				return result;
			}
		}

		public void ClearCachedQualities()
		{
			lock (_qualitiesLock)
			{
				_cachedQualities.Clear();
				_selectedQualityIndex = 0;
			}
		}

		public void ClearVideoInfoCache()
		{
			_videoInfoCache.Clear();
			_videoInfoCacheOrder.Clear();
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_cachedQualities.Clear();
				_videoInfoCache.Clear();
				_videoInfoCacheOrder.Clear();
				_apiSemaphore.Dispose();
			}
		}
	}
}
