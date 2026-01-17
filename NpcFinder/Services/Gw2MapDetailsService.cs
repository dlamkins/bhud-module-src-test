using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using NpcFinder.Models;

namespace NpcFinder.Services
{
	public class Gw2MapDetailsService
	{
		private class MapDetailsCache
		{
			public List<Tuple<string, int, int>> Pois { get; set; } = new List<Tuple<string, int, int>>();


			public List<Tuple<string, int, int>> Waypoints { get; set; } = new List<Tuple<string, int, int>>();

		}

		private static readonly Logger Logger = Logger.GetLogger<Gw2MapDetailsService>();

		private static readonly HttpClient Http = new HttpClient();

		private readonly CacheStore _cache;

		public Gw2MapDetailsService(CacheStore cache)
		{
			_cache = cache;
			try
			{
				if (!((object)Http.get_DefaultRequestHeaders().get_UserAgent()).ToString().Contains("NpcFinder-BlishHUD"))
				{
					Http.get_DefaultRequestHeaders().get_UserAgent().ParseAdd("NpcFinder-BlishHUD");
				}
			}
			catch
			{
			}
		}

		public async Task<PoiWpFloorResult> GetPoisAndWaypointsWithFloorFallbackAsync(int continentId, int defaultFloorId, int[] preferredFloors, int mapId, CancellationToken ct)
		{
			List<int> continentsToTry = new List<int>();
			AddUniqueInt(continentsToTry, continentId);
			List<int> allContinents = await GetAllContinentsAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			for (int j = 0; j < allContinents.Count; j++)
			{
				AddUniqueInt(continentsToTry, allContinents[j]);
			}
			for (int ci = 0; ci < continentsToTry.Count; ci++)
			{
				int cTry = continentsToTry[ci];
				List<int> floorsToTry = new List<int>();
				AddUniqueFloors(floorsToTry, preferredFloors);
				if (defaultFloorId >= 0)
				{
					AddUniqueInt(floorsToTry, defaultFloorId);
				}
				AddUniqueInt(floorsToTry, 1);
				AddUniqueInt(floorsToTry, 0);
				if (floorsToTry.Count < 8)
				{
					List<int> discovered = await GetContinentFloorsAsync(cTry, ct).ConfigureAwait(continueOnCapturedContext: false);
					for (int i = 0; i < discovered.Count; i++)
					{
						if (floorsToTry.Count >= 10)
						{
							break;
						}
						AddUniqueInt(floorsToTry, discovered[i]);
					}
				}
				Logger.Debug(string.Format("[MapDetails] mapId={0} trying continent={1} floors=[{2}]", mapId, cTry, string.Join(",", floorsToTry)));
				for (int fi = 0; fi < floorsToTry.Count; fi++)
				{
					int floor = floorsToTry[fi];
					try
					{
						Tuple<List<Tuple<string, int, int>>, List<Tuple<string, int, int>>> data = await GetPoisAndWaypointsByRegionAsync(cTry, floor, mapId, ct).ConfigureAwait(continueOnCapturedContext: false);
						if (data != null)
						{
							return new PoiWpFloorResult
							{
								UsedFloor = floor,
								Pois = data.Item1,
								Waypoints = data.Item2
							};
						}
					}
					catch (Exception ex)
					{
						Logger.Warn($"[MapDetails] mapId={mapId} continent={cTry} floor={floor} failed: {ex.Message}");
					}
				}
			}
			return new PoiWpFloorResult
			{
				UsedFloor = -1
			};
		}

		private async Task<Tuple<List<Tuple<string, int, int>>, List<Tuple<string, int, int>>>> GetPoisAndWaypointsByRegionAsync(int continentId, int floorId, int mapId, CancellationToken ct)
		{
			string regionCacheKey = $"regionForMap-c{continentId}-f{floorId}-m{mapId}-v2";
			if (!_cache.TryLoad<int>(regionCacheKey, out var regionId) || regionId == 0)
			{
				regionId = await FindRegionContainingMapAsync(continentId, floorId, mapId, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (regionId == 0)
				{
					Logger.Debug($"[MapDetails] mapId={mapId} not found on c={continentId} f={floorId}");
					return null;
				}
				_cache.Save(regionCacheKey, regionId);
				Logger.Debug($"[MapDetails] resolved regionId={regionId} for mapId={mapId} on c={continentId} f={floorId}");
			}
			string cacheKey = $"mapdetails-c{continentId}-f{floorId}-r{regionId}-m{mapId}-v2";
			if (_cache.TryLoad<MapDetailsCache>(cacheKey, out var cached) && cached != null)
			{
				return Tuple.Create(cached.Pois, cached.Waypoints);
			}
			string url = $"https://api.guildwars2.com/v2/continents/{continentId}/floors/{floorId}/regions/{regionId}/maps/{mapId}";
			Logger.Debug("[MapDetails] HTTP GET " + url);
			HttpResponseMessage resp = await Http.GetAsync(url, (HttpCompletionOption)0, ct).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Logger.Debug($"[MapDetails] HTTP {(int)resp.get_StatusCode()} {resp.get_ReasonPhrase()}");
				if (!resp.get_IsSuccessStatusCode())
				{
					return null;
				}
				string obj = await resp.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<Tuple<string, int, int>> pois = new List<Tuple<string, int, int>>();
				List<Tuple<string, int, int>> wps = new List<Tuple<string, int, int>>();
				JsonDocument doc = JsonDocument.Parse(obj, default(JsonDocumentOptions));
				try
				{
					JsonElement root = doc.get_RootElement();
					JsonElement poiObj = default(JsonElement);
					if (((JsonElement)(ref root)).TryGetProperty("points_of_interest", ref poiObj) && (int)((JsonElement)(ref poiObj)).get_ValueKind() == 1)
					{
						ObjectEnumerator val = ((JsonElement)(ref poiObj)).EnumerateObject();
						ObjectEnumerator enumerator = ((ObjectEnumerator)(ref val)).GetEnumerator();
						try
						{
							JsonElement i = default(JsonElement);
							JsonElement coord = default(JsonElement);
							JsonElement t = default(JsonElement);
							while (((ObjectEnumerator)(ref enumerator)).MoveNext())
							{
								JsonProperty prop = ((ObjectEnumerator)(ref enumerator)).get_Current();
								JsonElement poi = ((JsonProperty)(ref prop)).get_Value();
								string name = "";
								if (((JsonElement)(ref poi)).TryGetProperty("name", ref i) && (int)((JsonElement)(ref i)).get_ValueKind() == 3)
								{
									name = ((JsonElement)(ref i)).GetString() ?? "";
								}
								if (((JsonElement)(ref poi)).TryGetProperty("coord", ref coord) && (int)((JsonElement)(ref coord)).get_ValueKind() == 2 && ((JsonElement)(ref coord)).GetArrayLength() == 2)
								{
									JsonElement val2 = ((JsonElement)(ref coord)).get_Item(0);
									int x = (int)((JsonElement)(ref val2)).GetDouble();
									val2 = ((JsonElement)(ref coord)).get_Item(1);
									int y = (int)((JsonElement)(ref val2)).GetDouble();
									string type = "";
									if (((JsonElement)(ref poi)).TryGetProperty("type", ref t) && (int)((JsonElement)(ref t)).get_ValueKind() == 3)
									{
										type = ((JsonElement)(ref t)).GetString() ?? "";
									}
									if (string.Equals(type, "waypoint", StringComparison.OrdinalIgnoreCase))
									{
										wps.Add(Tuple.Create(name, x, y));
									}
									else
									{
										pois.Add(Tuple.Create(name, x, y));
									}
								}
							}
						}
						finally
						{
							((IDisposable)(ObjectEnumerator)(ref enumerator)).Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)doc)?.Dispose();
				}
				_cache.Save(cacheKey, new MapDetailsCache
				{
					Pois = pois,
					Waypoints = wps
				});
				Logger.Debug($"[MapDetails] parsed pois={pois.Count} wps={wps.Count}");
				return Tuple.Create(pois, wps);
			}
			finally
			{
				((IDisposable)resp)?.Dispose();
			}
		}

		private async Task<int> FindRegionContainingMapAsync(int continentId, int floorId, int mapId, CancellationToken ct)
		{
			string regionsUrl = $"https://api.guildwars2.com/v2/continents/{continentId}/floors/{floorId}/regions";
			Logger.Debug("[MapDetails] HTTP GET " + regionsUrl);
			HttpResponseMessage resp = await Http.GetAsync(regionsUrl, (HttpCompletionOption)0, ct).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!resp.get_IsSuccessStatusCode())
				{
					return 0;
				}
				string obj = await resp.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<int> regionIds = new List<int>();
				JsonDocument doc = JsonDocument.Parse(obj, default(JsonDocumentOptions));
				try
				{
					JsonElement rootElement = doc.get_RootElement();
					if ((int)((JsonElement)(ref rootElement)).get_ValueKind() == 2)
					{
						rootElement = doc.get_RootElement();
						ArrayEnumerator val = ((JsonElement)(ref rootElement)).EnumerateArray();
						ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
						try
						{
							int rid2 = default(int);
							while (((ArrayEnumerator)(ref enumerator)).MoveNext())
							{
								JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
								if ((int)((JsonElement)(ref el)).get_ValueKind() == 4 && ((JsonElement)(ref el)).TryGetInt32(ref rid2))
								{
									regionIds.Add(rid2);
								}
							}
						}
						finally
						{
							((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)doc)?.Dispose();
				}
				for (int i = 0; i < regionIds.Count; i++)
				{
					int rid = regionIds[i];
					string testUrl = $"https://api.guildwars2.com/v2/continents/{continentId}/floors/{floorId}/regions/{rid}/maps/{mapId}";
					HttpResponseMessage testResp = await Http.GetAsync(testUrl, (HttpCompletionOption)0, ct).ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						if (testResp.get_IsSuccessStatusCode())
						{
							return rid;
						}
					}
					finally
					{
						((IDisposable)testResp)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)resp)?.Dispose();
			}
			return 0;
		}

		private async Task<List<int>> GetAllContinentsAsync(CancellationToken ct)
		{
			string cacheKey = "continents-all-v1";
			if (_cache.TryLoad<List<int>>(cacheKey, out var cached) && cached != null && cached.Count > 0)
			{
				return cached;
			}
			string url = "https://api.guildwars2.com/v2/continents";
			HttpResponseMessage resp = await Http.GetAsync(url, (HttpCompletionOption)0, ct).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!resp.get_IsSuccessStatusCode())
				{
					return new List<int>();
				}
				string obj = await resp.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<int> ids = new List<int>();
				JsonDocument doc = JsonDocument.Parse(obj, default(JsonDocumentOptions));
				try
				{
					JsonElement rootElement = doc.get_RootElement();
					if ((int)((JsonElement)(ref rootElement)).get_ValueKind() == 2)
					{
						rootElement = doc.get_RootElement();
						ArrayEnumerator val = ((JsonElement)(ref rootElement)).EnumerateArray();
						ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
						try
						{
							int id = default(int);
							while (((ArrayEnumerator)(ref enumerator)).MoveNext())
							{
								JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
								if ((int)((JsonElement)(ref el)).get_ValueKind() == 4 && ((JsonElement)(ref el)).TryGetInt32(ref id))
								{
									ids.Add(id);
								}
							}
						}
						finally
						{
							((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)doc)?.Dispose();
				}
				_cache.Save(cacheKey, ids);
				return ids;
			}
			finally
			{
				((IDisposable)resp)?.Dispose();
			}
		}

		private async Task<List<int>> GetContinentFloorsAsync(int continentId, CancellationToken ct)
		{
			string cacheKey = "continentfloors-" + continentId + "-v2";
			if (_cache.TryLoad<List<int>>(cacheKey, out var cached) && cached != null && cached.Count > 0)
			{
				return cached;
			}
			string url = $"https://api.guildwars2.com/v2/continents/{continentId}/floors";
			HttpResponseMessage resp = await Http.GetAsync(url, (HttpCompletionOption)0, ct).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!resp.get_IsSuccessStatusCode())
				{
					return new List<int>();
				}
				string obj = await resp.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<int> floors = new List<int>();
				JsonDocument doc = JsonDocument.Parse(obj, default(JsonDocumentOptions));
				try
				{
					JsonElement rootElement = doc.get_RootElement();
					if ((int)((JsonElement)(ref rootElement)).get_ValueKind() == 2)
					{
						rootElement = doc.get_RootElement();
						ArrayEnumerator val = ((JsonElement)(ref rootElement)).EnumerateArray();
						ArrayEnumerator enumerator = ((ArrayEnumerator)(ref val)).GetEnumerator();
						try
						{
							int f = default(int);
							while (((ArrayEnumerator)(ref enumerator)).MoveNext())
							{
								JsonElement el = ((ArrayEnumerator)(ref enumerator)).get_Current();
								if ((int)((JsonElement)(ref el)).get_ValueKind() == 4 && ((JsonElement)(ref el)).TryGetInt32(ref f))
								{
									floors.Add(f);
								}
							}
						}
						finally
						{
							((IDisposable)(ArrayEnumerator)(ref enumerator)).Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)doc)?.Dispose();
				}
				_cache.Save(cacheKey, floors);
				return floors;
			}
			finally
			{
				((IDisposable)resp)?.Dispose();
			}
		}

		private static void AddUniqueFloors(List<int> list, int[] floors)
		{
			if (floors != null)
			{
				for (int i = 0; i < floors.Length; i++)
				{
					AddUniqueInt(list, floors[i]);
				}
			}
		}

		private static void AddUniqueInt(List<int> list, int value)
		{
			if (list != null && !list.Contains(value))
			{
				list.Add(value);
			}
		}
	}
}
