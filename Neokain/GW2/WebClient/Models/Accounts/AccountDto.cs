using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class AccountDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int World { get; set; }

		public bool Commander { get; set; }

		public DateTimeOffset Age { get; set; }

		public DateTimeOffset? LastFetched { get; set; }

		public DateTimeOffset? BankLastSyncedAt { get; set; }

		public DateTimeOffset? MaterialsLastSyncedAt { get; set; }

		public DateTimeOffset? CharactersLastSyncedAt { get; set; }

		public DateTimeOffset? AutoLastSyncedAt { get; set; }

		public DateTimeOffset? ManualLastSyncedAt { get; set; }

		public List<Guid> GuildIds { get; set; } = new List<Guid>();


		public List<Guid> GuildLeaderIds { get; set; } = new List<Guid>();


		public List<Guid> AllianceIds { get; set; } = new List<Guid>();


		public List<Guid> AllianceLeaderIds { get; set; } = new List<Guid>();


		[Obsolete("Guild order is always inferred since v2.3.0; override is open to all. No sync effect. Removed in v3.0.0.")]
		public bool ManualGuildOrder { get; set; }

		public bool GuildOrderNeedsReview { get; set; }

		public List<string> AvailableScopes { get; set; } = new List<string>();


		public Guid? PrimaryApiKeyId { get; set; }
	}
}
