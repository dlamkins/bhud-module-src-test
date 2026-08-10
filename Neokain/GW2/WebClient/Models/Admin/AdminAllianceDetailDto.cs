using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminAllianceDetailDto : AdminAllianceDto
	{
		public string? Description { get; set; }

		public Guid? OwnerId { get; set; }

		public string? OwnerAccountName { get; set; }

		public List<AdminAllianceGuildDto> Guilds { get; set; } = new List<AdminAllianceGuildDto>();

	}
}
