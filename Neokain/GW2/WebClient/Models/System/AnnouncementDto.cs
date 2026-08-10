using System;

namespace Neokain.GW2.WebClient.Models.System
{
	internal class AnnouncementDto
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Message { get; set; }

		public string Severity { get; set; } = "info";


		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? ExpiresAt { get; set; }
	}
}
