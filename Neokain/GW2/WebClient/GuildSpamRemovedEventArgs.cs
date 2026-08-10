using System;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildSpamRemovedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public Guid SpamId { get; }

		public GuildSpamRemovedEventArgs(Guid guildId, Guid spamId)
		{
			GuildId = guildId;
			SpamId = spamId;
		}
	}
}
