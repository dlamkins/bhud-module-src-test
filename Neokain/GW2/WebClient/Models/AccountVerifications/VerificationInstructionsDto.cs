namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class VerificationInstructionsDto
	{
		public string Step1 { get; set; } = "Log in to your GW2 account";


		public string Step2 { get; set; } = "Go to: Account Settings ? API Keys";


		public string? Step3 { get; set; }

		public string Step4 { get; set; } = "Select permissions: Account, Progression (minimum required)";


		public string Step5 { get; set; } = "Copy the generated API key";


		public string Step6 { get; set; } = "Submit the API key via the /api/accountverification/verify endpoint";

	}
}
