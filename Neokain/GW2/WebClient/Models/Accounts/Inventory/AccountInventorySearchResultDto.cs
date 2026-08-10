using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountInventorySearchResultDto
	{
		public ItemDto Item { get; set; }

		public AccountInventoryLocationDto Location { get; set; }

		public string? LocationDetail { get; set; }

		public int? SlotIndex { get; set; }

		public int Count { get; set; }

		public int? Charges { get; set; }

		public string? Binding { get; set; }
	}
}
