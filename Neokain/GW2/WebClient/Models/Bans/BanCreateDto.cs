using System;

namespace Neokain.GW2.WebClient.Models.Bans
{
	public class BanCreateDto
	{
		public string AccountName { get; set; }

		public string Reason { get; set; }

		public DateTimeOffset? Start { get; set; }

		public DateTimeOffset? Expiration { get; set; }
	}
}
