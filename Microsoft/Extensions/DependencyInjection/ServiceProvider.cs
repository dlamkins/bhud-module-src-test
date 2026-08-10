using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.ServiceLookup;

namespace Microsoft.Extensions.DependencyInjection
{
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	[DebuggerTypeProxy(typeof(ServiceProviderDebugView))]
	internal sealed class ServiceProvider : IServiceProvider, IKeyedServiceProvider, IDisposable, IAsyncDisposable
	{
		internal sealed class ServiceProviderDebugView
		{
			private readonly ServiceProvider _serviceProvider;

			public List<ServiceDescriptor> ServiceDescriptors => new List<ServiceDescriptor>(_serviceProvider.Root.RootProvider.CallSiteFactory.Descriptors);

			public List<object> Disposables => new List<object>(_serviceProvider.Root.Disposables);

			public bool Disposed => _serviceProvider.Root.Disposed;

			public bool IsScope => !_serviceProvider.Root.IsRootScope;

			public ServiceProviderDebugView(ServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider;
			}
		}

		private sealed class ServiceAccessor
		{
			public ServiceCallSite CallSite { get; set; }

			public Func<ServiceProviderEngineScope, object> RealizedService { get; set; }
		}

		private readonly CallSiteValidator _callSiteValidator;

		private readonly Func<ServiceIdentifier, ServiceAccessor> _createServiceAccessor;

		internal ServiceProviderEngine _engine;

		private bool _disposed;

		private readonly ConcurrentDictionary<ServiceIdentifier, ServiceAccessor> _serviceAccessors;

		internal static readonly bool s_allowNonKeyedServiceInject = AllowNonKeyedServiceInject;

		internal CallSiteFactory CallSiteFactory { get; }

		internal ServiceProviderEngineScope Root { get; }

		internal static bool VerifyOpenGenericServiceTrimmability { get; } = AppContext.TryGetSwitch("Microsoft.Extensions.DependencyInjection.VerifyOpenGenericServiceTrimmability", out var isEnabled) && isEnabled;


		internal static bool DisableDynamicEngine { get; } = AppContext.TryGetSwitch("Microsoft.Extensions.DependencyInjection.DisableDynamicEngine", out var isEnabled2) && isEnabled2;


		internal static bool AllowNonKeyedServiceInject { get; } = AppContext.TryGetSwitch("Microsoft.Extensions.DependencyInjection.AllowNonKeyedServiceInject", out var isEnabled3) && isEnabled3;


		internal static bool VerifyAotCompatibility => false;

		internal ServiceProvider(ICollection<ServiceDescriptor> serviceDescriptors, ServiceProviderOptions options)
		{
			Root = new ServiceProviderEngineScope(this, isRootScope: true);
			_engine = GetEngine();
			_createServiceAccessor = CreateServiceAccessor;
			_serviceAccessors = new ConcurrentDictionary<ServiceIdentifier, ServiceAccessor>();
			CallSiteFactory = new CallSiteFactory(serviceDescriptors);
			CallSiteFactory.Add(ServiceIdentifier.FromServiceType(typeof(IServiceProvider)), new ServiceProviderCallSite());
			CallSiteFactory.Add(ServiceIdentifier.FromServiceType(typeof(IServiceScopeFactory)), new ConstantCallSite(typeof(IServiceScopeFactory), Root));
			CallSiteFactory.Add(ServiceIdentifier.FromServiceType(typeof(IServiceProviderIsService)), new ConstantCallSite(typeof(IServiceProviderIsService), CallSiteFactory));
			CallSiteFactory.Add(ServiceIdentifier.FromServiceType(typeof(IServiceProviderIsKeyedService)), new ConstantCallSite(typeof(IServiceProviderIsKeyedService), CallSiteFactory));
			if (options.ValidateScopes)
			{
				_callSiteValidator = new CallSiteValidator();
			}
			if (options.ValidateOnBuild)
			{
				List<Exception> list = null;
				foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
				{
					try
					{
						ValidateService(serviceDescriptor);
					}
					catch (Exception item)
					{
						if (list == null)
						{
							list = new List<Exception>();
						}
						list.Add(item);
					}
				}
				if (list != null)
				{
					throw new AggregateException("Some services are not able to be constructed", list.ToArray());
				}
			}
			DependencyInjectionEventSource.Log.ServiceProviderBuilt(this);
		}

		public object? GetService(Type serviceType)
		{
			return GetService(ServiceIdentifier.FromServiceType(serviceType), Root);
		}

		public object? GetKeyedService(Type serviceType, object? serviceKey)
		{
			return GetKeyedService(serviceType, serviceKey, Root);
		}

		internal object? GetKeyedService(Type serviceType, object? serviceKey, ServiceProviderEngineScope serviceProviderEngineScope)
		{
			return GetService(new ServiceIdentifier(serviceKey, serviceType), serviceProviderEngineScope);
		}

		public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
		{
			return GetRequiredKeyedService(serviceType, serviceKey, Root);
		}

		internal object GetRequiredKeyedService(Type serviceType, object? serviceKey, ServiceProviderEngineScope serviceProviderEngineScope)
		{
			object keyedService = GetKeyedService(serviceType, serviceKey, serviceProviderEngineScope);
			if (keyedService == null)
			{
				throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.NoServiceRegistered, serviceType));
			}
			return keyedService;
		}

		internal bool IsDisposed()
		{
			return _disposed;
		}

		public void Dispose()
		{
			DisposeCore();
			Root.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			DisposeCore();
			return Root.DisposeAsync();
		}

		private void DisposeCore()
		{
			_disposed = true;
			DependencyInjectionEventSource.Log.ServiceProviderDisposed(this);
		}

		private void OnCreate(ServiceCallSite callSite)
		{
			_callSiteValidator?.ValidateCallSite(callSite);
		}

		private void OnResolve(ServiceCallSite callSite, IServiceScope scope)
		{
			if (callSite != null)
			{
				_callSiteValidator?.ValidateResolution(callSite, scope, Root);
			}
		}

		internal object? GetService(ServiceIdentifier serviceIdentifier, ServiceProviderEngineScope serviceProviderEngineScope)
		{
			if (_disposed)
			{
				ThrowHelper.ThrowObjectDisposedException();
			}
			ServiceAccessor orAdd = _serviceAccessors.GetOrAdd(serviceIdentifier, _createServiceAccessor);
			OnResolve(orAdd.CallSite, serviceProviderEngineScope);
			DependencyInjectionEventSource.Log.ServiceResolved(this, serviceIdentifier.ServiceType);
			return orAdd.RealizedService?.Invoke(serviceProviderEngineScope);
		}

		private void ValidateService(ServiceDescriptor descriptor)
		{
			if (descriptor.ServiceType.IsGenericType && !descriptor.ServiceType.IsConstructedGenericType)
			{
				return;
			}
			try
			{
				ServiceCallSite callSite = CallSiteFactory.GetCallSite(descriptor, new CallSiteChain());
				if (callSite != null)
				{
					OnCreate(callSite);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Error while validating the service descriptor '{descriptor}': {ex.Message}", ex);
			}
		}

		private ServiceAccessor CreateServiceAccessor(ServiceIdentifier serviceIdentifier)
		{
			ServiceCallSite callSite = CallSiteFactory.GetCallSite(serviceIdentifier, new CallSiteChain());
			if (callSite != null)
			{
				DependencyInjectionEventSource.Log.CallSiteBuilt(this, serviceIdentifier.ServiceType, callSite);
				OnCreate(callSite);
				if (callSite.Cache.Location == CallSiteResultCacheLocation.Root)
				{
					object value = CallSiteRuntimeResolver.Instance.Resolve(callSite, Root);
					return new ServiceAccessor
					{
						CallSite = callSite,
						RealizedService = (ServiceProviderEngineScope scope) => value
					};
				}
				Func<ServiceProviderEngineScope, object> realizedService = _engine.RealizeService(callSite);
				return new ServiceAccessor
				{
					CallSite = callSite,
					RealizedService = realizedService
				};
			}
			return new ServiceAccessor
			{
				CallSite = callSite,
				RealizedService = (ServiceProviderEngineScope _) => null
			};
		}

		internal void ReplaceServiceAccessor(ServiceCallSite callSite, Func<ServiceProviderEngineScope, object?> accessor)
		{
			_serviceAccessors[new ServiceIdentifier(callSite.Key, callSite.ServiceType)] = new ServiceAccessor
			{
				CallSite = callSite,
				RealizedService = accessor
			};
		}

		internal IServiceScope CreateScope()
		{
			if (_disposed)
			{
				ThrowHelper.ThrowObjectDisposedException();
			}
			return new ServiceProviderEngineScope(this, isRootScope: false);
		}

		private ServiceProviderEngine GetEngine()
		{
			return CreateDynamicEngine();
			[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode", Justification = "CreateDynamicEngine won't be called when using NativeAOT.")]
			ServiceProviderEngine CreateDynamicEngine()
			{
				return new DynamicServiceProviderEngine(this);
			}
		}

		private string DebuggerToString()
		{
			return Root.DebuggerToString();
		}
	}
}
