using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

		private readonly HttpClient _httpClient;

		private readonly ConcurrentDictionary<int, CachedItem<Mission>> _missionCache = new ConcurrentDictionary<int, CachedItem<Mission>>();

		private readonly TimeSpan _cacheTtl = TimeSpan.FromMinutes(15.0);

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
		}

		public async Task PreloadSeasonsAsync()
		{
			try
			{
				_seasons = JsonConvert.DeserializeObject<List<Season>>(await _httpClient.GetStringAsync("/v1/seasons"));
				Logger.Info($"Loaded {_seasons?.Count ?? 0} seasons from Story Times API.");
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to preload seasons: " + ex.Message);
				_seasons = new List<Season>();
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

		public async Task<bool> SubmitTimeAsync(int missionId, string category, double durationMins)
		{
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
					return true;
				}
				Logger.Warn($"Submission failed for mission {missionId}: HTTP {(int)response.get_StatusCode()}");
				return false;
			}
			catch (Exception ex)
			{
				Logger.Warn($"Submission error for mission {missionId}: {ex.Message}");
				return false;
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
