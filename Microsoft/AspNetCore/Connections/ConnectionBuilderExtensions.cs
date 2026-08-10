using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Connections
{
	internal static class ConnectionBuilderExtensions
	{
		public static IConnectionBuilder UseConnectionHandler<[_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMembers(_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TConnectionHandler>(this IConnectionBuilder connectionBuilder) where TConnectionHandler : ConnectionHandler
		{
			TConnectionHandler serviceOrCreateInstance = ActivatorUtilities.GetServiceOrCreateInstance<TConnectionHandler>(connectionBuilder.ApplicationServices);
			return connectionBuilder.Run(new Func<ConnectionContext, Task>(serviceOrCreateInstance.OnConnectedAsync));
		}

		public static IConnectionBuilder Use(this IConnectionBuilder connectionBuilder, Func<ConnectionContext, Func<Task>, Task> middleware)
		{
			Func<ConnectionContext, Func<Task>, Task> middleware2 = middleware;
			return connectionBuilder.Use((ConnectionDelegate next) => delegate(ConnectionContext context)
			{
				Func<Task> arg = () => next(context);
				return middleware2(context, arg);
			});
		}

		public static IConnectionBuilder Use(this IConnectionBuilder connectionBuilder, Func<ConnectionContext, ConnectionDelegate, Task> middleware)
		{
			Func<ConnectionContext, ConnectionDelegate, Task> middleware2 = middleware;
			return connectionBuilder.Use((ConnectionDelegate next) => (ConnectionContext context) => middleware2(context, next));
		}

		public static IConnectionBuilder Run(this IConnectionBuilder connectionBuilder, Func<ConnectionContext, Task> middleware)
		{
			Func<ConnectionContext, Task> middleware2 = middleware;
			return connectionBuilder.Use((ConnectionDelegate next) => (ConnectionContext context) => middleware2(context));
		}
	}
}
