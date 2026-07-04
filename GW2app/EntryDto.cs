using Newtonsoft.Json;

namespace GW2app
{
	internal class EntryDto
	{
		[JsonProperty("completed")]
		public bool Completed;

		[JsonProperty("autoCompleted")]
		public bool AutoCompleted;

		[JsonProperty("entry_type")]
		public string EntryType;

		[JsonProperty("has_hover_card")]
		public bool HasHoverCard;
	}
}
