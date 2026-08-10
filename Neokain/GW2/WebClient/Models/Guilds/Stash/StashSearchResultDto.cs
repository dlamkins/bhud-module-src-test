using System;

namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class StashSearchResultDto
	{
		public Guid GuildId { get; set; }

		public string GuildName { get; set; } = string.Empty;


		public string GuildTag { get; set; } = string.Empty;


		public int TabIndex { get; set; }

		public string? TabNote { get; set; }

		public int SlotIndex { get; set; }

		public ItemDto? Item { get; set; }

		public int ItemId { get; set; }

		public int Count { get; set; }
	}
}
