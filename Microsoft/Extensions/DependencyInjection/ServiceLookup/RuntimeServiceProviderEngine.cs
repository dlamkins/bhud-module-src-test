using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class RuntimeServiceProviderEngine : ServiceProviderEngine
	{
		public static RuntimeServiceProviderEngine Instance { get; } = new RuntimeServiceProviderEngine();


		private RuntimeServiceProviderEngine()
		{
		}

		public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
		{
			ServiceCallSite callSite2 = callSite;
			return (ServiceProviderEngineScope scope) => CallSiteRuntimeResolver.Instance.Resolve(callSite2, scope);
		}
	}
}
