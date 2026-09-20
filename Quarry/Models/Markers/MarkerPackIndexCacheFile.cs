using System.Collections.Generic;

namespace Quarry.Models.Markers
{
	public class MarkerPackIndexCacheFile
	{
		public int Schema { get; set; }

		public List<PackCacheEntry> Packs { get; set; } = new List<PackCacheEntry>();


		public Dictionary<int, AchievementRoute> AchievementsById { get; set; } = new Dictionary<int, AchievementRoute>();

	}
}
