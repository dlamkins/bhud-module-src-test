using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Microsoft.Extensions.Internal
{
	internal static class _003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003EParameterDefaultValue
	{
		public static bool TryGetDefaultValue(ParameterInfo parameter, out object defaultValue)
		{
			bool tryToGetDefaultValue;
			bool result = CheckHasDefaultValue(parameter, out tryToGetDefaultValue);
			defaultValue = null;
			if (parameter.HasDefaultValue)
			{
				if (tryToGetDefaultValue)
				{
					defaultValue = parameter.DefaultValue;
				}
				bool flag = parameter.ParameterType.IsGenericType && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>);
				if (defaultValue == null && parameter.ParameterType.IsValueType && !flag)
				{
					defaultValue = CreateValueType(parameter.ParameterType);
				}
				if (defaultValue != null && flag)
				{
					Type underlyingType = Nullable.GetUnderlyingType(parameter.ParameterType);
					if (underlyingType != null && underlyingType.IsEnum)
					{
						defaultValue = Enum.ToObject(underlyingType, defaultValue);
					}
				}
			}
			return result;
		}

		private static bool CheckHasDefaultValue(ParameterInfo parameter, out bool tryToGetDefaultValue)
		{
			tryToGetDefaultValue = true;
			try
			{
				return parameter.HasDefaultValue;
			}
			catch (FormatException) when (parameter.ParameterType == typeof(DateTime))
			{
				tryToGetDefaultValue = false;
				return true;
			}
		}

		private static object CreateValueType(Type t)
		{
			return FormatterServices.GetSafeUninitializedObject(t);
		}
	}
}
