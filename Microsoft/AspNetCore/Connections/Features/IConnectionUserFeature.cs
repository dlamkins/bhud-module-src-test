using System.Security.Claims;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionUserFeature
	{
		ClaimsPrincipal? User { get; set; }
	}
}
