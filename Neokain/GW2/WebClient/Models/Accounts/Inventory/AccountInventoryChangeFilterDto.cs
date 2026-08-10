using System;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountInventoryChangeFilterDto
	{
		public DateTimeOffset? From { get; set; }

		public DateTimeOffset? To { get; set; }

		public AccountInventoryChangeTypeDto[]? ChangeTypes { get; set; }

		public AccountInventoryLocationDto[]? Locations { get; set; }

		public string? CharacterName { get; set; }

		public int Limit { get; set; } = 100;


		public int Offset { get; set; }
	}
}
