using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	public sealed class MentorAchievementProgressEntry
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("current")]
		public int Current { get; set; }

		[JsonProperty("max")]
		public int Max { get; set; }

		[JsonProperty("done")]
		public bool Done { get; set; }

		public bool Equals(MentorAchievementProgressEntry? other)
		{
			if (other != null && Id == other!.Id && Current == other!.Current && Max == other!.Max)
			{
				return Done == other!.Done;
			}
			return false;
		}
	}
}
