using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections
{
	internal interface IConnectionListenerFactory
	{
		ValueTask<IConnectionListener> BindAsync(EndPoint endpoint, CancellationToken cancellationToken = default(CancellationToken));
	}
}
