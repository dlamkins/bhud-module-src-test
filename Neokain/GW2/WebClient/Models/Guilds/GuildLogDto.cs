using System;
using System.Text.Json;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class GuildLogDto
	{
		public Guid Id { get; set; }

		public Guid GuildId { get; set; }

		public string GuildName { get; set; } = string.Empty;


		public string GuildTag { get; set; } = string.Empty;


		public DateTimeOffset Time { get; set; }

		public string Type { get; set; } = string.Empty;


		public string? User { get; set; }

		public JsonElement Data { get; set; }

		public GuildLogSourceDto Source { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
