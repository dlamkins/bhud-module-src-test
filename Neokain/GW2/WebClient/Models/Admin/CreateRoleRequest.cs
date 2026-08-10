using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class CreateRoleRequest
	{
		public string Name { get; set; }

		public string? Description { get; set; }

		public List<string> Permissions { get; set; } = new List<string>();

	}
}
