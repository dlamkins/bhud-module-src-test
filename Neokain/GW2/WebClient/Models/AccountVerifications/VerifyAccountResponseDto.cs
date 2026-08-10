using Neokain.GW2.WebClient.Models.Accounts;

namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class VerifyAccountResponseDto
	{
		public bool Success { get; set; } = true;


		public string Message { get; set; } = "Account successfully verified and linked";


		public VerifiedAccountDto? Account { get; set; }

		public ApiKeyScopeInfoDto? ScopeInfo { get; set; }

		public bool RequiresConfirmation { get; set; }
	}
}
