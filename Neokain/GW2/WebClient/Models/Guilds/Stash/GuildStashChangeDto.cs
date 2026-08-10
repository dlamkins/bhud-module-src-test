using System;

namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class GuildStashChangeDto
	{
		public Guid Id { get; set; }

		public Guid GuildId { get; set; }

		public string GuildName { get; set; } = string.Empty;


		public string GuildTag { get; set; } = string.Empty;


		public DateTimeOffset DetectedAt { get; set; }

		public GuildStashChangeTypeDto ChangeType { get; set; }

		public GuildStashChangeLocationDto Location { get; set; }

		public int? TabIndex { get; set; }

		public int? SlotIndex { get; set; }

		public ItemDto? Item { get; set; }

		public int? ItemId { get; set; }

		public GuildUpgradeDto? Upgrade { get; set; }

		public int? UpgradeId { get; set; }

		public int? OldCount { get; set; }

		public int? NewCount { get; set; }

		public long? OldCoins { get; set; }

		public long? NewCoins { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
