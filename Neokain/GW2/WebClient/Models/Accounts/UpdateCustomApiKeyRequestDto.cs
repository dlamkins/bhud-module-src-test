using System;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class UpdateCustomApiKeyRequestDto
	{
		public string? Description { get; set; }

		public DateTimeOffset? ExpiresAt { get; set; }
	}
}
