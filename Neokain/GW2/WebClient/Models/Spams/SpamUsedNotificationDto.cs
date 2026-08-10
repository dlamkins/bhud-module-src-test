using System;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamUsedNotificationDto
	{
		public Guid SpamId { get; set; }

		public DateTimeOffset UsedAt { get; set; }

		public string? UsedByAccountName { get; set; }

		public int? MapId { get; set; }
	}
}
