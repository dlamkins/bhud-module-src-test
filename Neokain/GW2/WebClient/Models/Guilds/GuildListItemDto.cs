using System;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class GuildListItemDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Tag { get; set; } = string.Empty;


		public int MemberCount { get; set; }
	}
}
