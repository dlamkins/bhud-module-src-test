namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class UpdatePrimaryApiKeyRequestDto
	{
		public string ApiKey { get; set; }

		public string? Description { get; set; }

		public bool ConfirmMissingScopes { get; set; }
	}
}
