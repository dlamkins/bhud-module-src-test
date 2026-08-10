using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Common
{
	public class SlotItemDto
	{
		public int SlotIndex { get; set; }

		public ItemDto? Item { get; set; }

		public int? ItemId { get; set; }

		public int Count { get; set; }

		public int? Charges { get; set; }

		public string? Binding { get; set; }
	}
}
