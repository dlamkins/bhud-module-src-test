using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal static class ServiceDescriptorExtensions
	{
		public static bool HasImplementationInstance(this ServiceDescriptor serviceDescriptor)
		{
			return serviceDescriptor.GetImplementationInstance() != null;
		}

		public static bool HasImplementationFactory(this ServiceDescriptor serviceDescriptor)
		{
			return serviceDescriptor.GetImplementationFactory() != null;
		}

		public static bool HasImplementationType(this ServiceDescriptor serviceDescriptor)
		{
			return GetImplementationType(serviceDescriptor) != null;
		}

		public static object? GetImplementationInstance(this ServiceDescriptor serviceDescriptor)
		{
			if (!serviceDescriptor.IsKeyedService)
			{
				return serviceDescriptor.ImplementationInstance;
			}
			return serviceDescriptor.KeyedImplementationInstance;
		}

		public static object? GetImplementationFactory(this ServiceDescriptor serviceDescriptor)
		{
			if (!serviceDescriptor.IsKeyedService)
			{
				return serviceDescriptor.ImplementationFactory;
			}
			return serviceDescriptor.KeyedImplementationFactory;
		}

		[return: _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMembers(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes.PublicConstructors)]
		public static Type? GetImplementationType(this ServiceDescriptor serviceDescriptor)
		{
			if (!serviceDescriptor.IsKeyedService)
			{
				return serviceDescriptor.ImplementationType;
			}
			return serviceDescriptor.KeyedImplementationType;
		}

		public static bool TryGetImplementationType(this ServiceDescriptor serviceDescriptor, out Type? type)
		{
			type = GetImplementationType(serviceDescriptor);
			return type != null;
		}
	}
}
