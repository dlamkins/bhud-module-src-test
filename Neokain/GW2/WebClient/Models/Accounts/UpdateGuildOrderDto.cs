using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class UpdateGuildOrderDto
	{
		public List<GuildOrderEntry> GuildOrders { get; set; } = new List<GuildOrderEntry>();


		[Obsolete("Guild order is always inferred since v2.3.0; override is open to all. No sync effect. Removed in v3.0.0.")]
		public bool ManualGuildOrder { get; set; }
	}
}
