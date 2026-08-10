using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Models
{
	public class SpamContext
	{
		public SpamContextType ContextType { get; set; }

		public Guid? GuildId { get; set; }

		public Guid? AllianceId { get; set; }

		public Guid? AccountId { get; set; }

		public List<GuildMembershipDto> AvailableGuilds { get; set; } = new List<GuildMembershipDto>();


		public List<AllianceMembershipDto> AvailableAlliances { get; set; } = new List<AllianceMembershipDto>();


		public static SpamContext ForGuild(Guid guildId, GuildMembershipDto guildMembership)
		{
			return new SpamContext
			{
				ContextType = SpamContextType.Guild,
				GuildId = guildId,
				AvailableGuilds = ((guildMembership != null) ? new List<GuildMembershipDto> { guildMembership } : new List<GuildMembershipDto>())
			};
		}

		public static SpamContext ForAlliance(Guid allianceId, List<GuildMembershipDto> allianceGuilds)
		{
			return new SpamContext
			{
				ContextType = SpamContextType.Alliance,
				AllianceId = allianceId,
				AvailableGuilds = (allianceGuilds ?? new List<GuildMembershipDto>())
			};
		}

		public static SpamContext ForAccount(Guid accountId, List<GuildMembershipDto> allGuilds, List<AllianceMembershipDto> allAlliances)
		{
			return new SpamContext
			{
				ContextType = SpamContextType.Account,
				AccountId = accountId,
				AvailableGuilds = (allGuilds ?? new List<GuildMembershipDto>()),
				AvailableAlliances = (allAlliances ?? new List<AllianceMembershipDto>())
			};
		}
	}
}
