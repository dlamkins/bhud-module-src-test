using System;
using Neokain.GW2.WebClient.Models.Enums;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamFavoriteCreateDto
	{
		public Guid SpamId { get; set; }

		public SpamSourceType SourceType { get; set; }

		public Guid? SourceGuildId { get; set; }

		public Guid? SourceAllianceId { get; set; }

		public Guid? SourceAccountId { get; set; }
	}
}
