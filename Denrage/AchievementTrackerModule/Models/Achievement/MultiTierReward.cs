using System.Collections.Generic;
using System.Diagnostics;

namespace Denrage.AchievementTrackerModule.Models.Achievement
{
	[DebuggerDisplay("{Tiers[0].DisplayName}")]
	public class MultiTierReward : Reward
	{
		public class TierReward : ItemReward
		{
			public int Tier { get; set; }
		}

		public List<TierReward> Tiers { get; set; } = new List<TierReward>();

	}
}
