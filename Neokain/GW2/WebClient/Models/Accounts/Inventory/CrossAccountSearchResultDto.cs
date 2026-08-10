using System;
using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class CrossAccountSearchResultDto
	{
		public Guid AccountId { get; set; }

		public string AccountName { get; set; } = string.Empty;


		public ItemDto Item { get; set; }

		public AccountInventoryLocationDto Location { get; set; }

		public string? LocationDetail { get; set; }

		public int? SlotIndex { get; set; }

		public int Count { get; set; }

		public int? Charges { get; set; }

		public string? Binding { get; set; }
	}
}
