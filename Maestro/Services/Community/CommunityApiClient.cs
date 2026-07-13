using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

		private const string STATIC_HOST_BASE = "https://bhm.blishhud.com/Aex.Maestro";

		private const string UPLOAD_API_URL = "https://maestro-api.uwponcel.workers.dev/api";

		private readonly HttpClient _httpClient;

		private readonly string _clientId;

		public CommunityApiClient(string clientId = null)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			_clientId = clientId;
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(30.0));
			_httpClient = val;
			((HttpHeaders)_httpClient.get_DefaultRequestHeaders()).Add("User-Agent", "Maestro-BlishHUD-Module");
		}

		private static string NamespaceSegment(SongNamespace ns)
		{
			return ns switch
			{
				SongNamespace.Builtin => "builtin", 
				SongNamespace.Community => "community", 
				SongNamespace.CommunityPending => "community-pending", 
				_ => throw new ArgumentOutOfRangeException("ns", ns, null), 
			};
		}

		public async Task<CommunityManifest> FetchManifestAsync(SongNamespace ns, CancellationToken cancellationToken = default(CancellationToken))
		{
			_ = 1;
			try
			{
				string url = "https://bhm.blishhud.com/Aex.Maestro/" + NamespaceSegment(ns) + "/manifest.json";
				Logger.Info($"Fetching {ns} manifest from {url}");
				HttpResponseMessage obj = await _httpClient.GetAsync(url, cancellationToken);
				obj.EnsureSuccessStatusCode();
				CommunityManifest manifest = JsonConvert.DeserializeObject<CommunityManifest>(await obj.get_Content().ReadAsStringAsync());
				Logger.Info($"Fetched {ns} manifest with {(manifest?.Songs?.Count).GetValueOrDefault()} songs");
				return manifest;
			}
			catch (OperationCanceledException)
			{
				Logger.Debug($"{ns} manifest fetch cancelled");
				throw;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, $"Failed to fetch {ns} manifest");
				throw;
			}
		}

		public async Task<Song> FetchSongAsync(SongNamespace ns, string songId, CancellationToken cancellationToken = default(CancellationToken))
		{
			_ = 1;
			try
			{
				string url = "https://bhm.blishhud.com/Aex.Maestro/" + NamespaceSegment(ns) + "/songs/" + songId + ".json";
				Logger.Info($"Fetching {ns} song {songId}");
				HttpResponseMessage obj = await _httpClient.GetAsync(url, cancellationToken);
				obj.EnsureSuccessStatusCode();
				Song song = SongSerializer.DeserializeJsonContent(await obj.get_Content().ReadAsStringAsync());
				if (song != null)
				{
					if (ns == SongNamespace.Builtin)
					{
						song.BuiltInId = songId;
					}
					else
					{
						song.CommunityId = songId;
					}
				}
				Logger.Info($"Fetched {ns} song: {song?.Name}");
				return song;
			}
			catch (OperationCanceledException)
			{
				Logger.Debug($"{ns} song fetch cancelled for {songId}");
				throw;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, $"Failed to fetch {ns} song {songId}");
				throw;
			}
		}

		public async Task<UploadResponse> UploadSongAsync(Song song, CancellationToken cancellationToken = default(CancellationToken))
		{
			_ = 1;
			try
			{
				string url = "https://maestro-api.uwponcel.workers.dev/api/upload-song";
				Logger.Info("Uploading song " + song.Name + " to community");
				StringContent content = new StringContent(JsonConvert.SerializeObject((object)new
				{
					song = JsonConvert.DeserializeObject(SongSerializer.SerializeToJson(song)),
					transcriber = song.Transcriber,
					clientId = _clientId,
					existingSongId = song.CommunityId,
					durationMs = song.DurationMs
				}), Encoding.UTF8, "application/json");
				HttpRequestMessage val = new HttpRequestMessage(HttpMethod.get_Post(), url);
				val.set_Content((HttpContent)(object)content);
				HttpRequestMessage request = val;
				HttpResponseMessage response = await ((HttpMessageInvoker)_httpClient).SendAsync(request, cancellationToken);
				UploadResponse uploadResponse = JsonConvert.DeserializeObject<UploadResponse>(await response.get_Content().ReadAsStringAsync());
				if (!response.get_IsSuccessStatusCode() && uploadResponse != null && string.IsNullOrEmpty(uploadResponse.Error))
				{
					uploadResponse.Error = $"Upload failed with status {(int)response.get_StatusCode()}";
				}
				Logger.Info("Upload response: " + ((uploadResponse != null && uploadResponse.Success) ? "Success" : "Failed"));
				return uploadResponse ?? new UploadResponse
				{
					Success = false,
					Error = "Invalid response from server"
				};
			}
			catch (OperationCanceledException)
			{
				Logger.Debug("Song upload cancelled");
				throw;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to upload song " + song.Name);
				return new UploadResponse
				{
					Success = false,
					Error = ex.Message
				};
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
