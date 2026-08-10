using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Connections
{
	internal class DefaultConnectionContext : ConnectionContext, IConnectionIdFeature, IConnectionItemsFeature, IConnectionTransportFeature, IConnectionUserFeature, IConnectionLifetimeFeature, IConnectionEndPointFeature
	{
		private readonly CancellationTokenSource _connectionClosedTokenSource = new CancellationTokenSource();

		public override string ConnectionId { get; set; }

		public override IFeatureCollection Features { get; }

		public ClaimsPrincipal? User { get; set; }

		public override IDictionary<object, object?> Items { get; set; } = new ConnectionItems();


		public IDuplexPipe? Application { get; set; }

		public override IDuplexPipe Transport { get; set; }

		public override CancellationToken ConnectionClosed { get; set; }

		public override EndPoint? LocalEndPoint { get; set; }

		public override EndPoint? RemoteEndPoint { get; set; }

		public DefaultConnectionContext()
			: this(Guid.NewGuid().ToString())
		{
		}

		public DefaultConnectionContext(string id)
		{
			ConnectionId = id;
			Features = new FeatureCollection();
			Features.Set((IConnectionUserFeature?)this);
			Features.Set((IConnectionItemsFeature?)this);
			Features.Set((IConnectionIdFeature?)this);
			Features.Set((IConnectionTransportFeature?)this);
			Features.Set((IConnectionLifetimeFeature?)this);
			Features.Set((IConnectionEndPointFeature?)this);
			ConnectionClosed = _connectionClosedTokenSource.Token;
		}

		public DefaultConnectionContext(string id, IDuplexPipe transport, IDuplexPipe application)
			: this(id)
		{
			Transport = transport;
			Application = application;
		}

		public override void Abort(ConnectionAbortedException abortReason)
		{
			ThreadPool.UnsafeQueueUserWorkItem(delegate(object cts)
			{
				((CancellationTokenSource)cts).Cancel();
			}, _connectionClosedTokenSource);
		}

		public override ValueTask DisposeAsync()
		{
			_connectionClosedTokenSource.Dispose();
			return base.DisposeAsync();
		}
	}
}
