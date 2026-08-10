using System.Runtime.CompilerServices;

namespace System
{
	internal static class _003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper
	{
		internal static void ThrowIfNull(object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
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
		public static string IfNullOrWhitespace(string? argument, [CallerArgumentExpression("argument")] string paramName = "")
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
