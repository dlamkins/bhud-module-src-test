using System;
using System.Collections.Generic;
using System.Net;
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

		private static readonly bool DEBUG_LOGS = false;

		private static readonly Logger Logger = Logger.GetLogger<Gw2MapDetailsService>();

		private const string CACHE_VER = "v5";

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

		private async Task<PoiWpFloorResult> TryContinentFloorsAsync(int continentId, int defaultFloorId, int[] preferredFloors, int mapId, int regionIdHint, CancellationToken ct)
		{
			List<int> floorsToTry = new List<int>();
			AddUniqueFloors(floorsToTry, preferredFloors);
			if (defaultFloorId >= 0)
			{
				AddUniqueInt(floorsToTry, defaultFloorId);
			}
			AddUniqueInt(floorsToTry, 1);
			AddUniqueInt(floorsToTry, 0);
			if (floorsToTry.Count < 6)
			{
				List<int> discovered = await GetContinentFloorsAsync(continentId, ct).ConfigureAwait(continueOnCapturedContext: false);
				for (int i = 0; i < discovered.Count; i++)
				{
					if (floorsToTry.Count >= 6)
					{
						break;
					}
					int f = discovered[i];
					if (f <= 3 || f < 0)
					{
						AddUniqueInt(floorsToTry, f);
					}
				}
			}
			if (DEBUG_LOGS)
			{
				Logger.Debug(string.Format("[MapDetails] mapId={0} trying continent={1} floors=[{2}] regionIdHint={3}", mapId, continentId, string.Join(",", floorsToTry), regionIdHint));
			}
			for (int fi2 = 0; fi2 < floorsToTry.Count; fi2++)
			{
				int floor = floorsToTry[fi2];
				ct.ThrowIfCancellationRequested();
				Tuple<List<Tuple<string, int, int>>, List<Tuple<string, int, int>>> data = await GetPoisAndWaypointsByRegionAsync(continentId, floor, mapId, regionIdHint, allowScan: false, ct).ConfigureAwait(continueOnCapturedContext: false);
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
			for (int fi2 = 0; fi2 < floorsToTry.Count; fi2++)
			{
				int floor = floorsToTry[fi2];
				ct.ThrowIfCancellationRequested();
				Tuple<List<Tuple<string, int, int>>, List<Tuple<string, int, int>>> data2 = await GetPoisAndWaypointsByRegionAsync(continentId, floor, mapId, regionIdHint, allowScan: true, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (data2 != null)
				{
					return new PoiWpFloorResult
					{
						UsedFloor = floor,
						Pois = data2.Item1,
						Waypoints = data2.Item2
					};
				}
			}
			return null;
		}

		public async Task<PoiWpFloorResult> GetPoisAndWaypointsWithFloorFallbackAsync(int continentId, int defaultFloorId, int[] preferredFloors, int mapId, int regionId, CancellationToken ct)
		{
			PoiWpFloorResult primary = await TryContinentFloorsAsync(continentId, defaultFloorId, preferredFloors, mapId, regionId, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (primary != null)
			{
				return primary;
			}
			List<int> allContinents = await GetAllContinentsAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			for (int i = 0; i < allContinents.Count; i++)
			{
				int cTry = allContinents[i];
				if (cTry != continentId)
				{
					PoiWpFloorResult fallback = await TryContinentFloorsAsync(cTry, defaultFloorId, preferredFloors, mapId, regionId, ct).ConfigureAwait(continueOnCapturedContext: false);
					if (fallback != null)
					{
						return fallback;
					}
				}
			}
			return new PoiWpFloorResult
			{
				UsedFloor = -1
			};
		}

		private async Task<Tuple<List<Tuple<string, int, int>>, List<Tuple<string, int, int>>>> GetPoisAndWaypointsByRegionAsync(int continentId, int floorId, int mapId, int regionIdFromMapInfo, bool allowScan, CancellationToken ct)
		{
			if (regionIdFromMapInfo > 0)
			{
				string hintedMapDetailsKey = string.Format("mapdetails-c{0}-f{1}-r{2}-m{3}-{4}", continentId, floorId, regionIdFromMapInfo, mapId, "v5");
				if (_cache.TryLoad<MapDetailsCache>(hintedMapDetailsKey, out var hintedCached) && hintedCached != null)
				{
					return Tuple.Create(hintedCached.Pois, hintedCached.Waypoints);
				}
			}
			HttpResponseMessage resp2;
			if (regionIdFromMapInfo > 0)
			{
				string url = $"https://api.guildwars2.com/v2/continents/{continentId}/floors/{floorId}/regions/{regionIdFromMapInfo}/maps/{mapId}";
				if (DEBUG_LOGS)
				{
					Logger.Debug("[MapDetails] HTTP GET " + url);
				}
				resp2 = await Http.GetAsync(url, (HttpCompletionOption)0, ct).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					if (DEBUG_LOGS)
					{
						Logger.Debug($"[MapDetails] HTTP {(int)resp2.get_StatusCode()} {resp2.get_ReasonPhrase()}");
					}
					if (resp2.get_IsSuccessStatusCode())
					{
						return ParseAndCache(continentId, floorId, regionIdFromMapInfo, mapId, await resp2.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
					}
					if (!allowScan)
					{
						return null;
					}
					if (resp2.get_StatusCode() != HttpStatusCode.NotFound)
					{
						return null;
					}
				}
				finally
				{
					((IDisposable)resp2)?.Dispose();
				}
			}
			else if (!allowScan)
			{
				return null;
			}
			string regionCacheKey = string.Format("regionForMap-c{0}-f{1}-m{2}-{3}", continentId, floorId, mapId, "v5");
			if (!_cache.TryLoad<int>(regionCacheKey, out var scannedRegionId) || scannedRegionId == 0)
			{
				scannedRegionId = await FindRegionContainingMapAsync(continentId, floorId, mapId, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (scannedRegionId == 0)
				{
					if (DEBUG_LOGS)
					{
						Logger.Debug($"[MapDetails] mapId={mapId} not found on c={continentId} f={floorId}");
					}
					return null;
				}
				_cache.Save(regionCacheKey, scannedRegionId);
				if (DEBUG_LOGS)
				{
					Logger.Debug($"[MapDetails] resolved regionId={scannedRegionId} for mapId={mapId} on c={continentId} f={floorId}");
				}
			}
			string scannedMapDetailsKey = string.Format("mapdetails-c{0}-f{1}-r{2}-m{3}-{4}", continentId, floorId, scannedRegionId, mapId, "v5");
			if (_cache.TryLoad<MapDetailsCache>(scannedMapDetailsKey, out var scannedCached) && scannedCached != null)
			{
				return Tuple.Create(scannedCached.Pois, scannedCached.Waypoints);
			}
			string retryUrl = $"https://api.guildwars2.com/v2/continents/{continentId}/floors/{floorId}/regions/{scannedRegionId}/maps/{mapId}";
			if (DEBUG_LOGS)
			{
				Logger.Debug("[MapDetails] retry -> " + retryUrl);
			}
			resp2 = await Http.GetAsync(retryUrl, (HttpCompletionOption)0, ct).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!resp2.get_IsSuccessStatusCode())
				{
					return null;
				}
				string retryJson = await resp2.get_Content().ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				return ParseAndCache(continentId, floorId, scannedRegionId, mapId, retryJson);
			}
			finally
			{
				((IDisposable)resp2)?.Dispose();
			}
		}

		private Tuple<List<Tuple<string, int, int>>, List<Tuple<string, int, int>>> ParseAndCache(int continentId, int floorId, int regionId, int mapId, string json)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Invalid comparison between Unknown and I4
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Invalid comparison between Unknown and I4
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Invalid comparison between Unknown and I4
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Invalid comparison between Unknown and I4
			List<Tuple<string, int, int>> pois = new List<Tuple<string, int, int>>();
			List<Tuple<string, int, int>> wps = new List<Tuple<string, int, int>>();
			JsonDocument doc = JsonDocument.Parse(json, default(JsonDocumentOptions));
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
			string cacheKey = string.Format("mapdetails-c{0}-f{1}-r{2}-m{3}-{4}", continentId, floorId, regionId, mapId, "v5");
			_cache.Save(cacheKey, new MapDetailsCache
			{
				Pois = pois,
				Waypoints = wps
			});
			if (DEBUG_LOGS)
			{
				Logger.Debug($"[MapDetails] parsed pois={pois.Count} wps={wps.Count}");
			}
			return Tuple.Create(pois, wps);
		}

		private async Task<int> FindRegionContainingMapAsync(int continentId, int floorId, int mapId, CancellationToken ct)
		{
			string regionsUrl = $"https://api.guildwars2.com/v2/continents/{continentId}/floors/{floorId}/regions";
			if (DEBUG_LOGS)
			{
				Logger.Debug("[MapDetails] HTTP GET " + regionsUrl);
			}
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
