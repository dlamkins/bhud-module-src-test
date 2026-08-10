using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class ServiceProviderKeyedServiceExtensions
	{
		public static T? GetKeyedService<T>(this IServiceProvider provider, object? serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			IKeyedServiceProvider keyedServiceProvider = provider as IKeyedServiceProvider;
			if (keyedServiceProvider != null)
			{
				return (T)keyedServiceProvider.GetKeyedService(typeof(T), serviceKey);
			}
			throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.KeyedServicesNotSupported);
		}

		public static object GetRequiredKeyedService(this IServiceProvider provider, Type serviceType, object? serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			IKeyedServiceProvider keyedServiceProvider = provider as IKeyedServiceProvider;
			if (keyedServiceProvider != null)
			{
				return keyedServiceProvider.GetRequiredKeyedService(serviceType, serviceKey);
			}
			throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.KeyedServicesNotSupported);
		}

		public static T GetRequiredKeyedService<T>(this IServiceProvider provider, object? serviceKey) where T : notnull
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			return (T)provider.GetRequiredKeyedService(typeof(T), serviceKey);
		}

		public static IEnumerable<T> GetKeyedServices<T>(this IServiceProvider provider, object? serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			return provider.GetRequiredKeyedService<IEnumerable<T>>(serviceKey);
		}

		[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ERequiresDynamicCode("The native code for an IEnumerable<serviceType> might not be available at runtime.")]
		public static IEnumerable<object?> GetKeyedServices(this IServiceProvider provider, Type serviceType, object? serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			Type serviceType2 = typeof(IEnumerable<>).MakeGenericType(serviceType);
			return (IEnumerable<object>)provider.GetRequiredKeyedService(serviceType2, serviceKey);
		}
	}
}
