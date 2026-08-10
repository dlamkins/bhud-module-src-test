using System;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildBanRemovedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public Guid BanId { get; }

		public GuildBanRemovedEventArgs(Guid guildId, Guid banId)
		{
			GuildId = guildId;
			BanId = banId;
		}
	}
}
