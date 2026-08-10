using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class ConstructorCallSite : ServiceCallSite
	{
		internal ConstructorInfo ConstructorInfo { get; }

		internal ServiceCallSite[] ParameterCallSites { get; }

		public override Type ServiceType { get; }

		public override Type? ImplementationType => ConstructorInfo.DeclaringType;

		public override CallSiteKind Kind { get; } = CallSiteKind.Constructor;


		public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo)
			: this(cache, serviceType, constructorInfo, Array.Empty<ServiceCallSite>())
		{
		}

		public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo, ServiceCallSite[] parameterCallSites)
			: base(cache)
		{
			if (!serviceType.IsAssignableFrom(constructorInfo.DeclaringType))
			{
				throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.ImplementationTypeCantBeConvertedToServiceType, constructorInfo.DeclaringType, serviceType));
			}
			ServiceType = serviceType;
			ConstructorInfo = constructorInfo;
			ParameterCallSites = parameterCallSites;
		}
	}
}
