using System;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class CreateCustomApiKeyRequestDto
	{
		public string? Description { get; set; }

		public DateTimeOffset? ExpiresAt { get; set; }
	}
}
