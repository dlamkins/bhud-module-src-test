using Newtonsoft.Json;
using NodaTime;

namespace Estreya.BlishHUD.EventTable.Models
{
	internal class OnlineFillerEvent
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("duration")]
		public Duration Duration { get; set; }

		[JsonProperty("occurrences")]
		public Instant[] Occurences { get; set; }
	}
}
