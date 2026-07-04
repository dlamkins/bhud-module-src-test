using Newtonsoft.Json;

namespace GW2app
{
	internal class OpenHoverMessage
	{
		[JsonProperty("type")]
		public string Type;

		[JsonProperty("listId")]
		public string ListId;

		[JsonProperty("index")]
		public int Index;
	}
}
