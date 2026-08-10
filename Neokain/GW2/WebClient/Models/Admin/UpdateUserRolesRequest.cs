using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Admin
{
	internal class UpdateUserRolesRequest
	{
		public List<string> Roles { get; set; } = new List<string>();

	}
}
