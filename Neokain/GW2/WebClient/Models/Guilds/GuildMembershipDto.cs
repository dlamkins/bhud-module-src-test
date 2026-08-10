using System;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	public class GuildMembershipDto
	{
		public Guid GuildId { get; set; }

		public string GuildName { get; set; }

		public string GuildTag { get; set; }

		public string RankName { get; set; }

		public int RankOrder { get; set; }

		public int OfficerRankOrder { get; set; }

		public DateTimeOffset? Joined { get; set; }

		public bool WvwMember { get; set; }

		public int GuildIndex { get; set; }

		public string? OriginalRankName { get; set; }
	}
}
