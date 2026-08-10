namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class UpdatePrimaryApiKeyResponseDto
	{
		public bool Success { get; set; }

		public string Message { get; set; }

		public ApiKeyScopeInfoDto? ScopeInfo { get; set; }

		public bool RequiresConfirmation { get; set; }
	}
}
