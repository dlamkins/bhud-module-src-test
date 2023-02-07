using System;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	internal class OnlineFillerEvent
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("duration")]
		public int Duration { get; set; }

		[JsonProperty("occurences")]
		public DateTimeOffset[] Occurences { get; set; }
	}
}
