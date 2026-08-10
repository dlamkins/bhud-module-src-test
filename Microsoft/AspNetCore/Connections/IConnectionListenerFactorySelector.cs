using System.Net;

namespace Microsoft.AspNetCore.Connections
{
	internal interface IConnectionListenerFactorySelector
	{
		bool CanBind(EndPoint endpoint);
	}
}
