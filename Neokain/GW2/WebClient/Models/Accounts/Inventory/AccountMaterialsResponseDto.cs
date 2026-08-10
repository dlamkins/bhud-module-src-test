using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountMaterialsResponseDto
	{
		public List<AccountMaterialDto> Materials { get; set; }

		public Dictionary<int, ItemDto> CategoryItems { get; set; }

		public AccountMaterialsResponseDto(List<AccountMaterialDto> materials, Dictionary<int, ItemDto> categoryItems)
		{
			Materials = materials;
			CategoryItems = categoryItems;
		}
	}
}
