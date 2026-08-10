using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAllianceDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string? Tag { get; set; }

		public int GuildCount { get; set; }

		public int MemberCount { get; set; }

		public string? LeaderName { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
