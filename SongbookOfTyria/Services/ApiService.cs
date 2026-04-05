using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;
using SongbookOfTyria.Models;
using SongbookOfTyria.Models.Api;

namespace SongbookOfTyria.Services
{
	public sealed class ApiService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<ApiService>();

		private const string BaseUrl = "https://gw2opus.com/wp-json/blishhud/v1/";

		private const string ApiKeyHeader = "X-GW2-API-Key";

		private const int TimeoutSeconds = 30;

		private readonly HttpClient _httpClient;

		private readonly GuildAuthService _guildAuthService;

		public ApiService(GuildAuthService guildAuthService = null)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromSeconds(30.0));
			_httpClient = val;
			_guildAuthService = guildAuthService;
		}

		private HttpRequestMessage CreateRequest(HttpMethod method, string url)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			HttpRequestMessage request = new HttpRequestMessage(method, url);
			if (_guildAuthService != null && _guildAuthService.IsOpusMember && !string.IsNullOrEmpty(_guildAuthService.CurrentApiKey))
			{
				((HttpHeaders)request.get_Headers()).Add("X-GW2-API-Key", _guildAuthService.CurrentApiKey);
			}
			return request;
		}

		public async Task<TabsResponse> GetTabsAsync(string type = "all", string beginner = "all", string search = null)
		{
			string url = "https://gw2opus.com/wp-json/blishhud/v1/albums?type=" + type + "&beginner=" + beginner;
			if (!string.IsNullOrEmpty(search))
			{
				url = url + "&search=" + Uri.EscapeDataString(search);
			}
			try
			{
				HttpRequestMessage request = CreateRequest(HttpMethod.get_Get(), url);
				try
				{
					HttpResponseMessage httpResponse = await _httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
					if (!httpResponse.get_IsSuccessStatusCode())
					{
						Logger.Warn("GetTabsAsync: HTTP request failed with status code {StatusCode}", new object[1] { httpResponse.get_StatusCode() });
						return null;
					}
					string response = await httpResponse.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
					if (string.IsNullOrEmpty(response))
					{
						Logger.Warn("GetTabsAsync: Response was null or empty");
						return null;
					}
					TabsResponse result = JsonConvert.DeserializeObject<TabsResponse>(response);
					if (result?.Tabs != null)
					{
						int privateCount = result.Tabs.Count((MusicTab t) => t.IsPrivate);
						int publicCount = result.Tabs.Count((MusicTab t) => !t.IsPrivate);
						Logger.Debug("GetTabsAsync: Loaded {Total} tabs (Public: {PublicCount}, Private: {PrivateCount})", new object[3]
						{
							result.Tabs.Count,
							publicCount,
							privateCount
						});
						bool hasApiKey = _guildAuthService != null && _guildAuthService.IsOpusMember && !string.IsNullOrEmpty(_guildAuthService.CurrentApiKey);
						Logger.Debug("GetTabsAsync: Request sent with API key: {HasApiKey}", new object[1] { hasApiKey });
					}
					return result;
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			}
			catch (HttpRequestException val)
			{
				HttpRequestException ex4 = val;
				Logger.Warn((Exception)(object)ex4, "GetTabsAsync: HTTP request failed");
				return null;
			}
			catch (TaskCanceledException ex3)
			{
				Logger.Warn((Exception)ex3, "GetTabsAsync: Request timed out");
				return null;
			}
			catch (JsonException val2)
			{
				JsonException ex2 = val2;
				Logger.Warn((Exception)(object)ex2, "GetTabsAsync: JSON deserialization failed");
				return null;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "GetTabsAsync: Unexpected error");
				return null;
			}
		}

		public async Task<MusicTab> GetTabByUrlAsync(string apiUrl, bool includeSongs = true)
		{
			if (string.IsNullOrEmpty(apiUrl))
			{
				Logger.Warn("GetTabByUrlAsync: apiUrl was null or empty");
				return null;
			}
			string url = apiUrl;
			if (!url.Contains("include_songs"))
			{
				url = url + (url.Contains("?") ? "&" : "?") + "include_songs=" + includeSongs.ToString().ToLower();
			}
			try
			{
				HttpRequestMessage request = CreateRequest(HttpMethod.get_Get(), url);
				try
				{
					HttpResponseMessage httpResponse = await _httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
					if (!httpResponse.get_IsSuccessStatusCode())
					{
						Logger.Warn("GetTabByUrlAsync: HTTP request failed with status code {StatusCode}", new object[1] { httpResponse.get_StatusCode() });
						return null;
					}
					return JsonConvert.DeserializeObject<MusicTab>(await httpResponse.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
				}
				finally
				{
					((IDisposable)request)?.Dispose();
				}
			}
			catch (HttpRequestException val)
			{
				HttpRequestException ex2 = val;
				Logger.Warn((Exception)(object)ex2, "GetTabByUrlAsync: HTTP request failed");
				return null;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "GetTabByUrlAsync: Failed to fetch tab from {ApiUrl}", new object[1] { apiUrl });
				return null;
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
