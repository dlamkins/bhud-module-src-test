using System;

namespace Neokain.GW2.WebClient.Models.Guilds
{
	internal class UpdateGuildDto
	{
		public Guid? LeaderId { get; set; }

		public string? Name { get; set; }

		public string? Tag { get; set; }

		public string? Description { get; set; }
	}
}
