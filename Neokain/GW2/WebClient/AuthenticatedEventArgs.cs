using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.WebClient
{
	public sealed class AuthenticatedEventArgs : EventArgs
	{
		public Guid AccountId { get; }

		public string AccountName { get; }

		public IReadOnlyList<GuildMembershipDto> Guilds { get; }

		public IReadOnlyList<AllianceMembershipDto> Alliances { get; }

		public AccountDataDto AccountData { get; }

		public AuthenticatedEventArgs(AccountDataDto accountData, IReadOnlyList<GuildMembershipDto> guilds, IReadOnlyList<AllianceMembershipDto> alliances)
		{
			AccountData = accountData;
			AccountId = accountData.Id;
			AccountName = accountData.Name;
			Guilds = guilds;
			Alliances = alliances;
		}
	}
}
