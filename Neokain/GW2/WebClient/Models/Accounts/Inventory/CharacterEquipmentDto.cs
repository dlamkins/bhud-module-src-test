using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Common;
using Neokain.GW2.WebClient.Models.Guilds.Stash;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class CharacterEquipmentDto
	{
		public Guid Id { get; set; }

		public string Slot { get; set; } = string.Empty;


		public ItemDto? Item { get; set; }

		public int ItemId { get; set; }

		public int? SkinId { get; set; }

		public List<int> Dyes { get; set; } = new List<int>();


		public List<int> Upgrades { get; set; } = new List<int>();


		public List<int> Infusions { get; set; } = new List<int>();


		public string? Binding { get; set; }

		public string? BoundTo { get; set; }

		public EquipmentStatsDto? Stats { get; set; }

		public List<ItemDto> UpgradeItems { get; set; } = new List<ItemDto>();


		public List<ItemDto> InfusionItems { get; set; } = new List<ItemDto>();

	}
}
