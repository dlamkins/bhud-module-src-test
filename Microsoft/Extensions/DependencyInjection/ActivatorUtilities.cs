using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class ActivatorUtilities
	{
		private readonly struct FactoryParameterContext
		{
			public Type ParameterType { get; }

			public bool HasDefaultValue { get; }

			public object DefaultValue { get; }

			public int ArgumentIndex { get; }

			public object ServiceKey { get; }

			public FactoryParameterContext(Type parameterType, bool hasDefaultValue, object defaultValue, int argumentIndex, object serviceKey)
			{
				ParameterType = parameterType;
				HasDefaultValue = hasDefaultValue;
				DefaultValue = defaultValue;
				ArgumentIndex = argumentIndex;
				ServiceKey = serviceKey;
			}
		}

		private sealed class ConstructorInfoEx
		{
			public readonly ConstructorInfo Info;

			public readonly ParameterInfo[] Parameters;

			public readonly bool IsPreferred;

			private readonly object[] _parameterKeys;

			public ConstructorInfoEx(ConstructorInfo constructor)
			{
				Info = constructor;
				Parameters = constructor.GetParameters();
				IsPreferred = constructor.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), inherit: false);
				for (int i = 0; i < Parameters.Length; i++)
				{
					FromKeyedServicesAttribute fromKeyedServicesAttribute = (FromKeyedServicesAttribute)Attribute.GetCustomAttribute(Parameters[i], typeof(FromKeyedServicesAttribute), inherit: false);
					if (fromKeyedServicesAttribute != null)
					{
						if (_parameterKeys == null)
						{
							_parameterKeys = new object[Parameters.Length];
						}
						_parameterKeys[i] = fromKeyedServicesAttribute.Key;
					}
				}
			}

			public bool IsService(IServiceProviderIsService serviceProviderIsService, int parameterIndex)
			{
				ParameterInfo parameterInfo = Parameters[parameterIndex];
				object[] parameterKeys = _parameterKeys;
				object obj = ((parameterKeys != null) ? parameterKeys[parameterIndex] : null);
				if (obj != null)
				{
					IServiceProviderIsKeyedService serviceProviderIsKeyedService = serviceProviderIsService as IServiceProviderIsKeyedService;
					if (serviceProviderIsKeyedService != null)
					{
						return serviceProviderIsKeyedService.IsKeyedService(parameterInfo.ParameterType, obj);
					}
					throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.KeyedServicesNotSupported);
				}
				return serviceProviderIsService.IsService(parameterInfo.ParameterType);
			}

			public object GetService(IServiceProvider serviceProvider, int parameterIndex)
			{
				ParameterInfo parameterInfo = Parameters[parameterIndex];
				object[] parameterKeys = _parameterKeys;
				object obj = ((parameterKeys != null) ? parameterKeys[parameterIndex] : null);
				if (obj != null)
				{
					IKeyedServiceProvider keyedServiceProvider = serviceProvider as IKeyedServiceProvider;
					if (keyedServiceProvider != null)
					{
						return keyedServiceProvider.GetKeyedService(parameterInfo.ParameterType, obj);
					}
					throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.KeyedServicesNotSupported);
				}
				return serviceProvider.GetService(parameterInfo.ParameterType);
			}
		}

		private readonly struct ConstructorMatcher
		{
			private readonly ConstructorInfoEx _constructor;

			private readonly object[] _parameterValues;

			public ConstructorMatcher(ConstructorInfoEx constructor)
			{
				_constructor = constructor;
				_parameterValues = new object[constructor.Parameters.Length];
			}

			public int Match(object[] givenParameters, IServiceProviderIsService serviceProviderIsService)
			{
				for (int i = 0; i < givenParameters.Length; i++)
				{
					Type c = givenParameters[i]?.GetType();
					bool flag = false;
					for (int j = 0; j < _constructor.Parameters.Length; j++)
					{
						if (_parameterValues[j] == null && _constructor.Parameters[j].ParameterType.IsAssignableFrom(c))
						{
							flag = true;
							_parameterValues[j] = givenParameters[i];
							break;
						}
					}
					if (!flag)
					{
						return -1;
					}
				}
				for (int k = 0; k < _constructor.Parameters.Length; k++)
				{
					if (_parameterValues[k] == null && !_constructor.IsService(serviceProviderIsService, k))
					{
						if (!ParameterDefaultValue.TryGetDefaultValue(_constructor.Parameters[k], out var defaultValue))
						{
							return -1;
						}
						_parameterValues[k] = defaultValue;
					}
				}
				return _constructor.Parameters.Length;
			}

			public object CreateInstance(IServiceProvider provider)
			{
				for (int i = 0; i < _constructor.Parameters.Length; i++)
				{
					if (_parameterValues[i] != null)
					{
						continue;
					}
					object service = _constructor.GetService(provider, i);
					if (service == null)
					{
						if (!ParameterDefaultValue.TryGetDefaultValue(_constructor.Parameters[i], out var defaultValue))
						{
							throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.UnableToResolveService, _constructor.Parameters[i].ParameterType, _constructor.Info.DeclaringType));
						}
						_parameterValues[i] = defaultValue;
					}
					else
					{
						_parameterValues[i] = service;
					}
				}
				try
				{
					return _constructor.Info.Invoke(_parameterValues);
				}
				catch (TargetInvocationException ex) when (ex.InnerException != null)
				{
					ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
					throw;
				}
			}

			public void MapParameters(int?[] parameterMap, object[] givenParameters)
			{
				for (int i = 0; i < _constructor.Parameters.Length; i++)
				{
					if (parameterMap[i].HasValue)
					{
						_parameterValues[i] = givenParameters[parameterMap[i].Value];
					}
				}
			}
		}

		private static readonly MethodInfo GetServiceInfo = GetMethodInfo<Func<IServiceProvider, Type, Type, bool, object, object>>((IServiceProvider sp, Type t, Type r, bool c, object k) => GetService(sp, t, r, c, k));

		public static object CreateInstance(IServiceProvider provider, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, params object[] parameters)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (instanceType.IsAbstract)
			{
				throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.CannotCreateAbstractClasses);
			}
			ConstructorInfoEx[] array = CreateConstructorInfoExs(instanceType);
			IServiceProviderIsService service = provider.GetService<IServiceProviderIsService>();
			ConstructorInfoEx constructorInfoEx;
			if (service != null)
			{
				int num = -1;
				bool flag = false;
				ConstructorMatcher constructorMatcher = default(ConstructorMatcher);
				bool flag2 = false;
				for (int i = 0; i < array.Length; i++)
				{
					constructorInfoEx = array[i];
					ConstructorMatcher constructorMatcher2 = new ConstructorMatcher(constructorInfoEx);
					bool isPreferred = constructorInfoEx.IsPreferred;
					int num2 = constructorMatcher2.Match(parameters, service);
					if (isPreferred)
					{
						if (flag)
						{
							ThrowMultipleCtorsMarkedWithAttributeException();
						}
						if (num2 == -1)
						{
							ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
						}
					}
					if (isPreferred || num < num2)
					{
						num = num2;
						constructorMatcher = constructorMatcher2;
						flag2 = false;
					}
					else if (num == num2)
					{
						flag2 = true;
					}
					flag = flag || isPreferred;
				}
				if (num != -1)
				{
					if (flag2)
					{
						throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.MultipleCtorsFoundWithBestLength, instanceType, num));
					}
					return constructorMatcher.CreateInstance(provider);
				}
			}
			Type[] array2;
			if (parameters.Length == 0)
			{
				array2 = Type.EmptyTypes;
			}
			else
			{
				array2 = new Type[parameters.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = parameters[j]?.GetType();
				}
			}
			FindApplicableConstructor(instanceType, array2, out var matchingConstructor, out var matchingParameterMap);
			constructorInfoEx = null;
			ConstructorInfoEx[] array3 = array;
			foreach (ConstructorInfoEx constructorInfoEx2 in array3)
			{
				if ((object)constructorInfoEx2.Info == matchingConstructor)
				{
					constructorInfoEx = constructorInfoEx2;
					break;
				}
			}
			ConstructorMatcher constructorMatcher3 = new ConstructorMatcher(constructorInfoEx);
			constructorMatcher3.MapParameters(matchingParameterMap, parameters);
			return constructorMatcher3.CreateInstance(provider);
		}

		private static ConstructorInfoEx[] CreateConstructorInfoExs([_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			ConstructorInfoEx[] array = new ConstructorInfoEx[constructors.Length];
			for (int i = 0; i < constructors.Length; i++)
			{
				array[i] = new ConstructorInfoEx(constructors[i]);
			}
			return array;
		}

		public static ObjectFactory CreateFactory([_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, Type[] argumentTypes)
		{
			CreateFactoryInternal(instanceType, argumentTypes, out var provider, out var argumentArray, out var factoryExpressionBody);
			Expression<Func<IServiceProvider, object[], object>> expression = Expression.Lambda<Func<IServiceProvider, object[], object>>(factoryExpressionBody, new ParameterExpression[2] { provider, argumentArray });
			Func<IServiceProvider, object[], object> @object = expression.Compile();
			return new ObjectFactory(@object.Invoke);
		}

		public static ObjectFactory<T> CreateFactory<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] T>(Type[] argumentTypes)
		{
			CreateFactoryInternal(typeof(T), argumentTypes, out var provider, out var argumentArray, out var factoryExpressionBody);
			Expression<Func<IServiceProvider, object[], T>> expression = Expression.Lambda<Func<IServiceProvider, object[], T>>(factoryExpressionBody, new ParameterExpression[2] { provider, argumentArray });
			Func<IServiceProvider, object[], T> @object = expression.Compile();
			return new ObjectFactory<T>(@object.Invoke);
		}

		private static void CreateFactoryInternal([_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, Type[] argumentTypes, out ParameterExpression provider, out ParameterExpression argumentArray, out Expression factoryExpressionBody)
		{
			FindApplicableConstructor(instanceType, argumentTypes, out var matchingConstructor, out var matchingParameterMap);
			provider = Expression.Parameter(typeof(IServiceProvider), "provider");
			argumentArray = Expression.Parameter(typeof(object[]), "argumentArray");
			factoryExpressionBody = BuildFactoryExpression(matchingConstructor, matchingParameterMap, provider, argumentArray);
		}

		public static T CreateInstance<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] T>(IServiceProvider provider, params object[] parameters)
		{
			return (T)CreateInstance(provider, typeof(T), parameters);
		}

		public static T GetServiceOrCreateInstance<[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] T>(IServiceProvider provider)
		{
			return (T)GetServiceOrCreateInstance(provider, typeof(T));
		}

		public static object GetServiceOrCreateInstance(IServiceProvider provider, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
		{
			return provider.GetService(type) ?? CreateInstance(provider, type);
		}

		private static MethodInfo GetMethodInfo<T>(Expression<T> expr)
		{
			MethodCallExpression methodCallExpression = (MethodCallExpression)expr.Body;
			return methodCallExpression.Method;
		}

		private static object GetService(IServiceProvider sp, Type type, Type requiredBy, bool hasDefaultValue, object key)
		{
			object obj = ((key == null) ? sp.GetService(type) : GetKeyedService(sp, type, key));
			if (obj == null && !hasDefaultValue)
			{
				ThrowHelperUnableToResolveService(type, requiredBy);
			}
			return obj;
		}

		[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDoesNotReturn]
		private static void ThrowHelperUnableToResolveService(Type type, Type requiredBy)
		{
			throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.UnableToResolveService, type, requiredBy));
		}

		private static BlockExpression BuildFactoryExpression(ConstructorInfo constructor, int?[] parameterMap, Expression serviceProvider, Expression factoryArgumentArray)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			Expression[] array = new Expression[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type parameterType = parameterInfo.ParameterType;
				object defaultValue;
				bool flag = ParameterDefaultValue.TryGetDefaultValue(parameterInfo, out defaultValue);
				if (parameterMap[i].HasValue)
				{
					array[i] = Expression.ArrayAccess(factoryArgumentArray, Expression.Constant(parameterMap[i]));
				}
				else
				{
					FromKeyedServicesAttribute fromKeyedServicesAttribute = (FromKeyedServicesAttribute)Attribute.GetCustomAttribute(parameterInfo, typeof(FromKeyedServicesAttribute), inherit: false);
					Expression[] arguments = new Expression[5]
					{
						serviceProvider,
						Expression.Constant(parameterType, typeof(Type)),
						Expression.Constant(constructor.DeclaringType, typeof(Type)),
						Expression.Constant(flag),
						Expression.Constant(fromKeyedServicesAttribute?.Key)
					};
					array[i] = Expression.Call(GetServiceInfo, arguments);
				}
				if (flag)
				{
					ConstantExpression right = Expression.Constant(defaultValue);
					array[i] = Expression.Coalesce(array[i], right);
				}
				array[i] = Expression.Convert(array[i], parameterType);
			}
			return Expression.Block(Expression.IfThen(Expression.Equal(serviceProvider, Expression.Constant(null)), Expression.Throw(Expression.Constant(new ArgumentNullException("serviceProvider")))), Expression.New(constructor, array));
		}

		private static void FindApplicableConstructor([_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, Type[] argumentTypes, out ConstructorInfo matchingConstructor, out int?[] matchingParameterMap)
		{
			if (!TryFindPreferredConstructor(instanceType, argumentTypes, out var matchingConstructor2, out var parameterMap) && !TryFindMatchingConstructor(instanceType, argumentTypes, out matchingConstructor2, out parameterMap))
			{
				throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.CtorNotLocated, instanceType));
			}
			matchingConstructor = matchingConstructor2;
			matchingParameterMap = parameterMap;
		}

		private static bool TryFindMatchingConstructor([_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, Type[] argumentTypes, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ENotNullWhen(true)] out ConstructorInfo matchingConstructor, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ENotNullWhen(true)] out int?[] parameterMap)
		{
			matchingConstructor = null;
			parameterMap = null;
			ConstructorInfo[] constructors = instanceType.GetConstructors();
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				if (TryCreateParameterMap(constructorInfo.GetParameters(), argumentTypes, out var parameterMap2))
				{
					if (matchingConstructor != null)
					{
						throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.MultipleCtorsFound, instanceType));
					}
					matchingConstructor = constructorInfo;
					parameterMap = parameterMap2;
				}
			}
			if (matchingConstructor != null)
			{
				return true;
			}
			return false;
		}

		private static bool TryFindPreferredConstructor([_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMembers(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, Type[] argumentTypes, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ENotNullWhen(true)] out ConstructorInfo matchingConstructor, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ENotNullWhen(true)] out int?[] parameterMap)
		{
			bool flag = false;
			matchingConstructor = null;
			parameterMap = null;
			ConstructorInfo[] constructors = instanceType.GetConstructors();
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				if (constructorInfo.IsDefined(typeof(ActivatorUtilitiesConstructorAttribute), inherit: false))
				{
					if (flag)
					{
						ThrowMultipleCtorsMarkedWithAttributeException();
					}
					if (!TryCreateParameterMap(constructorInfo.GetParameters(), argumentTypes, out var parameterMap2))
					{
						ThrowMarkedCtorDoesNotTakeAllProvidedArguments();
					}
					matchingConstructor = constructorInfo;
					parameterMap = parameterMap2;
					flag = true;
				}
			}
			if (matchingConstructor != null)
			{
				return true;
			}
			return false;
		}

		private static bool TryCreateParameterMap(ParameterInfo[] constructorParameters, Type[] argumentTypes, out int?[] parameterMap)
		{
			parameterMap = new int?[constructorParameters.Length];
			for (int i = 0; i < argumentTypes.Length; i++)
			{
				bool flag = false;
				Type c = argumentTypes[i];
				for (int j = 0; j < constructorParameters.Length; j++)
				{
					if (!parameterMap[j].HasValue && constructorParameters[j].ParameterType.IsAssignableFrom(c))
					{
						flag = true;
						parameterMap[j] = i;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		private static void ThrowMultipleCtorsMarkedWithAttributeException()
		{
			throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.MultipleCtorsMarkedWithAttribute, "ActivatorUtilitiesConstructorAttribute"));
		}

		private static void ThrowMarkedCtorDoesNotTakeAllProvidedArguments()
		{
			throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.Format(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.MarkedCtorMissingArgumentTypes, "ActivatorUtilitiesConstructorAttribute"));
		}

		private static object GetKeyedService(IServiceProvider provider, Type type, object serviceKey)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(provider, "provider");
			IKeyedServiceProvider keyedServiceProvider = provider as IKeyedServiceProvider;
			if (keyedServiceProvider != null)
			{
				return keyedServiceProvider.GetKeyedService(type, serviceKey);
			}
			throw new InvalidOperationException(_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ESR.KeyedServicesNotSupported);
		}
	}
}
