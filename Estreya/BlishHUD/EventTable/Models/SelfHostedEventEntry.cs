using System;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class SelfHostedEventEntry
	{
		[JsonProperty("userGuid")]
		public string UserGUID { get; set; }

		[JsonProperty("eventKey")]
		public string EventKey { get; set; }

		[JsonProperty("eventName")]
		public string EventName { get; set; }

		[JsonProperty("startTime")]
		public DateTimeOffset StartTime { get; set; }

		[JsonProperty("duration")]
		public int Duration { get; set; }
	}
}
