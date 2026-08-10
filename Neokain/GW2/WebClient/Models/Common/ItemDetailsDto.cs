using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Common
{
	public class ItemDetailsDto
	{
		public string? SubType { get; set; }

		public string? WeightClass { get; set; }

		public int? Defense { get; set; }

		public string? DamageType { get; set; }

		public int? MinPower { get; set; }

		public int? MaxPower { get; set; }

		public List<ItemAttributeDto> Attributes { get; set; } = new List<ItemAttributeDto>();


		public string? BuffDescription { get; set; }

		public List<ItemInfusionSlotDto> InfusionSlots { get; set; } = new List<ItemInfusionSlotDto>();


		public string? Suffix { get; set; }

		public List<string> Bonuses { get; set; } = new List<string>();


		public bool HasSelectableStats { get; set; }
	}
}
