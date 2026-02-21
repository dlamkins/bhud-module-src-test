using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;

namespace CinemaModule.Services
{
	public class RadioMetadataService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<RadioMetadataService>();

		private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5.0);

		private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(15.0);

		private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(2.0);

		private const int MinShoutcastParts = 7;

		private const int TrackNameIndex = 6;

		private const int ListenersIndex = 0;

		private const int BitrateIndex = 5;

		private readonly HttpClient _httpClient;

		private CancellationTokenSource _pollingCts;

		private bool _isDisposed;

		public RadioTrackInfo CurrentTrackInfo { get; private set; }

		public event EventHandler<RadioTrackInfo> TrackInfoUpdated;

		public RadioMetadataService()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(RequestTimeout);
			_httpClient = val;
		}

		public void StartPolling(string streamUrl, string infoUrl)
		{
			StopPolling();
			string metadataUrl = ResolveMetadataUrl(streamUrl, infoUrl);
			if (string.IsNullOrEmpty(metadataUrl))
			{
				Logger.Debug("No valid metadata URL for radio stream");
				return;
			}
			_pollingCts = new CancellationTokenSource();
			PollMetadataAsync(metadataUrl, _pollingCts.Token);
		}

		public void StopPolling()
		{
			if (_pollingCts != null)
			{
				_pollingCts.Cancel();
				_pollingCts.Dispose();
				_pollingCts = null;
			}
			CurrentTrackInfo = null;
			this.TrackInfoUpdated?.Invoke(this, null);
		}

		private string ResolveMetadataUrl(string streamUrl, string infoUrl)
		{
			if (!string.IsNullOrEmpty(infoUrl))
			{
				return infoUrl;
			}
			return TryBuildShoutcastMetadataUrl(streamUrl);
		}

		private string TryBuildShoutcastMetadataUrl(string streamUrl)
		{
			if (string.IsNullOrEmpty(streamUrl))
			{
				return null;
			}
			try
			{
				Uri uri = new Uri(streamUrl);
				return $"{uri.Scheme}://{uri.Host}:{uri.Port}/7.html";
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to build metadata URL from stream URL");
				return null;
			}
		}

		private async Task PollMetadataAsync(string metadataUrl, CancellationToken cancellationToken)
		{
			await Task.Delay(InitialDelay, cancellationToken);
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					RadioTrackInfo trackInfo = await FetchTrackInfoAsync(metadataUrl, cancellationToken);
					if (trackInfo != null && !cancellationToken.IsCancellationRequested)
					{
						CurrentTrackInfo = trackInfo;
						this.TrackInfoUpdated?.Invoke(this, trackInfo);
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex)
				{
					Logger.Debug(ex, "Failed to fetch radio metadata");
				}
				try
				{
					await Task.Delay(PollingInterval, cancellationToken);
				}
				catch (OperationCanceledException)
				{
					return;
				}
			}
		}

		private async Task<RadioTrackInfo> FetchTrackInfoAsync(string url, CancellationToken cancellationToken)
		{
			HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
			try
			{
				if (!response.get_IsSuccessStatusCode())
				{
					return null;
				}
				return ParseShoutcastResponse(await response.get_Content().ReadAsStringAsync());
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
		}

		private RadioTrackInfo ParseShoutcastResponse(string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return null;
			}
			content = StripHtmlTags(content);
			string[] parts = content.Split(',');
			if (parts.Length < 7)
			{
				return null;
			}
			string trackName = parts[6].Trim();
			if (string.IsNullOrEmpty(trackName))
			{
				return null;
			}
			RadioTrackInfo trackInfo = new RadioTrackInfo
			{
				TrackName = trackName,
				Listeners = TryParseInt(parts[0]),
				Bitrate = TryParseInt(parts[5])
			};
			ParseArtistAndTitle(trackName, trackInfo);
			return trackInfo;
		}

		private string StripHtmlTags(string input)
		{
			return Regex.Replace(input, "<[^>]+>", string.Empty).Trim();
		}

		private int? TryParseInt(string value)
		{
			if (!int.TryParse(value.Trim(), out var result))
			{
				return null;
			}
			return result;
		}

		private void ParseArtistAndTitle(string trackName, RadioTrackInfo trackInfo)
		{
			int separatorIndex = trackName.IndexOf(" - ", StringComparison.Ordinal);
			if (separatorIndex > 0)
			{
				trackInfo.Artist = trackName.Substring(0, separatorIndex).Trim();
				trackInfo.Title = trackName.Substring(separatorIndex + 3).Trim();
			}
			else
			{
				trackInfo.Title = trackName;
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				StopPolling();
				((HttpMessageInvoker)_httpClient).Dispose();
				_isDisposed = true;
			}
		}
	}
}
