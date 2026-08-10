using System;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class GuildLogFilterDto
	{
		public DateTimeOffset? From { get; set; }

		public DateTimeOffset? To { get; set; }

		public string[]? Types { get; set; }

		public string? User { get; set; }

		public GuildLogSourceDto[]? Sources { get; set; }

		public int Limit { get; set; } = 100;


		public int Offset { get; set; }
	}
}
