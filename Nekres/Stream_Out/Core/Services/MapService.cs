using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class MapService : ExportService
	{
		private const string MAP_TYPE = "map_type.txt";

		private const string MAP_NAME = "map_name.txt";

		private Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		public MapService()
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			OnMapChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
		}

		private async Task<IEnumerable<ContinentFloorRegionMapSector>> RequestSectors(int continentId, int floor, int regionId, int mapId)
		{
			return await ((IAllExpandableClient<ContinentFloorRegionMapSector>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Continents()
				.get_Item(continentId)
				.get_Floors()
				.get_Item(floor)
				.get_Regions()
				.get_Item(regionId)
				.get_Maps()
				.get_Item(mapId)
				.get_Sectors()).AllAsync(default(CancellationToken)).ContinueWith((Task<IApiV2ObjectList<ContinentFloorRegionMapSector>> task) => (!task.IsFaulted) ? ((IEnumerable<ContinentFloorRegionMapSector>)task.Result) : Enumerable.Empty<ContinentFloorRegionMapSector>());
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			if (e.get_Value() <= 0)
			{
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_name.txt", string.Empty);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_type.txt", string.Empty);
				return;
			}
			Map map;
			try
			{
				map = await ((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(e.get_Value(), default(CancellationToken));
				if (map == null)
				{
					throw new NullReferenceException("Unknown error.");
				}
			}
			catch (Exception ex) when (ex is UnexpectedStatusException || ex is NullReferenceException)
			{
				StreamOutModule.Logger.Warn(StreamOutModule.Instance.WebApiDown);
				return;
			}
			catch (RequestException val)
			{
				RequestException exe = val;
				StreamOutModule.Logger.Error((Exception)(object)exe, ((Exception)(object)exe).Message);
				return;
			}
			string location = map.get_Name();
			if (map.get_Name().Equals(map.get_RegionName(), StringComparison.InvariantCultureIgnoreCase))
			{
				ContinentFloorRegionMapSector defaultSector = (await RequestSectors(map.get_ContinentId(), map.get_DefaultFloor(), map.get_RegionId(), map.get_Id())).FirstOrDefault();
				if (defaultSector != null && !string.IsNullOrEmpty(defaultSector.get_Name()))
				{
					location = defaultSector.get_Name().Replace("<br>", " ");
				}
			}
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_name.txt", location);
			string type = string.Empty;
			MapType value = map.get_Type().get_Value();
			switch (value - 1)
			{
			case 8:
			case 9:
			case 10:
			case 11:
			case 13:
			case 14:
			case 17:
				type = "WvW";
				break;
			case 4:
			case 15:
				type = ((map.get_Id() != 350) ? "PvE" : "PvP");
				break;
			case 1:
				type = "PvP";
				break;
			case 2:
				type = "GvG";
				break;
			case 0:
			case 3:
			case 5:
			case 6:
			case 7:
			case 12:
				type = ((Enum)(object)map.get_Type().get_Value()).ToDisplayString();
				break;
			}
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/map_type.txt", type);
		}

		public override async Task Clear()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await FileUtil.DeleteAsync(Path.Combine(dir, "map_name.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "map_type.txt"));
		}

		public override void Dispose()
		{
		}
	}
}
