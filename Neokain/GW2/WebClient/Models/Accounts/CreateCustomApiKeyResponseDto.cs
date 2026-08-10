using System;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class CreateCustomApiKeyResponseDto
	{
		public Guid Id { get; set; }

		public string Key { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? ExpiresAt { get; set; }

		public string? Description { get; set; }

		public bool IsValid { get; set; }
	}
}
