using Newtonsoft.Json;

namespace GW2app
{
	internal class EntryMessage
	{
		[JsonProperty("type")]
		public string Type;

		[JsonProperty("listId")]
		public string ListId;

		[JsonProperty("index")]
		public int Index;

		[JsonProperty("name")]
		public string Name;

		[JsonProperty("completed")]
		public bool Completed;

		[JsonProperty("autoCompleted")]
		public bool AutoCompleted;

		[JsonProperty("mime")]
		public string Mime;

		[JsonProperty("image_b64")]
		public string ImageB64;

		[JsonProperty("chat_link")]
		public string ChatLink;

		[JsonProperty("link")]
		public string Link;

		[JsonProperty("has_hover_card")]
		public bool HasHoverCard;
	}
}
