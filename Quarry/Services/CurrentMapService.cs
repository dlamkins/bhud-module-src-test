using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Quarry.Interfaces;
using Quarry.Models;

namespace Quarry.Services
{
	public class CurrentMapService : ICurrentMapService, IDisposable
	{
		private const double InchesPerMetre = 39.3701;

		private readonly Gw2ApiManager gw2ApiManager;

		private readonly Logger logger;

		private readonly ConcurrentDictionary<int, Map> mapCache = new ConcurrentDictionary<int, Map>();

		private readonly ConcurrentDictionary<int, IReadOnlyList<MapWaypoint>> waypointCache = new ConcurrentDictionary<int, IReadOnlyList<MapWaypoint>>();

		private readonly ConcurrentDictionary<int, IReadOnlyList<MapSector>> sectorCache = new ConcurrentDictionary<int, IReadOnlyList<MapSector>>();

		private readonly ConcurrentDictionary<int, bool> floorDataFetchStarted = new ConcurrentDictionary<int, bool>();

		private readonly EventHandler<ValueEventArgs<int>> mapChangedHandler;

		public int MapId { get; private set; } = -1;


		public string MapName { get; private set; }

		public event Action Changed;

		public CurrentMapService(Gw2ApiManager gw2ApiManager, Logger logger)
		{
			this.gw2ApiManager = gw2ApiManager;
			this.logger = logger;
			mapChangedHandler = delegate(object s, ValueEventArgs<int> e)
			{
				UpdateMap(e.get_Value());
			};
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged(mapChangedHandler);
			if (GameService.Gw2Mumble.get_IsAvailable())
			{
				UpdateMap(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			}
		}

		public bool TryGetWaypoints(int mapId, out IReadOnlyList<MapWaypoint> waypoints)
		{
			if (waypointCache.TryGetValue(mapId, out waypoints))
			{
				return waypoints.Count > 0;
			}
			return false;
		}

		public bool TryGetSectors(int mapId, out IReadOnlyList<MapSector> sectors)
		{
			if (sectorCache.TryGetValue(mapId, out sectors))
			{
				return sectors.Count > 0;
			}
			return false;
		}

		public bool TryGetMapName(int mapId, out string name)
		{
			if (mapCache.TryGetValue(mapId, out var map) && map != null)
			{
				name = map.get_Name();
				return true;
			}
			name = null;
			return false;
		}

		private async Task EnsureFloorDataAsync(int mapId)
		{
			if (waypointCache.ContainsKey(mapId) || !floorDataFetchStarted.TryAdd(mapId, value: true) || !mapCache.TryGetValue(mapId, out var map) || map == null)
			{
				return;
			}
			List<int> floorsToTry = new List<int> { map.get_DefaultFloor() };
			foreach (int floor in map.get_Floors())
			{
				if (!floorsToTry.Contains(floor))
				{
					floorsToTry.Add(floor);
				}
			}
			Exception lastException = null;
			foreach (int floor2 in floorsToTry)
			{
				try
				{
					ContinentFloorRegionMap floorMap = await ((IBulkExpandableClient<ContinentFloorRegionMap, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
						.get_Item(map.get_ContinentId())
						.get_Floors()
						.get_Item(floor2)
						.get_Regions()
						.get_Item(map.get_RegionId())
						.get_Maps()).GetAsync(mapId, default(CancellationToken));
					List<MapWaypoint> waypoints = new List<MapWaypoint>();
					Coordinates2 coord;
					foreach (ContinentFloorRegionMapPoi poi in floorMap.get_PointsOfInterest().Values)
					{
						if (!(poi.get_Type() != ApiEnum<PoiType>.op_Implicit((PoiType)2)) && !string.IsNullOrEmpty(poi.get_ChatLink()))
						{
							CurrentMapService currentMapService = this;
							coord = poi.get_Coord();
							double x = ((Coordinates2)(ref coord)).get_X();
							coord = poi.get_Coord();
							if (currentMapService.TryContinentToWorld(mapId, x, ((Coordinates2)(ref coord)).get_Y(), out var world2))
							{
								waypoints.Add(new MapWaypoint
								{
									Name = poi.get_Name(),
									ChatLink = poi.get_ChatLink(),
									World = world2
								});
							}
						}
					}
					List<MapSector> sectors = new List<MapSector>();
					foreach (ContinentFloorRegionMapSector sector in floorMap.get_Sectors().Values)
					{
						if (!string.IsNullOrEmpty(sector.get_Name()))
						{
							CurrentMapService currentMapService2 = this;
							coord = sector.get_Coord();
							double x2 = ((Coordinates2)(ref coord)).get_X();
							coord = sector.get_Coord();
							if (currentMapService2.TryContinentToWorld(mapId, x2, ((Coordinates2)(ref coord)).get_Y(), out var world))
							{
								sectors.Add(new MapSector
								{
									Name = sector.get_Name(),
									World = world
								});
							}
						}
					}
					waypointCache[mapId] = waypoints;
					sectorCache[mapId] = sectors;
					if (floor2 != map.get_DefaultFloor())
					{
						logger.Debug($"Map {mapId} 404'd on its default floor {map.get_DefaultFloor()}; floor {floor2} answered instead.");
					}
					logger.Debug(string.Format("Waypoints for map {0}: {1}, sectors: {2} — {3}", mapId, waypoints.Count, sectors.Count, string.Join(", ", waypoints.Select((MapWaypoint w) => $"{w.Name} ({w.World.X:F0},{w.World.Y:F0})"))));
					return;
				}
				catch (Exception ex)
				{
					lastException = ex;
				}
			}
			floorDataFetchStarted.TryRemove(mapId, out var _);
			logger.Warn(lastException, $"Couldn't fetch waypoints/sectors for map {mapId} on any of its {floorsToTry.Count} floor(s); the copy icon will use marker-pack codes instead.");
		}

		public bool TryContinentToWorld(int mapId, double continentX, double continentY, out Vector2 world)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			world = default(Vector2);
			if (!mapCache.TryGetValue(mapId, out var map) || map == null)
			{
				return false;
			}
			Rectangle continentRect = map.get_ContinentRect();
			Rectangle mapRect = map.get_MapRect();
			Coordinates2 val = ((Rectangle)(ref continentRect)).get_TopLeft();
			double x = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref continentRect)).get_BottomRight();
			double continentLeft = Math.Min(x, ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref continentRect)).get_TopLeft();
			double x2 = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref continentRect)).get_BottomRight();
			double continentRight = Math.Max(x2, ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref continentRect)).get_TopLeft();
			double y = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref continentRect)).get_BottomRight();
			double continentTop = Math.Min(y, ((Coordinates2)(ref val)).get_Y());
			val = ((Rectangle)(ref continentRect)).get_TopLeft();
			double y2 = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref continentRect)).get_BottomRight();
			double continentBottom = Math.Max(y2, ((Coordinates2)(ref val)).get_Y());
			if (continentX < continentLeft || continentX > continentRight || continentY < continentTop || continentY > continentBottom)
			{
				return false;
			}
			double continentWidth = continentRight - continentLeft;
			double continentHeight = continentBottom - continentTop;
			if (continentWidth <= 0.0 || continentHeight <= 0.0)
			{
				return false;
			}
			val = ((Rectangle)(ref mapRect)).get_TopLeft();
			double x3 = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref mapRect)).get_BottomRight();
			double mapLeft = Math.Min(x3, ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref mapRect)).get_TopLeft();
			double x4 = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref mapRect)).get_BottomRight();
			double mapRight = Math.Max(x4, ((Coordinates2)(ref val)).get_X());
			val = ((Rectangle)(ref mapRect)).get_TopLeft();
			double y3 = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref mapRect)).get_BottomRight();
			double mapBottom = Math.Min(y3, ((Coordinates2)(ref val)).get_Y());
			val = ((Rectangle)(ref mapRect)).get_TopLeft();
			double y4 = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref mapRect)).get_BottomRight();
			double mapTop = Math.Max(y4, ((Coordinates2)(ref val)).get_Y());
			double mapX = mapLeft + (continentX - continentLeft) / continentWidth * (mapRight - mapLeft);
			double mapY = mapBottom + (continentBottom - continentY) / continentHeight * (mapTop - mapBottom);
			world = new Vector2((float)(mapX / 39.3701), (float)(mapY / 39.3701));
			return true;
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged(mapChangedHandler);
		}

		private async Task UpdateMap(int mapId)
		{
			MapId = mapId;
			if (!mapCache.TryGetValue(mapId, out var cachedMap))
			{
				string mapName;
				try
				{
					Map map = await ((IBulkExpandableClient<Map, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(mapId, default(CancellationToken));
					mapName = map.get_Name();
					mapCache[mapId] = map;
				}
				catch (Exception ex)
				{
					logger.Warn(ex, $"Failed to resolve map data for map id {mapId}");
					mapName = null;
				}
				if (MapId != mapId)
				{
					return;
				}
				MapName = mapName;
			}
			else
			{
				MapName = cachedMap.get_Name();
			}
			EnsureFloorDataAsync(mapId);
			logger.Info($"Current map changed: Id={MapId} Name={MapName}");
			this.Changed?.Invoke();
		}
	}
}
