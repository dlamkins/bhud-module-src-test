using System;

namespace Neokain.GW2.WebClient.Models.Alliances
{
	public class AllianceRankDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public int Position { get; set; }

		public string Description { get; set; } = string.Empty;

	}
}
