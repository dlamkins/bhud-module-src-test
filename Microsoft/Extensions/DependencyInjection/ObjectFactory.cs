using System;

namespace Microsoft.Extensions.DependencyInjection
{
	internal delegate object ObjectFactory(IServiceProvider serviceProvider, object?[]? arguments);
	internal delegate T ObjectFactory<T>(IServiceProvider serviceProvider, object?[]? arguments);
}
