namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class InitiateVerificationResponseDto
	{
		public bool Success { get; set; } = true;


		public string Message { get; set; } = "Verification initiated successfully";


		public VerificationDetailsDto Verification { get; set; }
	}
}
