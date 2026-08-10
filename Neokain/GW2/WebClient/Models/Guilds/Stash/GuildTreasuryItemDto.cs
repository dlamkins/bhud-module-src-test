using System;

namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class GuildTreasuryItemDto
	{
		public Guid Id { get; set; }

		public ItemDto? Item { get; set; }

		public int ItemId { get; set; }

		public int Count { get; set; }

		public int? NeededBy { get; set; }

		public DateTimeOffset LastFetched { get; set; }
	}
}
