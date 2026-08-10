using System;

namespace Neokain.GW2.WebClient
{
	public sealed class GuildSpamCategoryRemovedEventArgs : EventArgs
	{
		public Guid GuildId { get; }

		public Guid CategoryId { get; }

		public GuildSpamCategoryRemovedEventArgs(Guid guildId, Guid categoryId)
		{
			GuildId = guildId;
			CategoryId = categoryId;
		}
	}
}
