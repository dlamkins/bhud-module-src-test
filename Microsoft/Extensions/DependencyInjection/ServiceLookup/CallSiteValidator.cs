using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class CallSiteValidator : CallSiteVisitor<CallSiteValidator.CallSiteValidatorState, Type>
	{
		internal struct CallSiteValidatorState
		{
			public ServiceCallSite? Singleton
			{
				get; [param: _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDisallowNull]
				set;
			}
		}

		private readonly ConcurrentDictionary<ServiceCacheKey, Type> _scopedServices = new ConcurrentDictionary<ServiceCacheKey, Type>();

		public void ValidateCallSite(ServiceCallSite callSite)
		{
			Type type = VisitCallSite(callSite, default(CallSiteValidatorState));
			if (type != null)
			{
				_scopedServices[callSite.Cache.Key] = type;
			}
		}

		public void ValidateResolution(ServiceCallSite callSite, IServiceScope scope, IServiceScope rootScope)
		{
			if (scope == rootScope && _scopedServices.TryGetValue(callSite.Cache.Key, out var value))
			{
				Type serviceType = callSite.ServiceType;
				if (serviceType == value)
				{
					throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.DirectScopedResolvedFromRootException, callSite.ServiceType, "Scoped".ToLowerInvariant()));
				}
				throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.ScopedResolvedFromRootException, callSite.ServiceType, value, "Scoped".ToLowerInvariant()));
			}
		}

		protected override Type? VisitConstructor(ConstructorCallSite constructorCallSite, CallSiteValidatorState state)
		{
			Type type = null;
			ServiceCallSite[] parameterCallSites = constructorCallSite.ParameterCallSites;
			foreach (ServiceCallSite callSite in parameterCallSites)
			{
				Type type2 = VisitCallSite(callSite, state);
				if ((object)type == null)
				{
					type = type2;
				}
			}
			return type;
		}

		protected override Type? VisitIEnumerable(IEnumerableCallSite enumerableCallSite, CallSiteValidatorState state)
		{
			Type type = null;
			ServiceCallSite[] serviceCallSites = enumerableCallSite.ServiceCallSites;
			foreach (ServiceCallSite callSite in serviceCallSites)
			{
				Type type2 = VisitCallSite(callSite, state);
				if ((object)type == null)
				{
					type = type2;
				}
			}
			return type;
		}

		protected override Type? VisitRootCache(ServiceCallSite singletonCallSite, CallSiteValidatorState state)
		{
			state.Singleton = singletonCallSite;
			return VisitCallSiteMain(singletonCallSite, state);
		}

		protected override Type? VisitScopeCache(ServiceCallSite scopedCallSite, CallSiteValidatorState state)
		{
			if (scopedCallSite.ServiceType == typeof(IServiceScopeFactory))
			{
				return null;
			}
			if (state.Singleton != null)
			{
				throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.ScopedInSingletonException, scopedCallSite.ServiceType, state.Singleton!.ServiceType, "Scoped".ToLowerInvariant(), "Singleton".ToLowerInvariant()));
			}
			VisitCallSiteMain(scopedCallSite, state);
			return scopedCallSite.ServiceType;
		}

		protected override Type? VisitConstant(ConstantCallSite constantCallSite, CallSiteValidatorState state)
		{
			return null;
		}

		protected override Type? VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, CallSiteValidatorState state)
		{
			return null;
		}

		protected override Type? VisitFactory(FactoryCallSite factoryCallSite, CallSiteValidatorState state)
		{
			return null;
		}
	}
}
