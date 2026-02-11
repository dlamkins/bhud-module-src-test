namespace RaidClears.Features.Shared.Models
{
	public sealed class MentorProgressChange
	{
		public int AchievementId { get; set; }

		public int PreviousCurrent { get; set; }

		public int NewCurrent { get; set; }

		public int Delta => NewCurrent - PreviousCurrent;
	}
}
