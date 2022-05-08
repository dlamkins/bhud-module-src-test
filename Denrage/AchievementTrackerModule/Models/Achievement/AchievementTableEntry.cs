using System.Diagnostics;

namespace Denrage.AchievementTrackerModule.Models.Achievement
{
	[DebuggerDisplay("{Name}")]
	public class AchievementTableEntry
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Link { get; set; }

		public bool HasLink => !string.IsNullOrEmpty(Link);

		public string Prerequisite { get; set; }

		public AchievementTitle Title { get; set; } = AchievementTitle.EmptyTitle;


		public Reward Reward { get; set; } = Reward.EmptyReward;


		public AchievementTableEntryDescription Description { get; set; }

		public string Cite { get; set; }
	}
}
