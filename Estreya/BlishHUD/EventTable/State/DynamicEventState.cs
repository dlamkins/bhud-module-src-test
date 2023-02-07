using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Estreya.BlishHUD.Shared.State;
using Flurl.Http;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.State
{
	public class DynamicEventState : ManagedState
	{
		public class Map
		{
			[JsonProperty("id")]
			public int ID { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }
		}

		public class DynamicEvent
		{
			public class DynamicEventLocation
			{
				[JsonProperty("type")]
				public string Type { get; set; }

				[JsonProperty("center")]
				public double[] Center { get; set; }

				[JsonProperty("radius")]
				public double Radius { get; set; }

				[JsonProperty("rotation")]
				public double Rotation { get; set; }

				[JsonProperty("z_range")]
				public double[] ZRange { get; set; }

				[JsonProperty("points")]
				public double[][] Points { get; set; }
			}

			[JsonIgnore]
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
		}

		private const string BASE_URL = "https://api.guildwars2.com/v1";

		private readonly IFlurlClient _flurlClient;

		public Map[] Maps { get; private set; }

		public DynamicEvent[] Events { get; private set; }

		public DynamicEventState(StateConfiguration configuration, IFlurlClient flurlClient)
			: base(configuration)
		{
			_flurlClient = flurlClient;
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override async Task Load()
		{
			Maps = await GetMaps();
			Events = await GetEvents();
		}

		public Map GetMap(int id)
		{
			return Maps.Where((Map m) => m.ID == id).FirstOrDefault();
		}

		public DynamicEvent[] GetEventsByMap(int mapId)
		{
			return Events.Where((DynamicEvent e) => e.MapId == mapId).ToArray();
		}

		public DynamicEvent GetEventById(string eventId)
		{
			return Events.Where((DynamicEvent e) => e.ID == eventId).FirstOrDefault();
		}

		private async Task<Map[]> GetMaps()
		{
			return await _flurlClient.Request("https://api.guildwars2.com/v1", "map_names.json").SetQueryParam("lang", "en").GetJsonAsync<Map[]>(default(CancellationToken), (HttpCompletionOption)0);
		}

		private async Task<DynamicEvent[]> GetEvents()
		{
			return JsonConvert.DeserializeAnonymousType(await _flurlClient.Request("https://api.guildwars2.com/v1", "event_details.json").SetQueryParam("lang", "en").GetStringAsync(default(CancellationToken), (HttpCompletionOption)0), new
			{
				events = new Dictionary<string, DynamicEvent>()
			})!.events.Select(delegate(KeyValuePair<string, DynamicEvent> x)
			{
				x.Value.ID = x.Key;
				return x.Value;
			}).ToArray();
		}
	}
}
