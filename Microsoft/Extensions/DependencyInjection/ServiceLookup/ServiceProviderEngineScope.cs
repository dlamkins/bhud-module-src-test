using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	[DebuggerTypeProxy(typeof(ServiceProviderEngineScopeDebugView))]
	internal sealed class ServiceProviderEngineScope : IServiceScope, IDisposable, IServiceProvider, IKeyedServiceProvider, IAsyncDisposable, IServiceScopeFactory
	{
		private sealed class ServiceProviderEngineScopeDebugView
		{
			private readonly ServiceProviderEngineScope _serviceProvider;

			public List<ServiceDescriptor> ServiceDescriptors => new List<ServiceDescriptor>(_serviceProvider.RootProvider.CallSiteFactory.Descriptors);

			public List<object> Disposables => new List<object>(_serviceProvider.Disposables);

			public bool Disposed => _serviceProvider._disposed;

			public bool IsScope => !_serviceProvider.IsRootScope;

			public ServiceProviderEngineScopeDebugView(ServiceProviderEngineScope serviceProvider)
			{
				_serviceProvider = serviceProvider;
			}
		}

		private bool _disposed;

		private List<object> _disposables;

		internal IList<object> Disposables
		{
			get
			{
				IList<object> disposables = _disposables;
				return disposables ?? Array.Empty<object>();
			}
		}

		internal Dictionary<ServiceCacheKey, object?> ResolvedServices { get; }

		internal bool Disposed => _disposed;

		internal object Sync => ResolvedServices;

		public bool IsRootScope { get; }

		internal ServiceProvider RootProvider { get; }

		public IServiceProvider ServiceProvider => this;

		public ServiceProviderEngineScope(ServiceProvider provider, bool isRootScope)
		{
			ResolvedServices = new Dictionary<ServiceCacheKey, object>();
			RootProvider = provider;
			IsRootScope = isRootScope;
		}

		public object? GetService(Type serviceType)
		{
			if (_disposed)
			{
				ThrowHelper.ThrowObjectDisposedException();
			}
			return RootProvider.GetService(ServiceIdentifier.FromServiceType(serviceType), this);
		}

		public object? GetKeyedService(Type serviceType, object? serviceKey)
		{
			if (_disposed)
			{
				ThrowHelper.ThrowObjectDisposedException();
			}
			return RootProvider.GetKeyedService(serviceType, serviceKey, this);
		}

		public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
		{
			if (_disposed)
			{
				ThrowHelper.ThrowObjectDisposedException();
			}
			return RootProvider.GetRequiredKeyedService(serviceType, serviceKey, this);
		}

		public IServiceScope CreateScope()
		{
			return RootProvider.CreateScope();
		}

		[return: _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ENotNullIfNotNull("service")]
		internal object? CaptureDisposable(object? service)
		{
			if (this == service || (!(service is IDisposable) && !(service is IAsyncDisposable)))
			{
				return service;
			}
			bool flag = false;
			lock (Sync)
			{
				if (_disposed)
				{
					flag = true;
				}
				else
				{
					if (_disposables == null)
					{
						_disposables = new List<object>();
					}
					_disposables.Add(service);
				}
			}
			if (flag)
			{
				IDisposable disposable = service as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				else
				{
					object localService = service;
					Task.Run(() => ((IAsyncDisposable)localService).DisposeAsync().AsTask()).GetAwaiter().GetResult();
				}
				ThrowHelper.ThrowObjectDisposedException();
			}
			return service;
		}

		public void Dispose()
		{
			List<object> list = BeginDispose();
			if (list == null)
			{
				return;
			}
			int num = list.Count - 1;
			while (num >= 0)
			{
				IDisposable disposable = list[num] as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
					num--;
					continue;
				}
				throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.AsyncDisposableServiceDispose, _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ETypeNameHelper.GetTypeDisplayName(list[num])));
			}
		}

		public ValueTask DisposeAsync()
		{
			List<object> list = BeginDispose();
			if (list != null)
			{
				try
				{
					for (int num = list.Count - 1; num >= 0; num--)
					{
						object obj = list[num];
						IAsyncDisposable asyncDisposable = obj as IAsyncDisposable;
						if (asyncDisposable != null)
						{
							ValueTask vt2 = asyncDisposable.DisposeAsync();
							if (!vt2.IsCompletedSuccessfully)
							{
								return Await(num, vt2, list);
							}
							vt2.GetAwaiter().GetResult();
						}
						else
						{
							((IDisposable)obj).Dispose();
						}
					}
				}
				catch (Exception exception)
				{
					return new ValueTask(Task.FromException(exception));
				}
			}
			return default(ValueTask);
			static async ValueTask Await(int i, ValueTask vt, List<object> toDispose)
			{
				await vt.ConfigureAwait(continueOnCapturedContext: false);
				i--;
				while (i >= 0)
				{
					object obj2 = toDispose[i];
					IAsyncDisposable asyncDisposable2 = obj2 as IAsyncDisposable;
					if (asyncDisposable2 != null)
					{
						await asyncDisposable2.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						((IDisposable)obj2).Dispose();
					}
					i--;
				}
			}
		}

		private List<object> BeginDispose()
		{
			lock (Sync)
			{
				if (_disposed)
				{
					return null;
				}
				DependencyInjectionEventSource.Log.ScopeDisposed(RootProvider.GetHashCode(), ResolvedServices.Count, _disposables?.Count ?? 0);
				_disposed = true;
			}
			if (IsRootScope && !RootProvider.IsDisposed())
			{
				RootProvider.Dispose();
			}
			return _disposables;
		}

		internal string DebuggerToString()
		{
			string text = $"ServiceDescriptors = {RootProvider.CallSiteFactory.Descriptors.Length}";
			if (!IsRootScope)
			{
				text += ", IsScope = true";
			}
			if (_disposed)
			{
				text += ", Disposed = true";
			}
			return text;
		}
	}
}
