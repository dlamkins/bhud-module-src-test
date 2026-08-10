using System;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountCharacterDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Race { get; set; } = string.Empty;


		public string Profession { get; set; } = string.Empty;


		public int Level { get; set; }

		public string? Guild { get; set; }

		public int Age { get; set; }

		public DateTimeOffset Created { get; set; }

		public DateTimeOffset? LastFetched { get; set; }
	}
}
