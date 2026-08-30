using System.Collections.Generic;
using Newtonsoft.Json;

namespace GW2app
{
	internal class SubscribeMessage
	{
		[JsonProperty("type")]
		public string Type;

		[JsonProperty("listIds")]
		public List<string> ListIds;

		[JsonProperty("module")]
		public string Module;
	}
}
