using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gorthax.Gilledwars
{
	public class SubmitResponse
	{
		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("accepted")]
		public List<AcceptedKey> Accepted { get; set; }
	}
}
