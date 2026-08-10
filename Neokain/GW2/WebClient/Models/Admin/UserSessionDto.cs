using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class UserSessionDto
	{
		public string Id { get; set; }

		public string? IpAddress { get; set; }

		public string? UserAgent { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset LastActiveAt { get; set; }
	}
}
