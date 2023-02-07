using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models
{
	internal class OnlineFillerCategory
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("fillers")]
		public OnlineFillerEvent[] Fillers { get; set; }
	}
}
