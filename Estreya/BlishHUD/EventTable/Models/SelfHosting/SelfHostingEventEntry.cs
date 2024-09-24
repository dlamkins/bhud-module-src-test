using System;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.SelfHosting
{
	public class SelfHostingEventEntry
	{
		[JsonProperty("categoryKey")]
		public string CategoryKey { get; set; }

		[JsonProperty("zoneKey")]
		public string ZoneKey { get; set; }

		[JsonProperty("eventKey")]
		public string EventKey { get; set; }

		[JsonProperty("accountName")]
		public string AccountName { get; set; }

		[JsonProperty("instanceIP")]
		public string InstanceIP { get; set; }

		[JsonProperty("startTime")]
		public DateTimeOffset StartTime { get; set; }

		[JsonProperty("duration")]
		public int Duration { get; set; }
	}
}
