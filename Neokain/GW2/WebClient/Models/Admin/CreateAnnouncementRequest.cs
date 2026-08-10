using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class CreateAnnouncementRequest
	{
		public string Title { get; set; }

		public string Message { get; set; }

		public string Severity { get; set; } = "info";


		public bool IsPublic { get; set; }

		public DateTimeOffset? ExpiresAt { get; set; }
	}
}
