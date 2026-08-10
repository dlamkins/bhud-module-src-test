using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class CharacterBagDto
	{
		public Guid Id { get; set; }

		public int SlotIndex { get; set; }

		public ItemDto? BagItem { get; set; }

		public int? BagItemId { get; set; }

		public int Size { get; set; }

		public List<CharacterInventoryItemDto> Items { get; set; } = new List<CharacterInventoryItemDto>();

	}
}
