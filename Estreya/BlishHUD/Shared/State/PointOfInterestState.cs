using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.State
{
	public class PointOfInterestState : APIState<PointOfInterest>
	{
		private const string BASE_FOLDER_STRUCTURE = "pois";

		private const string FILE_NAME = "pois.json";

		private const string LAST_UPDATED_FILE_NAME = "last_updated.txt";

		private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";

		private readonly string _baseFolderPath;

		private string DirectoryPath => Path.Combine(_baseFolderPath, "pois");

		public PointOfInterestState(APIStateConfiguration configuration, Gw2ApiManager apiManager, string baseFolderPath)
			: base(apiManager, configuration)
		{
			_baseFolderPath = baseFolderPath;
		}

		protected override async Task Load()
		{
			_ = 4;
			try
			{
				if (!(await ShouldLoadFiles()))
				{
					await base.Load();
					await Save();
				}
				else
				{
					try
					{
						base.Loading = true;
						List<PointOfInterest> pois = JsonConvert.DeserializeObject<List<PointOfInterest>>(await FileUtil.ReadStringAsync(Path.Combine(DirectoryPath, "pois.json")));
						using (await _apiObjectListLock.LockAsync())
						{
							base.APIObjectList.AddRange(pois);
						}
					}
					finally
					{
						base.Loading = false;
						SignalCompletion();
					}
				}
				Logger.Debug("Loaded {0} point of interests.", new object[1] { base.APIObjectList.Count });
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed loading point of interests:");
			}
		}

		private async Task<bool> ShouldLoadFiles()
		{
			if (!Directory.Exists(DirectoryPath))
			{
				return false;
			}
			if (!File.Exists(Path.Combine(DirectoryPath, "pois.json")))
			{
				return false;
			}
			string lastUpdatedFilePath = Path.Combine(DirectoryPath, "last_updated.txt");
			if (File.Exists(lastUpdatedFilePath))
			{
				if (!DateTime.TryParseExact(await FileUtil.ReadStringAsync(lastUpdatedFilePath), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastUpdated))
				{
					Logger.Debug("Failed parsing last updated.");
					return false;
				}
				return DateTime.UtcNow - new DateTime(lastUpdated.Ticks, DateTimeKind.Utc) <= TimeSpan.FromDays(5.0);
			}
			return false;
		}

		protected override async Task Save()
		{
			if (Directory.Exists(DirectoryPath))
			{
				Directory.Delete(DirectoryPath, recursive: true);
			}
			Directory.CreateDirectory(DirectoryPath);
			using (await _apiObjectListLock.LockAsync())
			{
				string poiJson = JsonConvert.SerializeObject(base.APIObjectList, Formatting.Indented);
				await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "pois.json"), poiJson);
			}
			await CreateLastUpdatedFile();
		}

		private async Task CreateLastUpdatedFile()
		{
			await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "last_updated.txt"), DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
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

		protected override async Task<List<PointOfInterest>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			List<PointOfInterest> pointOfInterests = new List<PointOfInterest>();
			foreach (ContinentDetails continent in ((IEnumerable<Continent>)(await ((IAllExpandableClient<Continent>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Continents()).AllAsync(default(CancellationToken)))).Select((Continent x) => new ContinentDetails(x)))
			{
				foreach (ContinentFloor item in (IEnumerable<ContinentFloor>)(await ((IAllExpandableClient<ContinentFloor>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Continents()
					.get_Item(continent.Id)
					.get_Floors()).AllAsync(default(CancellationToken))))
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
