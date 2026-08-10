namespace Neokain.GW2.WebClient.Models.Connection
{
	public class ServerHandshakeResponseDto
	{
		public bool Success { get; set; }

		public string ServerVersion { get; set; }

		public string? LatestClientVersion { get; set; }

		public bool UpdateAvailable { get; set; }

		public string? DownloadUrl { get; set; }

		public bool CurrentVersionSecurityRisk { get; set; }

		public string? SecurityNote { get; set; }

		public bool CurrentVersionCompatible { get; set; } = true;


		public string? CompatibilityMessage { get; set; }
	}
}
