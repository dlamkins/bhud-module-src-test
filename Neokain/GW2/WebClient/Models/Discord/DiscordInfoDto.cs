using System;

namespace Neokain.GW2.WebClient.Models.Discord
{
	internal class DiscordInfoDto
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string Discriminator { get; set; }

		public string AvatarUrl { get; set; }

		public string GlobalName { get; set; }

		public DateTime? LinkedAt { get; set; }
	}
}
