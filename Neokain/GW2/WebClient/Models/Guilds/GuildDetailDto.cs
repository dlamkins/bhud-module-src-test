using System;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	public class GuildDetailDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Tag { get; set; } = string.Empty;


		public string? Description { get; set; }

		public int OfficerRankOrder { get; set; }

		public string? LeaderName { get; set; }

		public int MemberCount { get; set; }

		public DateTimeOffset? MembersLastSyncedAt { get; set; }

		public DateTimeOffset? StashLastSyncedAt { get; set; }

		public DateTimeOffset? TreasuryLastSyncedAt { get; set; }

		public DateTimeOffset? StorageLastSyncedAt { get; set; }

		public DateTimeOffset? AutoLastSyncedAt { get; set; }

		public DateTimeOffset? ManualLastSyncedAt { get; set; }
	}
}
