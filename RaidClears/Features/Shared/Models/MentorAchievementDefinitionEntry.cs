using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	public sealed class MentorAchievementDefinitionEntry
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("max")]
		public int Max { get; set; }
	}
}
