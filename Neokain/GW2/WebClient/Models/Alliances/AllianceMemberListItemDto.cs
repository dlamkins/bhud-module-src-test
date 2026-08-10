using System;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	internal class AllianceMemberListItemDto
	{
		public string Name { get; set; } = string.Empty;


		public Guid? AccountId { get; set; }

		public string? RankId { get; set; }

		public int RankOrder { get; set; }
	}
}
