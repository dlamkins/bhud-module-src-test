using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	public class AllianceDetailDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Tag { get; set; } = string.Empty;


		public string Description { get; set; } = string.Empty;


		public Guid LeaderId { get; set; }

		public string LeaderName { get; set; } = string.Empty;


		public DateTime CreatedAt { get; set; }

		public List<AllianceRankDto> Ranks { get; set; } = new List<AllianceRankDto>();


		public List<AllianceMemberDetailDto> Members { get; set; } = new List<AllianceMemberDetailDto>();


		public List<Guid> GuildIds { get; set; } = new List<Guid>();


		public int MemberCount { get; set; }
	}
}
