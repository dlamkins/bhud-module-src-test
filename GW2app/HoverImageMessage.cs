using Newtonsoft.Json;

namespace GW2app
{
	internal class HoverImageMessage
	{
		[JsonProperty("type")]
		public string Type;

		[JsonProperty("listId")]
		public string ListId;

		[JsonProperty("index")]
		public int Index;

		[JsonProperty("mime")]
		public string Mime;

		[JsonProperty("image_b64")]
		public string ImageB64;
	}
}
