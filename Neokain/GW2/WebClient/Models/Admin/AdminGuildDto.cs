using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminGuildDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string? Tag { get; set; }

		public int MemberCount { get; set; }

		public int MemberCapacity { get; set; }

		public string? LeaderName { get; set; }

		public bool IsVerified { get; set; }

		public DateTimeOffset? LastSyncAt { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public Guid? AllianceId { get; set; }

		public string? AllianceName { get; set; }
	}
}
