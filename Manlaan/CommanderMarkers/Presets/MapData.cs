using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Manlaan.CommanderMarkers.Presets
{
	public class MapData : IDisposable
	{
		private class CacheFile
		{
			public int BuildId { get; set; }

			public Dictionary<int, Map> Maps { get; set; } = new Dictionary<int, Map>();

		}

		public static string MAPDATA_CACHE_FILENAME = "map_data_cache.json";

		private static readonly Logger Logger = Logger.GetLogger<MapData>();

		private const int Retries = 3;

		private readonly Dictionary<int, Map> _maps = new Dictionary<int, Map>();

		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		public Map? Current { get; private set; }

		public MapData(string cacheFilePath)
		{
			CacheFile cache = null;
			if (File.Exists(cacheFilePath))
			{
				Logger.Info("Found cache file, loading.");
				try
				{
					cache = JsonSerializer.Deserialize<CacheFile>(File.ReadAllText(cacheFilePath), (JsonSerializerOptions)null);
					if (cache == null)
					{
						Logger.Warn("Cache load resulted in null.");
					}
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Exception when loading cache.");
				}
			}
			_maps = cache?.Maps ?? new Dictionary<int, Map>();
			LoadMapData(cache?.BuildId ?? 0, cacheFilePath, _cts.Token);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapChanged);
		}

		public string Describe(int mapId)
		{
			Map? map = GetMap(mapId);
			return ((map != null) ? map!.get_Name() : null) ?? $"({mapId})";
		}

		public Map? GetMap(int id)
		{
			lock (_maps)
			{
				Map map;
				return _maps.TryGetValue(id, out map) ? map : null;
			}
		}

		public Vector2 WorldToScreenMap(Vector3 worldMeters)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return WorldToScreenMap(GameService.Gw2Mumble.get_CurrentMap().get_Id(), worldMeters);
		}

		public Vector2 WorldToScreenMap(int mapId, Vector3 worldMeters)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return WorldToScreenMap(mapId, worldMeters, ScreenMap.Data.MapCenter, ScreenMap.Data.Scale, ScreenMap.Data.MapRotation, ScreenMap.Data.BoundsCenter);
		}

		public Vector2 WorldToScreenMap(int mapId, Vector3 worldMeters, Vector2 mapCenter, float scale, Matrix rotation, Vector2 boundsCenter)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			Map map = GetMap(mapId);
			if (map == null)
			{
				return Vector2.get_Zero();
			}
			return MapToScreenMap(map.WorldMetersToMap(worldMeters), mapCenter, scale, rotation, boundsCenter);
		}

		public static Vector2 MapToScreenMap(Vector2 mapCoords)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return MapToScreenMap(mapCoords, ScreenMap.Data.MapCenter, ScreenMap.Data.Scale, ScreenMap.Data.MapRotation, ScreenMap.Data.BoundsCenter);
		}

		public static Vector2 MapToScreenMap(Vector2 mapCoords, Vector2 mapCenter, float scale, Matrix rotation, Vector2 boundsCenter)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return Vector2.Transform((mapCoords - mapCenter) * scale, rotation) + boundsCenter;
		}

		private async Task LoadMapData(int cachedVersion, string cacheFilePath, CancellationToken ct)
		{
			for (int i = 0; i < 30; i++)
			{
				if (GameService.Gw2Mumble.get_Info().get_BuildId() != 0)
				{
					break;
				}
				Logger.Warn("Waiting for mumble to update map data...");
				await Task.Delay(1000);
			}
			if (GameService.Gw2Mumble.get_Info().get_BuildId() == cachedVersion)
			{
				UpdateCurrent();
				return;
			}
			IEnumerable<Map> maps = null;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					maps = (IEnumerable<Map>)(await ((IAllExpandableClient<Map>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
						.get_Maps()).AllAsync(ct));
				}
				catch (Exception e)
				{
					if (i < 3)
					{
						Logger.Warn(e, "Failed to pull map data from the Gw2 API. Trying again in 30 seconds.");
						await Task.Delay(30000);
					}
					continue;
				}
				break;
			}
			if (maps == null)
			{
				Logger.Warn("Max retries exeeded. Skipping map data update.");
				return;
			}
			lock (_maps)
			{
				foreach (Map map in maps)
				{
					_maps[map.get_Id()] = map;
				}
			}
			Directory.CreateDirectory(Path.GetDirectoryName(cacheFilePath));
			File.WriteAllText(cacheFilePath, JsonConvert.SerializeObject(new CacheFile
			{
				BuildId = ((GameService.Gw2Mumble.get_Info().get_BuildId() != 0) ? GameService.Gw2Mumble.get_Info().get_BuildId() : cachedVersion),
				Maps = _maps
			}));
			UpdateCurrent();
		}

		private void UpdateCurrent()
		{
			lock (_maps)
			{
				Current = (_maps.TryGetValue(GameService.Gw2Mumble.get_CurrentMap().get_Id(), out var map) ? map : null);
			}
		}

		private void CurrentMapChanged(object sender, ValueEventArgs<int> e)
		{
			UpdateCurrent();
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapChanged);
			_cts?.Cancel();
		}
	}
}
