using System;

namespace Neokain.GW2.WebClient.Models.Alliances.Tags
{
	public class AllianceMemberTagExtensionDto
	{
		public Guid Id { get; set; }

		public int ExtensionDays { get; set; }

		public decimal? GoldAmount { get; set; }

		public string? Note { get; set; }

		public DateTimeOffset? AppliedAt { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
