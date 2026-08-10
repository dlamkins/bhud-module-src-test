using System;
using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountInventoryChangeDto
	{
		public Guid Id { get; set; }

		public Guid AccountId { get; set; }

		public string AccountName { get; set; } = string.Empty;


		public DateTimeOffset DetectedAt { get; set; }

		public AccountInventoryChangeTypeDto ChangeType { get; set; }

		public AccountInventoryLocationDto Location { get; set; }

		public string? LocationDetail { get; set; }

		public int? SlotIndex { get; set; }

		public ItemDto? Item { get; set; }

		public int? ItemId { get; set; }

		public int? OldCount { get; set; }

		public int? NewCount { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
