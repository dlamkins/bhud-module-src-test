using System;

namespace Neokain.GW2.WebClient.Models.Alliances.Tags
{
	public class AllianceTagHealthCheckResultDto
	{
		public AllianceTagHealthCheckType Type { get; set; }

		public string MemberName { get; set; } = string.Empty;


		public Guid? TagId { get; set; }

		public string? TagTypeName { get; set; }

		public string? GuildName { get; set; }

		public string? ExpectedRank { get; set; }

		public string? ActualRank { get; set; }

		public DateTimeOffset? ExpirationDate { get; set; }

		public string Message { get; set; } = string.Empty;

	}
}
