using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using CinemaModule.Models;
using CinemaModule.Services;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class StreamStatusLoader
	{
		private static readonly Logger Logger = Logger.GetLogger<StreamStatusLoader>();

		private readonly TwitchService _twitchService;

		public StreamStatusLoader(TwitchService twitchService)
		{
			_twitchService = twitchService;
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
			await Task.WhenAll(from i in items
				where !string.IsNullOrEmpty(i.ChannelData?.Url)
				select FetchUrlStatusAsync(i, token));
		}

		public async Task<Dictionary<string, StreamStatus>> FetchCustomStreamStatusesAsync(List<SavedStream> streams, CancellationToken token)
		{
			Dictionary<string, StreamStatus> statusMap = new Dictionary<string, StreamStatus>();
			List<SavedStream> twitchStreams = streams.Where((SavedStream s) => s.SourceType == StreamSourceType.TwitchChannel).ToList();
			List<SavedStream> urlStreams = streams.Where((SavedStream s) => s.SourceType == StreamSourceType.Url).ToList();
			await Task.WhenAll(FetchTwitchCustomStatusesAsync(twitchStreams, statusMap, token), FetchUrlCustomStatusesAsync(urlStreams, statusMap, token));
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
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Debug("Failed to check URL status: " + ex.Message);
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
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					Logger.Debug("Failed to fetch Twitch statuses for custom streams: " + ex.Message);
				}
			}
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
				catch (Exception ex)
				{
					if (!token.IsCancellationRequested)
					{
						Logger.Debug("Failed to check URL status for " + stream.Name + ": " + ex.Message);
						statusMap[stream.Id] = new StreamStatus
						{
							Subtitle = "Unknown"
						};
					}
				}
			}));
		}
	}
}
