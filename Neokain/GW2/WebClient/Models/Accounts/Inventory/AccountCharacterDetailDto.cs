using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountCharacterDetailDto : AccountCharacterDto
	{
		public List<CharacterBagDto> Bags { get; set; } = new List<CharacterBagDto>();


		public List<CharacterEquipmentTabDto> EquipmentTabs { get; set; } = new List<CharacterEquipmentTabDto>();

	}
}
