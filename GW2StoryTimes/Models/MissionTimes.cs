using Newtonsoft.Json;

namespace GW2StoryTimes.Models
{
	public class MissionTimes
	{
		[JsonProperty("full")]
		public TimeEstimate Full { get; set; }

		[JsonProperty("speed")]
		public TimeEstimate Speed { get; set; }
	}
}
