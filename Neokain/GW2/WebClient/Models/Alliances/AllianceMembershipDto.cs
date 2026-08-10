using System;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	public class AllianceMembershipDto
	{
		public Guid AllianceId { get; set; }

		public string AllianceName { get; set; }

		public string AllianceTag { get; set; }

		public string RankName { get; set; }
	}
}
