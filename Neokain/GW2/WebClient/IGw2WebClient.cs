using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Server;

namespace Neokain.GW2.WebClient
{
	public interface IGw2WebClient : IGw2HubReceiver, IDisposable, IGw2HubServer, IGw2HubServerAccount, IGw2HubServerGuild, IGw2HubServerAlliance, IGw2HubServerMap, IGw2HubServerConnection
	{
		bool IsConnected { get; }

		bool IsVerified { get; }

		ConnectionState State { get; }

		AuthenticationSource AuthenticationSource { get; }

		AccountDataDto? CurrentAccount { get; }

		IReadOnlyList<GuildMembershipDto>? CurrentGuilds { get; }

		IReadOnlyList<AllianceMembershipDto>? CurrentAlliances { get; }

		event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

		event EventHandler<RetryScheduledEventArgs>? RetryScheduled;

		event EventHandler<Exception>? Error;

		event EventHandler<AuthenticatedEventArgs>? Authenticated;

		event EventHandler<CompatibilityFailedEventArgs>? CompatibilityFailed;
	}
}
