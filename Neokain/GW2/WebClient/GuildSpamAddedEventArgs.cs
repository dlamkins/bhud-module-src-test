using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildSpamAddedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public SpamDetailDto Spam { get; }

		public GuildSpamAddedEventArgs(Guid guildId, SpamDetailDto spam)
		{
			GuildId = guildId;
			Spam = spam;
		}
	}
}
