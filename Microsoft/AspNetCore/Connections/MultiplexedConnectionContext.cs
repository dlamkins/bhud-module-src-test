using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Connections
{
	internal abstract class MultiplexedConnectionContext : BaseConnectionContext, IAsyncDisposable
	{
		public abstract ValueTask<ConnectionContext?> AcceptAsync(CancellationToken cancellationToken = default(CancellationToken));

		public abstract ValueTask<ConnectionContext> ConnectAsync(IFeatureCollection? features = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
