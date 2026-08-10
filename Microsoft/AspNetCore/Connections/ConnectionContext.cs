using System;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Connections.Features;

namespace Microsoft.AspNetCore.Connections
{
	internal abstract class ConnectionContext : BaseConnectionContext, IAsyncDisposable
	{
		public abstract IDuplexPipe Transport { get; set; }

		public override void Abort(ConnectionAbortedException abortReason)
		{
			Features.Get<IConnectionLifetimeFeature>()?.Abort();
		}

		public override void Abort()
		{
			Abort(new ConnectionAbortedException("The connection was aborted by the application via ConnectionContext.Abort()."));
		}
	}
}
