using Gw2Sharp.WebApi.V2.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Models
{
	public class HereCandidate
	{
		public AchievementTableEntry Achievement { get; set; }

		public AchievementCategory Category { get; set; }

		public int Current { get; set; }

		public int Max { get; set; }

		public int AchievementPoints { get; set; }

		public bool Guided { get; set; }

		public GuidanceInfo Guidance { get; set; } = GuidanceInfo.None;


		public bool Opportunistic { get; set; }
	}
}
