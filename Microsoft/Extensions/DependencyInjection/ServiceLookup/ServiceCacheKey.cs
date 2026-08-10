using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal readonly struct ServiceCacheKey : IEquatable<ServiceCacheKey>
	{
		public ServiceIdentifier ServiceIdentifier { get; }

		public int Slot { get; }

		public ServiceCacheKey(object key, Type type, int slot)
		{
			ServiceIdentifier = new ServiceIdentifier(key, type);
			Slot = slot;
		}

		public ServiceCacheKey(ServiceIdentifier type, int slot)
		{
			ServiceIdentifier = type;
			Slot = slot;
		}

		public bool Equals(ServiceCacheKey other)
		{
			if (ServiceIdentifier.Equals(other.ServiceIdentifier))
			{
				return Slot == other.Slot;
			}
			return false;
		}

		public override bool Equals([_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ENotNullWhen(true)] object? obj)
		{
			if (obj is ServiceCacheKey)
			{
				ServiceCacheKey other = (ServiceCacheKey)obj;
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (ServiceIdentifier.GetHashCode() * 397) ^ Slot;
		}
	}
}
