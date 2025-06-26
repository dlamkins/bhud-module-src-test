using System.Collections.Generic;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class LeaderboardStat
	{
		public string Id { get; set; } = "";


		public string Title { get; set; } = "";


		public string Subtitle { get; set; } = "";


		public List<string> Headers { get; set; } = new List<string>();


		public List<string> Alignments { get; set; } = new List<string>();


		public List<LeaderboardEntry> Entries { get; set; } = new List<LeaderboardEntry>();


		public int Width { get; set; } = 400;


		public bool ShowTrophies { get; set; }

		public int Order { get; set; }

		public string FooterText { get; set; } = "";

	}
}
