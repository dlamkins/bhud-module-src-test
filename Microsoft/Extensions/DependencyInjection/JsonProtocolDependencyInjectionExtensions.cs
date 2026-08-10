using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class JsonProtocolDependencyInjectionExtensions
	{
		public static TBuilder AddJsonProtocol<TBuilder>(this TBuilder builder) where TBuilder : ISignalRBuilder
		{
			return builder.AddJsonProtocol(delegate
			{
			});
		}

		public static TBuilder AddJsonProtocol<TBuilder>(this TBuilder builder, Action<JsonHubProtocolOptions> configure) where TBuilder : ISignalRBuilder
		{
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHubProtocol, JsonHubProtocol>());
			builder.Services.Configure(configure);
			return builder;
		}
	}
}
