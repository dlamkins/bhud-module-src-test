using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Shared
{
	internal static class ArgumentNullThrowHelper
	{
		public static void ThrowIfNull([_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ENotNull] object argument, [_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003ECallerArgumentExpression("argument")] string paramName = null)
		{
			if (argument == null)
			{
				Throw(paramName);
			}
		}

		[_003C3e9707ca_002D2ada_002D4862_002Da41b_002D92d27ca64063_003EDoesNotReturn]
		internal static void Throw(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}
	}
}
