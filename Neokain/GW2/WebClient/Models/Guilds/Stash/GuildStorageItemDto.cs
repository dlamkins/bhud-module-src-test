using System;

namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	internal class GuildStorageItemDto
	{
		public Guid Id { get; set; }

		public int UpgradeId { get; set; }

		public GuildUpgradeDto? Upgrade { get; set; }

		public int Count { get; set; }

		public DateTimeOffset? LastFetched { get; set; }
	}
}
