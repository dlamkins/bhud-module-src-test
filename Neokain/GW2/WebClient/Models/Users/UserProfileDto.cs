using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Discord;

namespace Neokain.GW2.WebClient.Models.Users
{
	internal class UserProfileDto
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public DiscordInfoDto Discord { get; set; } = new DiscordInfoDto();


		public List<LinkedAccountSummaryDto> Accounts { get; set; } = new List<LinkedAccountSummaryDto>();

	}
}
