namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class SystemConfigDto
	{
		public bool MaintenanceMode { get; set; }

		public bool RegistrationEnabled { get; set; }

		public int MaxApiKeysPerUser { get; set; }

		public int SyncIntervalMinutes { get; set; }

		public int SessionTimeoutMinutes { get; set; }
	}
}
