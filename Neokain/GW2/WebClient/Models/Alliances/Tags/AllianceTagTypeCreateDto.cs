namespace Neokain.GW2.WebClient.Models.Alliances.Tags
{
	public class AllianceTagTypeCreateDto
	{
		public string Name { get; set; } = string.Empty;


		public string? Description { get; set; }

		public string? Color { get; set; }

		public string? IconName { get; set; }

		public int? DefaultDurationDays { get; set; }

		public bool AllowExtension { get; set; }

		public bool RequiresNote { get; set; }

		public int GrantedByMinRankPosition { get; set; } = 1;


		public string? GuildRankRequirement { get; set; }

		public int? InactivityDaysOverride { get; set; }
	}
}
