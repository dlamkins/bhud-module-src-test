using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Extended;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Nekres.Mumble_Info.Core.Services
{
	internal class ApiService : IDisposable
	{
		private IReadOnlyList<ContinentFloorRegionMap> _regionMaps;

		private IReadOnlyList<ContinentFloorRegionMapSector> _mapSectors;

		private Dictionary<int, string> _raceNames;

		private Dictionary<int, string> _profNames;

		private Dictionary<int, AsyncTexture2D> _profIcons;

		private Dictionary<int, string> _eliteNames;

		private Dictionary<int, AsyncTexture2D> _eliteIcons;

		public Map Map { get; private set; }

		public ContinentFloorRegionMapSector Sector { get; private set; }

		public ContinentFloorRegionMapPoi ClosestWaypoint { get; private set; }

		public ContinentFloorRegionMapPoi ClosestPoi { get; private set; }

		public AsyncTexture2D WaypointIcon { get; private set; }

		public AsyncTexture2D PoiIcon { get; private set; }

		public AsyncTexture2D ProfessionIcon { get; private set; }

		public ApiService()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			ProfessionIcon = new AsyncTexture2D();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
		}

		public async Task Init()
		{
			await RequestMap(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			await RequestProfessions();
			WaypointIcon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(733330);
			PoiIcon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156624);
		}

		public void Update(GameTime gameTime)
		{
			FindClosestPoints();
		}

		private async void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			await Init();
		}

		public string GetRaceName(int raceId)
		{
			string name = default(string);
			if (!(_raceNames?.TryGetValue(raceId, out name) ?? false))
			{
				return string.Empty;
			}
			return name;
		}

		public string GetProfessionName(int professionId)
		{
			string name = default(string);
			if (!(_profNames?.TryGetValue(professionId, out name) ?? false))
			{
				return string.Empty;
			}
			return name;
		}

		public string GetSpecializationName(int specializationId)
		{
			string name = default(string);
			if (!(_eliteNames?.TryGetValue(specializationId, out name) ?? false))
			{
				return string.Empty;
			}
			return name;
		}

		public AsyncTexture2D GetClassIcon(int profession, int elite)
		{
			AsyncTexture2D icon = default(AsyncTexture2D);
			if (!(_eliteIcons?.TryGetValue(elite, out icon) ?? false))
			{
				if (!(_profIcons?.TryGetValue(profession, out icon) ?? false))
				{
					return AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel());
				}
				return icon;
			}
			return icon;
		}

		private async void OnMapChanged(object sender, ValueEventArgs<int> e)
		{
			await RequestMap(e.get_Value());
		}

		private async Task RequestMap(int mapId)
		{
			if (!MumbleInfoModule.Instance.Gw2ApiManager.IsApiAvailable())
			{
				Map = null;
				return;
			}
			Map = await TaskUtil.TryAsync(() => ((IBulkExpandableClient<Map, int>)(object)MumbleInfoModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(mapId, default(CancellationToken)), MumbleInfoModule.Logger);
			if (Map != null)
			{
				_regionMaps = await RequestRegionMap(Map);
				_mapSectors = await RequestMapSectors(Map);
			}
		}

		private async Task RequestProfessions()
		{
			IApiV2ObjectList<Race> races = await TaskUtil.TryAsync(() => ((IAllExpandableClient<Race>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Races()).AllAsync(default(CancellationToken)));
			if (races != null)
			{
				_raceNames = ((IEnumerable<Race>)races).ToDictionary((Race x) => (int)(RaceType)Enum.Parse(typeof(RaceType), x.get_Id(), ignoreCase: true), (Race x) => x.get_Name());
			}
			IApiV2ObjectList<Profession> professions = await TaskUtil.TryAsync(() => ((IAllExpandableClient<Profession>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Professions()).AllAsync(default(CancellationToken)));
			if (professions != null)
			{
				_profNames = ((IEnumerable<Profession>)professions).ToDictionary((Profession x) => (int)(ProfessionType)Enum.Parse(typeof(ProfessionType), x.get_Id(), ignoreCase: true), (Profession x) => x.get_Name());
				_profIcons = ((IEnumerable<Profession>)professions).ToDictionary((Profession x) => (int)(ProfessionType)Enum.Parse(typeof(ProfessionType), x.get_Id(), ignoreCase: true), (Profession x) => GameService.Content.GetRenderServiceTexture(RenderUrl.op_Implicit(x.get_IconBig())));
			}
			IApiV2ObjectList<Specialization> specializations = await TaskUtil.TryAsync(() => ((IAllExpandableClient<Specialization>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Specializations()).AllAsync(default(CancellationToken)));
			if (specializations != null)
			{
				List<Specialization> elites = ((IEnumerable<Specialization>)specializations).Where((Specialization x) => x.get_Elite()).ToList();
				_eliteNames = elites.ToDictionary((Specialization x) => x.get_Id(), (Specialization x) => x.get_Name());
				_eliteIcons = elites.ToDictionary((Specialization x) => x.get_Id(), delegate(Specialization x)
				{
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					ContentService content = GameService.Content;
					RenderUrl? professionIconBig = x.get_ProfessionIconBig();
					return content.GetRenderServiceTexture(professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null);
				});
			}
		}

		private async Task<IReadOnlyList<ContinentFloorRegionMap>> RequestRegionMap(Map map)
		{
			List<ContinentFloorRegionMap> regionMaps = new List<ContinentFloorRegionMap>();
			foreach (int floor in map.get_Floors())
			{
				regionMaps.Add(await TaskUtil.TryAsync(() => ((IBlobClient<ContinentFloorRegionMap>)(object)MumbleInfoModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(map.get_ContinentId())
					.get_Floors()
					.get_Item(floor)
					.get_Regions()
					.get_Item(map.get_RegionId())
					.get_Maps()
					.get_Item(map.get_Id())).GetAsync(default(CancellationToken))));
			}
			return regionMaps;
		}

		private async Task<List<ContinentFloorRegionMapSector>> RequestMapSectors(Map map)
		{
			List<ContinentFloorRegionMapSector> result = new List<ContinentFloorRegionMapSector>();
			foreach (int floor in map.get_Floors())
			{
				IApiV2ObjectList<ContinentFloorRegionMapSector> sectors = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<ContinentFloorRegionMapSector>)(object)MumbleInfoModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(map.get_ContinentId())
					.get_Floors()
					.get_Item(floor)
					.get_Regions()
					.get_Item(map.get_RegionId())
					.get_Maps()
					.get_Item(map.get_Id())
					.get_Sectors()).AllAsync(default(CancellationToken)));
				if (sectors != null && ((IEnumerable<ContinentFloorRegionMapSector>)sectors).Any())
				{
					result.AddRange(((IEnumerable<ContinentFloorRegionMapSector>)sectors).DistinctBy((ContinentFloorRegionMapSector sector) => sector.get_Id()));
				}
			}
			return result;
		}

		private void FindClosestPoints()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Invalid comparison between Unknown and I4
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Invalid comparison between Unknown and I4
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			if (Map == null)
			{
				return;
			}
			List<ContinentFloorRegionMapPoi> pois = _regionMaps?.Where((ContinentFloorRegionMap x) => x != null).SelectMany((ContinentFloorRegionMap x) => x.get_PointsOfInterest().Values.Distinct()).ToList();
			if (!pois.IsNullOrEmpty())
			{
				Coordinates3 continentPosition = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition().ToContinentCoords(CoordsUnit.Meters, Map.get_MapRect(), Map.get_ContinentRect());
				double closestPoiDistance = double.MaxValue;
				double closestWaypointDistance = double.MaxValue;
				ContinentFloorRegionMapPoi closestPoi = null;
				ContinentFloorRegionMapPoi closestWaypoint = null;
				foreach (ContinentFloorRegionMapPoi poi in pois)
				{
					double x2 = ((Coordinates3)(ref continentPosition)).get_X();
					Coordinates2 coord = poi.get_Coord();
					double x3 = Math.Abs(x2 - ((Coordinates2)(ref coord)).get_X());
					double z = ((Coordinates3)(ref continentPosition)).get_Z();
					coord = poi.get_Coord();
					double distanceZ = Math.Abs(z - ((Coordinates2)(ref coord)).get_Y());
					double distance = Math.Sqrt(Math.Pow(x3, 2.0) + Math.Pow(distanceZ, 2.0));
					PoiType value = poi.get_Type().get_Value();
					if ((int)value != 1)
					{
						if ((int)value == 2 && distance < closestWaypointDistance)
						{
							closestWaypointDistance = distance;
							closestWaypoint = poi;
						}
					}
					else if (distance < closestPoiDistance)
					{
						closestPoiDistance = distance;
						closestPoi = poi;
					}
				}
				ClosestWaypoint = closestWaypoint;
				ClosestPoi = closestPoi;
			}
			else
			{
				ClosestWaypoint = null;
				ClosestPoi = null;
			}
			Coordinates2 playerLocation = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition().ToContinentCoords(CoordsUnit.Meters, Map.get_MapRect(), Map.get_ContinentRect())
				.SwapYZ()
				.ToPlane();
			Sector = _mapSectors?.FirstOrDefault((ContinentFloorRegionMapSector sector) => playerLocation.Inside(sector.get_Bounds()));
		}

		public void Dispose()
		{
			AsyncTexture2D professionIcon = ProfessionIcon;
			if (professionIcon != null)
			{
				professionIcon.Dispose();
			}
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
		}
	}
}
