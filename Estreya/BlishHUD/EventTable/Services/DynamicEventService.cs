using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading.Events;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Services
{
	public class DynamicEventService : APIService<DynamicEvent>
	{
		private readonly string _apiBaseUrl;

		private readonly string _directoryBasePath;

		private readonly IFlurlClient _flurlClient;

		private List<DynamicEvent> _customEvents = new List<DynamicEvent>();

		private bool _loadedFiles;

		private const string BASE_FOLDER_STRUCTURE = "dynamic_events";

		private const string FILE_NAME = "custom.json";

		private string API_URL => _apiBaseUrl.TrimEnd('/') + "/v1/gw2/dynamicEvents";

		public List<DynamicEvent> Events => base.APIObjectList.Concat(_customEvents).ToList();

		public event AsyncEventHandler CustomEventsUpdated;

		public DynamicEventService(APIServiceConfiguration configuration, Gw2ApiManager apiManager, IFlurlClient flurlClient, string apiBaseUrl, string directoryBasePath)
			: base(apiManager, configuration)
		{
			_flurlClient = flurlClient;
			_apiBaseUrl = apiBaseUrl;
			_directoryBasePath = directoryBasePath;
		}

		public DynamicEvent[] GetEventsByMap(int mapId)
		{
			return Events?.Where((DynamicEvent e) => e.MapId == mapId).ToArray();
		}

		public DynamicEvent GetEventById(string eventId)
		{
			return Events?.Where((DynamicEvent e) => e.ID == eventId).FirstOrDefault();
		}

		private async Task<List<DynamicEvent>> GetEvents()
		{
			return JsonConvert.DeserializeObject<List<DynamicEvent>>(await _flurlClient.Request(API_URL).SetQueryParam("lang", "en").GetStringAsync(default(CancellationToken), (HttpCompletionOption)0));
		}

		public async Task AddCustomEvent(DynamicEvent dynamicEvent)
		{
			_customEvents.RemoveAll((DynamicEvent e) => e.ID == dynamicEvent.ID);
			dynamicEvent.IsCustom = true;
			_customEvents.Add(dynamicEvent);
			await Save();
			List<DynamicEvent> oldList = new List<DynamicEvent>(base.APIObjectList);
			base.APIObjectList.Clear();
			base.APIObjectList.AddRange(FilterCustomizedEvents(oldList));
		}

		public async Task RemoveCustomEvent(string id)
		{
			DynamicEvent existingEvent = GetEventById(id);
			if (existingEvent != null && existingEvent.IsCustom)
			{
				_customEvents.Remove(existingEvent);
				await Save();
				await Load();
			}
		}

		private List<DynamicEvent> FilterCustomizedEvents(IEnumerable<DynamicEvent> events)
		{
			return events.Where((DynamicEvent e) => !_customEvents.Any((DynamicEvent ce) => ce.ID == e.ID)).ToList();
		}

		public async Task NotifyCustomEventsUpdated()
		{
			await (this.CustomEventsUpdated?.Invoke(this) ?? Task.CompletedTask);
		}

		protected override async Task<List<DynamicEvent>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			return FilterCustomizedEvents(await GetEvents());
		}

		protected override async Task Save()
		{
			string directoryPath = Path.Combine(_directoryBasePath, "dynamic_events");
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			string path = Path.Combine(directoryPath, "custom.json");
			string json = JsonConvert.SerializeObject(_customEvents, Formatting.Indented);
			await FileUtil.WriteStringAsync(path, json);
		}

		protected override async Task Load()
		{
			if (!_loadedFiles)
			{
				string filePath = Path.Combine(_directoryBasePath, "dynamic_events", "custom.json");
				if (File.Exists(filePath))
				{
					List<DynamicEvent> customEvents = (_customEvents = JsonConvert.DeserializeObject<List<DynamicEvent>>(await FileUtil.ReadStringAsync(filePath)));
					_customEvents.ForEach(delegate(DynamicEvent ce)
					{
						ce.IsCustom = true;
					});
				}
				_loadedFiles = true;
			}
			await base.Load();
		}
	}
}
