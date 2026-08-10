using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class ExpressionResolverBuilder : CallSiteVisitor<object, Expression>
	{
		private static readonly ParameterExpression ScopeParameter = Expression.Parameter(typeof(ServiceProviderEngineScope));

		private static readonly ParameterExpression ResolvedServices = Expression.Variable(typeof(IDictionary<ServiceCacheKey, object>), ScopeParameter.Name + "resolvedServices");

		private static readonly ParameterExpression Sync = Expression.Variable(typeof(object), ScopeParameter.Name + "sync");

		private static readonly BinaryExpression ResolvedServicesVariableAssignment = Expression.Assign(ResolvedServices, Expression.Property(ScopeParameter, typeof(ServiceProviderEngineScope).GetProperty("ResolvedServices", BindingFlags.Instance | BindingFlags.NonPublic)));

		private static readonly BinaryExpression SyncVariableAssignment = Expression.Assign(Sync, Expression.Property(ScopeParameter, typeof(ServiceProviderEngineScope).GetProperty("Sync", BindingFlags.Instance | BindingFlags.NonPublic)));

		private static readonly ParameterExpression CaptureDisposableParameter = Expression.Parameter(typeof(object));

		private static readonly LambdaExpression CaptureDisposable = Expression.Lambda(Expression.Call(ScopeParameter, ServiceLookupHelpers.CaptureDisposableMethodInfo, CaptureDisposableParameter), CaptureDisposableParameter);

		private static readonly ConstantExpression CallSiteRuntimeResolverInstanceExpression = Expression.Constant(CallSiteRuntimeResolver.Instance, typeof(CallSiteRuntimeResolver));

		private readonly ServiceProviderEngineScope _rootScope;

		private readonly ConcurrentDictionary<ServiceCacheKey, Func<ServiceProviderEngineScope, object>> _scopeResolverCache;

		private readonly Func<ServiceCacheKey, ServiceCallSite, Func<ServiceProviderEngineScope, object>> _buildTypeDelegate;

		public ExpressionResolverBuilder(ServiceProvider serviceProvider)
		{
			_rootScope = serviceProvider.Root;
			_scopeResolverCache = new ConcurrentDictionary<ServiceCacheKey, Func<ServiceProviderEngineScope, object>>();
			_buildTypeDelegate = (ServiceCacheKey key, ServiceCallSite cs) => BuildNoCache(cs);
		}

		public Func<ServiceProviderEngineScope, object> Build(ServiceCallSite callSite)
		{
			ServiceCallSite callSite2 = callSite;
			if (callSite2.Cache.Location == CallSiteResultCacheLocation.Scope)
			{
				return _scopeResolverCache.GetOrAdd(callSite2.Cache.Key, (ServiceCacheKey key) => _buildTypeDelegate(key, callSite2));
			}
			return BuildNoCache(callSite2);
		}

		public Func<ServiceProviderEngineScope, object> BuildNoCache(ServiceCallSite callSite)
		{
			Expression<Func<ServiceProviderEngineScope, object>> expression = BuildExpression(callSite);
			DependencyInjectionEventSource.Log.ExpressionTreeGenerated(_rootScope.RootProvider, callSite.ServiceType, expression);
			return expression.Compile();
		}

		private Expression<Func<ServiceProviderEngineScope, object>> BuildExpression(ServiceCallSite callSite)
		{
			if (callSite.Cache.Location == CallSiteResultCacheLocation.Scope)
			{
				return Expression.Lambda<Func<ServiceProviderEngineScope, object>>(Expression.Block(new ParameterExpression[2] { ResolvedServices, Sync }, ResolvedServicesVariableAssignment, SyncVariableAssignment, BuildScopedExpression(callSite)), new ParameterExpression[1] { ScopeParameter });
			}
			return Expression.Lambda<Func<ServiceProviderEngineScope, object>>(Convert(VisitCallSite(callSite, null), typeof(object), forceValueTypeConversion: true), new ParameterExpression[1] { ScopeParameter });
		}

		protected override Expression VisitRootCache(ServiceCallSite singletonCallSite, object? context)
		{
			return Expression.Constant(CallSiteRuntimeResolver.Instance.Resolve(singletonCallSite, _rootScope));
		}

		protected override Expression VisitConstant(ConstantCallSite constantCallSite, object? context)
		{
			return Expression.Constant(constantCallSite.DefaultValue);
		}

		protected override Expression VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, object? context)
		{
			return ScopeParameter;
		}

		protected override Expression VisitFactory(FactoryCallSite factoryCallSite, object? context)
		{
			return Expression.Invoke(Expression.Constant(factoryCallSite.Factory), ScopeParameter);
		}

		protected override Expression VisitIEnumerable(IEnumerableCallSite callSite, object? context)
		{
			object context2 = context;
			IEnumerableCallSite callSite2 = callSite;
			if (callSite2.ServiceCallSites.Length == 0)
			{
				return Expression.Constant(GetArrayEmptyMethodInfo(callSite2.ItemType).Invoke(null, Array.Empty<object>()));
			}
			return NewArrayInit(callSite2.ItemType, callSite2.ServiceCallSites.Select((ServiceCallSite cs) => Convert(VisitCallSite(cs, context2), callSite2.ItemType)));
			[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode", Justification = "VerifyAotCompatibility ensures elementType is not a ValueType")]
			static MethodInfo GetArrayEmptyMethodInfo(Type elementType)
			{
				return ServiceLookupHelpers.GetArrayEmptyMethodInfo(elementType);
			}
			[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode", Justification = "VerifyAotCompatibility ensures elementType is not a ValueType")]
			static NewArrayExpression NewArrayInit(Type elementType, IEnumerable<Expression> expr)
			{
				return Expression.NewArrayInit(elementType, expr);
			}
		}

		protected override Expression VisitDisposeCache(ServiceCallSite callSite, object? context)
		{
			return TryCaptureDisposable(callSite, ScopeParameter, VisitCallSiteMain(callSite, context));
		}

		private static Expression TryCaptureDisposable(ServiceCallSite callSite, ParameterExpression scope, Expression service)
		{
			if (!callSite.CaptureDisposable)
			{
				return service;
			}
			return Expression.Invoke(GetCaptureDisposable(scope), service);
		}

		protected override Expression VisitConstructor(ConstructorCallSite callSite, object? context)
		{
			ParameterInfo[] parameters = callSite.ConstructorInfo.GetParameters();
			Expression[] array;
			if (callSite.ParameterCallSites.Length == 0)
			{
				array = Array.Empty<Expression>();
			}
			else
			{
				array = new Expression[callSite.ParameterCallSites.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Convert(VisitCallSite(callSite.ParameterCallSites[i], context), parameters[i].ParameterType);
				}
			}
			Expression expression = Expression.New(callSite.ConstructorInfo, array);
			if (callSite.ImplementationType!.IsValueType)
			{
				expression = Expression.Convert(expression, typeof(object));
			}
			return expression;
		}

		private static Expression Convert(Expression expression, Type type, bool forceValueTypeConversion = false)
		{
			if (type.IsAssignableFrom(expression.Type) && (!expression.Type.IsValueType || !forceValueTypeConversion))
			{
				return expression;
			}
			return Expression.Convert(expression, type);
		}

		protected override Expression VisitScopeCache(ServiceCallSite callSite, object? context)
		{
			Func<ServiceProviderEngineScope, object> value = Build(callSite);
			return Expression.Invoke(Expression.Constant(value), ScopeParameter);
		}

		private ConditionalExpression BuildScopedExpression(ServiceCallSite callSite)
		{
			ConstantExpression arg = Expression.Constant(callSite, typeof(ServiceCallSite));
			MethodCallExpression ifTrue = Expression.Call(CallSiteRuntimeResolverInstanceExpression, ServiceLookupHelpers.ResolveCallSiteAndScopeMethodInfo, arg, ScopeParameter);
			ConstantExpression arg2 = Expression.Constant(callSite.Cache.Key, typeof(ServiceCacheKey));
			ParameterExpression parameterExpression = Expression.Variable(typeof(object), "resolved");
			ParameterExpression resolvedServices = ResolvedServices;
			MethodCallExpression expression = Expression.Call(resolvedServices, ServiceLookupHelpers.TryGetValueMethodInfo, arg2, parameterExpression);
			Expression right = TryCaptureDisposable(callSite, ScopeParameter, VisitCallSiteMain(callSite, null));
			BinaryExpression arg3 = Expression.Assign(parameterExpression, right);
			MethodCallExpression arg4 = Expression.Call(resolvedServices, ServiceLookupHelpers.AddMethodInfo, arg2, parameterExpression);
			BlockExpression arg5 = Expression.Block(typeof(object), new ParameterExpression[1] { parameterExpression }, Expression.IfThen(Expression.Not(expression), Expression.Block(arg3, arg4)), parameterExpression);
			ParameterExpression parameterExpression2 = Expression.Variable(typeof(bool), "lockWasTaken");
			ParameterExpression sync = Sync;
			MethodCallExpression arg6 = Expression.Call(ServiceLookupHelpers.MonitorEnterMethodInfo, sync, parameterExpression2);
			MethodCallExpression ifTrue2 = Expression.Call(ServiceLookupHelpers.MonitorExitMethodInfo, sync);
			BlockExpression body = Expression.Block(arg6, arg5);
			ConditionalExpression @finally = Expression.IfThen(parameterExpression2, ifTrue2);
			return Expression.Condition(Expression.Property(ScopeParameter, typeof(ServiceProviderEngineScope).GetProperty("IsRootScope", BindingFlags.Instance | BindingFlags.Public)), ifTrue, Expression.Block(typeof(object), new ParameterExpression[1] { parameterExpression2 }, Expression.TryFinally(body, @finally)));
		}

		public static Expression GetCaptureDisposable(ParameterExpression scope)
		{
			if (scope != ScopeParameter)
			{
				throw new NotSupportedException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.GetCaptureDisposableNotSupported);
			}
			return CaptureDisposable;
		}
	}
}
