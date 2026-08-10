using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class GuildHealthSummaryDto
	{
		public Guid GuildId { get; set; }

		public string GuildName { get; set; }

		public int MemberCapacity { get; set; }

		public int TotalMemberCount { get; set; }

		public int InvitedCount { get; set; }

		public int ActiveMemberCount { get; set; }

		public int OpenSlots { get; set; }

		public string? LastSyncedAt { get; set; }

		public bool IsHealthy { get; set; }

		public List<GuildHealthCheckResultDto> Issues { get; set; }

		public GuildHealthSummaryDto()
		{
			GuildId = Guid.Empty;
			GuildName = string.Empty;
			LastSyncedAt = null;
			Issues = new List<GuildHealthCheckResultDto>();
		}
	}
}
