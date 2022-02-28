using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace KillProofModule.Models
{
	public class Wing
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("map_id")]
		public int MapId { get; set; }

		[JsonProperty("events")]
		public IReadOnlyList<Event> Events { get; set; }

		public IEnumerable<Token> GetTokens()
		{
			return from encounter in Events
				where encounter.Token != null
				select encounter into encounters
				select encounters.Token;
		}
	}
}
