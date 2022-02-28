using System.Collections.Generic;
using Newtonsoft.Json;

namespace KillProofModule.Models
{
	public class Raid
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("wings")]
		public IReadOnlyList<Wing> Wings { get; set; }
	}
}
