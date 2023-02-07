using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	internal class OnlineFillerRequest
	{
		public class OnlineFillerRequestModule
		{
			[JsonProperty("version")]
			public string Version { get; set; }
		}

		public class OnlineFillerRequestTimes
		{
			[JsonProperty("now")]
			public string Now_UTC_ISO { get; set; }

			[JsonProperty("min")]
			public string Min_UTC_ISO { get; set; }

			[JsonProperty("max")]
			public string Max_UTC_ISO { get; set; }
		}

		[JsonProperty("module")]
		public OnlineFillerRequestModule Module { get; set; }

		[JsonProperty("times")]
		public OnlineFillerRequestTimes Times { get; set; }

		[JsonProperty("eventKeys")]
		public string[] EventKeys { get; set; }
	}
}
