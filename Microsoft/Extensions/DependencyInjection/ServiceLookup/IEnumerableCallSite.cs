using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class IEnumerableCallSite : ServiceCallSite
	{
		internal Type ItemType { get; }

		internal ServiceCallSite[] ServiceCallSites { get; }

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode", Justification = "When ServiceProvider.VerifyAotCompatibility is true, which it is by default when PublishAot=true, CallSiteFactory ensures ItemType is not a ValueType.")]
		public override Type ServiceType => typeof(IEnumerable<>).MakeGenericType(ItemType);

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode", Justification = "When ServiceProvider.VerifyAotCompatibility is true, which it is by default when PublishAot=true, CallSiteFactory ensures ItemType is not a ValueType.")]
		public override Type ImplementationType => ItemType.MakeArrayType();

		public override CallSiteKind Kind { get; } = CallSiteKind.IEnumerable;


		public IEnumerableCallSite(ResultCache cache, Type itemType, ServiceCallSite[] serviceCallSites)
			: base(cache)
		{
			ItemType = itemType;
			ServiceCallSites = serviceCallSites;
		}
	}
}
