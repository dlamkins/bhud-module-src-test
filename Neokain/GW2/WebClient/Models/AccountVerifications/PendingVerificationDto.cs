using System;

namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class PendingVerificationDto
	{
		public Guid Id { get; set; }

		public string VerificationCode { get; set; }

		public string ExpectedApiKeyName { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime ExpiresAt { get; set; }

		public int AttemptCount { get; set; }

		public DateTime? LastAttemptAt { get; set; }

		public string? LastAttemptError { get; set; }

		public bool IsValid { get; set; }
	}
}
