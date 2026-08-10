namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class GetPendingVerificationResponseDto
	{
		public bool HasPending { get; set; }

		public PendingVerificationDto? Verification { get; set; }
	}
}
