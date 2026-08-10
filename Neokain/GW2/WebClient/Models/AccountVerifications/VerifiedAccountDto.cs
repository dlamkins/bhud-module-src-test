using System;

namespace Neokain.GW2.WebClient.Models.AccountVerifications
{
	internal class VerifiedAccountDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int World { get; set; }

		public DateTimeOffset Age { get; set; }

		public bool Commander { get; set; }
	}
}
