using System.Collections.Generic;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	public sealed class MentorAchievementProgressCache
	{
		[JsonProperty("version")]
		public string Version { get; set; } = "1.0";


		[JsonProperty("updated_utc")]
		public string? UpdatedUtc { get; set; }

		[JsonProperty("achievements")]
		public List<MentorAchievementProgressEntry> Achievements { get; set; } = new List<MentorAchievementProgressEntry>();

	}
}
