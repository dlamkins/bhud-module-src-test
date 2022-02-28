using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KillProofModule.Models
{
	public class Event
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("miniatures")]
		public IReadOnlyList<Miniature> Miniatures { get; set; }

		[JsonProperty("token")]
		public Token Token { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("type")]
		public RaidWingEventType Type { get; set; }
	}
}
