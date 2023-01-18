using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Ideka.RacingMeter
{
	public class MapData : IDisposable
	{
		private class CacheFile
		{
			public int BuildId { get; set; }

			public Dictionary<int, Map> Maps { get; set; } = new Dictionary<int, Map>();

		}

		private static readonly Logger Logger = Logger.GetLogger<MapData>();

		private const int Retries = 3;

		private readonly Dictionary<int, Map> _maps = new Dictionary<int, Map>();

		private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

		public Map? Current { get; private set; }

		public MapData(string cacheFilePath)
		{
			CacheFile cache = null;
			if (File.Exists(cacheFilePath))
			{
				Logger.Info("Found cache file, loading.");
				try
				{
					cache = System.Text.Json.JsonSerializer.Deserialize<CacheFile>(File.ReadAllText(cacheFilePath));
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
			LoadMapData(cache?.BuildId ?? 0, cacheFilePath, _cancellation.Token);
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
				return (_maps == null) ? null : (_maps.TryGetValue(id, out map) ? map : null);
			}
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
			_cancellation?.Cancel();
		}
	}
}
