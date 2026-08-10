using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminUserDetailDto : AdminUserDto
	{
		public List<AdminAccountSummaryDto> Accounts { get; set; } = new List<AdminAccountSummaryDto>();


		public string? BlockReason { get; set; }

		public DateTimeOffset? BlockedAt { get; set; }

		public string? BlockedBy { get; set; }
	}
}
