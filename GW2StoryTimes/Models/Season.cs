using System.Collections.Generic;
using Newtonsoft.Json;

namespace GW2StoryTimes.Models
{
	public class Season
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("order")]
		public int Order { get; set; }

		[JsonProperty("mission_count")]
		public int MissionCount { get; set; }

		[JsonProperty("total_full_mins")]
		public double TotalFullMins { get; set; }

		[JsonProperty("total_speed_mins")]
		public double TotalSpeedMins { get; set; }

		[JsonProperty("stories")]
		public List<Story> Stories { get; set; }
	}
}
