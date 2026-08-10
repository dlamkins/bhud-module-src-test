using System;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class InitiateApiKeyUpdateResponseDto
	{
		public bool Success { get; set; }

		public string Message { get; set; }

		public string ExpectedApiKeyName { get; set; }

		public DateTimeOffset ExpiresAt { get; set; }
	}
}
