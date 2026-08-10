using System;

namespace Neokain.GW2.WebClient.Models.Spams
{
	public class SpamDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTimeOffset? LastSpammed { get; set; }

		public string? CooldownFormatted { get; set; }

		public Guid? CategoryId { get; set; }

		public string? CategoryName { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset? ModifiedAt { get; set; }

		public Guid? CreatedById { get; set; }

		public Guid? ModifiedById { get; set; }
	}
}
