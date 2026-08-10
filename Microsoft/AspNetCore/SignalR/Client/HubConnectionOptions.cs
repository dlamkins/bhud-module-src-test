using System;

namespace Microsoft.AspNetCore.SignalR.Client
{
	internal sealed class HubConnectionOptions
	{
		public TimeSpan ServerTimeout { get; set; } = HubConnection.DefaultServerTimeout;


		public TimeSpan KeepAliveInterval { get; set; } = HubConnection.DefaultKeepAliveInterval;


		public long StatefulReconnectBufferSize { get; set; } = 100000L;

	}
}
