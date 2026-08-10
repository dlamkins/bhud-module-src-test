using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAnnouncementDto
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Message { get; set; }

		public string Severity { get; set; } = "info";


		public bool Active { get; set; }

		public bool IsPublic { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? ExpiresAt { get; set; }
	}
}
