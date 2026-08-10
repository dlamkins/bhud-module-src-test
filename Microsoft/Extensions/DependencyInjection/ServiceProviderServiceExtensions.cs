using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class ServiceProviderServiceExtensions
	{
		public static T? GetService<T>(this IServiceProvider provider)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			return (T)provider.GetService(typeof(T));
		}

		public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			ISupportRequiredService supportRequiredService = provider as ISupportRequiredService;
			if (supportRequiredService != null)
			{
				return supportRequiredService.GetRequiredService(serviceType);
			}
			object service = provider.GetService(serviceType);
			if (service == null)
			{
				throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.NoServiceRegistered, serviceType));
			}
			return service;
		}

		public static T GetRequiredService<T>(this IServiceProvider provider) where T : notnull
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			return (T)provider.GetRequiredService(typeof(T));
		}

		public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			return provider.GetRequiredService<IEnumerable<T>>();
		}

		[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ERequiresDynamicCode("The native code for an IEnumerable<serviceType> might not be available at runtime.")]
		public static IEnumerable<object?> GetServices(this IServiceProvider provider, Type serviceType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			Type serviceType2 = typeof(IEnumerable<>).MakeGenericType(serviceType);
			return (IEnumerable<object>)provider.GetRequiredService(serviceType2);
		}

		public static IServiceScope CreateScope(this IServiceProvider provider)
		{
			return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
		}

		public static AsyncServiceScope CreateAsyncScope(this IServiceProvider provider)
		{
			return new AsyncServiceScope(provider.CreateScope());
		}

		public static AsyncServiceScope CreateAsyncScope(this IServiceScopeFactory serviceScopeFactory)
		{
			return new AsyncServiceScope(serviceScopeFactory.CreateScope());
		}
	}
}
