using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Enums;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamFavoriteDto
	{
		public Guid Id { get; set; }

		public Guid SpamId { get; set; }

		public string SpamName { get; set; }

		public string? SpamDescription { get; set; }

		public DateTimeOffset? LastSpammed { get; set; }

		public string? CooldownFormatted { get; set; }

		public int CooldownSeconds { get; set; }

		public SpamSourceType SourceType { get; set; }

		public Guid? SourceGuildId { get; set; }

		public string? SourceGuildName { get; set; }

		public string? SourceGuildTag { get; set; }

		public Guid? SourceAllianceId { get; set; }

		public string? SourceAllianceName { get; set; }

		public string? SourceAllianceTag { get; set; }

		public int DisplayOrder { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public List<SpamMapHistoryDto> MapCooldowns { get; set; } = new List<SpamMapHistoryDto>();


		public bool HasMapLines { get; set; }
	}
}
