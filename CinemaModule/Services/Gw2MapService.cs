using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CinemaModule.Services
{
	public class Gw2MapService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<Gw2MapService>();

		private const string Gw2MapsApiUrl = "https://api.guildwars2.com/v2/maps";

		private const string CacheFileName = "gw2_map_cache.json";

		private const string DefaultMapNamePrefix = "Map";

		private const string UnknownMapName = "Unknown";

		private const int InvalidMapId = 0;

		private readonly HttpClient _httpClient;

		private readonly Dictionary<int, string> _mapNameCache;

		private readonly string _cacheFilePath;

		public Gw2MapService(string cacheDirectory)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (string.IsNullOrWhiteSpace(cacheDirectory))
			{
				throw new ArgumentException("Cache directory cannot be null or empty.", "cacheDirectory");
			}
			_httpClient = new HttpClient();
			_mapNameCache = new Dictionary<int, string>();
			_cacheFilePath = Path.Combine(cacheDirectory, "gw2_map_cache.json");
			LoadCacheFromFile();
		}

		public async Task<string> GetMapNameAsync(int mapId)
		{
			if (mapId <= 0)
			{
				return "Unknown";
			}
			if (_mapNameCache.TryGetValue(mapId, out var cachedName))
			{
				return cachedName;
			}
			return await FetchMapNameFromApiAsync(mapId).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<string> FetchMapNameFromApiAsync(int mapId)
		{
			_ = 1;
			try
			{
				string url = string.Format("{0}/{1}", "https://api.guildwars2.com/v2/maps", mapId);
				HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false);
				if (!response.get_IsSuccessStatusCode())
				{
					Logger.Warn($"Failed to fetch map name for ID {mapId}. Status: {response.get_StatusCode()}");
					return GetFallbackMapName(mapId);
				}
				string mapName = await ParseMapNameFromResponseAsync(response).ConfigureAwait(continueOnCapturedContext: false);
				if (!string.IsNullOrEmpty(mapName))
				{
					_mapNameCache[mapId] = mapName;
					SaveCacheToFile();
					return mapName;
				}
				return GetFallbackMapName(mapId);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, $"Failed to fetch map name for ID {mapId}");
				return GetFallbackMapName(mapId);
			}
		}

		private static async Task<string> ParseMapNameFromResponseAsync(HttpResponseMessage response)
		{
			return ((object)JObject.Parse(await response.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)).get_Item("name"))?.ToString();
		}

		private static string GetFallbackMapName(int mapId)
		{
			return string.Format("{0} {1}", "Map", mapId);
		}

		private void LoadCacheFromFile()
		{
			try
			{
				if (!File.Exists(_cacheFilePath))
				{
					Logger.Info("Map cache file not found, starting with empty cache");
					return;
				}
				string json = File.ReadAllText(_cacheFilePath);
				if (string.IsNullOrWhiteSpace(json))
				{
					Logger.Info("Map cache file is empty, starting with empty cache");
					return;
				}
				Dictionary<int, string> cache = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
				if (cache == null)
				{
					return;
				}
				foreach (KeyValuePair<int, string> entry in cache)
				{
					_mapNameCache[entry.Key] = entry.Value;
				}
				Logger.Info($"Loaded {_mapNameCache.Count} map names from cache");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load map cache file, starting with empty cache");
			}
		}

		private void SaveCacheToFile()
		{
			try
			{
				string directory = Path.GetDirectoryName(_cacheFilePath);
				if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				string json = JsonConvert.SerializeObject((object)_mapNameCache, (Formatting)1);
				File.WriteAllText(_cacheFilePath, json);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to save map cache to file");
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
