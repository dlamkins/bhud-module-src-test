using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Shared
{
	internal static class ArgumentOutOfRangeThrowHelper
	{
		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		private static void ThrowZero<T>(string paramName, T value)
		{
			throw new ArgumentOutOfRangeException(paramName, value, "'" + paramName + "' must be a non-zero value.");
		}

		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		private static void ThrowNegative<T>(string paramName, T value)
		{
			throw new ArgumentOutOfRangeException(paramName, value, "'" + paramName + "' must be a non-negative value.");
		}

		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		private static void ThrowNegativeOrZero<T>(string paramName, T value)
		{
			throw new ArgumentOutOfRangeException(paramName, value, "'" + paramName + "' must be a non-negative and non-zero value.");
		}

		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		private static void ThrowGreater<T>(string paramName, T value, T other)
		{
			throw new ArgumentOutOfRangeException(paramName, value, $"'{paramName}' must be less than or equal to '{other}'.");
		}

		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		private static void ThrowGreaterEqual<T>(string paramName, T value, T other)
		{
			throw new ArgumentOutOfRangeException(paramName, value, $"'{value}' must be less than '{other}'.");
		}

		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		private static void ThrowLess<T>(string paramName, T value, T other)
		{
			throw new ArgumentOutOfRangeException(paramName, value, $"'{value}' must be greater than or equal to '{other}'.");
		}

		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		private static void ThrowLessEqual<T>(string paramName, T value, T other)
		{
			throw new ArgumentOutOfRangeException(paramName, value, $"'{value}' must be greater than '{other}'.");
		}

		public static void ThrowIfZero(int value, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null)
		{
			if (value == 0)
			{
				ThrowZero(paramName, value);
			}
		}

		public static void ThrowIfNegative(int value, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null)
		{
			if (value < 0)
			{
				ThrowNegative(paramName, value);
			}
		}

		public static void ThrowIfNegative(long value, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null)
		{
			if (value < 0)
			{
				ThrowNegative(paramName, value);
			}
		}

		public static void ThrowIfNegativeOrZero(int value, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null)
		{
			if (value <= 0)
			{
				ThrowNegativeOrZero(paramName, value);
			}
		}

		public static void ThrowIfGreaterThan<T>(T value, T other, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null) where T : IComparable<T>
		{
			if (value.CompareTo(other) > 0)
			{
				ThrowGreater(paramName, value, other);
			}
		}

		public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null) where T : IComparable<T>
		{
			if (value.CompareTo(other) >= 0)
			{
				ThrowGreaterEqual(paramName, value, other);
			}
		}

		public static void ThrowIfLessThan<T>(T value, T other, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null) where T : IComparable<T>
		{
			if (value.CompareTo(other) < 0)
			{
				ThrowLess(paramName, value, other);
			}
		}

		public static void ThrowIfLessThanOrEqual<T>(T value, T other, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("value")] string paramName = null) where T : IComparable<T>
		{
			if (value.CompareTo(other) <= 0)
			{
				ThrowLessEqual(paramName, value, other);
			}
		}
	}
}
