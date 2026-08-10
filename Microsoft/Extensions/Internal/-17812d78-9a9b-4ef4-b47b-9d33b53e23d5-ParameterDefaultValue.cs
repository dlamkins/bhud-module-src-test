using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace Microsoft.Extensions.Internal
{
	internal static class _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EParameterDefaultValue
	{
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
			[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2067:UnrecognizedReflectionPattern", Justification = "CreateValueType is only called on a ValueType. You can always create an instance of a ValueType.")]
			static object? CreateValueType(Type t)
			{
				return FormatterServices.GetUninitializedObject(t);
			}
		}
	}
}
