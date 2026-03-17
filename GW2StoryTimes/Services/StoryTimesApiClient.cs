using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using GW2StoryTimes.Models;
using Newtonsoft.Json;

namespace GW2StoryTimes.Services
{
	public class StoryTimesApiClient : IDisposable
	{
		public class SubmitResult
		{
			public bool Success { get; private set; }

			public string Error { get; private set; }

			public static SubmitResult Ok()
			{
				return new SubmitResult
				{
					Success = true
				};
			}

			public static SubmitResult Fail(string error)
			{
				return new SubmitResult
				{
					Success = false,
					Error = error
				};
			}
		}

		private class CachedItem<T>
		{
			public T Value { get; }

			public DateTime CachedAt { get; }

			public CachedItem(T value)
			{
				Value = value;
				CachedAt = DateTime.UtcNow;
			}

			public bool IsExpired(TimeSpan ttl)
			{
				return DateTime.UtcNow - CachedAt > ttl;
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<StoryTimesApiClient>();

		private const string BaseUrl = "https://api.gw2storytimes.com/v1";

		private const string SeasonsCacheFile = "seasons.json";

		private readonly HttpClient _httpClient;

		private readonly ConcurrentDictionary<int, CachedItem<Mission>> _missionCache = new ConcurrentDictionary<int, CachedItem<Mission>>();

		private readonly TimeSpan _cacheTtl = TimeSpan.FromMinutes(15.0);

		private readonly string _cacheDir;

		private List<Season> _seasons;

		public StoryTimesApiClient()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			HttpClient val = new HttpClient();
			val.set_BaseAddress(new Uri("https://api.gw2storytimes.com/v1"));
			val.set_Timeout(TimeSpan.FromSeconds(10.0));
			_httpClient = val;
			((HttpHeaders)_httpClient.get_DefaultRequestHeaders()).Add("User-Agent", "GW2StoryTimes-BlishHUD/0.1.0");
			try
			{
				_cacheDir = GW2StoryTimesModule.Instance?.DirectoriesManager?.GetFullDirectoryPath("storytimes-data");
			}
			catch
			{
				_cacheDir = null;
			}
		}

		public async Task PreloadSeasonsAsync()
		{
			try
			{
				string response = await _httpClient.GetStringAsync("/v1/seasons");
				_seasons = JsonConvert.DeserializeObject<List<Season>>(response);
				Logger.Info($"Loaded {_seasons?.Count ?? 0} seasons from Story Times API.");
				WriteDiskCache("seasons.json", response);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to preload seasons: " + ex.Message);
				_seasons = ReadDiskCache<List<Season>>("seasons.json");
				if (_seasons != null && _seasons.Count > 0)
				{
					Logger.Info($"Loaded {_seasons.Count} seasons from disk cache.");
				}
				else
				{
					_seasons = new List<Season>();
				}
			}
		}

		public List<Season> GetCachedSeasons()
		{
			return _seasons ?? new List<Season>();
		}

		public async Task<Season> GetSeasonAsync(string seasonId)
		{
			try
			{
				return JsonConvert.DeserializeObject<Season>(await _httpClient.GetStringAsync("/v1/seasons/" + seasonId));
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to get season " + seasonId + ": " + ex.Message);
				return null;
			}
		}

		public async Task<Mission> GetMissionAsync(int missionId)
		{
			if (_missionCache.TryGetValue(missionId, out var cached) && !cached.IsExpired(_cacheTtl))
			{
				return cached.Value;
			}
			try
			{
				Mission mission = JsonConvert.DeserializeObject<Mission>(await _httpClient.GetStringAsync($"/v1/missions/{missionId}"));
				_missionCache[missionId] = new CachedItem<Mission>(mission);
				return mission;
			}
			catch (Exception ex)
			{
				Logger.Warn($"Failed to get mission {missionId}: {ex.Message}");
				return null;
			}
		}

		public async Task<SubmitResult> SubmitTimeAsync(int missionId, string category, double durationMins)
		{
			_ = 1;
			try
			{
				StringContent content = new StringContent(JsonConvert.SerializeObject(new
				{
					category = category,
					duration_mins = Math.Round(durationMins, 2),
					source = "blishhud"
				}), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _httpClient.PostAsync($"/v1/missions/{missionId}/submit", (HttpContent)(object)content);
				if (response.get_IsSuccessStatusCode())
				{
					Logger.Info($"Submitted time for mission {missionId}: {durationMins:F1} min ({category})");
					_missionCache.TryRemove(missionId, out var _);
					return SubmitResult.Ok();
				}
				string body = await response.get_Content().ReadAsStringAsync();
				string errorMsg = "Submission failed. Try again later.";
				try
				{
					var parsed = JsonConvert.DeserializeAnonymousType(body, new
					{
						error = ""
					});
					if (!string.IsNullOrEmpty(parsed?.error))
					{
						errorMsg = parsed.error;
					}
				}
				catch
				{
				}
				Logger.Warn($"Submission failed for mission {missionId}: HTTP {(int)response.get_StatusCode()} — {errorMsg}");
				return SubmitResult.Fail(errorMsg);
			}
			catch (Exception ex)
			{
				Logger.Warn($"Submission error for mission {missionId}: {ex.Message}");
				return SubmitResult.Fail("Network error. Check your connection.");
			}
		}

		private void WriteDiskCache(string filename, string json)
		{
			if (_cacheDir != null)
			{
				try
				{
					File.WriteAllText(Path.Combine(_cacheDir, filename), json);
				}
				catch (Exception ex)
				{
					Logger.Warn("Failed to write cache file " + filename + ": " + ex.Message);
				}
			}
		}

		private T ReadDiskCache<T>(string filename) where T : class
		{
			if (_cacheDir == null)
			{
				return null;
			}
			try
			{
				string path = Path.Combine(_cacheDir, filename);
				if (!File.Exists(path))
				{
					return null;
				}
				return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to read cache file " + filename + ": " + ex.Message);
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
