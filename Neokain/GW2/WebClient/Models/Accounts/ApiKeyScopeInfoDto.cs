using System.Collections.Generic;

namespace Neokain.GW2.WebClient.Models.Accounts
{
	internal class ApiKeyScopeInfoDto
	{
		public List<string> AvailableScopes { get; set; } = new List<string>();


		public List<string> MissingRequiredScopes { get; set; } = new List<string>();


		public List<string> MissingOptionalScopes { get; set; } = new List<string>();


		public bool HasAllRequiredScopes { get; set; }
	}
}
