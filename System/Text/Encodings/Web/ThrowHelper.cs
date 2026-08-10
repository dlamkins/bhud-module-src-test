using System.Diagnostics.CodeAnalysis;

namespace System.Text.Encodings.Web
{
	internal static class ThrowHelper
	{
		[_003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003EDoesNotReturn]
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw new ArgumentNullException(GetArgumentName(argument));
		}

		[_003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003EDoesNotReturn]
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw new ArgumentOutOfRangeException(GetArgumentName(argument));
		}

		private static string GetArgumentName(ExceptionArgument argument)
		{
			return argument.ToString();
		}
	}
}
