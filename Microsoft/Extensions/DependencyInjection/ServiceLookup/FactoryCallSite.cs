using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class FactoryCallSite : ServiceCallSite
	{
		public Func<IServiceProvider, object> Factory { get; }

		public override Type ServiceType { get; }

		public override Type? ImplementationType => null;

		public override CallSiteKind Kind { get; }

		public FactoryCallSite(ResultCache cache, Type serviceType, Func<IServiceProvider, object> factory)
			: base(cache)
		{
			Factory = factory;
			ServiceType = serviceType;
		}

		public FactoryCallSite(ResultCache cache, Type serviceType, object serviceKey, Func<IServiceProvider, object, object> factory)
		{
			Func<IServiceProvider, object, object> factory2 = factory;
			object serviceKey2 = serviceKey;
			base._002Ector(cache);
			Factory = (IServiceProvider sp) => factory2(sp, serviceKey2);
			ServiceType = serviceType;
		}
	}
}
