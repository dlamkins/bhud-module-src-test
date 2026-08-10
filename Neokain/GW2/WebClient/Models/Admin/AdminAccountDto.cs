using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAccountDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string? World { get; set; }

		public int GuildCount { get; set; }

		public int CharacterCount { get; set; }

		public bool IsVerified { get; set; }

		public string? OwnerUsername { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? LastSyncAt { get; set; }
	}
}
