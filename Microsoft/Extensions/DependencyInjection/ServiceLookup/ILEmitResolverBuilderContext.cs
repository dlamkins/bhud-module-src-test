using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class ILEmitResolverBuilderContext
	{
		public ILGenerator Generator { get; }

		public List<object?>? Constants { get; set; }

		public List<Func<IServiceProvider, object>>? Factories { get; set; }

		public ILEmitResolverBuilderContext(ILGenerator generator)
		{
			Generator = generator;
		}
	}
}
