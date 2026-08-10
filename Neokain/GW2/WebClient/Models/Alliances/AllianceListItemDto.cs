using System;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	internal class AllianceListItemDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Tag { get; set; } = string.Empty;


		public int MemberCount { get; set; }

		public int GuildCount { get; set; }
	}
}
