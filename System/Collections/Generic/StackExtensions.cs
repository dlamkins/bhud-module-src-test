using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
	internal static class StackExtensions
	{
		public static bool TryPeek<T>(this Stack<T> stack, [_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EMaybeNullWhen(false)] out T result)
		{
			if (stack.Count > 0)
			{
				result = stack.Peek();
				return true;
			}
			result = default(T);
			return false;
		}

		public static bool TryPop<T>(this Stack<T> stack, [_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EMaybeNullWhen(false)] out T result)
		{
			if (stack.Count > 0)
			{
				result = stack.Pop();
				return true;
			}
			result = default(T);
			return false;
		}
	}
}
