using System;

namespace Neokain.GW2.WebClient.Models.System
{
	internal class PublicSystemStatusDto
	{
		public bool MaintenanceMode { get; set; }

		public string? MaintenanceMessage { get; set; }

		public bool RegistrationEnabled { get; set; }

		public DateTimeOffset ServerTime { get; set; }
	}
}
