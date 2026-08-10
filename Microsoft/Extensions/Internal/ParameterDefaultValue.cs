using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace Microsoft.Extensions.Internal
{
	internal static class ParameterDefaultValue
	{
		public static bool TryGetDefaultValue(ParameterInfo parameter, out object? defaultValue)
		{
			bool tryToGetDefaultValue;
			bool flag = CheckHasDefaultValue(parameter, out tryToGetDefaultValue);
			defaultValue = null;
			if (flag)
			{
				if (tryToGetDefaultValue)
				{
					defaultValue = parameter.DefaultValue;
				}
				bool flag2 = parameter.ParameterType.IsGenericType && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>);
				if (defaultValue == null && parameter.ParameterType.IsValueType && !flag2)
				{
					defaultValue = CreateValueType(parameter.ParameterType);
				}
				if (defaultValue != null && flag2)
				{
					Type underlyingType = Nullable.GetUnderlyingType(parameter.ParameterType);
					if (underlyingType != null && underlyingType.IsEnum)
					{
						defaultValue = Enum.ToObject(underlyingType, defaultValue);
					}
				}
			}
			return flag;
			[_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2067:UnrecognizedReflectionPattern", Justification = "CreateValueType is only called on a ValueType. You can always create an instance of a ValueType.")]
			static object? CreateValueType(Type t)
			{
				return FormatterServices.GetUninitializedObject(t);
			}
		}

		public static bool CheckHasDefaultValue(ParameterInfo parameter, out bool tryToGetDefaultValue)
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
	}
}
