using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ERequiresDynamicCode("Creates DynamicMethods")]
	internal sealed class ILEmitResolverBuilder : CallSiteVisitor<ILEmitResolverBuilderContext, object>
	{
		private sealed class ILEmitResolverBuilderRuntimeContext
		{
			public object[] Constants;

			public Func<IServiceProvider, object>[] Factories;
		}

		private struct GeneratedMethod
		{
			public Func<ServiceProviderEngineScope, object> Lambda;

			public ILEmitResolverBuilderRuntimeContext Context;

			public DynamicMethod DynamicMethod;
		}

		private static readonly MethodInfo ResolvedServicesGetter = typeof(ServiceProviderEngineScope).GetProperty("ResolvedServices", BindingFlags.Instance | BindingFlags.NonPublic).GetMethod;

		private static readonly MethodInfo ScopeLockGetter = typeof(ServiceProviderEngineScope).GetProperty("Sync", BindingFlags.Instance | BindingFlags.NonPublic).GetMethod;

		private static readonly MethodInfo ScopeIsRootScope = typeof(ServiceProviderEngineScope).GetProperty("IsRootScope", BindingFlags.Instance | BindingFlags.Public).GetMethod;

		private static readonly MethodInfo CallSiteRuntimeResolverResolveMethod = typeof(CallSiteRuntimeResolver).GetMethod("Resolve", BindingFlags.Instance | BindingFlags.Public);

		private static readonly MethodInfo CallSiteRuntimeResolverInstanceField = typeof(CallSiteRuntimeResolver).GetProperty("Instance", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).GetMethod;

		private static readonly FieldInfo FactoriesField = typeof(ILEmitResolverBuilderRuntimeContext).GetField("Factories");

		private static readonly FieldInfo ConstantsField = typeof(ILEmitResolverBuilderRuntimeContext).GetField("Constants");

		private static readonly MethodInfo GetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");

		private static readonly ConstructorInfo CacheKeyCtor = typeof(ServiceCacheKey).GetConstructors()[0];

		private readonly ServiceProviderEngineScope _rootScope;

		private readonly ConcurrentDictionary<ServiceCacheKey, GeneratedMethod> _scopeResolverCache;

		private readonly Func<ServiceCacheKey, ServiceCallSite, GeneratedMethod> _buildTypeDelegate;

		public ILEmitResolverBuilder(ServiceProvider serviceProvider)
		{
			_rootScope = serviceProvider.Root;
			_scopeResolverCache = new ConcurrentDictionary<ServiceCacheKey, GeneratedMethod>();
			_buildTypeDelegate = (ServiceCacheKey key, ServiceCallSite cs) => BuildTypeNoCache(cs);
		}

		public Func<ServiceProviderEngineScope, object?> Build(ServiceCallSite callSite)
		{
			return BuildType(callSite).Lambda;
		}

		private GeneratedMethod BuildType(ServiceCallSite callSite)
		{
			if (callSite.Cache.Location == CallSiteResultCacheLocation.Scope)
			{
				return _scopeResolverCache.GetOrAdd(callSite.Cache.Key, (ServiceCacheKey key) => _buildTypeDelegate(key, callSite));
			}
			return BuildTypeNoCache(callSite);
		}

		private GeneratedMethod BuildTypeNoCache(ServiceCallSite callSite)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ResolveService", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof(object), new Type[2]
			{
				typeof(ILEmitResolverBuilderRuntimeContext),
				typeof(ServiceProviderEngineScope)
			}, GetType(), skipVisibility: true);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator(512);
			ILEmitResolverBuilderRuntimeContext iLEmitResolverBuilderRuntimeContext = GenerateMethodBody(callSite, iLGenerator);
			DependencyInjectionEventSource.Log.DynamicMethodBuilt(_rootScope.RootProvider, callSite.ServiceType, iLGenerator.ILOffset);
			GeneratedMethod result = default(GeneratedMethod);
			result.Lambda = (Func<ServiceProviderEngineScope, object>)dynamicMethod.CreateDelegate(typeof(Func<ServiceProviderEngineScope, object>), iLEmitResolverBuilderRuntimeContext);
			result.Context = iLEmitResolverBuilderRuntimeContext;
			result.DynamicMethod = dynamicMethod;
			return result;
		}

		protected override object? VisitDisposeCache(ServiceCallSite transientCallSite, ILEmitResolverBuilderContext argument)
		{
			if (transientCallSite.CaptureDisposable)
			{
				BeginCaptureDisposable(argument);
				VisitCallSiteMain(transientCallSite, argument);
				EndCaptureDisposable(argument);
			}
			else
			{
				VisitCallSiteMain(transientCallSite, argument);
			}
			return null;
		}

		protected override object? VisitConstructor(ConstructorCallSite constructorCallSite, ILEmitResolverBuilderContext argument)
		{
			ServiceCallSite[] parameterCallSites = constructorCallSite.ParameterCallSites;
			foreach (ServiceCallSite serviceCallSite in parameterCallSites)
			{
				VisitCallSite(serviceCallSite, argument);
				if (serviceCallSite.ServiceType.IsValueType)
				{
					argument.Generator.Emit(OpCodes.Unbox_Any, serviceCallSite.ServiceType);
				}
			}
			argument.Generator.Emit(OpCodes.Newobj, constructorCallSite.ConstructorInfo);
			if (constructorCallSite.ImplementationType!.IsValueType)
			{
				argument.Generator.Emit(OpCodes.Box, constructorCallSite.ImplementationType);
			}
			return null;
		}

		protected override object? VisitRootCache(ServiceCallSite callSite, ILEmitResolverBuilderContext argument)
		{
			AddConstant(argument, CallSiteRuntimeResolver.Instance.Resolve(callSite, _rootScope));
			return null;
		}

		protected override object? VisitScopeCache(ServiceCallSite scopedCallSite, ILEmitResolverBuilderContext argument)
		{
			GeneratedMethod generatedMethod = BuildType(scopedCallSite);
			AddConstant(argument, generatedMethod.Context);
			argument.Generator.Emit(OpCodes.Ldarg_1);
			argument.Generator.Emit(OpCodes.Call, generatedMethod.DynamicMethod);
			return null;
		}

		protected override object? VisitConstant(ConstantCallSite constantCallSite, ILEmitResolverBuilderContext argument)
		{
			AddConstant(argument, constantCallSite.DefaultValue);
			return null;
		}

		protected override object? VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, ILEmitResolverBuilderContext argument)
		{
			argument.Generator.Emit(OpCodes.Ldarg_1);
			return null;
		}

		protected override object? VisitIEnumerable(IEnumerableCallSite enumerableCallSite, ILEmitResolverBuilderContext argument)
		{
			if (enumerableCallSite.ServiceCallSites.Length == 0)
			{
				argument.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.GetArrayEmptyMethodInfo(enumerableCallSite.ItemType));
			}
			else
			{
				argument.Generator.Emit(OpCodes.Ldc_I4, enumerableCallSite.ServiceCallSites.Length);
				argument.Generator.Emit(OpCodes.Newarr, enumerableCallSite.ItemType);
				for (int i = 0; i < enumerableCallSite.ServiceCallSites.Length; i++)
				{
					argument.Generator.Emit(OpCodes.Dup);
					argument.Generator.Emit(OpCodes.Ldc_I4, i);
					ServiceCallSite serviceCallSite = enumerableCallSite.ServiceCallSites[i];
					VisitCallSite(serviceCallSite, argument);
					if (serviceCallSite.ServiceType.IsValueType)
					{
						argument.Generator.Emit(OpCodes.Unbox_Any, serviceCallSite.ServiceType);
					}
					argument.Generator.Emit(OpCodes.Stelem, enumerableCallSite.ItemType);
				}
			}
			return null;
		}

		protected override object? VisitFactory(FactoryCallSite factoryCallSite, ILEmitResolverBuilderContext argument)
		{
			if (argument.Factories == null)
			{
				List<Func<IServiceProvider, object>> list2 = (argument.Factories = new List<Func<IServiceProvider, object>>());
			}
			argument.Generator.Emit(OpCodes.Ldarg_0);
			argument.Generator.Emit(OpCodes.Ldfld, FactoriesField);
			argument.Generator.Emit(OpCodes.Ldc_I4, argument.Factories!.Count);
			argument.Generator.Emit(OpCodes.Ldelem, typeof(Func<IServiceProvider, object>));
			argument.Generator.Emit(OpCodes.Ldarg_1);
			argument.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.InvokeFactoryMethodInfo);
			argument.Factories!.Add(factoryCallSite.Factory);
			return null;
		}

		private static void AddConstant(ILEmitResolverBuilderContext argument, object value)
		{
			if (argument.Constants == null)
			{
				List<object> list2 = (argument.Constants = new List<object>());
			}
			argument.Generator.Emit(OpCodes.Ldarg_0);
			argument.Generator.Emit(OpCodes.Ldfld, ConstantsField);
			argument.Generator.Emit(OpCodes.Ldc_I4, argument.Constants!.Count);
			argument.Generator.Emit(OpCodes.Ldelem, typeof(object));
			argument.Constants!.Add(value);
		}

		private static void AddCacheKey(ILEmitResolverBuilderContext argument, ServiceCacheKey key)
		{
			ServiceIdentifier serviceIdentifier = key.ServiceIdentifier;
			AddConstant(argument, serviceIdentifier.ServiceKey);
			argument.Generator.Emit(OpCodes.Ldtoken, serviceIdentifier.ServiceType);
			argument.Generator.Emit(OpCodes.Call, GetTypeFromHandleMethod);
			argument.Generator.Emit(OpCodes.Ldc_I4, key.Slot);
			argument.Generator.Emit(OpCodes.Newobj, CacheKeyCtor);
		}

		private ILEmitResolverBuilderRuntimeContext GenerateMethodBody(ServiceCallSite callSite, ILGenerator generator)
		{
			ILEmitResolverBuilderContext iLEmitResolverBuilderContext = new ILEmitResolverBuilderContext(generator)
			{
				Constants = null,
				Factories = null
			};
			if (callSite.Cache.Location == CallSiteResultCacheLocation.Scope)
			{
				LocalBuilder local = iLEmitResolverBuilderContext.Generator.DeclareLocal(typeof(ServiceCacheKey));
				LocalBuilder local2 = iLEmitResolverBuilderContext.Generator.DeclareLocal(typeof(IDictionary<ServiceCacheKey, object>));
				LocalBuilder local3 = iLEmitResolverBuilderContext.Generator.DeclareLocal(typeof(object));
				LocalBuilder local4 = iLEmitResolverBuilderContext.Generator.DeclareLocal(typeof(bool));
				LocalBuilder local5 = iLEmitResolverBuilderContext.Generator.DeclareLocal(typeof(object));
				Label label = iLEmitResolverBuilderContext.Generator.DefineLabel();
				Label label2 = iLEmitResolverBuilderContext.Generator.DefineLabel();
				Label label3 = iLEmitResolverBuilderContext.Generator.DefineLabel();
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldarg_1);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Callvirt, ScopeIsRootScope);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Brfalse_S, label3);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Call, CallSiteRuntimeResolverInstanceField);
				AddConstant(iLEmitResolverBuilderContext, callSite);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldarg_1);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Callvirt, CallSiteRuntimeResolverResolveMethod);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ret);
				iLEmitResolverBuilderContext.Generator.MarkLabel(label3);
				AddCacheKey(iLEmitResolverBuilderContext, callSite.Cache.Key);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Stloc, local);
				iLEmitResolverBuilderContext.Generator.BeginExceptionBlock();
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldarg_1);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Callvirt, ResolvedServicesGetter);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Stloc, local2);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldarg_1);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Callvirt, ScopeLockGetter);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Stloc, local3);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local3);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloca, local4);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.MonitorEnterMethodInfo);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local2);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloca, local5);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Callvirt, ServiceLookupHelpers.TryGetValueMethodInfo);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Brtrue, label);
				VisitCallSiteMain(callSite, iLEmitResolverBuilderContext);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Stloc, local5);
				if (callSite.CaptureDisposable)
				{
					BeginCaptureDisposable(iLEmitResolverBuilderContext);
					iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local5);
					EndCaptureDisposable(iLEmitResolverBuilderContext);
					generator.Emit(OpCodes.Pop);
				}
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local2);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local5);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Callvirt, ServiceLookupHelpers.AddMethodInfo);
				iLEmitResolverBuilderContext.Generator.MarkLabel(label);
				iLEmitResolverBuilderContext.Generator.BeginFinallyBlock();
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local4);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Brfalse, label2);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local3);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Call, ServiceLookupHelpers.MonitorExitMethodInfo);
				iLEmitResolverBuilderContext.Generator.MarkLabel(label2);
				iLEmitResolverBuilderContext.Generator.EndExceptionBlock();
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ldloc, local5);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ret);
			}
			else
			{
				VisitCallSite(callSite, iLEmitResolverBuilderContext);
				iLEmitResolverBuilderContext.Generator.Emit(OpCodes.Ret);
			}
			return new ILEmitResolverBuilderRuntimeContext
			{
				Constants = iLEmitResolverBuilderContext.Constants?.ToArray(),
				Factories = iLEmitResolverBuilderContext.Factories?.ToArray()
			};
		}

		private static void BeginCaptureDisposable(ILEmitResolverBuilderContext argument)
		{
			argument.Generator.Emit(OpCodes.Ldarg_1);
		}

		private static void EndCaptureDisposable(ILEmitResolverBuilderContext argument)
		{
			argument.Generator.Emit(OpCodes.Callvirt, ServiceLookupHelpers.CaptureDisposableMethodInfo);
		}
	}
}
