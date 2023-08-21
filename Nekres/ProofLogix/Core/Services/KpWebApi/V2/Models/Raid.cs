using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Raid
	{
		[JsonObject(/*Could not decode attribute arguments.*/)]
		public sealed class Wing
		{
			[JsonObject(/*Could not decode attribute arguments.*/)]
			public sealed class Event
			{
				public enum EventType
				{
					Boss,
					Checkpoint
				}

				[JsonProperty("id")]
				public string Id { get; set; }

				[JsonProperty("type")]
				[JsonConverter(typeof(StringEnumConverter))]
				public EventType Type { get; set; }

				[JsonProperty("name")]
				public string Name { get; set; }

				[JsonProperty("token")]
				public Resource Token { get; set; }

				[JsonProperty("miniatures")]
				public List<Resource> Miniatures { get; set; }

				[JsonIgnore]
				public AsyncTexture2D Icon => Miniatures?.FirstOrDefault()?.Icon ?? Token?.Icon ?? GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1302744);

				public Event()
				{
					Miniatures = new List<Resource>();
				}

				public List<Resource> GetTokens()
				{
					List<Resource> result = Enumerable.Empty<Resource>().ToList();
					if (Token != null)
					{
						result.Add(Token);
					}
					return result;
				}
			}

			[JsonProperty("id")]
			public string Id { get; set; }

			[JsonProperty("map_id")]
			public int MapId { get; set; }

			[JsonProperty("events")]
			public List<Event> Events { get; set; }

			public Wing()
			{
				Events = new List<Event>();
			}
		}

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("wings")]
		public List<Wing> Wings { get; set; }

		public Raid()
		{
			Wings = new List<Wing>();
		}
	}
}
