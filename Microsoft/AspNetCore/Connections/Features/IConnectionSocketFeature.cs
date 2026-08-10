using System.Net.Sockets;

namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IConnectionSocketFeature
	{
		Socket Socket { get; }
	}
}
