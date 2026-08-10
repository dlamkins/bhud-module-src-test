using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildSpamCategoryChangedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public SpamCategoryDto Category { get; }

		public GuildSpamCategoryChangedEventArgs(Guid guildId, SpamCategoryDto category)
		{
			GuildId = guildId;
			Category = category;
		}
	}
}
