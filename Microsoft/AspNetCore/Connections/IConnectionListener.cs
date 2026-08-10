using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections
{
	internal interface IConnectionListener : IAsyncDisposable
	{
		EndPoint EndPoint { get; }

		ValueTask<ConnectionContext?> AcceptAsync(CancellationToken cancellationToken = default(CancellationToken));

		ValueTask UnbindAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
