using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class DynamicServiceProviderEngine : CompiledServiceProviderEngine
	{
		private readonly ServiceProvider _serviceProvider;

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ERequiresDynamicCode("Creates DynamicMethods")]
		public DynamicServiceProviderEngine(ServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
		{
			ServiceCallSite callSite2 = callSite;
			int callCount = 0;
			return delegate(ServiceProviderEngineScope scope)
			{
				object result = CallSiteRuntimeResolver.Instance.Resolve(callSite2, scope);
				if (Interlocked.Increment(ref callCount) == 2)
				{
					ThreadPool.UnsafeQueueUserWorkItem(delegate
					{
						try
						{
							_serviceProvider.ReplaceServiceAccessor(callSite2, base.RealizeService(callSite2));
						}
						catch (Exception exception)
						{
							DependencyInjectionEventSource.Log.ServiceRealizationFailed(exception, _serviceProvider.GetHashCode());
						}
					}, null);
				}
				return result;
			};
		}
	}
}
