using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal abstract class CompiledServiceProviderEngine : ServiceProviderEngine
	{
		public ILEmitResolverBuilder ResolverBuilder { get; }

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ERequiresDynamicCode("Creates DynamicMethods")]
		public CompiledServiceProviderEngine(ServiceProvider provider)
		{
			ResolverBuilder = new ILEmitResolverBuilder(provider);
		}

		public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
		{
			return ResolverBuilder.Build(callSite);
		}
	}
}
