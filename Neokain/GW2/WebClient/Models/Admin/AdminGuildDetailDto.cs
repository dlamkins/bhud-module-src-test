using System;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminGuildDetailDto : AdminGuildDto
	{
		public string? Motd { get; set; }

		public int Level { get; set; }

		public int Favor { get; set; }

		public int Aetherium { get; set; }

		public int Influence { get; set; }

		public Guid? OwnerId { get; set; }

		public string? OwnerAccountName { get; set; }
	}
}
