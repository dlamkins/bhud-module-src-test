using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Models.GW2API;
using Estreya.BlishHUD.EventTable.Utils;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.State
{
	public class PointOfInterestState : APIState<PointOfInterest>
	{
		private static readonly Logger Logger = Logger.GetLogger<PointOfInterestState>();

		private const string BASE_FOLDER_STRUCTURE = "pois";

		private const string LAST_UPDATED_FILE_NAME = "last_updated.txt";

		private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";

		private readonly string _baseFolderPath;

		private string FullPath => Path.Combine(_baseFolderPath, "pois");

		public bool Loading { get; private set; }

		public PointOfInterestState(Gw2ApiManager apiManager, string baseFolderPath)
			: base(apiManager, (List<TokenPermission>)null, (TimeSpan?)Timeout.InfiniteTimeSpan, awaitLoad: false, -1)
		{
			_baseFolderPath = baseFolderPath;
			base.FetchAction = async delegate(Gw2ApiManager apiManager)
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
			};
		}

		protected override async Task Load()
		{
			lock (this)
			{
				Loading = true;
			}
			try
			{
				bool loadFromApi = false;
				if (Directory.Exists(FullPath))
				{
					bool continueLoadingFiles = true;
					string lastUpdatedFilePath = Path.Combine(FullPath, "last_updated.txt");
					if (!File.Exists(lastUpdatedFilePath))
					{
						await CreateLastUpdatedFile();
					}
					if (!DateTime.TryParseExact(await FileUtil.ReadStringAsync(lastUpdatedFilePath), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastUpdated))
					{
						Logger.Debug("Failed parsing last updated.");
					}
					else if (DateTime.UtcNow - new DateTime(lastUpdated.Ticks, DateTimeKind.Utc) > TimeSpan.FromDays(5.0))
					{
						continueLoadingFiles = false;
						loadFromApi = true;
					}
					if (continueLoadingFiles)
					{
						string[] files = Directory.GetDirectories(FullPath).ToList().SelectMany((string dir) => Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
							.ToArray();
						if (files.Length != 0)
						{
							await LoadFromFiles(files);
						}
						else
						{
							loadFromApi = true;
						}
					}
				}
				else
				{
					loadFromApi = true;
				}
				if (loadFromApi)
				{
					await base.Load();
					await Save();
				}
				Logger.Debug("Loaded {0} point of interests.", new object[1] { base.APIObjectList.Count });
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed loading point of interests:");
			}
			finally
			{
				lock (this)
				{
					Loading = false;
				}
			}
		}

		private async Task LoadFromFiles(string[] files)
		{
			List<Task<string>> loadTasks = files.ToList().Select(delegate(string file)
			{
				if (!File.Exists(file))
				{
					Logger.Warn("Could not find file \"{0}\"", new object[1] { file });
					return Task.FromResult<string>(null);
				}
				return FileUtil.ReadStringAsync(file);
			}).ToList();
			await Task.WhenAll(loadTasks);
			using (await _listLock.LockAsync())
			{
				foreach (Task<string> item in loadTasks)
				{
					string result = item.Result;
					if (!string.IsNullOrWhiteSpace(result))
					{
						try
						{
							PointOfInterest poi = JsonConvert.DeserializeObject<PointOfInterest>(result);
							base.APIObjectList.Add(poi);
						}
						catch (Exception ex)
						{
							Logger.Warn(ex, "Could not parse poi: {0}", new object[1] { result.Replace("\n", "").Replace("\r", "") });
						}
					}
				}
			}
		}

		protected override async Task Save()
		{
			if (Directory.Exists(FullPath))
			{
				Directory.Delete(FullPath, recursive: true);
			}
			Directory.CreateDirectory(FullPath);
			using (await _listLock.LockAsync())
			{
				await Task.WhenAll(base.APIObjectList.Select(delegate(PointOfInterest poi)
				{
					string path = Path.Combine(FullPath, FileUtil.SanitizeFileName(poi.Continent.Name), FileUtil.SanitizeFileName(poi.Floor.Id.ToString()), FileUtil.SanitizeFileName(poi.Region.Name), FileUtil.SanitizeFileName(poi.Map.Name), FileUtil.SanitizeFileName(poi.Name) + ".txt");
					Directory.CreateDirectory(Path.GetDirectoryName(path));
					string data = JsonConvert.SerializeObject((object)poi, (Formatting)1);
					return FileUtil.WriteStringAsync(path, data);
				}));
			}
			await CreateLastUpdatedFile();
		}

		private async Task CreateLastUpdatedFile()
		{
			await FileUtil.WriteStringAsync(Path.Combine(FullPath, "last_updated.txt"), DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
		}

		public PointOfInterest GetPointOfInterest(string chatCode)
		{
			using (_listLock.Lock())
			{
				IEnumerable<PointOfInterest> foundPointOfInterests = base.APIObjectList.Where((PointOfInterest wp) => wp.ChatLink == chatCode);
				return foundPointOfInterests.Any() ? foundPointOfInterests.First() : null;
			}
		}

		public override Task DoClear()
		{
			return Task.CompletedTask;
		}

		protected override void DoUnload()
		{
		}
	}
}
