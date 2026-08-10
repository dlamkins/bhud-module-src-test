using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class CallSiteRuntimeResolver : CallSiteVisitor<RuntimeResolverContext, object>
	{
		public static CallSiteRuntimeResolver Instance { get; } = new CallSiteRuntimeResolver();


		private CallSiteRuntimeResolver()
		{
		}

		public object? Resolve(ServiceCallSite callSite, ServiceProviderEngineScope scope)
		{
			if (scope.IsRootScope)
			{
				object value = callSite.Value;
				if (value != null)
				{
					return value;
				}
			}
			return VisitCallSite(callSite, new RuntimeResolverContext
			{
				Scope = scope
			});
		}

		protected override object? VisitDisposeCache(ServiceCallSite transientCallSite, RuntimeResolverContext context)
		{
			return context.Scope.CaptureDisposable(VisitCallSiteMain(transientCallSite, context));
		}

		protected override object VisitConstructor(ConstructorCallSite constructorCallSite, RuntimeResolverContext context)
		{
			object[] array;
			if (constructorCallSite.ParameterCallSites.Length == 0)
			{
				array = Array.Empty<object>();
			}
			else
			{
				array = new object[constructorCallSite.ParameterCallSites.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = VisitCallSite(constructorCallSite.ParameterCallSites[i], context);
				}
			}
			try
			{
				return constructorCallSite.ConstructorInfo.Invoke(array);
			}
			catch (Exception ex) when (ex.InnerException != null)
			{
				ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
				throw;
			}
		}

		protected override object? VisitRootCache(ServiceCallSite callSite, RuntimeResolverContext context)
		{
			object value = callSite.Value;
			if (value != null)
			{
				return value;
			}
			RuntimeResolverLock runtimeResolverLock = RuntimeResolverLock.Root;
			ServiceProviderEngineScope root = context.Scope.RootProvider.Root;
			lock (callSite)
			{
				object value2 = callSite.Value;
				if (value2 != null)
				{
					return value2;
				}
				object obj = VisitCallSiteMain(callSite, new RuntimeResolverContext
				{
					Scope = root,
					AcquiredLocks = (context.AcquiredLocks | runtimeResolverLock)
				});
				root.CaptureDisposable(obj);
				callSite.Value = obj;
				return obj;
			}
		}

		protected override object? VisitScopeCache(ServiceCallSite callSite, RuntimeResolverContext context)
		{
			if (!context.Scope.IsRootScope)
			{
				return VisitCache(callSite, context, context.Scope, RuntimeResolverLock.Scope);
			}
			return VisitRootCache(callSite, context);
		}

		private object VisitCache(ServiceCallSite callSite, RuntimeResolverContext context, ServiceProviderEngineScope serviceProviderEngine, RuntimeResolverLock lockType)
		{
			bool lockTaken = false;
			object sync = serviceProviderEngine.Sync;
			Dictionary<ServiceCacheKey, object> resolvedServices = serviceProviderEngine.ResolvedServices;
			if ((context.AcquiredLocks & lockType) == 0)
			{
				Monitor.Enter(sync, ref lockTaken);
			}
			try
			{
				if (resolvedServices.TryGetValue(callSite.Cache.Key, out var value))
				{
					return value;
				}
				value = VisitCallSiteMain(callSite, new RuntimeResolverContext
				{
					Scope = serviceProviderEngine,
					AcquiredLocks = (context.AcquiredLocks | lockType)
				});
				serviceProviderEngine.CaptureDisposable(value);
				resolvedServices.Add(callSite.Cache.Key, value);
				return value;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(sync);
				}
			}
		}

		protected override object? VisitConstant(ConstantCallSite constantCallSite, RuntimeResolverContext context)
		{
			return constantCallSite.DefaultValue;
		}

		protected override object VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, RuntimeResolverContext context)
		{
			return context.Scope;
		}

		protected override object VisitIEnumerable(IEnumerableCallSite enumerableCallSite, RuntimeResolverContext context)
		{
			Array array = CreateArray(enumerableCallSite.ItemType, enumerableCallSite.ServiceCallSites.Length);
			for (int i = 0; i < enumerableCallSite.ServiceCallSites.Length; i++)
			{
				object value = VisitCallSite(enumerableCallSite.ServiceCallSites[i], context);
				array.SetValue(value, i);
			}
			return array;
			[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode", Justification = "VerifyAotCompatibility ensures elementType is not a ValueType")]
			static Array CreateArray(Type elementType, int length)
			{
				return Array.CreateInstance(elementType, length);
			}
		}

		protected override object VisitFactory(FactoryCallSite factoryCallSite, RuntimeResolverContext context)
		{
			return factoryCallSite.Factory(context.Scope);
		}
	}
}
