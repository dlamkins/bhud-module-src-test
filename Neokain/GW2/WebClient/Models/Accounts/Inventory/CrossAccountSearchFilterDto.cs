using System;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class CrossAccountSearchFilterDto
	{
		public Guid[]? AccountIds { get; set; }

		public string? Query { get; set; }

		public string? Rarity { get; set; }

		public string? Type { get; set; }

		public AccountInventoryLocationDto[]? Locations { get; set; }

		public int Limit { get; set; } = 100;


		public int Offset { get; set; }
	}
}
