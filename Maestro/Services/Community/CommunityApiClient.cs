using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Maestro.Models;
using Maestro.Services.Data;
using Newtonsoft.Json;

namespace Maestro.Services.Community
{
	public class CommunityApiClient : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<CommunityApiClient>();

		private const string BASE_URL = "https://raw.githubusercontent.com/uwponcel/maestro-songs/master";

		private readonly HttpClient _httpClient;

		public CommunityApiClient()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(30.0));
			_httpClient = val;
			((HttpHeaders)_httpClient.get_DefaultRequestHeaders()).Add("User-Agent", "Maestro-BlishHUD-Module");
		}

		public async Task<CommunityManifest> FetchManifestAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			_ = 1;
			try
			{
				string url = "https://raw.githubusercontent.com/uwponcel/maestro-songs/master/manifest.json";
				Logger.Info("Fetching community manifest from " + url);
				HttpResponseMessage obj = await _httpClient.GetAsync(url, cancellationToken);
				obj.EnsureSuccessStatusCode();
				CommunityManifest manifest = JsonConvert.DeserializeObject<CommunityManifest>(await obj.get_Content().ReadAsStringAsync());
				Logger.Info($"Fetched manifest with {(manifest?.Songs?.Count).GetValueOrDefault()} songs");
				return manifest;
			}
			catch (OperationCanceledException)
			{
				Logger.Debug("Manifest fetch cancelled");
				throw;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to fetch community manifest");
				throw;
			}
		}

		public async Task<Song> FetchSongAsync(string songId, CancellationToken cancellationToken = default(CancellationToken))
		{
			_ = 1;
			try
			{
				string url = "https://raw.githubusercontent.com/uwponcel/maestro-songs/master/songs/" + songId + ".json";
				Logger.Info("Fetching community song " + songId);
				HttpResponseMessage obj = await _httpClient.GetAsync(url, cancellationToken);
				obj.EnsureSuccessStatusCode();
				Song song = SongSerializer.DeserializeJsonContent(await obj.get_Content().ReadAsStringAsync());
				if (song != null)
				{
					song.CommunityId = songId;
				}
				Logger.Info("Fetched song: " + song?.Name);
				return song;
			}
			catch (OperationCanceledException)
			{
				Logger.Debug("Song fetch cancelled for " + songId);
				throw;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to fetch community song " + songId);
				throw;
			}
		}

		public void Dispose()
		{
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
	}
}
