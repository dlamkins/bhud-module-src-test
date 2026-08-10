using System;
using Microsoft.AspNetCore.SignalR.Client.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.SignalR.Client
{
	internal static class HubConnectionBuilderExtensions
	{
		public static IHubConnectionBuilder ConfigureLogging(this IHubConnectionBuilder hubConnectionBuilder, Action<ILoggingBuilder> configureLogging)
		{
			hubConnectionBuilder.Services.AddLogging(configureLogging);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithAutomaticReconnect(this IHubConnectionBuilder hubConnectionBuilder)
		{
			hubConnectionBuilder.Services.AddSingleton((IRetryPolicy)new DefaultRetryPolicy());
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithAutomaticReconnect(this IHubConnectionBuilder hubConnectionBuilder, TimeSpan[] reconnectDelays)
		{
			hubConnectionBuilder.Services.AddSingleton((IRetryPolicy)new DefaultRetryPolicy(reconnectDelays));
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithAutomaticReconnect(this IHubConnectionBuilder hubConnectionBuilder, IRetryPolicy retryPolicy)
		{
			hubConnectionBuilder.Services.AddSingleton(retryPolicy);
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithServerTimeout(this IHubConnectionBuilder hubConnectionBuilder, TimeSpan timeout)
		{
			hubConnectionBuilder.Services.Configure(delegate(HubConnectionOptions o)
			{
				o.ServerTimeout = timeout;
			});
			return hubConnectionBuilder;
		}

		public static IHubConnectionBuilder WithKeepAliveInterval(this IHubConnectionBuilder hubConnectionBuilder, TimeSpan interval)
		{
			hubConnectionBuilder.Services.Configure(delegate(HubConnectionOptions o)
			{
				o.KeepAliveInterval = interval;
			});
			return hubConnectionBuilder;
		}
	}
}
