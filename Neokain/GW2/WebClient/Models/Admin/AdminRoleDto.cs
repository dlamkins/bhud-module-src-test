using System;
using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class AdminRoleDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public List<string> Permissions { get; set; } = new List<string>();


		public int UserCount { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}
