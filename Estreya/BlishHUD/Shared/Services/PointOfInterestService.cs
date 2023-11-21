using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services
{
	public class PointOfInterestService : FilesystemAPIService<PointOfInterest>
	{
		protected override string BASE_FOLDER_STRUCTURE => "pois";

		protected override string FILE_NAME => "pois.json";

		public List<PointOfInterest> PointOfInterests => base.APIObjectList;

		public PointOfInterestService(APIServiceConfiguration configuration, Gw2ApiManager apiManager, string baseFolderPath)
			: base(apiManager, configuration, baseFolderPath)
		{
		}

		public PointOfInterest GetPointOfInterest(string chatCode)
		{
			using (_apiObjectListLock.Lock())
			{
				foreach (PointOfInterest poi in base.APIObjectList)
				{
					if (poi.ChatLink == chatCode)
					{
						return poi;
					}
				}
				return null;
			}
		}

		protected override async Task<List<PointOfInterest>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			List<PointOfInterest> pointOfInterests = new List<PointOfInterest>();
			progress.Report("Loading continents...");
			foreach (ContinentDetails continent in ((IEnumerable<Continent>)(await ((IAllExpandableClient<Continent>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Continents()).AllAsync(cancellationToken))).Select((Continent x) => new ContinentDetails(x)))
			{
				progress.Report("Loading floors of continent \"" + continent.Name + "\" ...");
				IApiV2ObjectList<ContinentFloor> obj = await ((IAllExpandableClient<ContinentFloor>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(continent.Id)
					.get_Floors()).AllAsync(cancellationToken);
				progress.Report("Parsing floors of continent \"" + continent.Name + "\" ...");
				foreach (ContinentFloor item in (IEnumerable<ContinentFloor>)obj)
				{
					ContinentFloorDetails floorDetails = new ContinentFloorDetails(item);
					foreach (ContinentFloorRegion value in item.get_Regions().Values)
					{
						ContinentFloorRegionDetails regionDetails = new ContinentFloorRegionDetails(value);
						foreach (ContinentFloorRegionMap value2 in value.get_Maps().Values)
						{
							ContinentFloorRegionMapDetails mapDetails = new ContinentFloorRegionMapDetails(value2);
							using IEnumerator<ContinentFloorRegionMapPoi> enumerator5 = value2.get_PointsOfInterest().Values.Where((ContinentFloorRegionMapPoi poi) => poi.get_Name() != null).GetEnumerator();
							while (enumerator5.MoveNext())
							{
								PointOfInterest landmark = new PointOfInterest(enumerator5.Current)
								{
									Continent = continent,
									Floor = floorDetails,
									Region = regionDetails,
									Map = mapDetails
								};
								pointOfInterests.Add(landmark);
							}
						}
					}
				}
			}
			return pointOfInterests.DistinctBy((PointOfInterest poi) => new { poi.Name }).ToList();
		}
	}
}
