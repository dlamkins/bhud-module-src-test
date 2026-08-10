using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAuditLogDto
	{
		public Guid Id { get; set; }

		public string? UserId { get; set; }

		public string? Username { get; set; }

		public string EntityType { get; set; }

		public string EntityId { get; set; }

		public string Action { get; set; }

		public DateTimeOffset Timestamp { get; set; }
	}
}
