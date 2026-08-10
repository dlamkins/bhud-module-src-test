using System.Runtime.CompilerServices;

namespace System
{
	internal static class _003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper
	{
		internal static void ThrowIfNull(object? argument, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ECallerArgumentExpression("argument")] string? paramName = null)
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
		public static string IfNullOrWhitespace(string? argument, [_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003ECallerArgumentExpression("argument")] string paramName = "")
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
