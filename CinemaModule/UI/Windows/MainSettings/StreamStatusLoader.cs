using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Models;
using CinemaModule.Models.Twitch;
using CinemaModule.Services.Twitch;
using CinemaModule.Services.YouTube;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Windows.MainSettings
{
	public class StreamStatusLoader
	{
		private static readonly Logger Logger = Logger.GetLogger<StreamStatusLoader>();

		private readonly TwitchService _twitchService;

		private readonly YouTubeService _youtubeService;

		public StreamStatusLoader(TwitchService twitchService, YouTubeService youtubeService)
		{
			_twitchService = twitchService;
			_youtubeService = youtubeService;
		}

		public async Task FetchTwitchStatusesAsync(List<StreamListItem> items, CancellationToken token)
		{
			List<string> channels = (from i in items
				select i.TwitchChannel into c
				where !string.IsNullOrEmpty(c)
				select c).ToList();
			if (channels.Count == 0)
			{
				return;
			}
			try
			{
				Dictionary<string, TwitchStreamInfo> infos = await _twitchService.GetMultipleStreamInfoAsync(channels);
				if (token.IsCancellationRequested)
				{
					return;
				}
				foreach (StreamListItem item in items)
				{
					item.ApplyStatus(CreateTwitchStatus(infos, item.TwitchChannel));
				}
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Warn(ex, "Failed to fetch Twitch statuses");
				}
			}
		}

		public async Task FetchUrlStatusesAsync(List<StreamListItem> items, CancellationToken token)
		{
			List<StreamListItem> twitchItems = items.Where((StreamListItem i) => (i.ChannelData?.IsTwitchChannel ?? false) && !string.IsNullOrEmpty(i.TwitchChannel)).ToList();
			List<StreamListItem> source = items.Where(delegate(StreamListItem i)
			{
				ChannelData channelData = i.ChannelData;
				return (channelData == null || !channelData.IsTwitchChannel) && !string.IsNullOrEmpty(i.ChannelData?.Url);
			}).ToList();
			Task twitchTask = FetchTwitchStatusesForItemsAsync(twitchItems, token);
			await Task.WhenAll(source.Select((StreamListItem i) => FetchUrlStatusAsync(i, token)).Concat(new Task[1] { twitchTask }));
		}

		private async Task FetchTwitchStatusesForItemsAsync(List<StreamListItem> items, CancellationToken token)
		{
			if (items.Count == 0)
			{
				return;
			}
			List<string> channelNames = (from i in items
				select i.TwitchChannel into c
				where !string.IsNullOrEmpty(c)
				select c).ToList();
			if (channelNames.Count == 0)
			{
				return;
			}
			try
			{
				Dictionary<string, TwitchStreamInfo> infos = await _twitchService.GetMultipleStreamInfoAsync(channelNames);
				if (token.IsCancellationRequested)
				{
					return;
				}
				foreach (StreamListItem item in items)
				{
					item.ApplyStatus(CreateTwitchStatus(infos, item.TwitchChannel));
				}
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Warn("Failed to fetch Twitch statuses for channel items: " + ex.Message);
				}
			}
		}

		public async Task<Dictionary<string, StreamStatus>> FetchCustomStreamStatusesAsync(List<SavedStream> streams, CancellationToken token)
		{
			Dictionary<string, StreamStatus> statusMap = new Dictionary<string, StreamStatus>();
			List<SavedStream> twitchStreams = streams.Where((SavedStream s) => s.SourceType == StreamSourceType.TwitchChannel).ToList();
			List<SavedStream> urlStreams = streams.Where((SavedStream s) => s.SourceType == StreamSourceType.Url).ToList();
			List<SavedStream> youtubeVideoStreams = streams.Where((SavedStream s) => s.SourceType == StreamSourceType.YouTubeVideo).ToList();
			List<SavedStream> youtubeChannelOrPlaylistStreams = streams.Where((SavedStream s) => s.IsYouTubeChannelOrPlaylist).ToList();
			await Task.WhenAll(FetchTwitchCustomStatusesAsync(twitchStreams, statusMap, token), FetchUrlCustomStatusesAsync(urlStreams, statusMap, token), FetchYouTubeCustomStatusesAsync(youtubeVideoStreams, statusMap, token), FetchYouTubeChannelPlaylistStatusesAsync(youtubeChannelOrPlaylistStreams, statusMap, token));
			return statusMap;
		}

		public static StreamStatus CreateTwitchStatus(Dictionary<string, TwitchStreamInfo> infos, string channelName)
		{
			if (infos.TryGetValue(channelName, out var info))
			{
				StreamStatus obj = (info.IsLive ? StreamStatus.Live(info.GameName, info.ViewerCount) : StreamStatus.Offline());
				obj.AvatarUrl = info.AvatarUrl;
				return obj;
			}
			return StreamStatus.Offline();
		}

		private async Task FetchUrlStatusAsync(StreamListItem item, CancellationToken token)
		{
			try
			{
				UrlAvailabilityResult result = await _twitchService.CheckUrlAvailabilityAsync(item.ChannelData.Url);
				if (!token.IsCancellationRequested)
				{
					item.ApplyStatus(GetUrlStatus(result.IsAvailable.GetValueOrDefault(), item.IsOnDemand));
				}
			}
			catch
			{
				if (!token.IsCancellationRequested)
				{
					item.ApplyStatus(StreamStatus.Offline());
				}
			}
		}

		private static StreamStatus GetUrlStatus(bool isAvailable, bool isOnDemand)
		{
			if (!isAvailable)
			{
				return StreamStatus.Offline();
			}
			if (!isOnDemand)
			{
				return StreamStatus.Available();
			}
			return StreamStatus.OnDemand();
		}

		private async Task FetchTwitchCustomStatusesAsync(List<SavedStream> twitchStreams, Dictionary<string, StreamStatus> statusMap, CancellationToken token)
		{
			if (twitchStreams.Count == 0)
			{
				return;
			}
			List<string> channelNames = (from s in twitchStreams
				select s.Value into v
				where !string.IsNullOrEmpty(v)
				select v).ToList();
			if (channelNames.Count == 0)
			{
				return;
			}
			try
			{
				Dictionary<string, TwitchStreamInfo> infos = await _twitchService.GetMultipleStreamInfoAsync(channelNames);
				if (token.IsCancellationRequested)
				{
					return;
				}
				foreach (SavedStream stream in twitchStreams)
				{
					statusMap[stream.Id] = CreateTwitchStatus(infos, stream.Value);
				}
			}
			catch
			{
			}
		}

		private async Task FetchYouTubeCustomStatusesAsync(List<SavedStream> youtubeStreams, Dictionary<string, StreamStatus> statusMap, CancellationToken token)
		{
			if (youtubeStreams.Count == 0)
			{
				return;
			}
			await Task.WhenAll(((IEnumerable<SavedStream>)youtubeStreams).Select((Func<SavedStream, Task>)async delegate(SavedStream stream)
			{
				try
				{
					YouTubeVideoInfo info = await _youtubeService.GetVideoInfoAsync(stream.Value);
					if (!token.IsCancellationRequested)
					{
						statusMap[stream.Id] = ((info != null) ? StreamStatus.YouTubeAvailable(info.Title) : StreamStatus.YouTubeInvalid());
					}
				}
				catch
				{
					if (!token.IsCancellationRequested)
					{
						statusMap[stream.Id] = StreamStatus.YouTubeInvalid();
					}
				}
			}));
		}

		private async Task FetchUrlCustomStatusesAsync(List<SavedStream> urlStreams, Dictionary<string, StreamStatus> statusMap, CancellationToken token)
		{
			if (urlStreams.Count == 0)
			{
				return;
			}
			await Task.WhenAll(((IEnumerable<SavedStream>)urlStreams).Select((Func<SavedStream, Task>)async delegate(SavedStream stream)
			{
				try
				{
					UrlAvailabilityResult result = await _twitchService.CheckUrlAvailabilityAsync(stream.Value);
					if (!token.IsCancellationRequested)
					{
						statusMap[stream.Id] = GetUrlStatus(result.IsAvailable.GetValueOrDefault(), isOnDemand: false);
					}
				}
				catch
				{
					if (!token.IsCancellationRequested)
					{
						statusMap[stream.Id] = new StreamStatus
						{
							Subtitle = "Unknown"
						};
					}
				}
			}));
		}

		private async Task FetchYouTubeChannelPlaylistStatusesAsync(List<SavedStream> streams, Dictionary<string, StreamStatus> statusMap, CancellationToken token)
		{
			if (streams.Count == 0)
			{
				return;
			}
			await Task.WhenAll(((IEnumerable<SavedStream>)streams).Select((Func<SavedStream, Task>)async delegate(SavedStream stream)
			{
				try
				{
					List<YouTubePlaylistVideo> videos = await _youtubeService.GetChannelVideosAsync(stream.Value, 1);
					if (!token.IsCancellationRequested)
					{
						if (videos.Count > 0)
						{
							statusMap[stream.Id] = new StreamStatus
							{
								Subtitle = "Click to browse videos",
								SubtitleColor = Color.get_LightGreen()
							};
						}
						else
						{
							statusMap[stream.Id] = new StreamStatus
							{
								Subtitle = "No videos found",
								SubtitleColor = Color.get_Gray()
							};
						}
					}
				}
				catch
				{
					if (!token.IsCancellationRequested)
					{
						statusMap[stream.Id] = new StreamStatus
						{
							Subtitle = "Could not load",
							SubtitleColor = Color.get_Gray()
						};
					}
				}
			}));
		}
	}
}
