using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Connections
{
	internal abstract class BaseConnectionContext : IAsyncDisposable
	{
		public abstract string ConnectionId { get; set; }

		public abstract IFeatureCollection Features { get; }

		public abstract IDictionary<object, object?> Items { get; set; }

		public virtual CancellationToken ConnectionClosed { get; set; }

		public virtual EndPoint? LocalEndPoint { get; set; }

		public virtual EndPoint? RemoteEndPoint { get; set; }

		public abstract void Abort();

		public abstract void Abort(ConnectionAbortedException abortReason);

		public virtual ValueTask DisposeAsync()
		{
			return default(ValueTask);
		}
	}
}
