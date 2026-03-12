using System.Collections.Generic;
using Newtonsoft.Json;

namespace GW2StoryTimes.Models
{
	public class Story
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("group_name")]
		public string GroupName { get; set; }

		[JsonProperty("order")]
		public int Order { get; set; }

		[JsonProperty("races")]
		public List<string> Races { get; set; }

		[JsonProperty("missions")]
		public List<Mission> Missions { get; set; }
	}
}
