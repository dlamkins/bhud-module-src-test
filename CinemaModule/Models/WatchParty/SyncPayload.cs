using Newtonsoft.Json;

namespace CinemaModule.Models.WatchParty
{
	public class SyncPayload
	{
		[JsonProperty("timestamp")]
		public double Timestamp { get; set; }

		[JsonProperty("isPlaying")]
		public bool IsPlaying { get; set; }

		[JsonProperty("sequenceNumber")]
		public long SequenceNumber { get; set; }
	}
}
