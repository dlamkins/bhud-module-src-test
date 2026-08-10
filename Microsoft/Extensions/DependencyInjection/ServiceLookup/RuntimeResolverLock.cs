using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	[Flags]
	internal enum RuntimeResolverLock
	{
		Scope = 0x1,
		Root = 0x2
	}
}
