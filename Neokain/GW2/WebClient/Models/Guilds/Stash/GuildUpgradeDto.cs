namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class GuildUpgradeDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string? Description { get; set; }

		public string Icon { get; set; } = string.Empty;


		public string Type { get; set; } = string.Empty;


		public int? BuildTime { get; set; }

		public int? RequiredLevel { get; set; }

		public int? Experience { get; set; }

		public bool IsUnknown { get; set; }
	}
}
