using System;

namespace Neokain.GW2.WebClient.Models.Guilds.Stash
{
	public class GuildStashChangeFilterDto
	{
		public DateTimeOffset? From { get; set; }

		public DateTimeOffset? To { get; set; }

		public GuildStashChangeTypeDto[]? ChangeTypes { get; set; }

		public GuildStashChangeLocationDto[]? Locations { get; set; }

		public int? TabIndex { get; set; }

		public int Limit { get; set; } = 100;


		public int Offset { get; set; }
	}
}
