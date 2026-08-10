using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class UpdateRoleRequest
	{
		public string? Name { get; set; }

		public string? Description { get; set; }

		public List<string>? Permissions { get; set; }
	}
}
