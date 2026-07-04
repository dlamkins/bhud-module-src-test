using System.Collections.Generic;
using Newtonsoft.Json;

namespace GW2app
{
	internal class StateMessage
	{
		[JsonProperty("protocol")]
		public int Protocol;

		[JsonProperty("type")]
		public string Type;

		[JsonProperty("lists")]
		public List<ListDto> Lists;
	}
}
