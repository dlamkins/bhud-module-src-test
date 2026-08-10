using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class GuildStashTabDto
	{
		public Guid Id { get; set; }

		public int TabIndex { get; set; }

		public int? UpgradeId { get; set; }

		public int Size { get; set; }

		public long Coins { get; set; }

		public string? Note { get; set; }

		public List<GuildStashItemDto> Items { get; set; } = new List<GuildStashItemDto>();


		public DateTimeOffset LastFetched { get; set; }
	}
}
