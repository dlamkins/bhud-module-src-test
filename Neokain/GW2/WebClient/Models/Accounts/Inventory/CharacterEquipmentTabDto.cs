using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class CharacterEquipmentTabDto
	{
		public Guid Id { get; set; }

		public int TabNumber { get; set; }

		public string? Name { get; set; }

		public bool IsActive { get; set; }

		public List<CharacterEquipmentDto> Equipment { get; set; } = new List<CharacterEquipmentDto>();

	}
}
