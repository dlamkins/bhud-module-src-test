using System.Collections.Generic;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class BestScoreStat : LeaderboardStat
	{
		public string CountHeader { get; set; } = "";


		public new List<BestScoreEntry> Entries { get; set; } = new List<BestScoreEntry>();


		public string ScoreText { get; set; } = "";

	}
}
