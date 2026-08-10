using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildSpamCategoryAddedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public SpamCategoryDto Category { get; }

		public GuildSpamCategoryAddedEventArgs(Guid guildId, SpamCategoryDto category)
		{
			GuildId = guildId;
			Category = category;
		}
	}
}
