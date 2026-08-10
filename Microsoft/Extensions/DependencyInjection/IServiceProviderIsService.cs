using System;

namespace Microsoft.Extensions.DependencyInjection
{
	internal interface IServiceProviderIsService
	{
		bool IsService(Type serviceType);
	}
}
