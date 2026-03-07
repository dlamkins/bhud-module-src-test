using Newtonsoft.Json;

namespace CinemaModule.Models.WatchParty
{
	public class QueueItem
	{
		[JsonProperty("videoId")]
		public string VideoId { get; set; }

		[JsonProperty("addedBy")]
		public string AddedBy { get; set; }
	}
}
