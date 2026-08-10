using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminUserDto
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string? Email { get; set; }

		public string? DiscordId { get; set; }

		public string? AvatarUrl { get; set; }

		public bool IsBlocked { get; set; }

		public DateTimeOffset? LastLoginAt { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public List<string> Roles { get; set; } = new List<string>();

	}
}
