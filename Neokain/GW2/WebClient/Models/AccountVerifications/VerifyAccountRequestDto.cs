namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class VerifyAccountRequestDto
	{
		public string AccountName { get; set; }

		public string ApiKey { get; set; }

		public bool ConfirmMissingScopes { get; set; }
	}
}
