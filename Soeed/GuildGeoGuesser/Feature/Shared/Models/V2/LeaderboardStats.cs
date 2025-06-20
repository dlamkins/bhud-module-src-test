using System.Collections.Generic;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class LeaderboardStats
	{
		public List<LeaderboardStat> Stats { get; set; } = new List<LeaderboardStat>();


		public BestScoreStat BestScore { get; set; } = new BestScoreStat();

	}
}
