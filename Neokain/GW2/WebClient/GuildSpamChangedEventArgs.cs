using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildSpamChangedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public SpamDetailDto Spam { get; }

		public GuildSpamChangedEventArgs(Guid guildId, SpamDetailDto spam)
		{
			GuildId = guildId;
			Spam = spam;
		}
	}
}
