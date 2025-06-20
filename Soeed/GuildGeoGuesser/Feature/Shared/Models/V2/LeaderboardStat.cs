using System.Collections.Generic;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class LeaderboardStat
	{
		public string Id { get; set; } = "";


		public string Title { get; set; } = "";


		public string ValueHeader { get; set; } = "";


		public List<LeaderboardEntry> Entries { get; set; } = new List<LeaderboardEntry>();

	}
}
