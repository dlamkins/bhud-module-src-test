using System;
using Neokain.GW2.WebClient.Models.Enums;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamUsageLogDto
	{
		public Guid Id { get; set; }

		public Guid SpamId { get; set; }

		public DateTimeOffset UsedAt { get; set; }

		public Guid? UsedByAccountId { get; set; }

		public string? UsedByAccountName { get; set; }

		public int? MapId { get; set; }

		public string? MapName { get; set; }

		public SpamSourceType ContextType { get; set; }

		public Guid ContextId { get; set; }
	}
}
