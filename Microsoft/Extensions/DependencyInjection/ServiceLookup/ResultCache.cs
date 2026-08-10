using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal struct ResultCache
	{
		public CallSiteResultCacheLocation Location { get; set; }

		public ServiceCacheKey Key { get; set; }

		public static ResultCache None(Type serviceType)
		{
			ServiceCacheKey cacheKey = new ServiceCacheKey(ServiceIdentifier.FromServiceType(serviceType), 0);
			return new ResultCache(CallSiteResultCacheLocation.None, cacheKey);
		}

		internal ResultCache(CallSiteResultCacheLocation lifetime, ServiceCacheKey cacheKey)
		{
			Location = lifetime;
			Key = cacheKey;
		}

		public ResultCache(ServiceLifetime lifetime, ServiceIdentifier serviceIdentifier, int slot)
		{
			switch (lifetime)
			{
			case ServiceLifetime.Singleton:
				Location = CallSiteResultCacheLocation.Root;
				break;
			case ServiceLifetime.Scoped:
				Location = CallSiteResultCacheLocation.Scope;
				break;
			case ServiceLifetime.Transient:
				Location = CallSiteResultCacheLocation.Dispose;
				break;
			default:
				Location = CallSiteResultCacheLocation.None;
				break;
			}
			Key = new ServiceCacheKey(serviceIdentifier, slot);
		}
	}
}
