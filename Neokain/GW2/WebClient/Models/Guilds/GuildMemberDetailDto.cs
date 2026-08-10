using System;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	public class GuildMemberDetailDto
	{
		public string Name { get; set; } = string.Empty;


		public string? RankId { get; set; }

		public int RankOrder { get; set; }

		public DateTimeOffset? Joined { get; set; }

		public bool WvwMember { get; set; }
	}
}
