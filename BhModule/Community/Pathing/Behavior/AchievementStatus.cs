using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;

namespace BhModule.Community.Pathing.Behavior
{
	public readonly struct AchievementStatus
	{
		public bool Done { get; }

		public HashSet<int> AchievementBits { get; }

		public bool Unlocked { get; }

		public AchievementStatus(AccountAchievement accountAchievement)
		{
			Done = accountAchievement.get_Done() && accountAchievement.get_Repeated().HasValue;
			IEnumerable<int> bits = accountAchievement.get_Bits();
			AchievementBits = new HashSet<int>(bits ?? Enumerable.Empty<int>());
			Unlocked = accountAchievement.get_Unlocked() ?? true;
		}
	}
}
