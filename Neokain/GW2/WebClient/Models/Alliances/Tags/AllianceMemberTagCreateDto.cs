using System;

namespace Neokain.GW2.WebClient.Models.Alliances.Tags
{
	public class AllianceMemberTagCreateDto
	{
		public Guid TagTypeId { get; set; }

		public DateTimeOffset? EndDate { get; set; }

		public string? Note { get; set; }

		public decimal? GoldAmount { get; set; }
	}
}
