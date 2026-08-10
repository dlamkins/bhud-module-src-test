using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminGuildMemberDto
	{
		public Guid Id { get; set; }

		public string AccountName { get; set; }

		public string? RankName { get; set; }

		public DateTimeOffset JoinedAt { get; set; }

		public bool IsOnline { get; set; }
	}
}
