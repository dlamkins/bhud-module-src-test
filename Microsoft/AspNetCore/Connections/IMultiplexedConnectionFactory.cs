using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Connections
{
	internal interface IMultiplexedConnectionFactory
	{
		ValueTask<MultiplexedConnectionContext> ConnectAsync(EndPoint endpoint, IFeatureCollection? features = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
