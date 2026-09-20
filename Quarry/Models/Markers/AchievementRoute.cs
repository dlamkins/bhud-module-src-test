using System.Collections.Generic;

namespace Quarry.Models.Markers
{
	public class AchievementRoute
	{
		public HashSet<int> MapIds { get; set; } = new HashSet<int>();


		public List<AchievementObjective> Objectives { get; set; } = new List<AchievementObjective>();


		public HashSet<string> Waypoints { get; set; } = new HashSet<string>();


		public HashSet<string> Packs { get; set; } = new HashSet<string>();

	}
}
