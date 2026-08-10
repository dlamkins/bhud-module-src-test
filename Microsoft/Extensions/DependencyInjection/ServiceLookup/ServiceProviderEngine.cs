using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal abstract class ServiceProviderEngine
	{
		public abstract Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite);
	}
}
