using System;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamMapHistoryDto
	{
		public int MapId { get; set; }

		public string? MapName { get; set; }

		public DateTimeOffset? LastSpammed { get; set; }
	}
}
