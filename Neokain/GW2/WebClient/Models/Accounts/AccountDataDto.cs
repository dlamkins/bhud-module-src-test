using System;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	public class AccountDataDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int World { get; set; }

		public bool Commander { get; set; }

		public DateTimeOffset Age { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? ModifiedAt { get; set; }

		public DateTimeOffset? LastFetched { get; set; }
	}
}
