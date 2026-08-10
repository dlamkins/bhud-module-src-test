using System;

namespace Microsoft.Extensions.DependencyInjection
{
	internal interface IServiceProviderIsKeyedService : IServiceProviderIsService
	{
		bool IsKeyedService(Type serviceType, object? serviceKey);
	}
}
