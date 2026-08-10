using System;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class LinkedAccountSummaryDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int World { get; set; }

		public DateTimeOffset Age { get; set; }

		public bool Commander { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? LastFetched { get; set; }

		public int GuildCount { get; set; }

		public int AllianceCount { get; set; }

		public bool IsGuildLeader { get; set; }

		public bool IsAllianceLeader { get; set; }
	}
}
