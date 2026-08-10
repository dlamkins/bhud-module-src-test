using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Accounts.Inventory
{
	internal class AccountBankTabDto
	{
		public Guid Id { get; set; }

		public int TabIndex { get; set; }

		public List<AccountBankItemDto> Items { get; set; } = new List<AccountBankItemDto>();


		public DateTimeOffset? LastFetched { get; set; }
	}
}
