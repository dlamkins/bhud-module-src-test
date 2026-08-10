using System.Runtime.CompilerServices;

namespace System
{
	internal static class ThrowHelper
	{
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw GetArgumentNullException(argument);
		}

		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw GetArgumentOutOfRangeException(argument);
		}

		private static ArgumentNullException GetArgumentNullException(ExceptionArgument argument)
		{
			return new ArgumentNullException(GetArgumentName(argument));
		}

		private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument)
		{
			return new ArgumentOutOfRangeException(GetArgumentName(argument));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static string GetArgumentName(ExceptionArgument argument)
		{
			return argument.ToString();
		}
	}
}
