using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Frtal.Wayfinder.Models;
using Frtal.Wayfinder.Util;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.Services
{
	public class MapObjectivesService
	{
		private static readonly Logger Logger = Logger.GetLogger<MapObjectivesService>();

		private readonly Gw2ApiManager _api;

		private int _loadedMapId = -1;

		private bool _loading;

		public IReadOnlyList<CompassTarget> Targets { get; private set; } = new List<CompassTarget>();


		public MapCalibration Calibration { get; private set; }

		public string MapName { get; private set; } = "";


		public int ContinentId { get; private set; } = -1;


		public int Floor { get; private set; }

		public Vector2 ContinentMin { get; private set; }

		public Vector2 ContinentMax { get; private set; }

		public bool HasRect { get; private set; }

		public MapObjectivesService(Gw2ApiManager api)
		{
			_api = api;
		}

		public void Update(int currentMapId)
		{
			if (currentMapId > 0 && currentMapId != _loadedMapId && !_loading)
			{
				_loading = true;
				LoadMapAsync(currentMapId);
			}
		}

		private async Task LoadMapAsync(int mapId)
		{
			_ = 1;
			try
			{
				List<CompassTarget> results = new List<CompassTarget>();
				Map map = await ((IBulkExpandableClient<Map, int>)(object)_api.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(mapId, default(CancellationToken));
				MapName = map.get_Name() ?? "";
				ContinentId = map.get_ContinentId();
				Floor = map.get_DefaultFloor();
				Rectangle val;
				Coordinates2 val2;
				try
				{
					MapObjectivesService mapObjectivesService = this;
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_TopLeft();
					float num = (float)((Coordinates2)(ref val2)).get_X();
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_TopLeft();
					mapObjectivesService.ContinentMin = new Vector2(num, (float)((Coordinates2)(ref val2)).get_Y());
					MapObjectivesService mapObjectivesService2 = this;
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_BottomRight();
					float num2 = (float)((Coordinates2)(ref val2)).get_X();
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_BottomRight();
					mapObjectivesService2.ContinentMax = new Vector2(num2, (float)((Coordinates2)(ref val2)).get_Y());
					HasRect = true;
				}
				catch
				{
					HasRect = false;
				}
				try
				{
					MapObjectivesService mapObjectivesService3 = this;
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_TopLeft();
					double x = ((Coordinates2)(ref val2)).get_X();
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_TopLeft();
					double y = ((Coordinates2)(ref val2)).get_Y();
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_BottomRight();
					double x2 = ((Coordinates2)(ref val2)).get_X();
					val = map.get_ContinentRect();
					val2 = ((Rectangle)(ref val)).get_BottomRight();
					double y2 = ((Coordinates2)(ref val2)).get_Y();
					val = map.get_MapRect();
					val2 = ((Rectangle)(ref val)).get_TopLeft();
					double x3 = ((Coordinates2)(ref val2)).get_X();
					val = map.get_MapRect();
					val2 = ((Rectangle)(ref val)).get_TopLeft();
					double y3 = ((Coordinates2)(ref val2)).get_Y();
					val = map.get_MapRect();
					val2 = ((Rectangle)(ref val)).get_BottomRight();
					double x4 = ((Coordinates2)(ref val2)).get_X();
					val = map.get_MapRect();
					val2 = ((Rectangle)(ref val)).get_BottomRight();
					mapObjectivesService3.Calibration = MapCalibration.Create(x, y, x2, y2, x3, y3, x4, ((Coordinates2)(ref val2)).get_Y());
				}
				catch (Exception ex2)
				{
					Logger.Warn(ex2, "Map calibration failed - distances may be inaccurate.");
				}
				ContinentFloorRegionMap details = await ((IBlobClient<ContinentFloorRegionMap>)(object)_api.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(map.get_ContinentId())
					.get_Floors()
					.get_Item(map.get_DefaultFloor())
					.get_Regions()
					.get_Item(map.get_RegionId())
					.get_Maps()
					.get_Item(map.get_Id())).GetAsync(default(CancellationToken));
				Vector2 pos2 = default(Vector2);
				foreach (ContinentFloorRegionMapPoi poi in details.get_PointsOfInterest().Values)
				{
					TargetKind kind = MapPoiType(((object)poi.get_Type())?.ToString());
					val2 = poi.get_Coord();
					float num3 = (float)((Coordinates2)(ref val2)).get_X();
					val2 = poi.get_Coord();
					((Vector2)(ref pos2))._002Ector(num3, (float)((Coordinates2)(ref val2)).get_Y());
					results.Add(new CompassTarget($"poi:{poi.get_Id()}", poi.get_Name() ?? kind.ToString(), kind, pos2));
				}
				Vector2 pos = default(Vector2);
				foreach (ContinentFloorRegionMapTask task in details.get_Tasks().Values)
				{
					val2 = task.get_Coord();
					float num4 = (float)((Coordinates2)(ref val2)).get_X();
					val2 = task.get_Coord();
					((Vector2)(ref pos))._002Ector(num4, (float)((Coordinates2)(ref val2)).get_Y());
					results.Add(new CompassTarget($"heart:{task.get_Id()}", task.get_Objective() ?? "Heart", TargetKind.Heart, pos));
				}
				Vector2 pos3 = default(Vector2);
				foreach (ContinentFloorRegionMapSkillChallenge skill in details.get_SkillChallenges())
				{
					val2 = skill.get_Coord();
					float num5 = (float)((Coordinates2)(ref val2)).get_X();
					val2 = skill.get_Coord();
					((Vector2)(ref pos3))._002Ector(num5, (float)((Coordinates2)(ref val2)).get_Y());
					string sid = null;
					try
					{
						sid = skill.get_Id();
					}
					catch
					{
					}
					string id = ((!string.IsNullOrEmpty(sid)) ? ("hp:" + sid) : $"skill:{pos3.X},{pos3.Y}");
					results.Add(new CompassTarget(id, "Hero point", TargetKind.SkillPoint, pos3));
				}
				Targets = results;
				_loadedMapId = mapId;
				Logger.Info($"Loaded {results.Count} objectives for map {mapId}.");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, $"Failed to load objectives for map {mapId}.");
			}
			finally
			{
				_loading = false;
			}
		}

		private static TargetKind MapPoiType(string type)
		{
			return type?.ToLowerInvariant() switch
			{
				"waypoint" => TargetKind.Waypoint, 
				"vista" => TargetKind.Vista, 
				"landmark" => TargetKind.PointOfInterest, 
				_ => TargetKind.PointOfInterest, 
			};
		}
	}
}
