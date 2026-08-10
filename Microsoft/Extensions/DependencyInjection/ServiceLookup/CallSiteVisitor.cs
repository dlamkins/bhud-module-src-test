using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal abstract class CallSiteVisitor<TArgument, TResult>
	{
		private readonly StackGuard _stackGuard;

		protected CallSiteVisitor()
		{
			_stackGuard = new StackGuard();
		}

		protected virtual TResult VisitCallSite(ServiceCallSite callSite, TArgument argument)
		{
			if (!_stackGuard.TryEnterOnCurrentStack())
			{
				return _stackGuard.RunOnEmptyStack(new Func<ServiceCallSite, TArgument, TResult>(VisitCallSite), callSite, argument);
			}
			return callSite.Cache.Location switch
			{
				CallSiteResultCacheLocation.Root => VisitRootCache(callSite, argument), 
				CallSiteResultCacheLocation.Scope => VisitScopeCache(callSite, argument), 
				CallSiteResultCacheLocation.Dispose => VisitDisposeCache(callSite, argument), 
				CallSiteResultCacheLocation.None => VisitNoCache(callSite, argument), 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}

		protected virtual TResult VisitCallSiteMain(ServiceCallSite callSite, TArgument argument)
		{
			return callSite.Kind switch
			{
				CallSiteKind.Factory => VisitFactory((FactoryCallSite)callSite, argument), 
				CallSiteKind.IEnumerable => VisitIEnumerable((IEnumerableCallSite)callSite, argument), 
				CallSiteKind.Constructor => VisitConstructor((ConstructorCallSite)callSite, argument), 
				CallSiteKind.Constant => VisitConstant((ConstantCallSite)callSite, argument), 
				CallSiteKind.ServiceProvider => VisitServiceProvider((ServiceProviderCallSite)callSite, argument), 
				_ => throw new NotSupportedException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.CallSiteTypeNotSupported, callSite.GetType())), 
			};
		}

		protected virtual TResult VisitNoCache(ServiceCallSite callSite, TArgument argument)
		{
			return VisitCallSiteMain(callSite, argument);
		}

		protected virtual TResult VisitDisposeCache(ServiceCallSite callSite, TArgument argument)
		{
			return VisitCallSiteMain(callSite, argument);
		}

		protected virtual TResult VisitRootCache(ServiceCallSite callSite, TArgument argument)
		{
			return VisitCallSiteMain(callSite, argument);
		}

		protected virtual TResult VisitScopeCache(ServiceCallSite callSite, TArgument argument)
		{
			return VisitCallSiteMain(callSite, argument);
		}

		protected abstract TResult VisitConstructor(ConstructorCallSite constructorCallSite, TArgument argument);

		protected abstract TResult VisitConstant(ConstantCallSite constantCallSite, TArgument argument);

		protected abstract TResult VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, TArgument argument);

		protected abstract TResult VisitIEnumerable(IEnumerableCallSite enumerableCallSite, TArgument argument);

		protected abstract TResult VisitFactory(FactoryCallSite factoryCallSite, TArgument argument);
	}
}
