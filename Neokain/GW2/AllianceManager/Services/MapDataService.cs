using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Maps;

namespace Neokain.GW2.AllianceManager.Services
{
	public class MapDataService : IMapDataService
	{
		private readonly Gw2WebClient _webClient;

		private readonly Dictionary<int, Gw2MapDto> _mapCache = new Dictionary<int, Gw2MapDto>();

		private readonly object _cacheLock = new object();

		public MapDataService(Gw2WebClient webClient)
		{
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
		}

		public async Task<Gw2MapDto> GetMapAsync(int mapId)
		{
			lock (_cacheLock)
			{
				if (_mapCache.TryGetValue(mapId, out var cached))
				{
					return cached;
				}
			}
			try
			{
				Gw2MapDto map = await _webClient.GetMap(mapId);
				if (map != null)
				{
					lock (_cacheLock)
					{
						_mapCache[mapId] = map;
					}
				}
				return map;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<string> GetMapNameAsync(int mapId)
		{
			lock (_cacheLock)
			{
				if (_mapCache.TryGetValue(mapId, out var cached))
				{
					return cached.Name;
				}
			}
			try
			{
				return (await _webClient.GetMapName(mapId)) ?? $"Unknown Map ({mapId})";
			}
			catch (Exception)
			{
				return $"Map {mapId}";
			}
		}

		public async Task PreloadMapsAsync(IEnumerable<int> mapIds)
		{
			if (mapIds == null)
			{
				return;
			}
			List<int> idsToFetch = new List<int>();
			lock (_cacheLock)
			{
				foreach (int mapId in mapIds)
				{
					if (!_mapCache.ContainsKey(mapId))
					{
						idsToFetch.Add(mapId);
					}
				}
			}
			if (idsToFetch.Count == 0)
			{
				return;
			}
			try
			{
				await _webClient.EnsureMapsExist(idsToFetch);
				List<Gw2MapDto> maps = await _webClient.GetMaps(idsToFetch);
				lock (_cacheLock)
				{
					foreach (Gw2MapDto map in maps)
					{
						_mapCache[map.Id] = map;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public void ClearCache()
		{
			lock (_cacheLock)
			{
				_mapCache.Clear();
			}
		}
	}
}
