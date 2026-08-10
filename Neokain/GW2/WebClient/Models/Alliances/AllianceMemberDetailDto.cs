using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	public class AllianceMemberDetailDto
	{
		public string Name { get; set; } = string.Empty;


		public string RankId { get; set; }

		public string RankName { get; set; }

		public int RankOrder { get; set; }

		public DateTimeOffset? Joined { get; set; }

		public List<AllianceMemberGuildDto> Guilds { get; set; } = new List<AllianceMemberGuildDto>();

	}
}
