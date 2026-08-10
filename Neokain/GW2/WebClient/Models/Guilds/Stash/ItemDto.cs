using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Common;
using Neokain.GW2.WebClient.Models.Items;

namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class ItemDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Icon { get; set; } = string.Empty;


		public string Rarity { get; set; } = string.Empty;


		public string Type { get; set; } = string.Empty;


		public int? Level { get; set; }

		public string? Description { get; set; }

		public int VendorValue { get; set; }

		public List<string> Flags { get; set; } = new List<string>();


		public string? ChatLink { get; set; }

		public ItemDetailsDto? Details { get; set; }

		public ItemPriceDto? Price { get; set; }
	}
}
