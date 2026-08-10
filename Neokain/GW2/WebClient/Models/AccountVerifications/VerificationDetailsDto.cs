using System;

namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class VerificationDetailsDto
	{
		public Guid Id { get; set; }

		public string AccountName { get; set; }

		public string VerificationCode { get; set; }

		public string ExpectedApiKeyName { get; set; }

		public DateTime ExpiresAt { get; set; }
	}
}
