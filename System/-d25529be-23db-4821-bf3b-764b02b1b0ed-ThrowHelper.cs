using System.Runtime.CompilerServices;

namespace System
{
	internal static class _003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EThrowHelper
	{
		internal static void ThrowIfNull(object? argument, [_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003ECallerArgumentExpression("argument")] string? paramName = null)
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
		public static string IfNullOrWhitespace(string? argument, [_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003ECallerArgumentExpression("argument")] string paramName = "")
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
