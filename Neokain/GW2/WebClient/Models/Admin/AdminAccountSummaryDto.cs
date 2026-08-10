using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAccountSummaryDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public bool Verified { get; set; }
	}
}
