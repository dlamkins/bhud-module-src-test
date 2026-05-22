using System;
using Newtonsoft.Json;

namespace TyriaPlanner.Hud.Api
{
	public sealed class Announcement
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("guildId")]
		public string GuildId { get; set; }

		[JsonProperty("guildName")]
		public string GuildName { get; set; }

		[JsonProperty("guildTag")]
		public string GuildTag { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }

		[JsonProperty("isFeatured")]
		public bool IsFeatured { get; set; }

		[JsonProperty("senderUsername")]
		public string SenderUsername { get; set; }

		[JsonProperty("senderDisplayName")]
		public string SenderDisplayName { get; set; }

		[JsonProperty("senderAccountName")]
		public string SenderAccountName { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }
	}
}
