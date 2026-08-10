using System;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class GuildHealthCheckResultDto
	{
		public GuildHealthCheckType Type { get; set; }

		public Guid GuildId { get; set; }

		public string GuildName { get; set; }

		public string Message { get; set; }

		public int CurrentValue { get; set; }

		public int ThresholdValue { get; set; }

		public string Severity { get; set; }

		public GuildHealthCheckResultDto()
		{
			GuildId = Guid.Empty;
			GuildName = string.Empty;
			Message = string.Empty;
			Severity = "Info";
		}

		public GuildHealthCheckResultDto(GuildHealthCheckType type, Guid guildId, string guildName, string message)
		{
			Type = type;
			GuildId = guildId;
			GuildName = guildName;
			Message = message;
			Severity = "Info";
		}
	}
}
