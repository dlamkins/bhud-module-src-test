using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class ServiceCollectionServiceExtensions
	{
		public static IServiceCollection AddTransient(this IServiceCollection services, Type serviceType, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationType, "implementationType");
			return Add(services, serviceType, implementationType, ServiceLifetime.Transient);
		}

		public static IServiceCollection AddTransient(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return Add(services, serviceType, implementationFactory, ServiceLifetime.Transient);
		}

		public static IServiceCollection AddTransient<TService, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddTransient(typeof(TService), typeof(TImplementation));
		}

		public static IServiceCollection AddTransient(this IServiceCollection services, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			return services.AddTransient(serviceType, serviceType);
		}

		public static IServiceCollection AddTransient<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddTransient(typeof(TService));
		}

		public static IServiceCollection AddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddTransient(typeof(TService), implementationFactory);
		}

		public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddTransient(typeof(TService), implementationFactory);
		}

		public static IServiceCollection AddScoped(this IServiceCollection services, Type serviceType, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationType, "implementationType");
			return Add(services, serviceType, implementationType, ServiceLifetime.Scoped);
		}

		public static IServiceCollection AddScoped(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return Add(services, serviceType, implementationFactory, ServiceLifetime.Scoped);
		}

		public static IServiceCollection AddScoped<TService, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddScoped(typeof(TService), typeof(TImplementation));
		}

		public static IServiceCollection AddScoped(this IServiceCollection services, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			return services.AddScoped(serviceType, serviceType);
		}

		public static IServiceCollection AddScoped<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddScoped(typeof(TService));
		}

		public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddScoped(typeof(TService), implementationFactory);
		}

		public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddScoped(typeof(TService), implementationFactory);
		}

		public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationType, "implementationType");
			return Add(services, serviceType, implementationType, ServiceLifetime.Singleton);
		}

		public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return Add(services, serviceType, implementationFactory, ServiceLifetime.Singleton);
		}

		public static IServiceCollection AddSingleton<TService, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddSingleton(typeof(TService), typeof(TImplementation));
		}

		public static IServiceCollection AddSingleton(this IServiceCollection services, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			return services.AddSingleton(serviceType, serviceType);
		}

		public static IServiceCollection AddSingleton<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddSingleton(typeof(TService));
		}

		public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddSingleton(typeof(TService), implementationFactory);
		}

		public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddSingleton(typeof(TService), implementationFactory);
		}

		public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, object implementationInstance)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
			ServiceDescriptor item = new ServiceDescriptor(serviceType, implementationInstance);
			services.Add(item);
			return services;
		}

		public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, TService implementationInstance) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
			return services.AddSingleton(typeof(TService), implementationInstance);
		}

		private static IServiceCollection Add(IServiceCollection collection, Type serviceType, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
		{
			ServiceDescriptor item = new ServiceDescriptor(serviceType, implementationType, lifetime);
			collection.Add(item);
			return collection;
		}

		private static IServiceCollection Add(IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
		{
			ServiceDescriptor item = new ServiceDescriptor(serviceType, implementationFactory, lifetime);
			collection.Add(item);
			return collection;
		}

		public static IServiceCollection AddKeyedTransient(this IServiceCollection services, Type serviceType, object? serviceKey, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationType, "implementationType");
			return AddKeyed(services, serviceType, serviceKey, implementationType, ServiceLifetime.Transient);
		}

		public static IServiceCollection AddKeyedTransient(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return AddKeyed(services, serviceType, serviceKey, implementationFactory, ServiceLifetime.Transient);
		}

		public static IServiceCollection AddKeyedTransient<TService, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object? serviceKey) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddKeyedTransient(typeof(TService), serviceKey, typeof(TImplementation));
		}

		public static IServiceCollection AddKeyedTransient(this IServiceCollection services, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType, object? serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			return services.AddKeyedTransient(serviceType, serviceKey, serviceType);
		}

		public static IServiceCollection AddKeyedTransient<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object? serviceKey) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddKeyedTransient(typeof(TService), serviceKey);
		}

		public static IServiceCollection AddKeyedTransient<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddKeyedTransient(typeof(TService), serviceKey, implementationFactory);
		}

		public static IServiceCollection AddKeyedTransient<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddKeyedTransient(typeof(TService), serviceKey, implementationFactory);
		}

		public static IServiceCollection AddKeyedScoped(this IServiceCollection services, Type serviceType, object? serviceKey, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationType, "implementationType");
			return AddKeyed(services, serviceType, serviceKey, implementationType, ServiceLifetime.Scoped);
		}

		public static IServiceCollection AddKeyedScoped(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return AddKeyed(services, serviceType, serviceKey, implementationFactory, ServiceLifetime.Scoped);
		}

		public static IServiceCollection AddKeyedScoped<TService, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object? serviceKey) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddKeyedScoped(typeof(TService), serviceKey, typeof(TImplementation));
		}

		public static IServiceCollection AddKeyedScoped(this IServiceCollection services, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType, object? serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			return services.AddKeyedScoped(serviceType, serviceKey, serviceType);
		}

		public static IServiceCollection AddKeyedScoped<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object? serviceKey) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddKeyedScoped(typeof(TService), serviceKey);
		}

		public static IServiceCollection AddKeyedScoped<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddKeyedScoped(typeof(TService), serviceKey, implementationFactory);
		}

		public static IServiceCollection AddKeyedScoped<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddKeyedScoped(typeof(TService), serviceKey, implementationFactory);
		}

		public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, Type serviceType, object? serviceKey, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationType, "implementationType");
			return AddKeyed(services, serviceType, serviceKey, implementationType, ServiceLifetime.Singleton);
		}

		public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return AddKeyed(services, serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton);
		}

		public static IServiceCollection AddKeyedSingleton<TService, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object? serviceKey) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddKeyedSingleton(typeof(TService), serviceKey, typeof(TImplementation));
		}

		public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType, object? serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			return services.AddKeyedSingleton(serviceType, serviceKey, serviceType);
		}

		public static IServiceCollection AddKeyedSingleton<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object? serviceKey) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			return services.AddKeyedSingleton(typeof(TService), serviceKey, typeof(TService));
		}

		public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddKeyedSingleton(typeof(TService), serviceKey, implementationFactory);
		}

		public static IServiceCollection AddKeyedSingleton<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
			return services.AddKeyedSingleton(typeof(TService), serviceKey, implementationFactory);
		}

		public static IServiceCollection AddKeyedSingleton(this IServiceCollection services, Type serviceType, object? serviceKey, object implementationInstance)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceType, "serviceType");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
			ServiceDescriptor item = new ServiceDescriptor(serviceType, serviceKey, implementationInstance);
			services.Add(item);
			return services;
		}

		public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection services, object? serviceKey, TService implementationInstance) where TService : class
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(services, "services");
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
			return services.AddKeyedSingleton(typeof(TService), serviceKey, implementationInstance);
		}

		private static IServiceCollection AddKeyed(IServiceCollection collection, Type serviceType, object serviceKey, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
		{
			ServiceDescriptor item = new ServiceDescriptor(serviceType, serviceKey, implementationType, lifetime);
			collection.Add(item);
			return collection;
		}

		private static IServiceCollection AddKeyed(IServiceCollection collection, Type serviceType, object serviceKey, Func<IServiceProvider, object, object> implementationFactory, ServiceLifetime lifetime)
		{
			ServiceDescriptor item = new ServiceDescriptor(serviceType, serviceKey, implementationFactory, lifetime);
			collection.Add(item);
			return collection;
		}
	}
}
