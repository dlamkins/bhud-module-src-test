using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal readonly struct ServiceIdentifier : IEquatable<ServiceIdentifier>
	{
		public object? ServiceKey { get; }

		public Type ServiceType { get; }

		public bool IsConstructedGenericType => ServiceType.IsConstructedGenericType;

		public ServiceIdentifier(Type serviceType)
		{
			ServiceKey = null;
			ServiceType = serviceType;
		}

		public ServiceIdentifier(object? serviceKey, Type serviceType)
		{
			ServiceKey = serviceKey;
			ServiceType = serviceType;
		}

		public static ServiceIdentifier FromDescriptor(ServiceDescriptor serviceDescriptor)
		{
			return new ServiceIdentifier(serviceDescriptor.ServiceKey, serviceDescriptor.ServiceType);
		}

		public static ServiceIdentifier FromServiceType(Type type)
		{
			return new ServiceIdentifier(null, type);
		}

		public bool Equals(ServiceIdentifier other)
		{
			if (ServiceKey == null && other.ServiceKey == null)
			{
				return ServiceType == other.ServiceType;
			}
			if (ServiceKey != null && other.ServiceKey != null)
			{
				if (ServiceType == other.ServiceType)
				{
					return ServiceKey!.Equals(other.ServiceKey);
				}
				return false;
			}
			return false;
		}

		public override bool Equals([_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ENotNullWhen(true)] object? obj)
		{
			if (obj is ServiceIdentifier)
			{
				return Equals((ServiceIdentifier)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (ServiceKey == null)
			{
				return ServiceType.GetHashCode();
			}
			return (ServiceType.GetHashCode() * 397) ^ ServiceKey!.GetHashCode();
		}

		public ServiceIdentifier GetGenericTypeDefinition()
		{
			return new ServiceIdentifier(ServiceKey, ServiceType.GetGenericTypeDefinition());
		}

		public override string? ToString()
		{
			if (ServiceKey == null)
			{
				return ServiceType.ToString();
			}
			return $"({ServiceKey}, {ServiceType})";
		}
	}
}
