using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class ILEmitServiceProviderEngine : ServiceProviderEngine
	{
		private readonly ILEmitResolverBuilder _expressionResolverBuilder;

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ERequiresDynamicCode("Creates DynamicMethods")]
		public ILEmitServiceProviderEngine(ServiceProvider serviceProvider)
		{
			_expressionResolverBuilder = new ILEmitResolverBuilder(serviceProvider);
		}

		public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
		{
			return _expressionResolverBuilder.Build(callSite);
		}
	}
}
