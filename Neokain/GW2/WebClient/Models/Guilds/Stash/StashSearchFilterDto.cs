namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class StashSearchFilterDto
	{
		public string? Query { get; set; }

		public string? Rarity { get; set; }

		public string? Type { get; set; }

		public int? MinLevel { get; set; }

		public int? MaxLevel { get; set; }

		public int Limit { get; set; } = 50;


		public int Offset { get; set; }
	}
}
