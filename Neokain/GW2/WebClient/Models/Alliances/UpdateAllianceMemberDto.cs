using System;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	internal class UpdateAllianceMemberDto
	{
		public string Name { get; set; } = string.Empty;


		public Guid? RankId { get; set; }

		public string? Notes { get; set; }
	}
}
