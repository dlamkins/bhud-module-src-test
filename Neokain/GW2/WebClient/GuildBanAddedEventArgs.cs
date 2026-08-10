using System;
using Neokain.GW2.WebClient.Models.Bans;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildBanAddedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public BanDto Ban { get; }

		public GuildBanAddedEventArgs(Guid guildId, BanDto ban)
		{
			GuildId = guildId;
			Ban = ban;
		}
	}
}
