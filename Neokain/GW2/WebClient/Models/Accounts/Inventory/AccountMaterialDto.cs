using System;
using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountMaterialDto
	{
		public Guid Id { get; set; }

		public int ItemId { get; set; }

		public ItemDto? Item { get; set; }

		public int CategoryId { get; set; }

		public MaterialCategoryDto? Category { get; set; }

		public int Count { get; set; }

		public DateTimeOffset? LastFetched { get; set; }
	}
}
