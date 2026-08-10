using System.Runtime.CompilerServices;

namespace System
{
	internal static class _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EThrowHelper
	{
		internal static void ThrowIfNull(object argument, [_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ECallerArgumentExpression("argument")] string paramName = null)
		{
			if (argument == null)
			{
				Throw(paramName);
			}
		}

		private static void Throw(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string IfNullOrWhitespace(string argument, [_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ECallerArgumentExpression("argument")] string paramName = "")
		{
			if (argument == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (string.IsNullOrWhiteSpace(argument))
			{
				if (argument == null)
				{
					throw new ArgumentNullException(paramName);
				}
				throw new ArgumentException(paramName, "Argument is whitespace");
			}
			return argument;
		}
	}
}
