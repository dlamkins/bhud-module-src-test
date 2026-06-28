using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public static class MapDataCache
	{
		private static readonly Dictionary<int, Map> Cache = new Dictionary<int, Map>();

		private static readonly HashSet<int> Loading = new HashSet<int>();

		public static bool TryGet(int mapId, out Map? map)
		{
			return Cache.TryGetValue(mapId, out map);
		}

		public static void Request(int mapId, Action<Map?> onLoaded)
		{
			Action<Map?> onLoaded2 = onLoaded;
			if (Cache.TryGetValue(mapId, out var cached))
			{
				onLoaded2(cached);
			}
			else
			{
				if (!Loading.Add(mapId))
				{
					return;
				}
				Task.Run(async delegate
				{
					Map map = null;
					try
					{
						Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
						if (((gw2ApiManager != null) ? gw2ApiManager.get_Gw2ApiClient() : null) != null)
						{
							map = await ((IBulkExpandableClient<Map, int>)(object)Service.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(mapId, default(CancellationToken));
						}
						if (map != null)
						{
							Cache[mapId] = map;
						}
					}
					catch (Exception ex)
					{
						Logger.GetLogger<Module>().Warn(ex, $"Failed to load map data for map {mapId}");
					}
					finally
					{
						Loading.Remove(mapId);
					}
					onLoaded2(map);
				});
			}
		}
	}
}
