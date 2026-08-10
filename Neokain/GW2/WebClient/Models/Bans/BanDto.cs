using System;

namespace Neokain.GW2.WebClient.Models.Bans
{
	public class BanDto
	{
		public Guid Id { get; set; }

		public Guid AccountId { get; set; }

		public string AccountName { get; set; }

		public string Reason { get; set; }

		public DateTimeOffset? Start { get; set; }

		public DateTimeOffset? Expiration { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? ModifiedAt { get; set; }

		public Guid? CreatedById { get; set; }

		public Guid? ModifiedById { get; set; }
	}
}
