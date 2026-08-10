using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Connections
{
	internal interface IMultiplexedConnectionListener : IAsyncDisposable
	{
		EndPoint EndPoint { get; }

		ValueTask UnbindAsync(CancellationToken cancellationToken = default(CancellationToken));

		ValueTask<MultiplexedConnectionContext?> AcceptAsync(IFeatureCollection? features = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
