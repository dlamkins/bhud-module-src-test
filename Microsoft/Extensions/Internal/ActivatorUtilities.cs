using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Microsoft.Extensions.Internal
{
	internal static class ActivatorUtilities
	{
		private struct ConstructorMatcher
		{
			private readonly ConstructorInfo _constructor;

			private readonly ParameterInfo[] _parameters;

			private readonly object[] _parameterValues;

			public ConstructorMatcher(ConstructorInfo constructor)
			{
				_constructor = constructor;
				_parameters = _constructor.GetParameters();
				_parameterValues = new object[_parameters.Length];
			}

			public int Match(object[] givenParameters)
			{
				int num = 0;
				int result = 0;
				for (int i = 0; i != givenParameters.Length; i++)
				{
					Type c = givenParameters[i]?.GetType();
					bool flag = false;
					int num2 = num;
					while (!flag && num2 != _parameters.Length)
					{
						if (_parameterValues[num2] == null && _parameters[num2].ParameterType.IsAssignableFrom(c))
						{
							flag = true;
							_parameterValues[num2] = givenParameters[i];
							if (num == num2)
							{
								num++;
								if (num2 == i)
								{
									result = num2;
								}
							}
						}
						num2++;
					}
					if (!flag)
					{
						return -1;
					}
				}
				return result;
			}

			public object CreateInstance(IServiceProvider provider)
			{
				for (int i = 0; i < _parameters.Length; i++)
				{
					ParameterInfo parameterInfo = _parameters[i];
					if (_parameterValues[i] != null)
					{
						continue;
					}
					object obj = provider.GetService(parameterInfo.ParameterType);
					if (obj == null)
					{
						if (!_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EParameterDefaultValue.TryGetDefaultValue(parameterInfo, out var defaultValue))
						{
							throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[i].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
						}
						obj = defaultValue;
					}
					_parameterValues[i] = obj;
				}
				try
				{
					return _constructor.Invoke(_parameterValues);
				}
				catch (TargetInvocationException ex) when (ex.InnerException != null)
				{
					ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
					throw;
				}
			}
		}

		private const _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes ActivatorAccessibility = _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes.PublicConstructors;

		public static object CreateInstance(IServiceProvider provider, [_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMembers(_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType, params object[] parameters)
		{
			int num = -1;
			ConstructorMatcher constructorMatcher = default(ConstructorMatcher);
			if (!instanceType.IsAbstract)
			{
				ConstructorInfo[] constructors = instanceType.GetConstructors();
				foreach (ConstructorInfo constructor in constructors)
				{
					ConstructorMatcher constructorMatcher2 = new ConstructorMatcher(constructor);
					int num2 = constructorMatcher2.Match(parameters);
					if (num < num2)
					{
						num = num2;
						constructorMatcher = constructorMatcher2;
					}
				}
			}
			if (num == -1)
			{
				throw new InvalidOperationException($"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.");
			}
			return constructorMatcher.CreateInstance(provider);
		}

		public static T CreateInstance<[_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMembers(_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes.PublicConstructors)] T>(IServiceProvider provider, params object[] parameters)
		{
			return (T)CreateInstance(provider, typeof(T), parameters);
		}

		public static T GetServiceOrCreateInstance<[_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMembers(_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes.PublicConstructors)] T>(IServiceProvider provider)
		{
			return (T)GetServiceOrCreateInstance(provider, typeof(T));
		}

		public static object GetServiceOrCreateInstance(IServiceProvider provider, [_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMembers(_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
		{
			return provider.GetService(type) ?? CreateInstance(provider, type);
		}
	}
}
