using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAllianceGuildDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string? Tag { get; set; }

		public int MemberCount { get; set; }
	}
}
