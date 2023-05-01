using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Flurl.Http;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Services
{
	public class DynamicEventService : APIService
	{
		public class DynamicEvent
		{
			public class DynamicEventLocation
			{
				[JsonProperty("type")]
				public string Type { get; set; }

				[JsonProperty("center")]
				public float[] Center { get; set; }

				[JsonProperty("radius")]
				public float Radius { get; set; }

				[JsonProperty("height")]
				public float Height { get; set; }

				[JsonProperty("rotation")]
				public float Rotation { get; set; }

				[JsonProperty("z_range")]
				public float[] ZRange { get; set; }

				[JsonProperty("points")]
				public float[][] Points { get; set; }
			}

			public class DynamicEventIcon
			{
				[JsonProperty("file_id")]
				public int FileID { get; set; }

				[JsonProperty("signature")]
				public string Signature { get; set; }
			}

			[JsonProperty("id")]
			public string ID { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("level")]
			public int Level { get; set; }

			[JsonProperty("map_id")]
			public int MapId { get; set; }

			[JsonProperty("flags")]
			public string[] Flags { get; set; }

			[JsonProperty("location")]
			public DynamicEventLocation Location { get; set; }

			[JsonProperty("icon")]
			public DynamicEventIcon Icon { get; set; }
		}

		private readonly IFlurlClient _flurlClient;

		private readonly string _apiBaseUrl;

		private string API_URL => _apiBaseUrl.TrimEnd('/') + "/v1/gw2/dynamicEvents";

		public DynamicEvent[] Events { get; private set; } = new DynamicEvent[0];


		public DynamicEventService(APIServiceConfiguration configuration, Gw2ApiManager apiManager, IFlurlClient flurlClient, string apiBaseUrl)
			: base(apiManager, configuration)
		{
			_flurlClient = flurlClient;
			_apiBaseUrl = apiBaseUrl;
		}

		public DynamicEvent[] GetEventsByMap(int mapId)
		{
			return Events?.Where((DynamicEvent e) => e.MapId == mapId).ToArray();
		}

		public DynamicEvent GetEventById(string eventId)
		{
			return Events?.Where((DynamicEvent e) => e.ID == eventId).FirstOrDefault();
		}

		private async Task<DynamicEvent[]> GetEvents()
		{
			return JsonConvert.DeserializeObject<List<DynamicEvent>>(await _flurlClient.Request(API_URL).SetQueryParam("lang", "en").GetStringAsync(default(CancellationToken), (HttpCompletionOption)0))!.ToArray();
		}

		protected override async Task FetchFromAPI(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			try
			{
				progress.Report("Loading events..");
				Events = await GetEvents();
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed loading events:");
			}
		}
	}
}
