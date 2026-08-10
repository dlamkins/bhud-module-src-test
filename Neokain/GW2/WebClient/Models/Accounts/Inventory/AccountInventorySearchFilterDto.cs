namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountInventorySearchFilterDto
	{
		public string? Query { get; set; }

		public string? Rarity { get; set; }

		public string? Type { get; set; }

		public AccountInventoryLocationDto[]? Locations { get; set; }

		public string[]? CharacterNames { get; set; }

		public int Limit { get; set; } = 50;


		public int Offset { get; set; }
	}
}
